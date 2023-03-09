using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Burning Recipe", menuName = "Scriptable Object/Burning Recipe")] 
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}
