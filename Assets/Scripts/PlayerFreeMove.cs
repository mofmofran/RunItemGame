using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFreeMove : MonoBehaviour
{
    CharacterController controller;
    [SerializeField]private GameObject animatorBody;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

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
             if (_moveAction.ReadValue<Vector2>().y != 0f)
            {
                //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
                moveDirection.z = _moveAction.ReadValue<Vector2>().y * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);
            transform.Rotate(0, _moveAction.ReadValue<Vector2>().x * 3, 0);

                //if (Input.GetButton("Jump"))
                if(_jumpAction.IsPressed())
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
}
