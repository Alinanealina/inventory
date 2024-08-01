using System;
using System.Collections.Generic;

public interface IDataSaver
{
    void SaveState();
    void LoadState();
}

[Serializable]
public class SavableData
{
    public List<Slot> slots;
}