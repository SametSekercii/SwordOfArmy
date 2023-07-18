using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FortController : MonoBehaviour
{
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    private int maxQueueSize;
    private int queueSize;
    public Fort fort;
    private float health;
    public Image healthBar;


    private void Start()
    {
        health = fort.health;
        healthBar.fillAmount = health / fort.health;
        maxQueueSize = transform.GetChild(0).childCount;
        soldiersInQueue = new GameObject[maxQueueSize];
        queuePoints = new Transform[maxQueueSize];
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            queuePoints[i] = transform.GetChild(0).GetChild(i);
        }
        queueSize = 0;
        if (transform.CompareTag("EnemyFort")) StartCoroutine("EnemyBehaviour");
        InvokeRepeating("BuyVikingSoldier", 3, 1);


    }

    public void BuyVikingSoldier()
    {
        if (queueSize < maxQueueSize)
        {
            defaultBlueVikingSoldier = Instantiate(defaultBlueVikingSoldierPrefab, spawnPoint.position, spawnPoint.rotation);
            defaultBlueVikingSoldier.transform.parent = transform;
            soldiersInQueue[maxQueueSize - queueSize - 1] = defaultBlueVikingSoldier;
            defaultBlueVikingSoldier.GetComponent<SoldierController>().SetQueueNumber(maxQueueSize - queueSize - 1);
            queueSize++;

        }



    }


    public int GetQueueAmount() => queueSize;
    public Transform GetQueuePoint(int index) => queuePoints[index];

    public void EquipSoldier(string itemName, float itemValue)
    {
        GameObject firstSoldier = soldiersInQueue[maxQueueSize - 1];
        firstSoldier.GetComponent<SoldierController>().TakeUpArms(itemName, itemValue);
        MoveTheQueue();

    }


    private void MoveTheQueue()
    {

        for (int i = 0; i < maxQueueSize; i++)
        {
            if (soldiersInQueue[i] != null && i != maxQueueSize - 1)
            {
                soldiersInQueue[i + 1] = soldiersInQueue[i];
                soldiersInQueue[i].GetComponent<SoldierController>().IncreaseQueueNumber();
                soldiersInQueue[i] = null;

            }
            else if (queueSize == 1) soldiersInQueue[i] = null;

        }
        queueSize--;
    }
    public bool CheckQueue()
    {
        if (queueSize > 0) return true;
        else return false;
    }

    IEnumerator EnemyBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            BuyVikingSoldier();
            yield return new WaitForSeconds(3);
            EquipSoldier("Level5Equipment", 50);
        }


    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / fort.health;
    }
}
