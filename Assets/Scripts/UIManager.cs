using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements & Settings")]
    [SerializeField] Animator perfectTapText;
    [SerializeField] Button continueButton;
    [SerializeField] Button startButton;
    [SerializeField] Button rewardButton;
    [SerializeField] Button restartButton;
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

    public void SetRewardButton(bool state)
    {
        rewardButton.gameObject.SetActive(state);
    }
    public void SetRestartButton(bool state)
    {
        restartButton.gameObject.SetActive(state);
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
