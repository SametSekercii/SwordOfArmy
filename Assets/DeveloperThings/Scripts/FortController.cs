using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FortController : MonoBehaviour
{
    private string[] equipmentNames = { "null", "Level1Equipment", "Level2Equipment", "Level3Equipment", "Level4Equipment", "Level5Equipment", "Level6Equipment" };
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    private int maxQueueSize;
    private int queueSize;
    public Fort fort;
    private float health;
    private float coolDown;
    public Image healthBar;
    public Image coolDownBar;


    private void Start()
    {
        coolDown = fort.coolDown;
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

        if (transform.CompareTag("EnemyFort"))
        {
            StartCoroutine("EnemyFortSpawner");

        }
        if (transform.CompareTag("PlayerFort"))
        {
            StartCoroutine("PlayerFortBehaviour");
        }



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
        if (firstSoldier != null)
        {
            firstSoldier.GetComponent<SoldierController>().TakeUpArms(itemName, itemValue);
            MoveTheQueue();

        }

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

    IEnumerator EnemyEquiper()
    {
        int counter = 3;


        yield return new WaitForSeconds(Random.Range(1, 3));
        if (counter > 0)
        {
            int rand = Random.Range(1, 4);
            EquipSoldier(equipmentNames[rand], rand * 10);
            yield return null;
            counter--;

        }
        else
        {
            int rand = Random.Range(5, 6);
            EquipSoldier(equipmentNames[rand], rand * 10);
            yield return null;
            counter = 3;

        }
        yield return null;





    }
    IEnumerator EnemyFortSpawner()
    {


        while (true)
        {
            float timer = coolDown;

            if (queueSize < maxQueueSize)
            {
                while (timer > 0)
                {

                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                }
                BuyVikingSoldier();
                StartCoroutine("EnemyEquiper");

            }


            yield return null;
        }

    }
    IEnumerator PlayerFortBehaviour()
    {

        while (true)
        {
            float timer = coolDown;
            if (queueSize < maxQueueSize)
            {
                while (timer > 0)
                {
                    coolDownBar.fillAmount = timer / coolDown;
                    yield return new WaitForSeconds(0.05f);
                    timer -= 0.05f;
                }
                BuyVikingSoldier();
            }
            else coolDownBar.fillAmount = timer / coolDown;

            yield return null;
        }

    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / fort.health;
        if (health <= 0)
        {

            if (transform.CompareTag("EnemyFort")) GameManager.Instance.gameWinner = GameManager.Winner.Player;
            else GameManager.Instance.gameWinner = GameManager.Winner.Enemy;

            GameManager.Instance.FinishGame();
        }
    }

    public int GetFortId() => fort.id;
}
