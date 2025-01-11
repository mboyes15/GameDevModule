using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed;
   public float stepSize;

   public bool isMoving;

   public Vector2 input;

   private Animator animator;
   
   public LayerMask solidObjectsLayer;
   public LayerMask interactablesLayer;

   public float countdownTime = 120f; // Gameplay countdown timer
   public TextMeshProUGUI countdownText;

   public GameManagerScript gameManager;

   private bool gameOver;

   private void Awake()
   {
        animator = GetComponent<Animator>();
   }

   private void Start()
    {
        StartCoroutine(GameCountdown());
    }


   private void Update()
   {
       if (!isMoving)
       {
            // get the button being pushed
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
          //   Debug.Log("This is input.x" + input.x);
          //   Debug.Log("This is input.y" + input.y);
            

            if (input.x!=0) input.y = 0;
            if (input != Vector2.zero)
            {
                //  set the animator moveX and moveY to the input
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y);

                var targetPos = transform.position;
                targetPos.x += input.x * stepSize;
                targetPos.y += input.y * stepSize;

               // check if there are objects
               if (IsWalkable(targetPos)){
                // actually move the character
                StartCoroutine(Move(targetPos));
               }
            }

       } 

        animator.SetBool("isMoving", isMoving);

          // check if there is a puzzle there
        if (Input.GetKeyDown("space"))
          {
               InteractPuzzle();
          }
   }

   private IEnumerator GameCountdown()
   {
     float timeRemaining = countdownTime;

     while (timeRemaining > 0)
     {
          countdownText.text = "Time Remaining: " + Mathf.Ceil(timeRemaining);
          // Debug.Log("Time Remaining: " + timeRemaining);
          timeRemaining--;
          yield return new WaitForSeconds(1f);
     }
     if(!gameOver)
     {
          gameOver = true;
          // disable character input so that you can't move when the game is over
          stepSize = 0;
          countdownText.text = "Time's Up!";
          Debug.Log("Time's up! Game Over!");
          gameManager.GameOver();
     }

   }

   void InteractPuzzle()
   {
     var facingDir = new Vector3(animator.GetFloat("moveX"),animator.GetFloat("moveY"));
     // calculating the space infront of where the player is looking
     var interactPos = transform.position + facingDir;

     // Debug.DrawLine(transform.position,interactPos,Color.red,1f);

     var collider = Physics2D.OverlapCircle(interactPos,0.2f,interactablesLayer);
     if (collider!=null)
     {
          collider.GetComponent<Interactable>()?.Interact();
     }
   }

 
   IEnumerator Move(Vector3 targetPos)
   {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

   }

   private bool IsWalkable(Vector3 targetPos)
   {
     // if we are overlapping then return false
     if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactablesLayer)!=null)
     {
          return false;
     }
     return true;

   }
}
