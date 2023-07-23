using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : UnitySingleton<EquipmentSlot>
{
    enum SlotState { filled, empty }
    [SerializeField] private MergeArea mergeArea;
    private Collider col;
    private SlotState state;
    private Transform slotPointTransform;
    private int slotLevel;
    private float slotCost;
    private GameObject equipmentOnSLot;
    private GameObject tabletOnSlot;
    private TMP_Text costText;
    private GameObject slotCanvas;
    private GameObject slotLockCanvas;
    private float purchaseCost = 1000;



    void Start()
    {
        slotLevel = mergeArea.areaLevel;
        costText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        slotCanvas = transform.GetChild(1).gameObject;
        col = transform.GetComponent<Collider>();
        slotPointTransform = transform.GetChild(0).transform;
        slotCost = slotLevel * 100;
        costText.text = slotCost.ToString();
        if (mergeArea.isSolded)
        {
            slotCanvas.SetActive(true);
            slotLockCanvas.SetActive(false);
        }
        else
        {
            slotCanvas.SetActive(false);
            slotLockCanvas.SetActive(true);
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
        CheckSlotState();
    }
    public void BuySlot()
    {
        
        GameManager.Instance.SpendMoney(purchaseCost);
        if (mergeArea.isSolded)
        {
            slotCanvas.SetActive(true);
            slotLockCanvas.SetActive(false);
        }
        else
        {
            slotCanvas.SetActive(false);
            slotLockCanvas.SetActive(true);
        }
    }

    public bool CheckSlotState()
    {

        if (equipmentOnSLot != null)
        {

            this.state = SlotState.filled;
            slotCanvas.gameObject.SetActive(false);
            col.enabled = false;
            return true;
        }
        else
        {

            this.state = SlotState.empty;
            slotCanvas.gameObject.SetActive(true);

            col.enabled = true;



            return false;
        }

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






}
