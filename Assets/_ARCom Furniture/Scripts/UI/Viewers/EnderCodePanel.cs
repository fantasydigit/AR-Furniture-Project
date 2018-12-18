using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polaris.Unity.Base.Utilities.UI;
using UnityEngine.UI;
public class EnderCodePanel : ViewableObject  {
    public Text codeText;
    public InputField inputField;

    private void OnEnable()
    {
        inputField.text = "";
    }

}
