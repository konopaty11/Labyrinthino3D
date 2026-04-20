using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpeedTest
{
    [Test]
    public void SpeedReachesMax()
    {
        GameObject go = new GameObject();
        PlayerController player = go.AddComponent<PlayerController>();

        float time = 0f;
        float dt = 0.02f;
        float prevSpeed = -1;

        while (Mathf.Abs(player.CurrentSpeed - prevSpeed) > 0.05f)
        {
            prevSpeed = player.CurrentSpeed;

            player.SimulateMove(dt, Vector2.up);
            time += dt;
        }

        Debug.Log($"Max speed: {player.CurrentSpeed} \nTime: {time}");

        Assert.AreEqual(1.7f, player.CurrentSpeed, 0.1f);
    }
}
