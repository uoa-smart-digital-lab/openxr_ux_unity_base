/**********************************************************************************************************************************************************
 * XRGenericButtons
 * ----------------
 *
 * 2021-08-30
 *
 * A set of buttons where one or more (or none) can be pressed at a time.  In this code, you can either specify the buttons by hand in Unity by
 * creating rbuttons and adding them to the button group, or you can just fill in the Titles and have a list of buttons created dynamically at
 * runtime.  Or both.
 *
 * Roy Davies, Smart Digital Lab, University of Auckland.
 **********************************************************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Public functions
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public interface _XRButtonGroup
{
    void Input(XRData newData);
}
// ----------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Main class
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
[AddComponentMenu("OpenXR UX/Objects/XR Button Group")]
public class XRButtonGroup : MonoBehaviour, _XRButtonGroup
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    [Header("____________________________________________________________________________________________________")]
    [Header("A Group of Buttons\n\nAdd Buttons as children or create them dynamically below\n____________________________________________________________________________________________________")]
    [Header("INPUTS\n\n - Input( XRData ) - Integer value to change which button is selected as if it was being pressed.")]

    [Header("____________________________________________________________________________________________________")]
    [Header("SETTINGS")]
    [Header("Prefab to be used when creating dynamic Buttons.")]
    public GameObject buttonPrefab;    // Link to the button prefab
    [Header("Titles of Buttons to be created dynamically.\nThese will appear below the last Button child object.")]
    public string[] dynamicRadioButtons;    // Buttons that are created dynamically with the titles filled in from here.
    [Header("Vertical spacing between buttons.")]
    public float dynamicRadioButtonSpacing = 0.025f;

    [Header("____________________________________________________________________________________________________")]
    [Header("OUTPUTS")]
    public UnityXRDataEvent onChange;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variales
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private List<XRButton> allButtons; // The buttons that are children of this GameObject folowed by the buttons created dynamically.
    private bool firstTime = true;
    private int fixedItems = 0; // The number of fixed Items (ie the ones that are children of this GameObject)
    private int currentButton = 0;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set everything up
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        // Create the list
        allButtons = new List<XRButton>();

        // Find all the buttons in the children of this GameObject - these are the fixed items
        XRButton[] existingButtons = GetComponentsInChildren<XRButton>();
        foreach (XRButton buttonScript in existingButtons)
        {
            // Add this button to the list
            allButtons.Add(buttonScript);
        }

        // Store the number of fixed items in the list after adding the fixed items to the list.
        fixedItems = allButtons.Count;

        // Create the dynamic buttons.
        if (buttonPrefab == null)
        {
            Debug.Log("No RadioButton Prefab to duplicate");
        }
        else
        {
            int counter = 0;
            foreach (string title in dynamicRadioButtons)
            {
                // Create a new RadioButton
                GameObject newButton = Instantiate(buttonPrefab);
                // Get the transform to use as the grouping object
                Transform group = this.gameObject.transform;
                // Add the new button to the group
                newButton.transform.SetParent(group);
                // Position it relative to the zero position (going down from the top)
                newButton.transform.localPosition = new Vector3(0, (fixedItems + counter) * -dynamicRadioButtonSpacing, 0);
                newButton.transform.localRotation = new Quaternion(0, 0, 0, 1);
                // Set the title
                XRButton buttonScript = newButton.GetComponent<XRButton>();
                // Add it to the list of buttons already there.
                allButtons.Add(buttonScript);
                counter++;
            }
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set things up
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // Run this once after all the start functions in the other GameObjects have been run so that all the links etc are in the right places before
        // we set up the links to the listeners etc, text in titles etc.  Some GameObjects need to do it this way.
        if (firstTime)
        {
            // Set all the buttons off except the first one
            for (int counter = 0; counter < allButtons.Count; counter++)
            {
                allButtons[counter].Input(new XRData(counter == currentButton, true));
                int temp = counter; // Assign this to a temporary variable so the lambda functions below works wtih the correct parameter value
                allButtons[counter].onChange.AddListener((XRData theData) => { ButtonChange(theData, temp); });

                // Set the titles of the dynamic items
                if (counter >= fixedItems)
                    allButtons[counter].Title(dynamicRadioButtons[counter - fixedItems]);
            }
            firstTime = false;
        }
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // What to do when one of the buttons is pressed
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ButtonChange(XRData theData, int buttonNumber)
    {
        Set (buttonNumber);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Change the state of the given button
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Input(XRData theData)
    {
        Set (theData.ToInt(), theData.quietly);
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Set the appropriate button on, and the others off.
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Set(int buttonNumber, bool quietly = false)
    {
        buttonNumber = buttonNumber % allButtons.Count;
        for (int counter = 0; counter < allButtons.Count; counter++)
        {
            allButtons[counter].Input(new XRData(counter == buttonNumber, true));
        }

        if ((onChange != null) && !quietly) onChange.Invoke(new XRData(buttonNumber));        
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}
