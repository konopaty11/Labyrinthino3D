public class PickUpBall : TaskBase
{
    void OnEnable()
    {
        PlayerController.PickupItem += PickupBall;
    }

    void OnDisable()
    {
        PlayerController.PickupItem -= PickupBall;
    }

    void PickupBall(ItemType itemType)
    {
        if (itemType == ItemType.Ball)
            Complete();
    }
}
