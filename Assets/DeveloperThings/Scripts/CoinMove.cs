using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CoinMove : MonoBehaviour
{
    [SerializeField] private Transform iconTransform;
    [SerializeField] private TMP_Text coinText;
    private Vector3 startedTransform;


    private void OnEnable()
    {
        startedTransform = new Vector3(0.5f, 0.5f, 0.5f);
        coinText = transform.GetChild(0).GetComponent<TMP_Text>();
        iconTransform = GameManager.Instance.GetMoneyIconTransform();
        StartMove();
    }


    private void StartMove()
    {
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1.5f);
        transform.DOMove(iconTransform.position, 1.5f).OnComplete(() =>
        {
            transform.DOScale(startedTransform, 1.5f);
            gameObject.SetActive(false);
        });

    }

    public void SetCoinText(float value) => coinText.text = value.ToString();
}
