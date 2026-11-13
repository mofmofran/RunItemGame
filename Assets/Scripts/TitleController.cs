using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{    
    //ボタン関連
    public GameObject newGameButton;
    public GameObject loadGameButton;
    //シーン関連
    [Header("読み込みシーン")]
    public string newGameScene;
    public string loadGameScene;
    public UpgradeData[] allUpgrades;

    //メソッド関連
    public void NewGame()
    {
        GameData.ResetData();
        SceneManager.LoadScene(newGameScene);//シーン名に記載のシーンから始める

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(loadGameScene);
        GameData.Load(allUpgrades);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!GameData.HasSave())
        {
            loadGameButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
