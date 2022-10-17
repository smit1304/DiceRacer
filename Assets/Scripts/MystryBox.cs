using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MystryBox : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> MystyBox_transforn = new List<GameObject>();
    public Player player;
    private int Offset = 0;
    public int inputValueForPosition;
    public bool collided;

    public static MystryBox instance;

    private void Start()
    {
        if (!instance)
        {
            instance = FindObjectOfType<MystryBox>();

            if (!instance)
            {
                instance = new GameObject("CoroutineExecuter").AddComponent<MystryBox>();
            }
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0,1,0) * Time.deltaTime * 50);
        if (collided)
        {
            if (inputValueForPosition > 0)
            {
                _SpecialOne(player);

            }
            else if (inputValueForPosition < 0)
            {
                _SpecialTwo(player);
            }
        }
    }


    public IEnumerator _MoveBackwards(Player player, int dice_value, List<GameObject> position_list)
    {
        Debug.Log(player.position);
        player.position--;
        dice_value--;
        GameManager.instance.current_agent.SetDestination(position_list[player.position].transform.position);
        // set text
        if (GameManager.instance.turn == GameManager.instance.PLAYER_ONE)
        {
            GameManager.instance.Player_one_position.text = player.position.ToString() + "/80";

        }
        else
        {
            GameManager.instance.Player_two_position.text = player.position.ToString() + "/80";
        }
        Debug.Log("current destination is : " + player.position);

        if (dice_value > 0)
        {
            yield return new WaitUntil(() => GameManager.instance.ReachedDestinationOrGaveUp() == true);
            GameManager.instance.BonusInformation.text = "";
            StartCoroutine(_MoveBackwards(player, dice_value, position_list));
        }
        //GameManager.instance._CheckForWinner();
        if (!GameManager.instance.GAMEOVER && dice_value == 0 && !GameManager.moveThroughMystryBox)
        {
            yield return new WaitUntil(() => GameManager.instance.ReachedDestinationOrGaveUp() == true);
            GameManager.instance.Car_sound.Stop();
            GameManager.instance._SwitchTurn();
        }
    }

    public void _SpecialOne(Player player)
    {
        collided = false;
        GameManager.instance.Positive_bonus.Play();
        
        //if (inputValueForPosition < 0)
        //{
        //    inputValueForPosition *= -1;
        //}
        Debug.Log("you lucky mf");
        //increment in movement
        GameManager.moveThroughMystryBox = true;
        if(player.id == 1)
        {
            GameManager.instance.BonusInformation.text = "PlayerOne will move  " + inputValueForPosition + " Extra steps in Forward.";
            StartCoroutine(GameManager.instance._Move(player, inputValueForPosition, GameManager.instance.position_list_one));
        }
        else
        {
            GameManager.instance.BonusInformation.text = "PlayerTwo will move  " + inputValueForPosition + " Extra steps in Forward.";
            StartCoroutine(GameManager.instance._Move(player, inputValueForPosition, GameManager.instance.position_list_two));
        }
        //ameManager.moveThroughMystryBox = false;
    }
    public void _SpecialTwo(Player player)
    {
        collided = false;
        GameManager.instance.Negative_bonus.Play();
        if (inputValueForPosition < 0)
        {
            GameManager.moveThroughMystryBox = true;
            if (player.id == 1)
            {
                GameManager.instance.BonusInformation.text = "PlayerTwo will move  " + inputValueForPosition*-1 + " Extra steps in Backwards.";
                StartCoroutine(_MoveBackwards(player, inputValueForPosition*-1, GameManager.instance.position_list_one));
                //yield return new WaitForSeconds()
            }
            else
            {
                GameManager.instance.BonusInformation.text = "PlayerTwo will move  " + inputValueForPosition*-1 + " Extra steps in Backwards.";
                StartCoroutine(_MoveBackwards(player, inputValueForPosition*-1, GameManager.instance.position_list_two));
            }
        }
        //decrement in movement
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("collision detected");
            player = other.gameObject.GetComponent<Player>();
            //collided = true;
            if (player.destination ==
            gameObject.GetComponent<Position>().value)
            {
                collided = true; 
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            collided = false;
            if (player.destination ==
            gameObject.GetComponent<Position>().value)
            {
                this.gameObject.GetComponentInChildren<Canvas>().enabled = false;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            
        }
    }

}
