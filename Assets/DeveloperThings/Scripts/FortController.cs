using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using RayFire;

public class FortController : MonoBehaviour
{
    private GameObject defaultBlueVikingSoldier;
    [SerializeField] private GameObject defaultBlueVikingSoldierPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePoints;
    [SerializeField] private GameObject[] soldiersInQueue;
    [SerializeField] private GameObject[] rayFireElements;
    [SerializeField] private Transform upgradeEffectTransform;
    private int maxQueueSize;
    private int queueSize;
    public Fort fort;
    private FortStats stats;
    [SerializeField]private int level;
    private float health;
    private float coolDown;
    public Image healthBar;
    public Image coolDownBar;
    private int equippedCounter;
    private int equippedCounterMax;


    private void Start()
    {
        stats = new FortStats(fort.id);
        FortStats _stats = GameManager.Instance.GetFortStats(fort.id);
        if (stats != null) stats = _stats;
        level=stats.level;
        if(transform.CompareTag("PlayerFort"))upgradeEffectTransform = transform.GetChild(6).transform;

        SetDifficultyCounter();
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

    public void UpgradeFort()
    {
        level += 1;
        stats.level = level;
        var upgradeEffect = ObjectPooler.Instance.GetUpgradeFortParticlesFromPool();
        if(upgradeEffect != null)
        {
            upgradeEffect.transform.position = upgradeEffectTransform.position;
            upgradeEffect.SetActive(true);
        }
        var upgradePopUp = ObjectPooler.Instance.GetFortUpgradePopUp();
        if(upgradePopUp != null)
        {
            upgradePopUp.transform.position = new Vector3(upgradeEffectTransform.position.x, upgradeEffectTransform.position.y+8, upgradeEffectTransform.position.z);
            upgradePopUp.SetActive(true);
            Vector3 targetPos =new Vector3 (upgradePopUp.transform.position.x, upgradePopUp.transform.position.y+5, upgradePopUp.transform.position.z);
            upgradePopUp.transform.DOMove(targetPos,0.7f).OnComplete(() =>
            {
                upgradePopUp.SetActive(false);
            });
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
    public void EquipSoldier(string itemName, float itemDamage, float itemHealth)
    {
        GameObject firstSoldier = soldiersInQueue[maxQueueSize - 1];
        if (firstSoldier != null)
        {
            firstSoldier.GetComponent<SoldierController>().TakeUpArms(itemName, itemDamage, itemHealth);
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
    private void SetDifficultyCounter()
    {
        int difficultyTier = GameManager.Instance.GetDifficultyTier();
        if (difficultyTier == 1)
        {
            equippedCounter = 6;
            equippedCounterMax = equippedCounter;
        }
        if (difficultyTier == 2)
        {
            equippedCounter = 6;
            equippedCounterMax = equippedCounter;
        }
        if (difficultyTier == 3)
        {
            equippedCounter = 3;
            equippedCounterMax = equippedCounter;
        }

    }

    IEnumerator EnemyEquiper()
    {

        GameObject[] items = GameManager.Instance.GetAllItemTypes();
        int difficultyTier = GameManager.Instance.GetDifficultyTier();
        Item item;
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.GetPlayerLevel() <= 1)
        {
            item = items[1].GetComponent<EquipmentController>().item;

            EquipSoldier(item.itemName, item.damage, item.health);

        }
        else
        {
            if (difficultyTier == 1)
            {


                if (equippedCounter > 0)
                {
                    int rand = Random.Range(1, 3);
                    item = items[rand].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter--;
                }
                else
                {
                    item = items[3].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter = equippedCounterMax;

                }
                yield return null;
            }
            if (difficultyTier == 2)
            {


                if (equippedCounter > 0)
                {
                    int rand = Random.Range(2, 5);
                    item = items[rand].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter--;

                }
                else
                {
                    item = items[4].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter = equippedCounterMax;

                }
                yield return null;
            }
            if (difficultyTier == 3)
            {

                if (equippedCounter > 0)
                {
                    int rand = Random.Range(3, 6);
                    item = items[rand].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter--;

                }
                else
                {
                    int rand = Random.Range(5, 7);
                    item = items[rand].GetComponent<EquipmentController>().item;
                    EquipSoldier(item.itemName, item.damage, item.health);
                    yield return null;
                    equippedCounter = equippedCounterMax;

                }
                yield return null;
            }


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
        if (transform.CompareTag("PlayerFort"))
        {
            Vibrator.Vibrate(100);
            GameManager.Instance.ShakeCamera();
        }

    }
    IEnumerator TakeDamageAnimated(float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            health -= damage / 3;
            healthBar.fillAmount = health / fort.health;
            yield return new WaitForSeconds(0.03f);

        }
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
        Item item;
        GameObject[] items = GameManager.Instance.GetAllItemTypes();
        item = items[1].GetComponent<EquipmentController>().item;
        while (!TutorialManager.Instance.GetFirstEquipState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        yield return new WaitForSeconds(1.5f);
        EquipSoldier(item.itemName, item.damage, item.health);
        while (!TutorialManager.Instance.GetSecondEquipState())
        {
            yield return null;
        }
        BuyVikingSoldier();
        EquipSoldier(item.itemName, item.damage, item.health);
        StartCoroutine("EnemyFortSpawner");

    }
}
