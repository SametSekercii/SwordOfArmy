using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : UnitySingleton<GameManager>
{
    public enum Winner { Player, Enemy }
    public static GameData gameData;
    public static event Action setGameData;
    public static event Action loadGameData;
    public Winner gameWinner;
    private float playerMoney = 0;
    private int playerLevel = 0;
    private int playerGoblet = 0;
    private bool isGameOver = false;
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private Transform iconTransform;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;

    private void Start()
    {
        loadGameData += LoadGameDatas;
        setGameData += SetGameDatas;
        gameData = new GameData();
        gameData = SaveSystem.Load(gameData);
        loadGameData?.Invoke();
        isGameOver = false;
    }
    private void SetGameDatas()
    {
        gameData.playerMoney = playerMoney;
        gameData.playerLevel = playerLevel;
        gameData.playerGoblet = playerGoblet;
    }
    private void LoadGameDatas()
    {
        playerMoney = gameData.playerMoney;
        playerLevel = gameData.playerLevel;
        playerGoblet = gameData.playerGoblet;
    }

    private void Update()
    {
        setGameData?.Invoke();
        SaveSystem.Save(gameData);
        moneyText.text = Mathf.RoundToInt(playerMoney).ToString();
    }

    public void unlimitedmoney()
    {
        playerMoney = 999999999;
    }
    public void limitedmoney()
    {
        playerMoney = 500;
    }
    public Transform GetMoneyIconTransform() => iconTransform;
    public bool IsGameOver() => isGameOver;
    public Winner GetGameWinner() => gameWinner;

    public void FinishGame()
    {
        isGameOver = true;
        if (gameWinner == Winner.Player) winPanel.SetActive(true);
        else failPanel.SetActive(true);
    }
    public void SpendMoney(float value) => playerMoney -= value;
    public float GetMoneyValue() => playerMoney;
    public void EarnMoney(float value) => playerMoney += value;


}
