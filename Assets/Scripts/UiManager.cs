using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{

    public AudioSource onClickButtonSound;
    public AudioSource onClickDiceButtonSound;
    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void _Pause()
    {
        _ButtonClick();
        Time.timeScale = 0;
    }
    public void _Resume()
    {
        _ButtonClick();
        Time.timeScale = 1;
    }

    public void _Restart()
    {
        _ButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void _LoadLevel()
    {
        _ButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void _ExitGame()
    {
        _ButtonClick();
        Debug.Log("Play..");
        Application.Quit();
    }
    public void _LoadHomeMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void _ButtonClick()
    {
        onClickButtonSound.Play();
    }
    public void _DiceRolled()
    {
        onClickDiceButtonSound.Play();
    }
}
