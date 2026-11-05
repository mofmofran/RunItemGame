using UnityEngine;

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
    public static GameState gamaState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamaState = GameState.IsPlay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
