using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playerMoney;
    public int playerLevel;
    public int playerGoblet;

    public GameData()
    {
        playerMoney = 200;
        playerLevel = 0;
        playerGoblet = 0;

    }

}
