using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SoldierController : MonoBehaviour
{
    enum SoldierState { inQueue, inWar, Dead }
    public Soldier soldier;
    public PathCreator queuePath;
    private float speed = 5;
    float distancetravelled;
    private int queueNumber;
    SoldierState state;

    private void OnEnable()
    {
        if (transform.gameObject.CompareTag("PlayerSoldier")) queuePath = GameObject.FindWithTag("PlayerFortPathCreator").GetComponent<PathCreator>();
        state = SoldierState.inQueue;
        StartCoroutine("GetInQueue");


    }



    public void SetQueueNumber(int value) => queueNumber = value;

    IEnumerator GetInQueue()
    {

        while (state == SoldierState.inQueue)
        {
            Transform targetPos;
            targetPos = FortController.Instance.GetQueuePoint(queueNumber);
            if (Vector3.Distance(transform.position, targetPos.position) > 0.4f)
            {
                distancetravelled += speed * Time.deltaTime;
                transform.position = queuePath.path.GetPointAtDistance(distancetravelled);
                transform.rotation = queuePath.path.GetRotationAtDistance(distancetravelled);
            }


            yield return null;
        }
        yield return null;

    }
    IEnumerator GoToWar()
    {
        while (true)
        {
            Debug.Log("savaşıyor");
            yield return null;
        }

    }

    public void SetStateInWar()
    {
        state = SoldierState.inWar;
        StartCoroutine("GoToWar");

    }
    public void IncreaseQueueNumber() => queueNumber++;

}
