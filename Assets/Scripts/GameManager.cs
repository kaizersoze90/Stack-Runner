using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] StackSpawner spawner;
    [SerializeField] PlayerController player;
    [SerializeField] SuccessiveChecker successChecker;
    [SerializeField] UIManager uiManager;

    [Header("General Settings")]
    [SerializeField] AudioClip failSFX;
    [SerializeField] float reloadDelay, continueDelay;

    public bool gameActive;

    void Awake()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!gameActive) { return; }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            MovingStack.CurrentStack.ProccesSlice();

            spawner.SpawnStack();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        gameActive = true;

        spawner.gameObject.SetActive(true);
        uiManager.SetScoreText(true);
        uiManager.SetHeaderText(false);
        uiManager.SetStartButton(false);
    }

    public void ContinueGame()
    {
        gameActive = true;

        spawner.GetReadyForNextLevel();
        player.GetReadyForNextlevel();
        successChecker.ResetPitch();

        uiManager.SetContinueButton(false);
    }

    public void ProcessVictory()
    {
        StartCoroutine(nameof(Victory));
    }

    public void ProcessGameOver()
    {
        if (!gameActive) { return; }

        gameActive = false;

        StartCoroutine(nameof(GameOver));
    }

    IEnumerator GameOver()
    {
        AudioSource.PlayClipAtPoint(failSFX, Camera.main.transform.position, 0.4f);

        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Victory()
    {
        gameActive = false;

        yield return new WaitForSeconds(continueDelay);

        uiManager.SetContinueButton(true);
    }
}
