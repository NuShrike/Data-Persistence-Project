using System.Collections;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using UnityEngine;

public class GameState : SingletonMonoBehavior<GameState>
{
    // NOTE: write doesn't work with the => accessor?
    //public uint HighScore => _state.HighScore;

    public uint HighScore {
        get { return _state.HighScore; }
        set { _state.HighScore = value; }
    }
    public string HighScorePlayerName {
        get { return _state.HighScore_PlayerName; }
        set { _state.HighScore_PlayerName = value; }
    }

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

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            var sessionState = JsonConvert.DeserializeObject<SessionState>(json);
            if (null != sessionState) {
                _state = sessionState;
            }
        }
        else {
            _state = new SessionState();
        }
    }

    public void SaveStateToStorage()
    {
        string json = JsonConvert.SerializeObject(_state);
        File.WriteAllText(Application.persistentDataPath + _saveFileName, json);
    }
}
