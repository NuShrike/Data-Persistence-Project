using System.IO;

using Newtonsoft.Json;

using UnityEngine;

public class GameState : SingletonMonoBehavior<GameState>
{
    internal HighScoresList HighScoresList { get => _state.hsList; }

    private const string _saveFileName = "/sessionState.json";

    private string _playerName;
    internal SessionState _state;

    public string PlayerName {
        get { return _playerName; }
        set {
            if (!string.IsNullOrEmpty(value)) {
                _playerName = value;
            }
        }
    }

    protected override void LoadSessionState()
    {
        LoadStateFromStorage();
    }

    public void LoadStateFromStorage()
    {
        string path = Application.persistentDataPath + _saveFileName;

        // delete file whenever data-format changes
#pragma warning disable CS0162 // Unreachable code detected
        if (false) {
            File.Delete(path);
        }
#pragma warning restore CS0162 // Unreachable code detected

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            //Debug.Log("json: " + json);
            var sessionState = JsonConvert.DeserializeObject<SessionState>(json);
            if (null != sessionState) {
                _state = sessionState;
            }
        }
        else {
            _state = new SessionState().Init();
        }
    }

    public void SaveStateToStorage()
    {
        string json = JsonConvert.SerializeObject(_state);
        //Debug.Log("SaveStateToStorage: " + json);
        File.WriteAllText(Application.persistentDataPath + _saveFileName, json);
    }
}
