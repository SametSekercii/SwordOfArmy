using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class SoldierController : MonoBehaviour
{
    enum SoldierState { inQueue, inWar, Dead }
    enum EquipmentState { isArmed, isNotArmed }
    private Animator anim;
    public Soldier soldier;
    public PathCreator queuePath;
    float distancetravelled;
    [SerializeField] private int queueNumber;
    private SoldierState state;
    private EquipmentState equipmentState;
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


    private void OnEnable()
    {

        health = soldier.health;
        damage = soldier.damage;
        moveSpeed = 0;
        healthBar.fillAmount = health / soldier.health;
        anim = transform.GetComponent<Animator>();
        equipmentState = EquipmentState.isNotArmed;
        state = SoldierState.inQueue;
        rightArm = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0);
        eye = transform.GetChild(1);
        if (transform.CompareTag("PlayerSoldier"))
        {
            queuePath = GameObject.FindWithTag("PlayerFortPathCreator").GetComponent<PathCreator>();
            soldierfort = GameObject.FindWithTag("PlayerFort").GetComponent<FortController>();
            fortId = 1;


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
        while (state != SoldierState.Dead)
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
                    moveSpeed = 0;
                    if (!hitInfo.transform.CompareTag(transform.tag) && hitInfo.transform.GetComponent<FortController>() == null)
                    {
                        enemyFromForward = hitInfo.transform.gameObject;

                        anim.SetBool("isAttacking", true);
                    }
                    if (hitInfo.transform.GetComponent<FortController>() != null)
                    {
                        Destroy(gameObject);
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

        state = SoldierState.inWar;

    }
    public void GiveDamage()
    {
        if (enemyFromForward != null)
        {
            SoldierController enemy = enemyFromForward.GetComponent<SoldierController>();
            enemy.TakeDamage(damage);
        }
    }
    public void SetQueueNumber(int value) => queueNumber = value;
    public void IncreaseQueueNumber() => queueNumber++;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / soldier.health;


    }

    public int GetFortId() => fortId;

}
