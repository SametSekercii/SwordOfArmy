using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : UnitySingleton<ObjectPooler>
{
    private List<GameObject> level1Equipments = new List<GameObject>();
    private List<GameObject> level2Equipments = new List<GameObject>();
    private List<GameObject> level3Equipments = new List<GameObject>();
    private List<GameObject> level4Equipments = new List<GameObject>();
    private List<GameObject> level5Equipments = new List<GameObject>();
    private List<GameObject> level6Equipments = new List<GameObject>();
    private int amountOfEachEquipment = 10;
    [SerializeField] private GameObject level1EquipmentPrefab;
    [SerializeField] private GameObject level2EquipmentPrefab;
    [SerializeField] private GameObject level3EquipmentPrefab;
    [SerializeField] private GameObject level4EquipmentPrefab;
    [SerializeField] private GameObject level5EquipmentPrefab;
    [SerializeField] private GameObject level6EquipmentPrefab;

    void Awake()
    {
        CreateAllEquipmentPool();
    }

    private void CreateAllEquipmentPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level1Equipment = Instantiate(level1EquipmentPrefab);
            level1Equipment.transform.SetParent(transform);
            level1Equipment.SetActive(false);
            level1Equipments.Add(level1Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level2Equipment = Instantiate(level2EquipmentPrefab);
            level2Equipment.transform.SetParent(transform);
            level2Equipment.SetActive(false);
            level2Equipments.Add(level2Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level3Equipment = Instantiate(level3EquipmentPrefab);
            level3Equipment.transform.SetParent(transform);
            level3Equipment.SetActive(false);
            level3Equipments.Add(level3Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level4Equipment = Instantiate(level4EquipmentPrefab);
            level4Equipment.transform.SetParent(transform);
            level4Equipment.SetActive(false);
            level4Equipments.Add(level4Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level5Equipment = Instantiate(level5EquipmentPrefab);
            level5Equipment.transform.SetParent(transform);
            level5Equipment.SetActive(false);
            level5Equipments.Add(level5Equipment);
        }
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            var level6Equipment = Instantiate(level6EquipmentPrefab);
            level6Equipment.transform.SetParent(transform);
            level6Equipment.SetActive(false);
            level6Equipments.Add(level6Equipment);
        }
    }

    public GameObject getEquipmentFromPool(int level)
    {
        switch (level)
        {
            case 1:
                return getlevel1EquipmentFromPool();
            case 2:
                return getlevel2EquipmentFromPool();
            case 3:
                return getlevel3EquipmentFromPool();
            case 4:
                return getlevel4EquipmentFromPool();
            case 5:
                return getlevel5EquipmentFromPool();
            case 6:
                return getlevel6EquipmentFromPool();
        }
        return null;

    }
    public GameObject getlevel1EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level1Equipments[i].activeSelf) return level1Equipments[i];
        }
        return null;
    }
    public GameObject getlevel2EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level2Equipments[i].activeSelf) return level2Equipments[i];
        }
        return null;
    }
    public GameObject getlevel3EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level3Equipments[i].activeSelf) return level3Equipments[i];
        }
        return null;
    }
    public GameObject getlevel4EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level4Equipments[i].activeSelf) return level4Equipments[i];
        }
        return null;
    }
    public GameObject getlevel5EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level5Equipments[i].activeSelf) return level5Equipments[i];
        }
        return null;
    }
    public GameObject getlevel6EquipmentFromPool()
    {
        for (int i = 0; i < amountOfEachEquipment; i++)
        {
            if (!level6Equipments[i].activeSelf) return level6Equipments[i];
        }
        return null;
    }
}

