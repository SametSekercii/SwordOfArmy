using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : UnitySingleton<GameManager>
{

    public enum Winner { Player, Enemy }
    public static GameData gameData;
    public static event Action SetGameData;
    public static event Action LoadGameData;
    public Winner gameWinner;
    private float playerMoney;
    private int playerLevel = 0;
    private int lastScene = 0;
    private int playerGoblet;
    private bool isGameOver = false;
    private bool isGameGoing = true;
    [SerializeField] private int soldierEquipped = 0;
    private int mergedEquipment = 0;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text gobletText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text winPanelGobletText;
    [SerializeField] private Transform moneyIconTransform;
    [SerializeField] private Transform gobletIconTransform;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject playerPathObject;
    [SerializeField] private GameObject enemyPathObject;
    [SerializeField] private GameObject playerFortObject;
    [SerializeField] private GameObject enemyFortObject;

    private void OnEnable()
    {
        LoadGameData += LoadGameDatas;
        SetGameData += SetGameDatas;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        gameData = new GameData();
        gameData = SaveSystem.Load(gameData);
        LoadGameData?.Invoke();
        if (SceneManager.GetActiveScene().buildIndex != lastScene) SceneManager.LoadScene(lastScene);
        isGameOver = false;
        isGameGoing = true;
    }

    private void SetGameDatas()
    {
        gameData.playerMoney = playerMoney;
        gameData.playerLevel = playerLevel;
        gameData.playerGoblet = playerGoblet;
        gameData.lastScene = lastScene;
    }
    private void LoadGameDatas()
    {
        playerMoney = gameData.playerMoney;
        playerLevel = gameData.playerLevel;
        playerGoblet = gameData.playerGoblet;
        lastScene = gameData.lastScene;
    }

    private void Update()
    {
        SetGameData?.Invoke();
        SaveSystem.Save(gameData);
        moneyText.text = Mathf.RoundToInt(playerMoney).ToString();
        gobletText.text = playerGoblet.ToString();
        levelText.text = "LEVEL" + playerLevel.ToString();
        lastScene = SceneManager.GetActiveScene().buildIndex;


    }


    public Transform GetMoneyIconTransform() => moneyIconTransform;
    public Transform GetGobletIconTransform() => gobletIconTransform;
    public bool IsGameOver() => isGameOver;
    public bool IsGameGoing() => isGameGoing;
    public Winner GetGameWinner() => gameWinner;
    public void SetGameState(bool value) => isGameGoing = value;


    public void FinishGame()
    {
        isGameOver = true;
        if (gameWinner == Winner.Player)
        {
            winPanel.SetActive(true);
            playerGoblet += 90;
            winPanelGobletText.text = 90.ToString();
            WinRewardManager.Instance.StartRewardingGoblet(90);

        }
        else failPanel.SetActive(true);
    }


    public void IncreaseLevel() => playerLevel++;
    public void SpendMoney(float value) => playerMoney -= value;
    public float GetMoneyValue() => playerMoney;
    public void EarnMoney(float value) => playerMoney += value;
    public int GetPlayerLevel() => playerLevel;
    public void IncreaseMergedEquipment() => mergedEquipment++;
    public void IncreaseEquippedSoldier() => soldierEquipped++;
    public int GetMergedEquipment() => mergedEquipment;
    public int GetEquippedSoldier() => soldierEquipped;
    public int GetLastScene() => lastScene;
    public GameObject GetPlayerPathObject() => playerPathObject;
    public GameObject GetEnemyPathObject() => enemyPathObject;
    public GameObject GetPlayerFortObject() => playerFortObject;
    public GameObject GetEnemyFortObject() => enemyFortObject;
    public void unlimitedmoney()
    {
        playerMoney = 999999999;
    }
    public void limitedmoney()
    {
        playerMoney = 500;
    }


}
