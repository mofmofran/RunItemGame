using UnityEngine;
public enum ItemType
{
    Life,
    Bullet,}

public class Item : MonoBehaviour
{
    public ItemType type;
    [SerializeField]private int _value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (type) 
            {
                case ItemType.Life:
                    other.gameObject.GetComponent<PlayerRun>().RecoverLife(_value);
                    //FindAnyObjectByType<UIController>().
                    break;

                    case ItemType.Bullet:
                    other.gameObject.GetComponent<PlayerShooter>().AddBullet(_value);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
