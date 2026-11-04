using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerSlash : MonoBehaviour
{
    public GameObject slashObject;//斬撃エフェクト
    InputAction _slashAction;//スラッシュアクションのための参照

    [Header("斬撃のクールタイム")]
    public float slashTime = 0.5f;

    Coroutine slashStart;

    void Slash()
    {
        if (slashStart == null)
        {
            slashObject.SetActive(true);
            slashStart = StartCoroutine(SlashStart());
        }
    }

    IEnumerator SlashStart()
    {
        yield return new WaitForSeconds(slashTime);
        slashObject.SetActive(false);
        slashStart = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slashObject.SetActive(false);
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        _slashAction = actionMap.FindAction("Crouch");
    }

    // Update is called once per frame
    void Update()
    {
        if (_slashAction.WasPerformedThisFrame())
        {
            Slash(); //スラッシュ攻撃メソッドの発動
        }
    }
}
