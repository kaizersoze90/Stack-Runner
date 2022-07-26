using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] StackSpawner spawner;
    [SerializeField] AudioClip failSFX;
    [SerializeField] float reloadDelay;

    public bool gameActive = true;

    void Update()
    {
        if (!gameActive) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovingStack.CurrentStack.ProccesSlice();

            spawner.SpawnStack();
        }

    }

    public void ProcessGameOver()
    {
        StartCoroutine(nameof(GameOver));
    }

    IEnumerator GameOver()
    {
        AudioSource.PlayClipAtPoint(failSFX, Camera.main.transform.position);

        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
