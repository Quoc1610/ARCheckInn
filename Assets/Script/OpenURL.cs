using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenWebURL(string url)
    {
        Application.OpenURL(url);
    }
}
