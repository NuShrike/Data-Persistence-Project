using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class HighScoresManager : MonoBehaviour
{
    public GameObject HighScoreLinePrefab;

    public uint MaxNumScores = 4;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHighScoreDisplay();
    }

    // NOTE: currentPoints could be negative depending on brick scoring
    public bool UpdateHighScores(int currentPoints, string currentName)
    {
        // NOTE: GameState is singleton that auto-loads scores at app-startup
        var hsList = GameState.Instance.HighScoresList;
        bool listModified = false;

#pragma warning disable CS0162 // Unreachable code detected
        if (false)
        {
            bool addScore = false;
            if (0 != hsList.Count)
            {
                IList<uint> score = hsList.Keys;

                // reverse-iterate through sorted highScores data-model and populate it
                for(int scoreIndex = 0; scoreIndex < hsList.Count; ++scoreIndex)
                {
                    if (score[scoreIndex] < currentPoints)
                    {   // NOTE: cannot support multiple scores of equivalent value
                        addScore = true;
                        break;
                    }
                }

                if (!addScore) {
                    if (0 < currentPoints)
                    {   // add non-zero score anyways
                        addScore = true;
                    }
                }
            }
            else {  // initial empty scoreList special case
                addScore = true;
            }

            if (addScore) {
                listModified = hsList.AddScore((uint)currentPoints, currentName);
            }
        }
#pragma warning restore CS0162 // Unreachable code detected
        else {
            listModified = hsList.AddScore((uint)currentPoints, currentName);
        }

        if (listModified)
        {
            while (MaxNumScores < (uint)hsList.Count)
            {   // trim highscore list size
                hsList.RemoveAt(hsList.Count - 1);
            }

            UpdateHighScoreDisplay();
        }

        return listModified;
    }

    public void UpdateHighScoreDisplay()
    {
        // NOTE: GameState is singleton that auto-loads scores at app-startup
        var hsList = GameState.Instance.HighScoresList;
        IList<uint> score = hsList.Keys;
        IList<string> name = hsList.Values;
        
        var container = transform;

        Transform hsLine;
        int childNum = container.childCount;
        int childIndex = 0;

        uint scoreCount = 0;
        bool lineUpdated;

        // iterate through sorted highScores data-model and build/refresh visual-model
        for(int scoreIndex = 0; scoreIndex < hsList.Count; ++scoreIndex)
        {
            lineUpdated = false;

            // update existing visual item
            for(; childIndex < childNum; ++childIndex )
            {   // find first "active" child
                hsLine = container.GetChild(childIndex);

                if (hsLine.gameObject.activeSelf) {
                    UpdateScoreComponents(hsLine, score[scoreIndex], name[scoreIndex]);
                    lineUpdated = true;
                    ++childIndex;
                    break;
                }
                else {  // ignore inactive-debug items
                    continue;
                }
            }

            if (!lineUpdated)
            {   // create new visual item
                hsLine = Instantiate(HighScoreLinePrefab).transform;
                UpdateScoreComponents(hsLine, score[scoreIndex], name[scoreIndex]);
                hsLine.SetParent(transform, false);
            }

            if (MaxNumScores < ++scoreCount)
            {   // do not add more than allowed number of highscores to track
                break;
            }
        }
    }

    private void UpdateScoreComponents(Transform line, uint scoreNum, string scorePlayerName)
    {
        foreach(Transform component in line)
        {
            switch (component.name)
            {
            case "Score": {
                TextMeshProUGUI score = component.GetComponent<TextMeshProUGUI>();
                if (null != score) {
                    score.text = $"{scoreNum:0000000}";
                    //score.text = "0123456";
                }
            }
                break;
            case "Name": {
                TextMeshProUGUI name = component.GetComponent<TextMeshProUGUI>();
                if (null != name) {
                    name.text = scorePlayerName;
                    //var testString = "AABBCCDDEEFFGGHHIIJJKK";
                    //name.text = $"{testString,12}"; 
                }
            }
                break;

            default:
                break;
            }
        }
    }
}
internal class HighScoresList : SortedList<uint, string>
{
    public HighScoresList()
        : base(1, new DescendingComparer())
    {
    }

    private class DescendingComparer : IComparer<uint>
    {
        public int Compare(uint x, uint y) {
            return y.CompareTo(x);
        }
    }

    public bool AddScore(uint score, string playerName)
    {
        try {
            Add(score, playerName);
            return true;
        }
        catch (ArgumentException) {
            // duplicate key, so ignore
        }
        return false;
    }
}
