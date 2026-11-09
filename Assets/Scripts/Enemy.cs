using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _enemyLife = 3;
    public GameObject enemyBody;
    public float freezeTime =1.0f;
    Coroutine _damageStart;
    public GameObject effectObject; 


    void CreateEffect()
    {
        Instantiate(
            effectObject,
            transform.position,
            Quaternion.identity);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_damageStart != null)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                enemyBody.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                enemyBody.GetComponent<MeshRenderer>().enabled = false;
            }
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //ダメージコルーチンが発動していなければ（ダメージ中でなければ）
        if (_damageStart == null)
        {
            //ぶつかった相手がBulletなら
            if (other.gameObject.tag == "Bullet")
            {
                _enemyLife--; //体力が1減る
                if (_enemyLife <= 0)
                {
                    CreateEffect();
                    Destroy(gameObject); //消滅
                }
                else //体力が残っていれば
                {
                    _damageStart = StartCoroutine(DamageStart()); //コルーチンの発動
                }
            }
            //ぶつかった相手がSlashなら即撃破
            else if (other.gameObject.tag == "Slash")
            {
                CreateEffect();
                Destroy(gameObject);
                Debug.Log("Slash!");
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_damageStart == null)//ダメージ中でなければ
        {
            if (collision.gameObject.tag == "Bullet")//それがBulletならLifeを減らす
            {
                _enemyLife--;
                if (_enemyLife <= 0) Destroy(gameObject);//Lifeが0なら消滅
                else _damageStart = StartCoroutine(DamageStart());//そうでなければDamageStartコルーチン
            }
        }
    }
    IEnumerator DamageStart()
    {
        yield return new WaitForSeconds(freezeTime);//freezeTimeだけ待ってnullを返す
        _damageStart = null;
        enemyBody.GetComponent<MeshRenderer>().enabled = true;
    }
}
