using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("生成される弾")]
    public GameObject bullet;

    InputAction _shootAction; //Attackアクションボタンの割り振り

    Coroutine _shootStart; //シュートされているかどうかを知るための変数

    [Header("弾速・クールタイム")]
    public float bulletSpeed = 10f;
    public float shootInterval = 0.3f;

    private Transform _gateTransform;

    const int StartRemaining = 10;

    private int _bulletRemaining = StartRemaining;

    //メソッド関連
    public int GetRemaining()//弾数を参照するメソッド
    {
        return _bulletRemaining;
    }

    public void AddBullet(int num)//弾数を回復するメソッド
    {
        _bulletRemaining += num;
    }

    void Start()
    {
        //PlayerInputコンポーネントに設定されている現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        //現マップ上にいる「Attack」というアクションを取得
        _shootAction = actionMap.FindAction("Attack");
        _gateTransform = transform.Find("Gate");
    }

    // Update is called once per frame
    void Update()
    {
        //Attackアクションボタンが押された時 Shootメソッド
        if (_shootAction.WasPressedThisFrame()&&_bulletRemaining>0) Shoot();
    }

    void Shoot()
    {
        if (_shootStart == null)
        {
            //攻撃開始Bulletプレハブの生成とＺ軸前方へシュート

            GameObject obj = Instantiate(bullet, _gateTransform.transform.position, Quaternion.identity);
            Rigidbody rbody = obj.GetComponent<Rigidbody>();
            rbody.AddForce(new Vector3(0, 0, bulletSpeed), ForceMode.Impulse);
            _bulletRemaining--;
            //インターバルとなるコルーチンの開始
            _shootStart = StartCoroutine(ShootStart());
        }
    }

    //インターバル用コルーチン
    IEnumerator ShootStart()
    {
        //0.5ｆ秒待つ
        yield return new WaitForSeconds(shootInterval);
        //コルーチン変数の解放
        _shootStart = null;
    }
}