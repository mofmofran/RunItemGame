using UnityEngine;

public class Goal : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//Playerがぶつかったら
        {
            GameManager.gameState = GameState.GameClear;//ゲームをクリアにする
            Debug.Log("Clear");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
