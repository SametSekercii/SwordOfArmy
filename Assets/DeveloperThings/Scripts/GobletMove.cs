using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GobletMove : MonoBehaviour
{
    private Transform gobletIconTransform;
    private TMP_Text gobletText;
    private Vector3 startedScale;


    private void OnEnable()
    {
        startedScale = Vector3.zero;
        gobletText = transform.GetChild(1).GetComponent<TMP_Text>();
        gobletIconTransform = GameManager.Instance.GetGobletIconTransform();
        StartCoroutine("StartMove");



    }

    IEnumerator StartMove()
    {
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1.5f);
        yield return new WaitForSeconds(0.6f);
        transform.DOMove(gobletIconTransform.position, 1.2f).OnComplete(() =>
        {
            transform.DOScale(startedScale, 0.5f);
            gameObject.SetActive(false);
        });


    }


    public void SetGobletText(int value) => gobletText.text = value.ToString();


}
