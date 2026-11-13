using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TalkManager : MonoBehaviour
{
    public TalkData currentTalkData;//取り扱うデータベース

    //UI側のパーツ
    public GameObject talkPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public Image faceImage;
    public string nextScene;//次のシーン名
    public float textSpeed = 0.2f;//メッセージ速度


    private int _currentIndex = -1;//現在表示している会話の番号。－1から0にあげて始めるほうが都合がいい
    private string _currentFullText = "";//現在のテキストの保管
    private bool _isTyping = false;//文字送り中か
    private Coroutine _typingCoroutine;//文字送りコルーチン

    //ボタン入力
    private InputAction _submitAction;


    
    //メソッド関連
    public void AdvanceTalk()
    {
        if (currentTalkData == null || currentTalkData.talks.Count == 0)
        {
            return;
        }

        if (_isTyping)
        {
            //コルーチン止める
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            messageText.text = _currentFullText;
            _isTyping = false; //フラグオフ
            return;
        }

        _currentIndex++; //番号を1更新

        //対象としているTalkData(のもっているリスト)の数をIndexが上回ったら終了
        if (_currentIndex >= currentTalkData.talks.Count)
        {
            EndTalk(); //終了メソッド
            return;
        }

        //トークパネルが実在していれば
        if (talkPanel != null && talkPanel.activeSelf)
        {
            talkPanel.SetActive(true); //トークUIの表示
        }

        //対象データベースに記載されているリストのIndex番目を参照
        //SingleTalkクラスには名前・メッセージ・顔イラスト
        SingleTalk talk = currentTalkData.talks[_currentIndex];

        //データベースから抜き出したSingleTalkクラスのnameをUIに代入 ※空なら何も表示されない
        if (nameText != null) nameText.text = talk.name;

        if (faceImage != null)
        {
            //データベースから抜き出したSingleTalkクラスの顔イメージがあれば出す/なければ出さない
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage; //あれば差し替え nullならnull
        }

        _currentFullText = talk.message; //抜き出したSingleTalkクラスのメッセージを変数に
        //messageText.text = currentFullText; //メッセージUIに反省
        _typingCoroutine = StartCoroutine(TypeTextCoroutine(_currentFullText));
    }
    void EndTalk()
    {
        _currentIndex = -1; //再利用できるようにリセット

        if (talkPanel != null)
        {
            talkPanel.SetActive(false);
        }

        if (nextScene != "") //次のシーンの名前があれば
        {
            StartCoroutine(NextScene());
        }
    }

    //時間差で次のシーン
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator TypeTextCoroutine(string text)
    {
        _isTyping = true;
        messageText.text = ""; //まずリセット

        //引数に渡したメッセージ全文を分解
        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        _isTyping = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //現マップ(UI)の情報を取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        _submitAction = actionMap.FindAction("Submit"); //UIマップ上で定義されているSubmitアクションを参照

        //最初のメッセージ更新
        AdvanceTalk();
    }
    void Update()
    {
        if (_submitAction.WasPressedThisFrame())
        {
            //メッセージを更新
            AdvanceTalk();
        }
    }
}
