using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : UnitySingleton<GameManager>
{
    private float playerMoney = 500;
    [SerializeField] private TMP_Text moneyText;
    public void SpendMoney(float value) => playerMoney -= value;
    public float GetMoneyValue() => playerMoney;
    public void EarnMoney(float value) => playerMoney += value;
    [SerializeField] private Transform iconTransform;

    private void Update()
    {
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


}
