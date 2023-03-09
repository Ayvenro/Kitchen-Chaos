using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
         {
            player.GetKitchenObject().DestroySelf();
         }
    }
}
