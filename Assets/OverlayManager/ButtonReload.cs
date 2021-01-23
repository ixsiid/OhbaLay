using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReload : MonoBehaviour
{
    public void OnClick()
    {
        Initializer.Instance.Reload();
    }
}
