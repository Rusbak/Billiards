using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EightBall : MonoBehaviour
{
    public TextMeshProUGUI playerTurn;

    //these are the bools used for the ruleset
    public bool playerGreenTurn    = false; //used by other script
    public bool playerRedTurn      = false; //used by other script
    public bool firstHitLegal          = false; //used by other script
    public bool firstPocketLegal       = false; //changed by other script
    public bool playerGreenMatchball   = false; //used by other script
    public bool playerRedMatchball     = false; //used by other script
    public bool cueInPocket            = false; //changed and used by other script
    public bool blackInPocket          = false; //changed by other script //not necessary?
    public bool anyBallHit             = false;
    public bool anyPocketHit           = false; //changed by other script
    public bool illegalMove     = false;
    public bool ballsAreMoving  = false;

    private Vector3 ballPos0, ballPos1, ballPos2, ballPos3, ballPos4, ballPos5, ballPos6, ballPos7, ballPos8, ballPos9, ballPos10, ballPos11, ballPos12, ballPos13, ballPos14, ballPos15;
    public GameObject[] balls;
    public GameObject whiteBall;

    public Player cueBall;
    public BallInHole ballInHole;

    void Start()
    {
        BallInHole ballInHole = GetComponent<BallInHole>();
        Player cueBall = GetComponent<Player>();

        PlaceBalls();
    }

    public void PlaceBalls()
    {
        //since the ballposition is placed with math setting the ballsize means that they wont spawn inside of each other
        float ballSize = 5.5f;

        //    #
        //
        //
        //
        //    #
        //   # #
        //  # # #
        // # # # #
        //# # # # #

        //could maybe put positions in an array, but that would just mean the same amount of code somewhere else
        whiteBall.transform.position = new Vector3(50, 3.75f, 0); //cue ball

        balls[0].transform.position  = new Vector3(-50, 3.75f, 0);

        balls[1].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (1 * ballSize)), 3.75f, 0.5f   * ballSize);
        balls[2].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (1 * ballSize)), 3.75f, -0.5f  * ballSize);

        balls[3].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (2 * ballSize)), 3.75f, 1      * ballSize);
        balls[4].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (2 * ballSize)), 3.75f, 0      * ballSize); //black
        balls[5].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (2 * ballSize)), 3.75f, -1     * ballSize);

        balls[6].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (3 * ballSize)), 3.75f, 1.5f   * ballSize);
        balls[7].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (3 * ballSize)), 3.75f, 0.5f   * ballSize);
        balls[8].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (3 * ballSize)), 3.75f, -0.5f  * ballSize);
        balls[9].transform.position  = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (3 * ballSize)), 3.75f, -1.5f  * ballSize);

        balls[10].transform.position = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (4 * ballSize)), 3.75f, 2      * ballSize);
        balls[11].transform.position = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (4 * ballSize)), 3.75f, 1      * ballSize);
        balls[12].transform.position = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (4 * ballSize)), 3.75f, 0      * ballSize);
        balls[13].transform.position = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (4 * ballSize)), 3.75f, -1     * ballSize);
        balls[14].transform.position = new Vector3(-50 - (Mathf.Sin((60 * Mathf.PI) / 180) * (4 * ballSize)), 3.75f, -2     * ballSize);

        playerTurn.text = "Break Shot";
    }

    //is called whenever the cue ball is pocketed
    public void PlaceCueBall()
    {
        whiteBall.transform.position = new Vector3(50, 3.75f, 0);
    }

    void Update()
    {
        //would have liked to check if balls are moving, so the player can only shoot when all balls are still
        //CheckBallMovement();
    }

    //whenever the shot is over, rules are checked through
    public void ShotIsOver()
    {
        if (!cueBall.cueBallIsShot)
        {
            if (illegalMove || !firstHitLegal || !firstPocketLegal || !anyBallHit || cueInPocket) //get cueInPocket from detection script
            {
                SwitchPlayerTurn();
            }
            else if (blackInPocket && (!playerGreenMatchball || !playerRedMatchball) || blackInPocket && cueInPocket) //get blackInPocket information from detector script
            {
                ballInHole.gameOutcome.text = "You Lose...";
                Debug.Log("YOU LOSE!!");
            }
            else if (blackInPocket && playerGreenMatchball && playerGreenTurn || blackInPocket && playerRedMatchball && playerRedTurn) //handle this from the other script?
            {
                ballInHole.gameOutcome.text = "You Win!";
                Debug.Log("YOU WIN!!");
            }
        }
    }

    private void SwitchPlayerTurn()
    {
        if (playerGreenTurn)
        {
            playerGreenTurn = false;
            playerRedTurn = true;

            playerTurn.text = "Red Turn";
        }
        else if (playerRedTurn)
        {
            playerRedTurn = false;
            playerGreenTurn= true;

            playerTurn.text = "Green Turn";
        }
        firstHitLegal = false;
        cueInPocket   = false;
        anyBallHit    = false;
        anyPocketHit  = false;
        illegalMove   = false;
    }
}
