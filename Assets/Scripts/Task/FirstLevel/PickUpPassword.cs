using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPassword : TaskBase
{
    [SerializeField] Door door;

    void OnEnable()
    {
        PlayerController.PickupItem += PickupPassword;
    }

    void OnDisable()
    {
        PlayerController.PickupItem -= PickupPassword;
    }

    void PickupPassword(ItemType itemType)
    {
        if (itemType == ItemType.Password)
        {
            door.isLocked = false;
            Complete();
        }
    }
}
