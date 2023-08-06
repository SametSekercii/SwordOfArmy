using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using RayFire;

public class FortController : MonoBehaviour
{
    private string[] equipmentNames = { "null", "Level1Equipment", "Level2Equipment", "Level3Equipment", "Level4Equipment", "Level5Equipment", "Level6Equipment" };
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    [SerializeField] private GameObject[] rayFireElements;
    private int maxQueueSize;
    private int queueSize;
    public Fort fort;
    private float health;
    private float coolDown;
    public Image healthBar;
    public Image coolDownBar;


    private void Start()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
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

        if (GameManager.Instance.GetPlayerLevel() > 0)
        {
            Debug.Log(GameManager.Instance.GetPlayerLevel());

            if (transform.CompareTag("EnemyFort"))
            {

                StartCoroutine("EnemyFortSpawner");

            }
            if (transform.CompareTag("PlayerFort"))
            {
                StartCoroutine("PlayerFortBehaviour");
            }
        }
        else
        {
            if (transform.CompareTag("EnemyFort"))
            {
                StartCoroutine("EnemyFortTutorial");

            }
            if (transform.CompareTag("PlayerFort"))
            {
                StartCoroutine("PlayerFortTutorial");
            }

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
        if (GameManager.Instance.GetPlayerLevel() == 0 && transform.CompareTag("PlayerFort"))
        {
            GameManager.Instance.IncreaseEquippedSoldier();
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

        if (GameManager.Instance.GetPlayerLevel() <= 1)
        {
            yield return new WaitForSeconds(1);
            EquipSoldier(equipmentNames[1], 1 * 10);

        }
        else
        {
            yield return new WaitForSeconds(1);
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
    }
    IEnumerator EnemyFortSpawner()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
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
            }
            yield return null;
        }
    }
    IEnumerator PlayerFortBehaviour()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
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

            }
            yield return null;
        }
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine("TakeDamageAnimated", damage);
    }
    IEnumerator TakeDamageAnimated(float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            health -= damage / 3;
            healthBar.fillAmount = health / fort.health;
            yield return new WaitForSeconds(0.03f);

        }
        Debug.Log(health);
        AudioManager.Instance.PlaySFX("HitFort");
        var particle = ObjectPooler.Instance.GetHitParticlesFromPool();
        if (particle != null)
        {
            particle.transform.position = transform.position;
            particle.SetActive(true);

        }
        for (int i = 0; i < rayFireElements.Length; i++)
        {
            rayFireElements[i].transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

        }
        if (health <= 1)
        {
            for (int i = 0; i < rayFireElements.Length; i++)
            {
                rayFireElements[i].GetComponent<Rigidbody>().isKinematic = false;

            }


            if (transform.CompareTag("EnemyFort")) GameManager.Instance.gameWinner = GameManager.Winner.Player;
            else GameManager.Instance.gameWinner = GameManager.Winner.Enemy;
            AudioManager.Instance.PlaySFX("FortDown");
            GameManager.Instance.FinishGame();
        }
    }

    IEnumerator PlayerFortTutorial()
    {

        BuyVikingSoldier();
        while (!TutorialManager.Instance.GetFirstMergeState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        StartCoroutine("PlayerFortBehaviour");
    }
    IEnumerator EnemyFortTutorial()
    {
        while (!TutorialManager.Instance.GetFirstEquipState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        yield return new WaitForSeconds(1.5f);
        EquipSoldier(equipmentNames[1], 1 * 9f);
        while (!TutorialManager.Instance.GetSecondEquipState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        EquipSoldier(equipmentNames[1], 1 * 10);
        StartCoroutine("EnemyFortSpawner");

    }
}
