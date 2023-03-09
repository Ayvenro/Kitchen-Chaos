using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutting Recipe", menuName = "Scriptable Object/Cutting Recipe")] 
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
