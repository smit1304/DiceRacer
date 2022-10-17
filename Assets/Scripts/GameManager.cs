using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public Player current_player;
    public Player player_one;
    public Player player_two;

    public NavMeshAgent current_agent;
    public NavMeshAgent agent_one;
    public NavMeshAgent agent_two;

    public TMP_Text Current_player_position;
    public TMP_Text Player_one_position;
    public TMP_Text Player_two_position;

    //public Cinemachine. cam;
    public GameObject Player01Cam;
    public List<GameObject> position_list_one = new List<GameObject>();
    public List<GameObject> position_list_two = new List<GameObject>();
    public List<Sprite> Dice = new List<Sprite>();
    
    public string turn;
    public int dice_value;
    public GameObject player_one_dice;
    public GameObject player_two_dice;
    public GameObject WinScreen;
    public TMP_Text WinnerName;
    public TMP_Text BonusInformation;
    //public GameObject current_player;
    public bool GAMEOVER = false;
    public Image Dice_sprite;
    public int Random_dice_side;
    public int destination_index;
    public static GameManager instance;
    public static bool moveThroughMystryBox;
    public Animator anim_one;
    public Animator anim_two;
    public AudioSource Car_sound;
    public AudioSource Positive_bonus;
    public AudioSource Negative_bonus;
    public AudioSource Winner_sound;
    public string PLAYER_ONE = "playerone";
    public string PLAYER_TWO = "playertwo";
    
    //private vars
    int end_line = 10;


    private void Awake()
    {
        player_one = agent_one.transform.GetComponent<Player>();
        player_two = agent_two.transform.GetComponent<Player>();

        player_one_dice.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(_RollDice()); });
        player_two_dice.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(_RollDice()); });

        turn = PLAYER_ONE;
        current_player = player_one;
        current_agent = agent_one;  
        //end_line = position_list_one.Count;

        if (!instance)
        {
            instance = FindObjectOfType<GameManager>();

            if (!instance)
            {
                instance = new GameObject("CoroutineExecuter").AddComponent<GameManager>();
            }
        }
    }
    void Start()
    {
        anim_one.SetBool("Player01", false);
        anim_two.SetBool("Player02", false);
        for (int i = 0; i < position_list_one.Count; i++)
        {
            position_list_one[i].AddComponent<Position>().value = i;
            position_list_two[i].AddComponent<Position>().value = i;
        }
        Debug.Log(player_one.id);
        Debug.Log(player_two.id);
        _StartTurn();
    }

    public void _StartTurn()
    {
        Debug.Log(turn + "'s turn");
        //activate dice button
        if(turn == PLAYER_ONE)
        {
            player_one_dice.GetComponent<Button>().interactable = true;
        }
        else
        {
            player_two_dice.GetComponent<Button>().interactable = true;
        }
        
    }

    public IEnumerator _RollDice()
    {
        player_one_dice.GetComponent<Button>().interactable = false;
        player_two_dice.GetComponent<Button>().interactable = false;
        
        for (int i = 0; i <= 20; i++)
        {
            dice_value = Random.Range(1,6);
            Dice_sprite.sprite = Dice[dice_value];
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("dice value is : "+dice_value);
        Car_sound.Play();
        if (turn == PLAYER_ONE)
        {
            player_one_dice.GetComponent<Button>().interactable = false;
            destination_index = player_one.position + dice_value;
            player_one.destination = destination_index;
            StartCoroutine(_Move(player_one, dice_value, position_list_one));
        }
        else
        {
            player_two_dice.GetComponent<Button>().interactable = false;
            Debug.Log("dice value is : " + dice_value);
            destination_index = player_two.position + dice_value;
            player_two.destination = destination_index;
            StartCoroutine(_Move(player_two,dice_value, position_list_two));
        }
    }

    public IEnumerator _Move(Player player,int dice_value, List<GameObject> position_list)
    {

        player.position++;
        dice_value--;
        current_agent.SetDestination(position_list[player.position].transform.position);
        // set text
        if (turn == PLAYER_ONE)
        {
            Player_one_position.text = player.position.ToString() + "/80";
            
        }
        else
        {
            Player_two_position.text = player.position.ToString() + "/80";
        }
        Debug.Log("current destination is : " + player.position);
        if (dice_value > 0)
        {
            yield return new WaitUntil(() => ReachedDestinationOrGaveUp() == true);
            instance.BonusInformation.text = "";
            StartCoroutine(_Move(player, dice_value, position_list));
        }
        
        if (!GAMEOVER && dice_value == 0 && !moveThroughMystryBox)
        {
            yield return new WaitUntil(() => ReachedDestinationOrGaveUp() == true);
            Car_sound.Stop();
            _SwitchTurn();
        }
    }

   
    public void _SwitchTurn()
    {
        if (turn == PLAYER_ONE)
        {
            player_one_dice.GetComponent<Button>().interactable = false;
            turn = PLAYER_TWO;
            Player01Cam.SetActive(false);
            current_player = player_two;
            current_agent = agent_two;
            Current_player_position = Player_two_position;
        }
        else
        {
            player_two_dice.GetComponent<Button>().interactable = false;
            turn = PLAYER_ONE;
            Player01Cam.SetActive(true);
            current_player = player_one;
            current_agent = agent_one;
            Current_player_position = Player_one_position;
        }
        GameManager.moveThroughMystryBox = false;
        //yield return new WaitForSeconds(2f);
        _StartTurn();
    }

    //public void _CheckForWinner()
    //{
    //    if (turn == PLAYER_ONE)
    //    {
    //        if(player_one.position >= end_line)
    //        {
    //            Winner_sound.Play();
    //            WinScreen.SetActive(true);
    //            Debug.Log(turn + " is winner!");
    //            WinnerName.text = "Player 01";
    //            GAMEOVER = true;
    //            player_one_dice.GetComponent<Button>().interactable = false;
    //            player_two_dice.GetComponent<Button>().interactable = false;
    //            Time.timeScale = 0;
    //        }
    //    }
    //    else
    //    {
    //        if (player_two.position >= end_line)
    //        {
    //            Winner_sound.Play();
    //            WinScreen.SetActive(true);
    //            Debug.Log(turn + " is winner!");
    //            WinnerName.text = "Player 02";
    //            GAMEOVER = true;
    //            player_one_dice.GetComponent<Button>().interactable = false;
    //            player_two_dice.GetComponent<Button>().interactable = false;
    //            Time.timeScale = 0;
    //        }
    //    }
    //}


    public bool ReachedDestinationOrGaveUp()
    {

        if (!current_agent.pathPending)
        {
            if (current_agent.remainingDistance <= current_agent.stoppingDistance)
            {
                if (!current_agent.hasPath || current_agent.velocity.sqrMagnitude == 0f)
                {
                   
                    
                    return true;
                }
            }
        }
        return false;
    }


    private void Update()
    {
        ReachedDestinationOrGaveUp();
        
        if(agent_one.velocity.magnitude  <= 0.1)
        {
            anim_one.SetBool("Player01", false);
            //anim_two.SetBool("Player02", false);
        }
        else
        {
            anim_one.SetBool("Player01", true);
        }

        if (agent_two.velocity.magnitude <= 0.1)
        {
            anim_two.SetBool("Player02", false);
            //anim_two.SetBool("Player02", false);
        }
        else
        {
            anim_two.SetBool("Player02", true);
        }


    }
}
