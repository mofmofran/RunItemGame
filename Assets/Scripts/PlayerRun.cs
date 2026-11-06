using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;

    const int DefaultLife = 3;//デフォルト最大体力
    const float StunDuration = 0.5f;//スタン時間
    CharacterController controller;
    [SerializeField] private GameObject animatorBody;
    Animator animator;
    Vector3 _moveDirection = Vector3.zero;
    private int _targetLane;

    int _life = DefaultLife;//現在HP
    float _recoverTime = 0.0f;//硬直カウントダウン

    [Header("重力・前進・横移動・ジャンプ")]
    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;

    [Header("前進加速力")]
    public float accelerationZ = 10.0f;

    [Header("ノックバック")]
    public float knockBackForceZ = -6.0f;
    Coroutine resetIntervalCol;//インプットインターバルを置くための変数

    private InputAction _moveAction;
    private InputAction _jumpAction;

    //メソッド関連
    public int Life()//ライフ参照用
    {
        return _life;
    }

    bool IsStun()//スタン硬直中かどうか
    {
        return _recoverTime > 0.0f || _life <= 0;//リカバータイムがある時かライフがゼロ以下の時
    }

    public void MoveToLeft()
    {
        if (IsStun()) return;
        if (_targetLane > MinLane)
        {
            _targetLane--;
            resetIntervalCol = StartCoroutine(ResetIntervalCol());
        }
    }
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (_targetLane < MaxLane)
        {
            _targetLane++;
            resetIntervalCol = StartCoroutine(ResetIntervalCol());
        }
    }
    //特定の時間(インターバル)だけ入力を待つコルーチン
    IEnumerator ResetIntervalCol()
    {
        yield return new WaitForSeconds(0.1f);//0.1秒待ってから
        resetIntervalCol = null;//コルーチン発動中である証明だった変数を解放
    }

    public void Jump()
    {
        if (IsStun()) return;
        if (controller.isGrounded)
        {
            _moveDirection.y = speedJump;
            //ジャンプトリガー設定
            //animator.SetTriger("Jump");
        }
    }

    //CharacterControllerに衝突判定が生じた時の処理
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        if (hit.gameObject.tag == "Enemy")
        {
            //ライフを減らしてノックバックさせスタンさせる
            LifeDown();//ライフ減少メソッド
            _moveDirection.z = knockBackForceZ;//ノックバックさせる
            _recoverTime = StunDuration;//スタン時間をリカバータイムに代入
            Debug.Log("HIT");
        }
    }
    public void LifeDown()//Life減少メソッド
    {
        _life--; 
        //UIControllerを探して、ライフ更新メソッド
        UIController controller = GameObject.FindAnyObjectByType<UIController>();
        controller.UpdateLife();

        //ライフが0以下になったら
        if (_life <= 0)
        {
            //ステータス変更
            GameManager.gameState = GameState.GameOver;
        }
    }

    //Life回復メソッド
    public void RecoverLife(int val)
    {
        _life += val;
        if (_life >= DefaultLife) _life = DefaultLife;
        //UIControllerを探して、ライフ更新メソッド
        UIController controller = GameObject.FindAnyObjectByType<UIController>();
        controller.UpdateLife();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネント取得
        controller = GetComponent<CharacterController>();
        //animator = animatorBody.GetComponent<Animator>();
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;//PlayerInputコンポーネントに設定されている現マップを取得する
        _moveAction = actionMap.FindAction("Move");
        _jumpAction = actionMap.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStun())//ダメージ中
        {
            float val = Mathf.Sin(Time.time * 50);//点滅させる
            if (val > 0)
            {
                animatorBody.SetActive(true);
            }
            else
            {
                animatorBody.SetActive(false);
            }
        }
        else
        {
            animatorBody.SetActive(true);
        }
        //入力検知系はアップデートに書いた方が良い
        //CharacterControllerコンポーネントが接地判断をもっている
        //入力検知は地面にいる時
        if (controller.isGrounded)
        {
            //上下キーによる前か後ろかの値の設定
            //if (Input.GetAxis("Vertical") != 0.0f)
            //if(moveAction.ReadValue<Vector2>().y != 0.0f)
            //{
            //    //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            //    moveDirection.z = moveAction.ReadValue<Vector2>().y * speedZ;
            //}
            //else
            //{
            //    moveDirection.z = 0;
            //}

            //横回転 Rotateメソッドで直接回転させる
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 300 * Time.deltaTime, 0);
            //transform.Rotate(0, moveAction.ReadValue<Vector2>().x * 300 * Time.deltaTime, 0);

            //コルーチンが発動していない時だけ入力処理
            if (resetIntervalCol == null)
            {
                if (_moveAction.ReadValue<Vector2>().x < 0) MoveToLeft(); //左のレーン番号に変更
                else if (_moveAction.ReadValue<Vector2>().x > 0) MoveToRight(); //右のレーン番号に変更
            }

            if (IsStun()) //硬直中なら何もできない
            {
                _moveDirection.x = 0.0f; //横方向には動けない
                //Z方向は0.05fずつノックバックが解消されるが、最大でも0で止まる
                _moveDirection.z = Mathf.Min(_moveDirection.z + 0.05f, 0.0f);

                _recoverTime -= Time.deltaTime; //カウントダウンする
            }
            else
            {
                //対象とするレーンをもとにmoveDirectionのX(横移動)を決める
                float ratioX = (_targetLane * LaneWidth - transform.position.x) / LaneWidth;
                _moveDirection.x = ratioX * speedX;

                //自動前進するためのmoveDirectionのZ(前進加速)を決める
                float acceleratedZ = _moveDirection.z + (accelerationZ * Time.deltaTime);
                _moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            }

            //ジャンプボタン　押されている間
            //if (Input.GetButton("Jump"))
            if (_jumpAction.IsPressed())
            {
                if (IsStun()) return; //スタン中は何も起こさない

                _moveDirection.y = speedJump; //y方向をセッティング
                //animator.SetTrigger("jump"); //ジャンプアニメへの切替
            }
        }

        //常に重力はかかっている
        _moveDirection.y -= gravity;

        //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
        //※実際にPlayerが動く Time.deltaTimeでPC性能差によらないように動作安定
        //回転を含む前進だと、グローバルな座標指定からズレるので、グローバルな座標になるように変換してからMoveに与える
        Vector3 globalDirection = transform.TransformDirection(_moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //地面にいるのであればY成分の力を毎回0にリセット
        if (controller.isGrounded) _moveDirection.y = 0;

        //アニメの制御
        //animator.SetBool("run", moveDirection.z != 0.0f);
    }
   
}
