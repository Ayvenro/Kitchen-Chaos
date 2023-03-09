using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frying Recipe", menuName = "Scriptable Object/Frying Recipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax;
}
