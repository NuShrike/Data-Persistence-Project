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
        //NameInput.onSubmit.AddListener(OnSubmit);
        //NameInput.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void OnValueChanged(string text)
    //{
    //    Debug.Log("OnValueChanged: " + text);
    //}

    public void OnEndEdit(string text)
    {
        Debug.Log("OnEndEdit: " + text);
    }

    //public void OnSubmit(string text)
    //{
    //    Debug.Log("OnSubmit: " + text);
    //}

    public void OnStart()
    {
        SceneManager.LoadScene("main");
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
