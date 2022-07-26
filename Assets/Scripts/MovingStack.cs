using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingStack : MonoBehaviour
{
    public static MovingStack CurrentStack { get; private set; }
    public static MovingStack LastStack { get; set; }

    [SerializeField] float stackSpeed;
    [SerializeField] float perfectFitTolerance;

    Renderer _myRenderer;
    GameManager _gameManager;
    SuccessiveChecker _successiveChecker;

    void Awake()
    {
        if (LastStack == null)
        {
            LastStack = this;
        }
    }

    void OnEnable()
    {
        _successiveChecker = FindObjectOfType<SuccessiveChecker>();
        _gameManager = FindObjectOfType<GameManager>();
        _myRenderer = GetComponent<Renderer>();
        _myRenderer.material.color = GetRandomColor();

        CurrentStack = this;

        transform.localScale = new Vector3(LastStack.transform.localScale.x,
                                            transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        transform.position += transform.right * stackSpeed * Time.deltaTime;
    }

    public void ProccesSlice()
    {
        stackSpeed = 0f;

        float remaining = transform.position.x - LastStack.transform.position.x;

        if (Mathf.Abs(remaining) >= LastStack.transform.localScale.x)
        {
            GameOver();
            return;
        }
        else if (Mathf.Abs(remaining) < perfectFitTolerance)
        {
            PerfectFit();
            return;
        }

        float direction = remaining > 0 ? 1f : -1f;

        SliceStack(remaining, direction);
    }

    void GameOver()
    {
        _gameManager.gameActive = false;
        _gameManager.ProcessGameOver();

        LastStack = null;
        CurrentStack = null;

        gameObject.AddComponent<Rigidbody>();
    }

    void PerfectFit()
    {
        _successiveChecker.PlayPerfectSound();

        transform.position = new Vector3(LastStack.transform.position.x,
                                            transform.position.y, transform.position.z);
        LastStack = this;
    }


    void SliceStack(float remaining, float direction)
    {
        _successiveChecker.PlayNormalSound();

        float newXSize = LastStack.transform.localScale.x - Mathf.Abs(remaining);
        float newXPos = LastStack.transform.position.x + (remaining / 2);

        float fallingStackSize = transform.localScale.x - newXSize;

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

        LastStack = this;

        float stackEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingStackXPos = stackEdge + fallingStackSize / 2f * direction;

        SpawnFallingStack(fallingStackXPos, fallingStackSize);
    }

    void SpawnFallingStack(float fallingStackXPos, float fallingStackSize)
    {
        var stack = GameObject.CreatePrimitive(PrimitiveType.Cube);

        stack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        stack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);

        stack.AddComponent<Rigidbody>();
        stack.GetComponent<Renderer>().material.color = _myRenderer.material.color;

        Destroy(stack, 2f);
    }

    Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}
