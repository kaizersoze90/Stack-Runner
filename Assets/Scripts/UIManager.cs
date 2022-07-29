using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements & Settings")]
    [SerializeField] Animator perfectTapText;
    [SerializeField] Button continueButton, startButton;
    [SerializeField] TextMeshProUGUI headerText, scoreText;

    public void PlayPerfectText()
    {
        perfectTapText.SetTrigger("isPerfect");
    }

    public void SetContinueButton(bool state)
    {
        continueButton.gameObject.SetActive(state);
    }

    public void SetStartButton(bool state)
    {
        startButton.gameObject.SetActive(state);
    }

    public void SetHeaderText(bool state)
    {
        headerText.gameObject.SetActive(state);
    }

    public void SetScoreText(bool state)
    {
        scoreText.gameObject.SetActive(state);
    }
}
