using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : UnitySingleton<TutorialManager>
{
    public GameObject tutorialCanvas;
    public GameObject firstBuyPanel;
    public GameObject secondBuyPanel;
    public GameObject showMergePanel;
    public GameObject showEquipPanel;
    public GameObject firstBuyButton;
    public GameObject secondBuyButton;
    public EquipmentSlot slotForFirstBuy;
    public EquipmentSlot slotForSecondBuy;
    private bool is0LevelTutorialPlayed;
    private bool isFirstMergeComplete = false;
    private bool isFirstEquipComplete = false;

    private void SetGameDatas()
    {
        GameManager.gameData.is0LevelTutorialPlayed = is0LevelTutorialPlayed;

    }
    private void LoadGameDatas()
    {
        is0LevelTutorialPlayed = GameManager.gameData.is0LevelTutorialPlayed;
    }
    private void OnEnable()
    {
        GameManager.loadGameData += LoadGameDatas;
        GameManager.setGameData += SetGameDatas;
    }
    private void OnDisable()
    {
        GameManager.loadGameData -= LoadGameDatas;
        GameManager.setGameData -= SetGameDatas;

    }
    private void Start()
    {
        GameManager.loadGameData += LoadGameDatas;
        GameManager.setGameData += SetGameDatas;
        if (GameManager.Instance.GetPlayerLevel() == 0)
        {
            if (GameManager.Instance.GetMoneyValue() < slotForFirstBuy.GetSlotCost() + slotForSecondBuy.GetSlotCost())
            {
                GameManager.Instance.EarnMoney(slotForFirstBuy.GetSlotCost());
                GameManager.Instance.EarnMoney(slotForSecondBuy.GetSlotCost());
            }
            GameManager.Instance.SetGameState(false);
            tutorialCanvas.SetActive(true);
            Start0LevelTutorial();

        }
    }

    #region 0 level tutorial

    private void Start0LevelTutorial()
    {
        firstBuyPanel.SetActive(true);
    }
    public void BuyFirstEquipment()
    {
        slotForFirstBuy.BuyEquipment();
        firstBuyPanel.SetActive(false);
        secondBuyPanel.SetActive(true);
    }
    public void BuySecondEquipment()
    {
        slotForSecondBuy.BuyEquipment();
        secondBuyPanel.SetActive(false);
        showMergePanel.SetActive(true);
        StartCoroutine("ControlPlayerForEquipSystem");
    }

    IEnumerator ControlPlayerForEquipSystem()
    {
        GameManager.Instance.SetGameState(true);
        while (!isFirstMergeComplete)
        {
            Debug.Log("Waiting for first merge");
            yield return null;
        }
        isFirstMergeComplete = true;
        showMergePanel.SetActive(false);
        showEquipPanel.SetActive(true);
        while (!isFirstEquipComplete)
        {
            Debug.Log("Waiting for first equip");
            yield return null;
        }
        isFirstEquipComplete = true;
        showEquipPanel.SetActive(false);
        is0LevelTutorialPlayed = true;
        tutorialCanvas.SetActive(false);

        yield return null;


    }

    public void SetFirstMergeState(bool value) => isFirstMergeComplete = value;
    public void SetFirstEquipState(bool value) => isFirstEquipComplete = value;

    #endregion


}
