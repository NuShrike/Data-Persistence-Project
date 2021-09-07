using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMPro.TMP_InputField NameInput;

    // Start is called before the first frame update
    void Start()
    {
        NameInput.onEndEdit.AddListener(OnEndEdit);

        NameInput.text = GameState.Instance.PlayerName;
    }

    public void OnEndEdit(string playerName)
    {
        GameState.Instance.PlayerName = playerName;
    }

    public void OnStart()
    {
        SceneManager.LoadScene("main");
    }

    public void OnQuit()
    {
        GameState.Instance.SaveStateToStorage();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
