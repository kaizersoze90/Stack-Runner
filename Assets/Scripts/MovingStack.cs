using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingStack : MonoBehaviour
{
    public static MovingStack CurrentStack { get; private set; }
    public static MovingStack LastStack { get; set; }
    public static MovingStack StartStack { get; set; }

    Renderer _myRenderer;
    GameManager _gameManager;
    SuccessiveChecker _successiveChecker;
    CinemachineManager _cinemachineManager;
    ScoreKeeper _scoreKeeper;
    StackSpawner _spawner;
    UIManager _UIManager;

    float _stackSpeed;
    float _perfectTapTolerance;

    void Awake()
    {
        if (LastStack == null)
        {
            LastStack = this;
        }

        if (StartStack == null)
        {
            StartStack = this;
        }
    }

    void OnEnable()
    {
        _cinemachineManager = FindObjectOfType<CinemachineManager>();
        _successiveChecker = FindObjectOfType<SuccessiveChecker>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
        _gameManager = FindObjectOfType<GameManager>();
        _spawner = FindObjectOfType<StackSpawner>();
        _UIManager = FindObjectOfType<UIManager>();
        _myRenderer = GetComponent<Renderer>();

        if (gameObject.CompareTag("MovingStack"))
        {
            _myRenderer.material.color = GetRandomColor();
        }

        CurrentStack = this;

        //Adjust X scale size according to last stack
        transform.localScale = new Vector3(LastStack.transform.localScale.x,
                                            transform.localScale.y, transform.localScale.z);
    }

    void Start()
    {
        if (gameObject.CompareTag("MovingStack"))
        {
            _stackSpeed = _spawner.GetStackSpeed();
            _perfectTapTolerance = _spawner.GetTolerance();
        }
    }

    void Update()
    {
        transform.position += transform.right * _stackSpeed * Time.deltaTime;
    }

    public void ProccesSlice()
    {
        //Stop current stack
        _stackSpeed = 0f;

        //Calculate surplus piece according to last stack
        float remaining = transform.position.x - LastStack.transform.position.x;

        //If miss the last stack then game over
        if (Mathf.Abs(remaining) >= LastStack.transform.localScale.x)
        {
            GameOver();
            return;
        }
        //If perfectly tapped, process perfect situations
        else if (Mathf.Abs(remaining) < _perfectTapTolerance)
        {
            PerfectFit();
            _scoreKeeper.IncrementScore();
            return;
        }

        _scoreKeeper.IncrementScore();

        //Calculate on which side that piece will be sliced
        float direction = remaining > 0 ? 1f : -1f;

        SliceStack(remaining, direction);
    }

    void GameOver()
    {
        _gameManager.ProcessFail();

        LastStack = null;
        CurrentStack = null;

        gameObject.AddComponent<Rigidbody>();
    }

    void PerfectFit()
    {
        _scoreKeeper.IncrementScore();      //for double score
        _successiveChecker.PlayPerfectSound();
        _UIManager.PlayPerfectText();
        _cinemachineManager.ShakeCamera();

        //Align current stack to the last stack exact position
        transform.position = new Vector3(LastStack.transform.position.x,
                                            transform.position.y, transform.position.z);
        LastStack = this;
    }


    void SliceStack(float remaining, float direction)
    {
        _successiveChecker.PlayNormalSound();

        //Calculate new size and position for current stack like it got sliced
        float newXSize = LastStack.transform.localScale.x - Mathf.Abs(remaining);
        float newXPos = LastStack.transform.position.x + (remaining / 2);

        //Calculate X scale of piece that will fall
        float fallingStackSize = transform.localScale.x - newXSize;

        //Apply calculated size and position to stack to pretend it sliced
        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

        LastStack = this;

        //Calculate edge of stack and position for falling piece
        float stackEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingStackXPos = stackEdge + fallingStackSize / 2f * direction;

        //Pass calculated info for instantiate falling piece
        SpawnFallingStack(fallingStackXPos, fallingStackSize);
    }

    void SpawnFallingStack(float fallingStackXPos, float fallingStackSize)
    {
        //Create a cube
        var stack = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Re-adjust size and position with passed info for falling piece
        stack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        stack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);

        //Add rigidbody to make it drop and match the color with main stack
        stack.AddComponent<Rigidbody>();
        stack.GetComponent<Renderer>().material.color = _myRenderer.material.color;

        Destroy(stack, 2f);
    }

    Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}
