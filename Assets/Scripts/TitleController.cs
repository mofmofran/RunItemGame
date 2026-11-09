using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{    
    //ボタン関連
    public GameObject newGameButton;
    public GameObject loadGameButton;
    //シーン関連
    [Header("読み込みシーン")]
    public string newGameScene;
    public string loadGameScene;

    //メソッド関連
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);//シーン名に記載のシーンから始める
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
