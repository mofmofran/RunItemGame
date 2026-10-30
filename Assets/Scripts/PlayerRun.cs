using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1f;
    CharacterController controller;
    [SerializeField] private GameObject animatorBody;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;
    private int _targetLane;

    [Header("重力・前進・横移動・ジャンプ")]
    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;

    [Header("前進加速力")]
    public float accelerationZ = 10.0f;

    Coroutine resetIntervalCol;//インプットインターバルを置くための変数

    private InputAction _moveAction;
    private InputAction _jumpAction;

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
        if (controller.isGrounded)
        {
            //キャラクターコントローラーや入力関連はアップデートに書いた方が安定する
            //if (Input.GetAxis("Vertical") != 0f)
            //if (_moveAction.ReadValue<Vector2>().y != 0f)
            //{
            //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            //   moveDirection.z = _moveAction.ReadValue<Vector2>().y * speedZ;
            //}
            //else
            // {
            //  moveDirection.z = 0;
            //}
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
            //transform.Rotate(0, _moveAction.ReadValue<Vector2>().x * 3, 0);
            
            //徐々に加速しZ方向に前進させる
            float acceleratedZ = moveDirection.z+(accelerationZ*Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);//Clamp(変数,下限,上限)

            if (resetIntervalCol == null)//コルーチンが発動していない時だけ
            {
                if (_moveAction.ReadValue<Vector2>().x < 0)MoveToLeft();
                
                else if (_moveAction.ReadValue<Vector2>().x > 0) MoveToRight();
              
            }
            float ratioX = (_targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX *speedX;

            //if (Input.GetButton("Jump"))
            if (_jumpAction.IsPressed())
            {
                moveDirection.y = speedJump;
                //animator.SetTrigger("jump");
            }

        }            //重力分の力を毎フレーム追加
        moveDirection.y -= gravity;// * Time.deltaTimeは片方でいい
                                   //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);//グローバル座標になってしまうのでローカル座標に変換するメソッド
        controller.Move(globalDirection * Time.deltaTime);
        //移動後設置してたらY方向の速度はリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする
        //animator.SetBool("run", moveDirection.z != 0.0f);
    }
    public void MoveToLeft()
    {
        if (_targetLane > MinLane)
        {
            _targetLane--;
            resetIntervalCol = StartCoroutine(ResetIntervalCol());
        }
    }
    public void MoveToRight()
    {
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
        if(controller.isGrounded)
        {
            moveDirection.y = speedJump;
            //ジャンプトリガー設定
            //animator.SetTriger("Jump");
        }
    }
}
