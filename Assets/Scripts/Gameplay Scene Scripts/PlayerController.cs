using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody body;

    [SerializeField]
    private float interactCastOffsetY = 0.1f;
    private InteractableObject lastHit;

    public string sexIdentifier;
    public Transform hand;
    private InHandItem itemInHand;
    
    public float playerSpeed = 1.0f;
    [SerializeField]
    private float[] playerWeight;
    private int weightIndex;

    private int playerNum = 0;
    private Vector3 deltaPosition = Vector3.zero;
    private bool actionButton;

    private event Action<int> OnWeightChange;

    public InHandItem ItemInHand
    {
        get { return itemInHand;  }
    }



    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // calculate new position and rotation based on player input and player number
        if (playerNum == 0)
        {
            //bug.DrawRay(new Vector3(transform.position.x, transform.position.y + interactCastOffsetY, transform.position.z), transform.forward, Color.blue);
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + interactCastOffsetY, transform.position.z), -transform.up, Color.blue);

            deltaPosition = new Vector3(InputManager.Instance.HorizontalAxisP1, 0, InputManager.Instance.VerticalAxisP1);
            actionButton = InputManager.Instance.ActionButtonP1;
        }
        else if (playerNum == 1)
        {
            //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + interactCastOffsetY, transform.position.z), transform.forward, Color.red);

            deltaPosition = new Vector3(InputManager.Instance.HorizontalAxisP2, 0, InputManager.Instance.VerticalAxisP2);
            actionButton = InputManager.Instance.ActionButtonP2;
        }

        // clamp delta position magnitude to 1.0f
        if (deltaPosition.sqrMagnitude > 1.0f)
        {
            deltaPosition = deltaPosition / deltaPosition.magnitude;
        }

        Debug.Log(deltaPosition);


        RaycastHit hit;
        //Vector3 rayStartPos = new Vector3(transform.position.x, transform.position.y + interactCastOffsetY, transform.position.z);

        bool rayHitted = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + interactCastOffsetY, transform.position.z), -transform.up, out hit /*, interactRange*/);

        if (rayHitted)
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (lastHit != null)
            {
                lastHit.Unlit();
                lastHit = null;
            }

            if (interactable != null)
            {
                lastHit = interactable;
                interactable.Highlight();
            }

            // check action input and interact
            if (actionButton)
            {
                if (interactable != null)
                {
                    interactable.Interact(this);
                }
                else
                {
                    interactable = hit.collider.GetComponentInParent<InteractableObject>();

                    if (interactable != null)
                    {
                        interactable.Interact(this);
                    }
                }
            }
        }
        else
        {
            if (lastHit != null)
            {
                lastHit.Unlit();
            }
        }
    }

    private void FixedUpdate()
    {
        // move and rotate rigidbody
        body.MovePosition(transform.position + deltaPosition * playerSpeed * playerWeight[weightIndex] * Time.deltaTime);

        if (deltaPosition != Vector3.zero)
        {
            body.MoveRotation(Quaternion.Euler(Vector3.up * Mathf.Rad2Deg * Mathf.Atan2(deltaPosition.x, deltaPosition.z)));
        }
    }

    // change current item
    public void ChangeItem(GameObject item)
    {
        if (itemInHand != null)
        {
            itemInHand.UnregisterOnWeightChange(ChangeWeight);
            Destroy(itemInHand.gameObject);
        }
        
        itemInHand = Instantiate(item, hand).GetComponent<InHandItem>();
        itemInHand.RegisterOnWeightChange(ChangeWeight);
    }

    // change current weight
    public void ChangeWeight(int charges)
    {
        weightIndex = charges;
        OnWeightChange?.Invoke(weightIndex);
    }

    // register on weight change event
    public void RegisterOnWeightChange(Action<int> action)
    {
        OnWeightChange += action;
        OnWeightChange?.Invoke(weightIndex);
    }

    // unregister on weight change event
    public void UnregisterOnWeightChange(Action<int> action)
    {
        OnWeightChange?.Invoke(weightIndex);
        OnWeightChange -= action;
    }

    // set player number
    public void SetNumber(int number)
    {
        playerNum = number;
    }
}
