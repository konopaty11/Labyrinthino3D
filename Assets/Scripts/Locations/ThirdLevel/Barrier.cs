using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Barrier : MonoBehaviour
{
    [SerializeField] BallColorType ballColorType;
    [SerializeField] bool isDetecting = true;

    public BallColorType BallColorType => ballColorType;

    string _playerTag = "Player";

    PlayerController _player;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_playerTag) || !isDetecting)
            return;

        if (_player == null)
            _player = other.GetComponent<PlayerController>();

        if (_player.CurrentColorType != ballColorType)
            SceneManager.LoadScene("ThirdLevel");
    }
}
