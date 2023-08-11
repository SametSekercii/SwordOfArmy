using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playerMoney;
    public int playerLevel;
    public int playerGoblet;
    public int lastScene;
    public bool is0LevelTutorialPlayed;

    public GameData()
    {
    
        lastScene = 0;
        playerMoney = 100;
        playerLevel = 0;
        playerGoblet = 0;
        is0LevelTutorialPlayed = false;

    }

}
