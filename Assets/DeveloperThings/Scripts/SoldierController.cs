using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class SoldierController : MonoBehaviour
{
    enum SoldierState { inQueue, inWar, Dead }

    [SerializeField] private bool inFort;
    [SerializeField] private Transform[] coinPopUpSpots;
    private Animator anim;
    public Soldier soldier;
    public PathCreator queuePath;
    float distancetravelled;
    [SerializeField] private int queueNumber;
    private SoldierState state;
    private FortController soldierfort;
    private Transform targetPos;
    private Transform eye;
    private Transform rightArm;
    private GameObject equippedArmor;
    private GameObject equippedSword;
    [SerializeField] private float health;
    private int moveSpeed;
    [SerializeField] private float damage;
    private GameObject enemyFromForward;
    private int fortId;
    public Image healthBar;
    private float gainMoneyValue;


    private void OnEnable()
    {

        coinPopUpSpots = new Transform[transform.GetChild(2).childCount];
        inFort = true;
        health = soldier.health;
        damage = soldier.damage;
        gainMoneyValue = damage * 3;
        moveSpeed = 0;
        healthBar.fillAmount = health / soldier.health;
        anim = transform.GetComponent<Animator>();
        state = SoldierState.inQueue;
        rightArm = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        eye = transform.GetChild(1);
        if (transform.CompareTag("PlayerSoldier"))
        {
            queuePath = GameObject.FindWithTag("PlayerFortPathCreator").GetComponent<PathCreator>();
            soldierfort = GameObject.FindWithTag("PlayerFort").GetComponent<FortController>();
            fortId = 1;
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                coinPopUpSpots[i] = transform.GetChild(2).GetChild(i);
            }


        }
        if (transform.CompareTag("EnemySoldier"))
        {
            queuePath = GameObject.FindWithTag("EnemyFortPathCreator").GetComponent<PathCreator>();
            soldierfort = GameObject.FindWithTag("EnemyFort").GetComponent<FortController>();
            fortId = 2;

        }

        StartCoroutine("GetInGame");

    }
    IEnumerator GetInGame()
    {


        while (state != SoldierState.Dead && !GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.IsGameGoing())
            {
                anim.SetInteger("moveSpeed", moveSpeed);
                targetPos = soldierfort.GetQueuePoint(queueNumber);
                distancetravelled += moveSpeed * Time.deltaTime;
                transform.position = queuePath.path.GetPointAtDistance(distancetravelled);
                transform.rotation = queuePath.path.GetRotationAtDistance(distancetravelled);

                if (state == SoldierState.inQueue)
                {
                    if (Vector3.Distance(transform.position, targetPos.position) > 0.4f) moveSpeed = 2;
                    else moveSpeed = 0;

                }

                if (state == SoldierState.inWar)
                {

                    Ray ray = new Ray(eye.position, eye.TransformDirection(Vector3.forward));
                    Debug.DrawRay(eye.position, eye.TransformDirection(Vector3.forward) * 3f, Color.red);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f))
                    {

                        if (transform.CompareTag("PlayerSoldier"))
                        {
                            if (hitInfo.transform.CompareTag("EnemySoldier") || hitInfo.transform.CompareTag("EnemyFort"))
                            {
                                moveSpeed = 0;
                                enemyFromForward = hitInfo.transform.gameObject;

                                anim.SetBool("isAttacking", true);

                            }
                            if (hitInfo.transform.CompareTag("PlayerSoldier")) moveSpeed = 0;
                        }
                        if (transform.CompareTag("EnemySoldier"))
                        {
                            if (hitInfo.transform.CompareTag("PlayerSoldier") || hitInfo.transform.CompareTag("PlayerFort"))
                            {
                                moveSpeed = 0;
                                enemyFromForward = hitInfo.transform.gameObject;

                                anim.SetBool("isAttacking", true);
                            }
                            if (hitInfo.transform.CompareTag("EnemySoldier")) moveSpeed = 0;
                        }
                    }
                    else
                    {
                        moveSpeed = soldier.moveSpeed;
                        anim.SetBool("isAttacking", false);
                    }
                    if (health <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                yield return null;
            }
        }
        if (transform.CompareTag("PlayerSoldier") && GameManager.Instance.GetGameWinner() == GameManager.Winner.Player || transform.CompareTag("EnemySoldier") && GameManager.Instance.GetGameWinner() == GameManager.Winner.Enemy)
        {
            moveSpeed = 0;
            anim.SetInteger("moveSpeed", moveSpeed);
            anim.SetBool("isAttacking", false);
            Debug.Log("seviniyor");
        }
        else Destroy(gameObject);

        yield return null;

    }
    public void TakeUpArms(string itemName, float itemValue)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(itemName))
            {
                transform.GetChild(i).gameObject.SetActive(true);
                equippedArmor = transform.GetChild(i).gameObject;

            }

        }
        for (int i = 0; i < rightArm.childCount; i++)
        {
            if (rightArm.GetChild(i).CompareTag(itemName))
            {
                rightArm.GetChild(i).gameObject.SetActive(true);
                equippedSword = transform.GetChild(i).gameObject;
            }

        }
        damage += itemValue;
        gainMoneyValue = damage * 3;

        state = SoldierState.inWar;

    }
    public void GiveDamage()
    {
        if (health <= 0) return;

        if (enemyFromForward != null)
        {
            if (enemyFromForward.GetComponent<FortController>() != null)
            {
                enemyFromForward.GetComponent<FortController>().TakeDamage(damage);

            }
            if (enemyFromForward.GetComponent<SoldierController>() != null)
            {
                enemyFromForward.GetComponent<SoldierController>().TakeDamage(damage);

            }

        }
        if (transform.CompareTag("PlayerSoldier"))
        {
            var coinPopUp = ObjectPooler.Instance.GetCoinPopUp();
            if (coinPopUp != null)
            {
                coinPopUp.transform.position = coinPopUpSpots[Random.Range(0, 1)].position;
                coinPopUp.SetActive(true);
                coinPopUp.GetComponent<CoinMove>().SetCoinText(gainMoneyValue);

            }

            GameManager.Instance.EarnMoney(gainMoneyValue);
        }
    }
    public void SetQueueNumber(int value) => queueNumber = value;
    public void IncreaseQueueNumber() => queueNumber++;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / soldier.health;


    }
    private void OnTriggerExit(Collider other)
    {
        inFort = false;
    }

    public int GetFortId() => fortId;

}
