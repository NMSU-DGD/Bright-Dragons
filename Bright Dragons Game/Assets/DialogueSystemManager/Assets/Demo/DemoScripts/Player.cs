using UnityEngine;
using System.Collections;
using DialogueSystem;

public class Player : MonoBehaviour
{
    private float speed { get; set; }
    private Rigidbody2D rigidBody { get; set; }
    private bool inInteractRange { get; set; }
    private string interactableNPCName { get; set; }

    void Awake()
    {
        speed = 5.50f;
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!DialogueSystemManager.Instance.DialogueSystemIsActive())
        {
            if (Input.GetKey(KeyCode.UpArrow))
                rigidBody.MovePosition(rigidBody.position + Vector2.up * speed * Time.fixedDeltaTime);
            else if (Input.GetKey(KeyCode.DownArrow))
                rigidBody.MovePosition(rigidBody.position + Vector2.down * speed * Time.fixedDeltaTime);
            else if (Input.GetKey(KeyCode.LeftArrow))
                rigidBody.MovePosition(rigidBody.position + Vector2.left * speed * Time.fixedDeltaTime);
            else if (Input.GetKey(KeyCode.RightArrow))
                rigidBody.MovePosition(rigidBody.position + Vector2.right * speed * Time.fixedDeltaTime);

            if (inInteractRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Used to start a dialogue conversation. Pass in the name of the starter you set for the conversation
                    DialogueSystemManager.Instance.StartDialogueForNpc(interactableNPCName);
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //This will unlock a conversation for a specific npc and a specific phase you want to unlock
                DialogueSystemManager.Instance.UnlockConversationsForNPCByPhase(interactableNPCName,1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        inInteractRange = true;
        interactableNPCName = col.gameObject.name;
        if (interactableNPCName == "Grave")
            //This call is to open a notification. Pass it the name of the starter you set in your notification node
            DialogueSystemManager.Instance.OpenNotification(interactableNPCName);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        inInteractRange = false;
        if (interactableNPCName == "Grave")
            //This call will close the last opened notification
            DialogueSystemManager.Instance.CloseNotification();
    }
}
