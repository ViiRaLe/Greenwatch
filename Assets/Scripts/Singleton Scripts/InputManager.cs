using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private bool cancelButton;

    private bool actionButtonP1;
    private float horizontalAxisP1;
    private float verticalAxisP1;

    private bool actionButtonP2;
    private float horizontalAxisP2;
    private float verticalAxisP2;

    private bool inputEnabled = true;


    public bool CancelButton { get { return cancelButton; } }
    public bool ActionButtonP1 { get { return actionButtonP1; } }
    public float HorizontalAxisP1 { get { return horizontalAxisP1; } }
    public float VerticalAxisP1 { get { return verticalAxisP1; } }
    public bool ActionButtonP2 { get { return actionButtonP2; } }
    public float HorizontalAxisP2 { get { return horizontalAxisP2; } }
    public float VerticalAxisP2 { get { return verticalAxisP2; } }



    // get input
    private void Update()
    {
        if (inputEnabled)
        {
            cancelButton = Input.GetButtonDown("Cancel");

            actionButtonP1 = Input.GetButtonDown("FireP1");
            horizontalAxisP1 = Input.GetAxis("HorizontalP1");
            verticalAxisP1 = Input.GetAxis("VerticalP1");

            actionButtonP2 = Input.GetButtonDown("FireP2");
            horizontalAxisP2 = Input.GetAxis("HorizontalP2");
            verticalAxisP2 = Input.GetAxis("VerticalP2");
        }
    }

    // enable or disable input update
    public void Activate(bool enable)
    {
        InitializeInputs();
        inputEnabled = enable;
    }

    // initialize all inputs
    private void InitializeInputs()
    {
        cancelButton = false;

        actionButtonP1 = false;
        horizontalAxisP1 = 0;
        verticalAxisP1 = 0;

        actionButtonP2 = false;
        horizontalAxisP2 = 0;
        verticalAxisP2 = 0;
    }
}
