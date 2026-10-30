using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff;
    GameObject player;
    public float followSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        diff = player.transform.position - transform.position;  //維持する距離を定義する
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //現在地から目的地にどんどん詰めていき、最終的に近づき続ける。
        //イコールにはならないが描画上は止まっているかのように表現される
        transform.position = Vector3.Lerp(
            transform.position,//現在地
            player.transform.position-diff,//目標の位置から維持する距離を引いた数値
            Time.deltaTime*followSpeed );//前フレームからの時間にfollowSpeedを掛けた進捗率を出す
    }
}
