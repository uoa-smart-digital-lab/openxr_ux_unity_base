/******************************************************************************************************************************************************
 * RotateToCamera
 * --------------
 *
 * 2021-08-25
 * 
 * Used by the heads down and heads up GUI - rotates the object it is placed on towards where the camera is positioned.
 * 
 * Roy Davies, Smart Digital Lab, University of Auckland.
 ******************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public Transform theCamera; // The GameObject containing the Camera
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private float initalYRotationOffset; // Used to keep the item in the same position relative to how it is placed in the editor.
    private bool moveOver = false; // Used to ensure movement only occurs if the head is turned past a certain point.
    private float smoothness = 0.05f; // Speed by which it rotates
    private float yVelocity = 0.0f; // Current velocity of rotation
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set up the variables ready to go.
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        // Get the initial offset
        initalYRotationOffset = transform.eulerAngles.y - theCamera.eulerAngles.y;
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Update the Y rotation value depending on how much the camera / head has turned.
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // The angle to move to
        float newAngle = theCamera.eulerAngles.y + initalYRotationOffset;

        // If the head has turned a lot, just flick it around immediately.
        // if (Mathf.Abs(newAngle - transform.eulerAngles.y) > 60)
        // {
        //     moveOver = false;
        //     transform.rotation = Quaternion.Euler(0, newAngle, 0);
        // }
        // If the head has turned a fair bit, move it fast
        // if (Mathf.Abs(newAngle - transform.eulerAngles.y) > 60)
        // {
        //     moveOver = true;
        //     smoothness = 0.03f;
        // }
        // If the head has turned just a little bit, move it slowly
        // if (Mathf.Abs(newAngle - transform.eulerAngles.y) > 20)
        // {
        //     moveOver = true;
        //     smoothness = 0.5f;
        // }
        // If the head has turned only a tiny bit, don't move it all - allows you to look at stuff in the UI without it moving away from view.
        if (Mathf.Abs(newAngle - transform.eulerAngles.y) < 15)
        {
            moveOver = false;
        }
        else
        {
            moveOver = true;
            smoothness = 0.5f;            
        }

        // Smoothly rotate around
        if (moveOver)
        {
            float floatingAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, newAngle, ref yVelocity, smoothness);
            transform.rotation = Quaternion.Euler(0, floatingAngle, 0);
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}