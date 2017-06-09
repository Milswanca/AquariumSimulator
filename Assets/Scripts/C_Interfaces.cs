using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Purchasable 
{
	void OnPurchased();
}

public interface I_Savable
{
    FAquariumSaveData GetSaveData();
    void LoadFromData(FAquariumSaveData saveData);
}