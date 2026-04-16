using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTask : TaskBase
{
    [SerializeField] Door door;

    public void Jump()
    {
        if (!IsDetecting)
            return;

        door.isLocked = false;
        Complete();
    }
}
