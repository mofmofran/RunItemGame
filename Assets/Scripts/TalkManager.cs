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
    private Coroutine _typingCorutine;//文字送りコルーチン

    //ボタン入力
    private InputAction _submitAction;


    
    //メソッド関連
    public void AdvanceTalk()
    {
        if (currentTalkData == null || currentTalkData.talks.Count == 0) return;//中身がない場合、参照できない数値の場合は返す
        if (_isTyping == true)//文字送り中
        {
            if(_typingCorutine != null)
            {
                StopCoroutine(_typingCorutine);//コルーチンを止める
                _isTyping=false;//フラグをオフ
                messageText.text = _currentFullText;
                return;
            }
        }
        _currentIndex++;
        if (_currentIndex >= currentTalkData.talks.Count)//対象としているTalkDataのリスト数をIndexが上回ったら終了 
        {
            EndTalk();
            return;
        }
        if (talkPanel != null && talkPanel.activeSelf)
        {
            talkPanel.SetActive(true);//トークUIの表示                                          
        }
        SingleTalk talk = currentTalkData.talks[_currentIndex];//対象データベースの_currentIndex番を表示する
        //SingleTalkクラスには名前・メッセージ・顔イラストが格納されている
        if (nameText!=null) nameText.text = talk.name;//データベースから抜き出したSingkeTalkクラスのnameをUIに代入※空なら何も表示しない
        if (faceImage != null)//データベースから抜き出した顔イメージがあれば出す
        {
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage;//あれば差し替えnullならnull
        }
        _currentFullText = talk.message;//データベースから抜き出したメッセージを変数に一度格納
        messageText.text = _currentFullText;//メッセージUIに
        _typingCorutine = StartCoroutine(TypeTextCoroutine(_currentFullText));
    }
    public void EndTalk()
    {
        _currentIndex = -1;//再利用できるようにリセット
        if (talkPanel != null) talkPanel.SetActive(false);//トークパネルがあるなら消す
        if(nextScene != "")//nextScene
        {
            StartCoroutine(NextScene());
        }
    }

    IEnumerator TypeTextCoroutine(string text)
    {
        _isTyping = true;
        messageText.text = "";//まずリセット
        foreach(char c in text)//引数に渡したメッセージ全文を分解
        {
            messageText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        _isTyping=false;
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextScene);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputActionMap actionMap = GetComponent < PlayerInput >().currentActionMap;
        _submitAction = actionMap.FindAction("Submit");//UIマップ上のサブミットを検知
        //最初のメッセージ更新
        AdvanceTalk();
    }

    // Update is called once per frame
    void Update()
    {
        if (_submitAction.WasPressedThisFrame())
        {
            //メッセージを更新
            AdvanceTalk() ;
        }
    }
}
