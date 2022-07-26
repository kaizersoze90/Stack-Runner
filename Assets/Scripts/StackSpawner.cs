using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPos;
    [SerializeField] GameObject stack;

    GameManager _gameManager;

    int _spawnIndex;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        SpawnStack();
    }

    public void SpawnStack()
    {
        if (!_gameManager.gameActive) { return; }

        _spawnIndex = _spawnIndex == 0 ? 1 : 0;

        GameObject createdStack = Instantiate(stack, spawnPos[_spawnIndex].position,
                                                Quaternion.identity);

        if (_spawnIndex == 1)
        {
            createdStack.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        transform.position += (new Vector3(0, 0, createdStack.transform.localScale.z));
    }

}
