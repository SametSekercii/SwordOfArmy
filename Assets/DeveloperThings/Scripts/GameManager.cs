using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : UnitySingleton<GameManager>
{
    [SerializeField] private float playerMoney = 9999999;
    public void SpendMoney(float value) => playerMoney -= value;
    public float GetMoneyValue() => playerMoney;
    public void EarnMoney(float value) => playerMoney += value;

}
