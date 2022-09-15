using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] Transform[] spawnPos;
    [SerializeField] GameObject stack, finishStack;

    [Header("Difficulty Settings")]
    [SerializeField] int levelLenght;
    [SerializeField] int levelIncrement;
    [SerializeField] float stackSpeed, stackSpeedIncrement;
    [SerializeField] float perfectTapTolerance;

    GameManager _gameManager;
    GameObject _startStack;
    Vector3 _finishPos;

    int _spawnIndex;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        //Generate level by editor input and place finish stack
        PlaceFinishStack();
    }

    public void SpawnStack()
    {
        if (!_gameManager.GameActive) { return; }

        //Check if stack spawner passed finish line
        if (transform.position.z > _finishPos.z - (finishStack.transform.localScale.z / 2))
        {
            _gameManager.ProcessVictory();
            return;
        }

        //Switch index number to spawn stacks in order left and rigth
        _spawnIndex = _spawnIndex == 0 ? 1 : 0;

        //Instantiate moving stack
        GameObject createdStack = Instantiate(stack, spawnPos[_spawnIndex].position,
                                                Quaternion.identity);

        //Re-adjust the rotation of instantiated stack according to left or right
        if (_spawnIndex == 1)
        {
            createdStack.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        //Move stack spawner forward according to instantiated stack size
        transform.position += (new Vector3(0, 0, createdStack.transform.localScale.z));
    }

    public void GetReadyForNextLevel()
    {
        MovingStack.LastStack = finishStack.GetComponent<MovingStack>();
        MovingStack.StartStack = _startStack.GetComponent<MovingStack>();

        //Increase difficulty
        levelLenght += levelIncrement;
        stackSpeed += stackSpeedIncrement;

        //Re-adjust spawner position for new level according to last finish stack
        transform.position = new Vector3(transform.position.x, transform.position.y,
                                       _finishPos.z + (finishStack.transform.localScale.z / 2) +
                                       (stack.transform.localScale.z / 2));

        //Re-generate level and re-place finish stack
        PlaceFinishStack();
    }

    public float GetStackSpeed()
    {
        return stackSpeed;
    }

    public float GetTolerance()
    {
        return perfectTapTolerance;
    }

    public Vector3 SetAndGetPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y,
                                       MovingStack.StartStack.transform.position.z
                                       + (MovingStack.StartStack.transform.localScale.z / 2)
                                       + (stack.transform.localScale.z / 2));

        return MovingStack.StartStack.transform.position;
    }

    void PlaceFinishStack()
    {
        //Calculate finish stack position according to generated level
        _finishPos = new Vector3(transform.position.x, transform.position.y,
                                 transform.position.z + (stack.transform.localScale.z * levelLenght) +
                                (finishStack.transform.localScale.z / 2) -
                                (stack.transform.localScale.z / 2));

        _startStack = Instantiate(finishStack, _finishPos, Quaternion.identity);
    }
}
