using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //ゲームのステータス用のテキスト
    public GameObject statusText;

    //スライダー関連
    public GameObject sliderPanel;
    public Slider lifeSlider;

    //バレット関連
    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;

    //ボタン関連
    public GameObject buttonPanel;
    public GameObject retryButton;
    public GameObject nextButton;

    //シーン関連
    public string nextScene;

    //メソッド関連

    public void UpdateLife()    //ライフUIを更新
    {
        //PlayerRunスクリプトを参照できるようにする
        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();

        //残Lifeをスライダー値に代入
        lifeSlider.value = player.Life();
    }

 
    public void UpdateBullet()   //Bulletの残弾数を更新
    {
        //PlayerShooterスクリプトを参照できるようにする
        PlayerShooter player = GameObject.FindAnyObjectByType<PlayerShooter>();

        //残弾を文字列に変換してテキストに代入
        bulletText.text = player.GetRemaining().ToString();
    }

    
    public void Retry()//リトライボタン
    {
        //現シーンの情報を取得
        string currentScene = SceneManager.GetActiveScene().name;
        //取得した現シーンを読み直す
        SceneManager.LoadScene(currentScene);
    }

    
    public void GameOverDisplay()//ゲームオーバー時、GameManagerから呼び出されるメソッド
    {
        buttonPanel.SetActive(true); //ボタンの表示
        retryButton.SetActive(true);
        //ステータス「GAME OVER」の表示
        statusText.SetActive(true);
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME OVER";

        sliderPanel.SetActive(false); //スライダーパネルの非表示
        bulletPanel.SetActive(false); //Bulletパネルの非表示
    }

    public void GameClearDisplay()
    {
        buttonPanel.SetActive(true); //ボタンの表示
        nextButton.SetActive(true);
        //ステータス「GAME Clear」の表示
        statusText.SetActive(true);
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME CLEAR";

        sliderPanel.SetActive(false); //スライダーパネルの非表示
        bulletPanel.SetActive(false); //Bulletパネルの非表示

    }

    void Start()
    {
        //ボタンを非表示
        buttonPanel.SetActive(false);
        retryButton.SetActive(false);
        nextButton.SetActive(false);

        //時間差でステータス文字を非表示
        StartCoroutine(DisplayChange());

        UpdateLife(); //プレイヤー体力を反映
        UpdateBullet(); //残弾数を反映
    }

    //時間差でステータス文字を非表示
    IEnumerator DisplayChange()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

   
}
