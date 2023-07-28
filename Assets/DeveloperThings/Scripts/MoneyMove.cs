using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoneyMove : MonoBehaviour
{
    [SerializeField] private Transform moneyIconTransform;
    [SerializeField] private TMP_Text moneyText;
    private Vector3 startedScale;

    private Camera mainCamera;

    private void OnEnable()
    {
        startedScale = new Vector3(0.5f, 0.5f, 0.5f);
        mainCamera = Camera.main;
        moneyText = transform.GetChild(0).GetComponent<TMP_Text>();
        moneyIconTransform = GameManager.Instance.GetMoneyIconTransform();
        StartMove();
    }


    private void StartMove()
    {
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1.5f);
        transform.DOMove(moneyIconTransform.position, 1.5f).OnComplete(() =>
        {
            transform.DOScale(startedScale, 1.5f);
            gameObject.SetActive(false);
        });

    }
    public void SetMoneyText(float value) => moneyText.text = value.ToString();

}
