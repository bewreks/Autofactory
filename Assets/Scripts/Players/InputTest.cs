using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public void OnEye(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }
}
