using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    IsPlay,
    GameClear,
    GameOver,
    GameEnd,
}
public class GameManager : MonoBehaviour
{
    public static GameState gameState;
    private GameObject _mixingCamera;

     public CinemachineCamera[] cameras;
    public PlayableDirector playableDirector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.IsPlay;
        //mixingカメラを探索
        //_mixingCamera = GameObject.FindGameObjectWithTag("MixngCamera");
        ////mixingカメラを非表示
        //_mixingCamera.SetActive(false);
        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState == GameState.GameOver)
        {
            Debug.Log("Gameover");
            //UIControllerを探し出してゲームオーバー用のUI表示命令
            UIController contoller = GameObject.FindAnyObjectByType<UIController>();
            contoller.GameOverDisplay();

            //_mixingCamera.SetActive(true); //mixingカメラを復活
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;//ブレインを復活

            playableDirector.Play();
            StartCoroutine(GameOverCamera());
                
            
            gameState = GameState.GameEnd;
            Debug.Log("GameEnd");
        }
       IEnumerator GameOverCamera()
        {
            yield return null;

            cameras[1].Priority = 2;
        }
        if (gameState == GameState.GameClear)
        {
            Debug.Log("GameClear");
            //UIControllerを探し出してゲームクリア用のUI表示命令
            UIController contoller = GameObject.FindAnyObjectByType<UIController>();
            contoller.GameClearDisplay();

            //_mixingCamera.SetActive(true); //mixingカメラを復活
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;//ブレインを復活

            playableDirector.Play();
            StartCoroutine(GameOverCamera());


            gameState = GameState.GameEnd;
            string name = SceneManager.GetActiveScene().name;
            if (!GameData.clearStages.Contains(name))
            {
                GameData.clearStages.Add(name);
            }
            Debug.Log("GameEnd");
        }
    }
}
