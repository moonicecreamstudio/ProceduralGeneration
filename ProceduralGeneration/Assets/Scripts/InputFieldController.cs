using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    public TMP_InputField _inputField;

    void Start()
    {
        _inputField.characterLimit = 5;
    }
}
