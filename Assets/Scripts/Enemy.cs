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
    private void OnTriggerEnter(Collider other)//何かに当たったら
    {
        if (_damageStart == null)//ダメージ中でなければ
        {
            if (other.gameObject.tag == "Bullet")//それがBulletならLifeを減らす
            {
                _enemyLife--;
                if (_enemyLife <= 0)
                { 
                    Destroy(gameObject);
                    CreateEffect(); 
                }//Lifeが0なら消滅
                else _damageStart = StartCoroutine(DamageStart());//そうでなければDamageStartコルーチン
            }
            else if (other.gameObject.tag == "Slash")
            {
                Destroy(this.gameObject);
                CreateEffect();
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
