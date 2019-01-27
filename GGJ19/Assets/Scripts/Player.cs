using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public bool canMove;
    Rigidbody rb;
    Refugee refugee;
    public Refugee RefugeeRef{
        get { return refugee; }
        set { refugee = value; }
    }
    Coroutine interact;
    MovementController mController;
    public float timeInteracting;

    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        mController = GetComponent<MovementController>();
    }

    void Update()
    {
        
        //if the gimmick is applied the player can't interact with anything
        if (canMove)
        {

            //This controls the interactions with objects
            if (Input.GetButtonDown("BtnA" + playerId) && refugee != null )
            {
                interactWithRef();
            }

            

        }

    }
    void interactWithRef()
    {
        if (interact!=null)
        {
            StopCoroutine(interact);
        }
        interact = StartCoroutine(InteractRoutine());
    }

    IEnumerator InteractRoutine()
    {
        anim.SetInteger("State",2);
        mController.SetCanMove = false;
        yield return new WaitForSeconds(timeInteracting);
        refugee.Go2Shelter();
        anim.SetInteger("State", 0);
        mController.SetCanMove = true;
    }
}
