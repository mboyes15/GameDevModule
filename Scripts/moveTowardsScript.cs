using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowardsScript : MonoBehaviour, Interactable
{
    public GameObject NPC;
    public GameObject Player;
    public float speed;

    public float stopDistance = 1f;

    public GameObject NPCScreen;

    private Animator animator;


    private void Awake(){
        animator = GetComponent<Animator>();
    }


    private bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

     public void Interact()
    {
        Debug.Log("NPC Tutorial");
        NPCScreen.SetActive(true);
      
    }

    public void DialogClose(){
        Debug.Log("CLOSE THE DIALOG");
        NPC.SetActive(false);
        NPCScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         // Calculate the distance between the NPC and the player
        float distance = Vector3.Distance(NPC.transform.position, Player.transform.position);
         animator.SetBool("isMoving", !stop);

        // Only move the NPC if it is farther than the stop distance
        if (distance > stopDistance && !stop)
        {
            // Calculate direction from NPC to Player
            Vector3 direction = (Player.transform.position - NPC.transform.position).normalized;

            // Set the moveX and moveY parameters based on the direction
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            NPC.transform.position = Vector3.MoveTowards(NPC.transform.position, Player.transform.position, speed);

 
        }
        else{
            stop = true;
        }
        
    }
}
