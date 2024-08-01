using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSaverInventory : MonoBehaviour, IDataSaver
{
    public SavableData savableData = new();
    private string save_path = Application.dataPath + "/inventory_data_test.json";
    private Container container;
    private void Awake()
    {
        container = GetComponent<Container>();
    }
    
    public void SaveState()
    {
        savableData.slots = ConvertSlotsToSavable();
        string json = JsonUtility.ToJson(savableData);
        File.WriteAllText(save_path, json);
    }
    private List<Slot> ConvertSlotsToSavable()
    {
        List<Slot> savable = new();
        foreach (SlotObject slotObject in container.slots)
        {
            savable.Add(slotObject.slot);
        }
        return savable;
    }

    public void LoadState()
    {
        if (File.Exists(save_path))
        {
            string json = File.ReadAllText(save_path);
            savableData = JsonUtility.FromJson<SavableData>(json);
            ConvertSlotsFromSavable();
        }
    }
    private void ConvertSlotsFromSavable()
    {
        for (var i = 0; i < container.slots.Count; i++)
        {
            container.slots[i].slot = savableData.slots[i];
        }
    }
}
