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
        Cursor.visible = true;
        Application.targetFrameRate = 30;

        NameInput.onEndEdit.AddListener(OnEndEdit);
        NameInput.text = GameState.Instance.PlayerName;
    }

    public void OnEndEdit(string playerName)
    {
        GameState.Instance.PlayerName = playerName;
    }

    public void OnStart()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.PlayerName))
        {   // require name-entry before game can start
            SceneManager.LoadScene("main");
        }
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
