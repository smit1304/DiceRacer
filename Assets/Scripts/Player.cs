using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int id;
    public int position = 0;
    public int destination;
    //public Button dice_button;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WinnerPoint"))
        {
            if (id == 1)
            {
                GameManager.instance.Winner_sound.Play();
                GameManager.instance.WinScreen.SetActive(true);
                Debug.Log(GameManager.instance.turn + " is winner!");
                GameManager.instance.WinnerName.text = "Player 01";
                GameManager.instance.GAMEOVER = true;
                GameManager.instance.player_one_dice.GetComponent<Button>().interactable = false;
                GameManager.instance.player_two_dice.GetComponent<Button>().interactable = false;
                Time.timeScale = 0;
            }
            else
            {
                GameManager.instance.Winner_sound.Play();
                GameManager.instance.WinScreen.SetActive(true);
                Debug.Log(GameManager.instance.turn + " is winner!");
                GameManager.instance.WinnerName.text = "Player 02";
                GameManager.instance.GAMEOVER = true;
                GameManager.instance.player_one_dice.GetComponent<Button>().interactable = false;
                GameManager.instance.player_two_dice.GetComponent<Button>().interactable = false;
                Time.timeScale = 0;
            }
        }
    }
    
}
