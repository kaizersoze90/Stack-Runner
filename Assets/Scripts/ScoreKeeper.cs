using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    int _score;

    void Start()
    {
        scoreText.text = _score.ToString();
    }

    public void IncrementScore()
    {
        _score++;
        scoreText.text = _score.ToString();
    }
}
