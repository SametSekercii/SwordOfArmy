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

    private void Update()
    {
        moneyText.text = Mathf.RoundToInt(playerMoney).ToString();
    }





}
