using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public float deleteTime =1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
