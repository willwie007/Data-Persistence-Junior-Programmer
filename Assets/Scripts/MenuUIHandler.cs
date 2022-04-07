using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
# if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    
    [SerializeField] TMP_Text playerNameInput;


    // Start is called before the first frame update
    void Start()
    {
        //playerNameInput = GetComponent<TMP_InputField>().text;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayerNameSet()
    {
        PlayerDataHandler.Instance.playerName = playerNameInput.text;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}
