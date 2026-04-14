using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTask : TaskBase
{
    public void Jump()
    {
        if (!IsDetecting)
            return;

        Complete();
    }
}
