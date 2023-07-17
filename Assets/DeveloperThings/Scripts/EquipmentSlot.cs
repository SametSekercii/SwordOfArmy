using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : UnitySingleton<EquipmentSlot>
{
    enum SlotState { filled, empty }
    private Collider col;
    [SerializeField] private SlotState state;
    private Transform slotPointTransform;
    private int slotLevel = 1;
    private float slotCost;
    [SerializeField] private GameObject equipmentOnSLot;




    void Start()
    {

        col = transform.GetComponent<Collider>();
        slotPointTransform = transform.GetChild(0).transform;
        slotCost = slotLevel * 100;
        CheckSlotState();

    }

    public void BuyEquipment()
    {
        if (GameManager.Instance.GetMoneyValue() >= slotCost)
        {
            GameManager.Instance.SpendMoney(slotCost);
            var equipment = ObjectPooler.Instance.getEquipmentFromPool(slotLevel);
            if (equipment != null)
            {
                equipment.transform.parent = transform;
                SetNewEquipmentTransform(equipment);
                equipmentOnSLot = equipment;
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
            col.enabled = false;
            return true;
        }
        else
        {
            this.state = SlotState.empty;
            col.enabled = true;
            return false;
        }

    }
    public void EmpySlot()
    {
        equipmentOnSLot = null;
        CheckSlotState();
    }
    public void FillSlot(GameObject equipment)
    {
        equipmentOnSLot = equipment;
        CheckSlotState();
    }
    public void SetNewEquipmentTransform(GameObject newEquipment)
    {
        newEquipment.transform.position = slotPointTransform.position;
        newEquipment.transform.rotation = slotPointTransform.rotation;

    }






}
