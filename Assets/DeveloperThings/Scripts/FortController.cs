using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FortController : UnitySingleton<FortController>
{
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    private int maxQueueSize;
    private int queueSize;
    private float soldierCost;
    private int fortLevel = 1;

    

    private void Start()
    {
        maxQueueSize = transform.GetChild(0).childCount;
        soldiersInQueue = new GameObject[maxQueueSize];
        queuePoints = new Transform[maxQueueSize];
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            queuePoints[i] = transform.GetChild(0).GetChild(i);
        }
        queueSize = 0;
        InvokeRepeating("BuyVikingSoldier", 10, 1);




    }
    private void OnEnable()
    {
        StartCoroutine("GainMoneyRegular");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
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

    IEnumerator GainMoneyRegular()

    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            GameManager.Instance.EarnMoney(5 * fortLevel);

        }
    }
    public int GetQueueAmount() => queueSize;
    public Transform GetQueuePoint(int index) => queuePoints[index];

    public void EquipSoldier(int itemId)
    {


        SendToWar();
    }
    private void SendToWar()
    {
        GameObject firstSoldier = soldiersInQueue[maxQueueSize - 1];
        firstSoldier.GetComponent<SoldierController>().SetStateInWar();
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
}
