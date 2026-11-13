using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _deleteTime = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject,_deleteTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameData.HasUpgrade(UpgradeType.PierceBullet))
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameData.HasUpgrade(UpgradeType.PierceBullet))
        {
            return;
        }
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
