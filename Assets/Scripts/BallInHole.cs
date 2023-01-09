using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallInHole : MonoBehaviour
{public bool greenBallPocketedFirst      = false;
    public bool redBallPocketedFirst        = false;
    public bool blackBallPocketed           = false;

    public int greenPoints;
    public int redPoints;

    public TextMeshProUGUI greenPointsText;
    public TextMeshProUGUI redPointsText;
    public TextMeshProUGUI gameOutcome;
    public TextMeshProUGUI matchPoint;

    public EightBall eightBall;

    // Start is called before the first frame update
    void Start()
    {
        EightBall eightBall = GetComponent<EightBall>(); //to get match ball information
    }

    //is called whenever the cue ball hits another ball
    void OnTriggerEnter(Collider other)
    {
        //when you pocket a Green ball
        if (other.gameObject.CompareTag("Green")) //disse kan sagtens forkortes
        {
            if (!eightBall.anyPocketHit)
            {
                greenBallPocketedFirst = true;
                eightBall.anyPocketHit = true;

                greenPoints++;
                greenPointsText.text = greenPoints.ToString();

                Debug.Log("You pocketed a Green Ball as the first ball!");
            }
            else
            {
                greenPoints++;
                greenPointsText.text = greenPoints.ToString();

                Debug.Log("You pocketed a Green Ball!");
            }

            if (!eightBall.playerGreenTurn && !eightBall.playerRedTurn)
            {
                eightBall.firstPocketLegal = true;
                eightBall.playerGreenTurn = true; //the bool automatically switches in another script, so this is the best fix

            }

            //there is a problem with balls jumping in and out of the hole and then counting as more than one point, but this is the best solution, 
            //other than calling Destroy on the pocketed ball, which is not that nice to look at (maybe you could just call Destroy on the balls collider. hmm..)
            if (greenPoints == 7) 
            {
                eightBall.playerGreenMatchball = true;
                matchPoint.text = "Green MatchBall";
            }
        }

        //when you pocket a Red ball
        if (other.gameObject.CompareTag("Red"))
        {
            if (!eightBall.anyPocketHit)
            {
                redBallPocketedFirst = true;
                eightBall.anyPocketHit = true;

                redPoints++;
                redPointsText.text = redPoints.ToString();

                Debug.Log("You pocketed a Red Ball as the first ball!");
            }
            else
            {
                redPoints++;
                redPointsText.text = redPoints.ToString();

                Debug.Log("You pocketed a Red Ball!");
            }

            if (!eightBall.playerGreenTurn && !eightBall.playerRedTurn)
            {
                eightBall.firstPocketLegal = true;
                eightBall.playerRedTurn = true; //the bool automatically switches in another script, so this is the best fix
            }

            if (redPoints == 7)
            {
                eightBall.playerRedMatchball = true;
                matchPoint.text = "Red MatchBall";
            }
        }

        //when you pocket the black ball
        if (other.gameObject.CompareTag("Black Ball"))
        {
            if (eightBall.firstHitLegal && !eightBall.cueInPocket && (eightBall.playerGreenTurn && eightBall.playerGreenMatchball || eightBall.playerRedTurn && eightBall.playerRedMatchball))
            {
                gameOutcome.text = "You Lose..."; //i think this is flipped because hitting the black ball is always an illegal move...
            }
            else
            {
                gameOutcome.text = "You Win!";
            }

            blackBallPocketed = true;

            Debug.Log("You pocketed the Black Ball!!");
        }

        //when you pocket the cue ball
        if (other.gameObject.CompareTag("Cue Ball"))
        {
            eightBall.cueInPocket = true;
            eightBall.PlaceCueBall();

            Debug.Log("You pocketed the Cue Ball!");
        }
    }
}
