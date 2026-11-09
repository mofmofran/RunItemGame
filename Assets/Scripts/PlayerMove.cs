using UnityEngine;
using UnityEngine.InputSystem;//InputSystemに必要

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rbody;//Rigitbody2D変数
    float axisH = 0.0f;//水平入力変数
    public float speed = 5.0f; // 歩きの速度
    public float jumpPower = 3.0f; // ジャンプの力
    public float airSpeed = 0.3f;//ジャンプ中の速度
    
    public float airAcceleration = 1.0f; // 空中での加速力
    public float jumpHoldAcceleration = 20.0f; // 長押し中の上昇力
    public float maxRiseSpeed = 10.0f;//上昇速度上限

    public float boostSpeed = 5.0f;//ブースト速度
    
    public int hp = 750; //HP

    public float mainAmmo = 7.0f;//メイン射撃弾数
    public float subAmmo = 2.0f;//サブ射撃弾数
    public float spAmmo = 2.0f;//特殊射撃弾数
    public float boostGauge = 1000.0f; // ブーストゲージ
    public float eXgauge = 0f;//EXゲージ

    public float cShoot;//CSチャージゲージ
    public float mCShoot;//格闘CSチャージゲージ

    public LayerMask groundLayer; // 接地できるレイヤー
    private bool _onJump =false;//ジャンプ開始フラグ
    bool onGround = false; // 接地しているかどうかのフラグ
    bool recoveryTime = false; // 硬直中フラグ

    //アニメーション対応
    Animator animator;//アニメーター対応変数
    private string idleAnime = "PlayerIdle";//停止アニメーション名
    private string startWalkAnime = "PlayerStartWalk";//歩き出しのアニメーション

    private string jumpAnime = "PlayerJump";//ジャンプアニメーション
    string nowAnime;//現在のアニメーション名
    string oldAnime;//前回のアニメーション名

    InputAction _moveAction;//歩きアクション
    InputAction _jumpAction;//ジャンプアクション
    InputAction _boostAction;
    private bool _jumpHeld =false;//ジャンプが押されているか
    private int _jumpCnt;
    private float _lastClickTime = 0f;
    private float _jumpInterval = 0.2f; // 連打判定の猶予
    private bool _onBoost = false;//ブースト初動

    public void Recovery()//硬直中フラグを解除するメソッド
    {
        recoveryTime = false;
        _onJump = false;
        _onBoost = false;
        rbody.gravityScale = 1;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();//Rigitbody2Dコンポーネントを取得
        animator = GetComponent<Animator>();//アニメーターコンポーネントを取得
        nowAnime = idleAnime;//初期アニメーションを停止アニメーションに設定
        oldAnime = idleAnime;//前回アニメーションを停止アニメーションに設定
        PlayerInput input = GetComponent<PlayerInput>();//PlayerInputを取得
        _moveAction = input.currentActionMap.FindAction("Move");//移動アクションを探す
        _jumpAction = input.currentActionMap.FindAction("Jump");//ジャンプアクションを探す
        _boostAction = input.currentActionMap.FindAction("Boost");//ブーストアクションを探す
        InputActionMap uiMap = input.actions.FindActionMap("UI");//UIマップを探す
        uiMap.Disable();//UIマップを無効化
    }

    // Update is called once per frame
    void Update()
    {
        
        axisH = _moveAction.ReadValue<Vector2>().x;//水平入力値を取得
        if (axisH > 0.0f)
        {
            Debug.Log("右向き");
            transform.localScale = new Vector2(1.0f, 1.0f);//右向き
        }
        else if (axisH < 0.0f)
        {
            Debug.Log("左向き");
            transform.localScale = new Vector2(-1.0f, 1.0f);//左向き
        }
        //接地判定
        onGround = Physics2D.CircleCast(transform.position, 0.1f, Vector2.down, 0.0f, groundLayer);//接地判定
        if (_boostAction.WasPressedThisFrame())//ジャンプ二度押しした場合
        {
            Debug.Log("ブースト");
            _onBoost = true;
            Invoke("Recovery", 0.3f);//0.3秒後に硬直中フラグを解除する
        }
        if (_jumpAction.WasPressedThisFrame()&&!_onBoost)//ジャンプボタンが押された場合
        {
            if (boostGauge > 0)//ブーストゲージが残っている場合
            {
                if (recoveryTime == false)//硬直中でない場合
                {
                    Debug.Log("ジャンプ");
                    //ジャンプさせる
                    Vector2 jump = new Vector2(0.0f, jumpPower);//現在地とジャンプ力をVector2型で代入
                    rbody.AddForce(jump, ForceMode2D.Impulse);//ジャンプさせる
                    _onJump = true;
                    boostGauge -= 100;//ブーストゲージを減らす
                    nowAnime = jumpAnime;//ジャンプアニメーションを設定
                    animator.Play(nowAnime);//アニメーション再生
                    if (boostGauge <= 0)
                    {
                        boostGauge = 0;//ブーストゲージが0以下になる場合は0にする
                    }
                    recoveryTime = true;//硬直中フラグを立てる
                    nowAnime = jumpAnime;
                    Invoke("Recovery", 0.3f);//0.3秒後に硬直中フラグを解除する
                }

            }
        }
   
        if (onGround&&_onJump==false)//接地している場合でジャンプ開始中でない場合
        {
            if (axisH == 0.0f)//水平入力がない場合
            {
                nowAnime = idleAnime;//停止アニメーションに設定
                animator.Play(nowAnime);//現在アニメを再生
            }
            else//水平入力がある場合
            {
                nowAnime = startWalkAnime;//歩きアニメーションに設定
            }
        }
        else//接地していない場合
        {
            nowAnime = jumpAnime;//ジャンプアニメーションに設定
        }
        if (nowAnime != oldAnime)//現在のアニメーションが前回のアニメーションと違う場合
        {
            animator.Play(nowAnime);//現在のアニメーションを再生する
            oldAnime = nowAnime;//前回のアニメーションを現在のアニメーションに設定
        }
    }
    void FixedUpdate()
    {
        if (_jumpAction.IsPressed())//ジャンプ長押しを検知
        {
            _jumpHeld = true;
        }
        else
        {
            _jumpHeld = false;
        }
        // ここから下は、現在の移動・ジャンプ処理
        if (onGround && axisH != 0.0f)//接地していて水平入力がある場合
        {
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);//Rigitbody2Dの速度を設定
        }
        if (onGround && axisH == 0.0f)//接地していて水平入力がない場合
        {
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);//Rigitbody2Dの速度を設定
        }
        if (!onGround && axisH != 0.0f)//空中にいて水平入力がある場合
        {
            // 現在の水平速度を取得
            float vx = rbody.linearVelocity.x;

            // 入力方向に少しずつ加速
            vx += axisH * airAcceleration * Time.fixedDeltaTime;

            // 水平方向の速度を更新（Y方向は維持）
            rbody.linearVelocity = new Vector2(vx,rbody.linearVelocity.y);
        }
        if (_jumpHeld && boostGauge>0&&!_onBoost)
        {
            float vy = rbody.linearVelocity.y;

            // 下向きの速度を0にする
            if (vy < 0f)
            {
                vy = 0f;
            }

            // 長押し中は上昇する力を加える
            vy += jumpHoldAcceleration * Time.fixedDeltaTime;

            // 上昇速度の上限を設定する
            vy = Mathf.Min(vy, maxRiseSpeed);

            rbody.linearVelocity =
                new Vector2(rbody.linearVelocity.x, vy);
            boostGauge -= 10f;
        }

        if (boostGauge < 1000 && onGround && !recoveryTime)//ブーストゲージが満タンでなく、接地していて、硬直中でない場合
        {
            Debug.Log("接地＆硬直無し");
            boostGauge = 1000;//ブーストゲージを回復する
        }


       
    }
}
