using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float shotSpeed;

    public Rigidbody player;

    private Vector2 moveInput;
    private Vector2 moveVelocity;

    private float horizontal;
    private float direction;
    public bool lightDirection = false;
    public bool cueBallIsShot = false;

    public EightBall eightBall;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
        EightBall eightBall = GetComponent<EightBall>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) //tør ikke at bruge "while", har crashet alt for mange gange
        {
            lightDirection = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            lightDirection = false;
        }

        //rotates the cue ball
        player.transform.Rotate(0f, direction, 0f);

        //project a vector 100 units TO THE LEFT of the cue ball
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 100f;

        //shows the raycast line in Unity Viewer
        Debug.DrawRay(transform.position, forward, Color.green);
    }

    //kan måske bruges til hvis man skyder cue ball i hul
    //public void Move(InputAction.CallbackContext context)
    //{
    //    horizontal = context.ReadValue<Vector2>().x;
    //    vertical = context.ReadValue<Vector2>().y;
    //}

    //checking every balls position every frame is very memory heavy, so im just saying that a turn last 4 seconds
    //this does raise some issues, since some balls take longer than that to go down the hole and get registrered...
    IEnumerator UpdateBallMovement()
    {
        Debug.Log("waiting,,,");
        yield return new WaitForSeconds(4f);
        Debug.Log("waited");
        cueBallIsShot = false;

        eightBall.ShotIsOver();
    }

    //tracks the right/left arrow input to rotate the cue ball and the attached aimline
    public void Aim(InputAction.CallbackContext context)
    {
        if (!lightDirection)
        {
            direction = (context.ReadValue<Vector2>().x) / 5f;
        }
        else if (lightDirection)
        {
            direction = (context.ReadValue<Vector2>().x) / 100f;
        }

        //use RayCast to project a aiming line?
        //use RayCastCollision(?) to show small direction change
    }

    //projectiles the cue ball forwards in the direction of the direction value in Aim()
    public void Shoot(InputAction.CallbackContext context)
    {

        if (context.performed && !cueBallIsShot)
        {
            player.AddForce(player.transform.forward * shotSpeed, ForceMode.Impulse);

            cueBallIsShot = true;
            StartCoroutine(UpdateBallMovement());

            Debug.Log("i mean. you shot your shot");
        }

        //have a slider to adjust the power of the shot
        //use up/down (W/S) or mouse + slider
    }

    //allows for information about collision to be used in the ruleset in the other script
    private void OnCollisionEnter(Collision other)
    {
        //check if shots are legal (different for each gamemode)

        //checks ball hits for 8-Ball
        if (!eightBall.anyBallHit && (other.gameObject.CompareTag("Green") && eightBall.playerGreenTurn || other.gameObject.CompareTag("Red") && eightBall.playerRedTurn))
        {
            eightBall.firstHitLegal = true;
            eightBall.anyBallHit = true;
        }
        else if (!eightBall.anyBallHit && (eightBall.playerGreenTurn && !other.gameObject.CompareTag("Green") || eightBall.playerRedTurn && !other.gameObject.CompareTag("Red"))) //this makes any breakshot illegal...
        {
            eightBall.illegalMove = true;
        }
        else if (!eightBall.firstHitLegal && (eightBall.playerGreenTurn && !eightBall.playerGreenMatchball && other.gameObject.CompareTag("Black Ball") || eightBall.playerRedTurn && !eightBall.playerRedMatchball && other.gameObject.CompareTag("Black Ball")))
        {
            eightBall.illegalMove = true;
            eightBall.anyBallHit = true;
        }
    }
}
