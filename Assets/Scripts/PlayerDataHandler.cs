using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    // Class to save player data on game play

    public static PlayerDataHandler Instance;
    public string playerName;
    public int score;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


}
