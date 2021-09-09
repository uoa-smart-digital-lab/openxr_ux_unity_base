/**********************************************************************************************************************************************************
 * XRFloat
 * -------
 *
 * 2021-09-05
 *
 * Sends an Float when told to.
 *
 * Roy Davies, Smart Digital Lab, University of Auckland.
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Public functions
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public interface _XRFloat
{
    void Input();
}
// ----------------------------------------------------------------------------------------------------------------------------------------------------------



// ----------------------------------------------------------------------------------------------------------------------------------------------------------
// Main class
// ----------------------------------------------------------------------------------------------------------------------------------------------------------
public class XRFloat : MonoBehaviour, _XRFloat
{
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Public variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    [Header("____________________________________________________________________________________________________")]
    [Header("Generate a Float variable for XRData.\n____________________________________________________________________________________________________")]
    [Header("INPUTS\n\n - Input() - Trigger to activate function.")]

    [Header("____________________________________________________________________________________________________")]
    [Header("SETTINGS")]
    [Header("The Float variable.")]
    public float value;

    [Header("____________________________________________________________________________________________________")]
    [Header("OUTPUTS")]
    public UnityXRDataEvent onChange;
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Private variables
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------------------------------------------



    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    // Input values of on or off
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Input ()
    {
        if (onChange != null) onChange.Invoke(new XRData(value));
    }
    // ------------------------------------------------------------------------------------------------------------------------------------------------------
}
