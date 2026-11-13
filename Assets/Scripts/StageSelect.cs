using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public string nextScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameData.clearStages.Contains(nextScene))
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "★";
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "☆";
        }
        //行き先のシーンの名前を入れる
        GetComponentInChildren<TextMeshProUGUI>().text += nextScene;
    }

    public void Load()
    {
        SceneManager.LoadScene(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
