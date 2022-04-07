using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text playerName; //variable to store player name
    public Text bestPlayerName; // variable to store best player upon getting high score
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private static int bestScore;
    private static string bestPlayer;


    void Awake()
    {
        PlayerGameRank();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        playerName.text = PlayerDataHandler.Instance.playerName;

        BestPlayerSet();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandler.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        BestPlayerCheck();
        GameOverText.SetActive(true);
        
    }

    public void PlayerGameRank()
    {
        string path = Application.persistentDataPath + "savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.TheBestPlayer;
            bestScore = data.HighestScore;
        }
    }

    // condition to check and set score as best score and best player
    private void BestPlayerCheck()
    {
        int gameScore = PlayerDataHandler.Instance.score;

        if (gameScore > bestScore)
        {
            bestPlayer = PlayerDataHandler.Instance.playerName;
            bestScore = gameScore;

            bestPlayerName.text = $"Best Score : {bestPlayer}: {bestScore}";

            SaveGameRank(bestPlayer, bestScore);
        }
    }

    //here we check player againt player score and decide to save player name as best or not
    private void BestPlayerSet()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            bestPlayerName.text = "";
        }
        else
        {
            bestPlayerName.text = $"Best Score : {bestPlayer}: {bestScore}";
        }
    }

    // here we check the player and score of game play and save
    public void SaveGameRank(string nameOfBestPlayer, int playerWithBestScore)
    {
        SaveData data = new SaveData();

        data.TheBestPlayer = nameOfBestPlayer;
        data.HighestScore = playerWithBestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


    [System.Serializable]
    class SaveData
    {
        public int HighestScore;
        public string TheBestPlayer;
    }
}
