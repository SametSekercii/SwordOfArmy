using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipmentSlot : UnitySingleton<EquipmentSlot>
{
    enum SlotState { filled, empty }
    private Collider col;
    [SerializeField] private SlotState state;
    private Transform slotPointTransform;
    private int slotLevel = 1;
    private float slotCost;
    [SerializeField] private GameObject equipmentOnSLot;
    [SerializeField] private GameObject tabletOnSlot;
    [SerializeField] private TMP_Text costText;



    void Start()
    {
        costText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        col = transform.GetComponent<Collider>();
        slotPointTransform = transform.GetChild(0).transform;
        slotCost = slotLevel * 100;
        costText.text = slotCost.ToString();
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

    public bool CheckSlotState()
    {

        if (equipmentOnSLot != null)
        {

            this.state = SlotState.filled;
            costText.gameObject.SetActive(false);
            col.enabled = false;
            return true;
        }
        else
        {

            this.state = SlotState.empty;
            costText.gameObject.SetActive(true);

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






}
