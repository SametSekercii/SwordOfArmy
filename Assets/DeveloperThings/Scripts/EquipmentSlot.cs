using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EquipmentSlot : UnitySingleton<EquipmentSlot>
{
    enum SlotState { filled, empty }
    [SerializeField] private MergeArea mergeArea;
    private Collider col;
    private SlotState state;
    private Transform slotPointTransform;
    private int slotLevel;

    private GameObject equipmentOnSLot;
    private GameObject tabletOnSlot;
    private TMP_Text costText;
    private float slotCost;
    private TMP_Text purchaseCostText;
    private float purchaseCost = 5000;
    private GameObject slotCanvas;
    [SerializeField] private GameObject slotLockCanvas;

    private float changeColorDuration = 0.5f;




    void Start()
    {
        slotLevel = mergeArea.areaLevel;
        costText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        slotCanvas = transform.GetChild(1).gameObject;
        purchaseCostText = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        slotLockCanvas = transform.GetChild(2).gameObject;
        col = transform.GetComponent<Collider>();
        slotPointTransform = transform.GetChild(0).transform;
        slotCost = slotLevel * 100;
        costText.text = slotCost.ToString();
        purchaseCostText.text = purchaseCost.ToString();
        if (mergeArea.isSolded)
        {
            slotLockCanvas.SetActive(false);
            slotCanvas.SetActive(true);
        }
        else
        {
            slotLockCanvas.SetActive(true);
            slotCanvas.SetActive(false);

        }
        CheckSlotState();

    }

    public void BuyEquipment()
    {
        if (GameManager.Instance.GetMoneyValue() >= slotCost)
        {
            GameManager.Instance.SpendMoney(slotCost);
            var equipment = ObjectPooler.Instance.GetEquipmentFromPool(slotLevel);
            if (equipment != null)
            {
                equipment.transform.parent = transform;
                SetNewEquipmentTransform(equipment);
                FillSlot(equipment);
                equipment.SetActive(true);

            }

        }
        else
        {
            for (int i = 0; i < transform.GetChild(1).childCount; i++)
            {
                if (transform.GetChild(1).GetChild(i).GetComponent<Image>() != null)
                {
                    Image image;
                    image = transform.GetChild(1).GetChild(i).GetComponent<Image>();
                    image.DOColor(Color.red, changeColorDuration).OnComplete(() => { image.DOColor(Color.white, changeColorDuration); });


                }
                if (transform.GetChild(1).GetChild(i).GetComponent<TMP_Text>() != null)
                {
                    TMP_Text text;
                    text = transform.GetChild(1).GetChild(i).GetComponent<TMP_Text>();
                    text.DOColor(Color.red, changeColorDuration).OnComplete(() => { text.DOColor(Color.white, changeColorDuration); });

                }
            }

        }
        CheckSlotState();
    }
    public void BuySlot()
    {
        if (GameManager.Instance.GetMoneyValue() >= purchaseCost)
        {
            GameManager.Instance.SpendMoney(purchaseCost);
            mergeArea.isSolded = true;
            CheckSlotState();
        }
        else
        {
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                if (transform.GetChild(2).GetChild(i).GetComponent<Image>() != null)
                {
                    Image image;
                    image = transform.GetChild(2).GetChild(i).GetComponent<Image>();
                    image.DOColor(Color.red, changeColorDuration).OnComplete(() => { image.DOColor(Color.white, changeColorDuration); });


                }
                if (transform.GetChild(2).GetChild(i).GetComponent<TMP_Text>() != null)
                {
                    TMP_Text text;
                    text = transform.GetChild(2).GetChild(i).GetComponent<TMP_Text>();
                    text.DOColor(Color.red, changeColorDuration).OnComplete(() => { text.DOColor(Color.white, changeColorDuration); });
                }
            }

        }
    }

    public bool CheckSlotState()
    {


        if (mergeArea.isSolded && equipmentOnSLot != null)
        {
            this.state = SlotState.filled;
            slotCanvas.gameObject.SetActive(false);
            slotLockCanvas.gameObject.SetActive(false);
            col.enabled = false;
            return true;
        }
        else if (mergeArea.isSolded && equipmentOnSLot == null)
        {
            this.state = SlotState.empty;
            slotCanvas.gameObject.SetActive(true);
            slotLockCanvas.gameObject.SetActive(false);
            col.enabled = true;
            return false;
        }
        else if (!mergeArea.isSolded)
        {
            slotCanvas.gameObject.SetActive(false);
            slotLockCanvas.gameObject.SetActive(true);
            col.enabled = true;
            return false;

        }
        else return false;





    }
    public void EmpySlot()
    {
        if (tabletOnSlot != null)
        {
            tabletOnSlot.transform.parent = FindObjectOfType<ObjectPooler>().transform;
            tabletOnSlot.SetActive(false);
            tabletOnSlot = null;
        }

        equipmentOnSLot = null;
        CheckSlotState();
    }
    public void FillSlot(GameObject equipment)
    {
        if (tabletOnSlot != null)
        {
            tabletOnSlot.transform.parent = FindObjectOfType<ObjectPooler>().transform;
            tabletOnSlot.SetActive(false);
            tabletOnSlot = null;
        }
        equipmentOnSLot = equipment;
        var tablet = ObjectPooler.Instance.GetTabletFromPool(equipmentOnSLot.GetComponent<EquipmentController>().GetItemLevel());
        if (tablet != null)
        {
            tablet.transform.position = transform.position;
            tablet.transform.rotation = transform.rotation;
            tablet.transform.parent = transform;
            tabletOnSlot = tablet;
            tablet.SetActive(true);

        }
        CheckSlotState();
    }
    public void SetNewEquipmentTransform(GameObject newEquipment)
    {
        newEquipment.transform.position = slotPointTransform.position;
        newEquipment.transform.rotation = slotPointTransform.rotation;

    }
    public bool GetAreaLockState()
    {
        if (mergeArea.isSolded) return true;
        else return false;

    }
    public MergeArea GetMergeAreaInfo() => mergeArea;






}
