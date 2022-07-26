using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] CinemachineVirtualCamera followCam;

    Rigidbody _myRigidbody;
    GameManager _gameManager;

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killer"))
        {
            _gameManager.gameActive = false;
            _gameManager.ProcessGameOver();

            followCam.Follow = null;
            followCam.LookAt = null;
        }
    }
}
