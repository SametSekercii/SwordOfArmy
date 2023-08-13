using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playerMoney;
    public int playerLevel;
    public int playerGoblet;
    public int lastSceneIndex;



    public GameData()
    {
        playerLevel = 1;
        lastSceneIndex = 0;
        playerMoney = 0;
        playerGoblet = 0;
    }

}
