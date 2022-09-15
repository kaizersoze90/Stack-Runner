using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CinemachineManager cinemachine;
    [SerializeField] StackSpawner spawner;

    [Header("General Settings")]
    [SerializeField] AudioClip victorySFX;
    [SerializeField] float moveSpeed, moveSpeedIncrement;

    GameManager _gameManager;
    Animator _myAnimator;

    bool _isVictory;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isVictory) { return; }
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killer"))
        {
            _gameManager.ProcessFail();
            cinemachine.ReleaseFollowCam();
        }
        else if (other.CompareTag("FinishLine"))
        {
            ProcessVictory();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ContinueLine"))
        {
            spawner.SpawnStack();
        }
    }

    public void GetReadyForNextlevel()
    {
        moveSpeed += moveSpeedIncrement;
        _isVictory = false;
        _myAnimator.SetBool("isDancing", false);
        cinemachine.SwitchCamera();
    }

    public void PrepareToContinue()
    {
        transform.position = spawner.SetAndGetPosition();

        cinemachine.SetFollowCam(transform);

        GameObject[] stacks = GameObject.FindGameObjectsWithTag("MovingStack");

        foreach (GameObject stack in stacks)
        {
            Destroy(stack.gameObject);
        }
    }

    void ProcessVictory()
    {
        _isVictory = true;
        AudioSource.PlayClipAtPoint(victorySFX, transform.position, 1f);
        _myAnimator.SetBool("isDancing", true);
        cinemachine.SwitchCamera();
    }
}
