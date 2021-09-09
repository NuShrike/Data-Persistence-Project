using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using System.Threading;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 8;
    public Rigidbody Ball;

    public GameObject GameOverText;
    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI HighScoreName;

    public AudioSource AudioSource;
    public AudioClip StartGameClip;
    public AudioClip WinGameClip;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
#if UNITY_EDITOR
    private bool _logBricks;
#endif

    private int brickCount;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

#if UNITY_EDITOR
        if (string.IsNullOrEmpty(GameState.Instance.PlayerName))
        {   // force back to MENU to acquire a playerName
            SceneManager.LoadScene("menu");
        }
#endif
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5,6,6};

        brickCount = 0;

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                ++brickCount;
            }
        }

        Cursor.visible = false;
        _logBricks = false;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = 60;

                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                // detach Ball from Paddle and launch it
                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);

                AudioSource.PlayOneShot(StartGameClip);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
            SceneManager.LoadScene("menu");
        }
    }

    void AddPoint(int point)
    {
        Interlocked.Add(ref m_Points, point);
        Interlocked.Decrement(ref brickCount);

        ScoreText.text = $"{m_Points:0000000}";
#if UNITY_EDITOR
        if (_logBricks) {
            Debug.Log("=== brickCount: " + brickCount);
        }
#endif
        if (0 >= brickCount)
        {   // stop game when all bricks are destroyed
            Brick[] bricks = FindObjectsOfType<Brick>();
            var bricksNum = bricks.Length;
            if (1 < bricksNum)
            {   // attempt to patch weird bug where brickCount isn't decremented correctly
#if UNITY_EDITOR
                Debug.Log("<<< brickCount: " + brickCount + " currently: " + bricksNum);
#endif
            }
            else {
                AudioSource.PlayOneShot(WinGameClip);
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        Ball ball = FindObjectOfType<Ball>();
        if (null != ball)
        {   // stop the ball once all bricks are done
            Destroy(ball.gameObject);
        }

        m_GameOver = true;
        GameOverText.SetActive(true);

        UpdateHighScore();

        Cursor.visible = true;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    void UpdateHighScore()
    {
        var gameState = GameState.Instance;
        HighScoresManager highScoresManager = FindObjectOfType<HighScoresManager>();

        if (highScoresManager.UpdateHighScores(m_Points, gameState.PlayerName))
        {
            gameState.SaveStateToStorage();
        }

    }
}
