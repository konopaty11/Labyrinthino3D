using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JumpTest
{
    [Test]
    public void JumpDurationTest()
    {
        GameObject go = new GameObject();
        PlayerController player = go.AddComponent<PlayerController>();

        float time = 0f;
        float dt = 0.05f;

        float y = 0f;

        player.ForceJump();

        while (true)
        {
            player.SimulateGravity(dt);
            y += player.Velocity.y * dt;
            time += dt;

            if (y <= 0f && time > 0f)
                break;

            if (time > 5f)
                Assert.Fail("Loop stuck");
        }

        Debug.Log($"Jump duration: {time}");

        Assert.Greater(time, 0.5f);
    }
}
