using UnityEngine;
using UnityEngine.UI;
public class TestOpenPackage : MonoBehaviour
{
    public Button btnOpenPackage;

    private void Start()
    {
        btnOpenPackage.onClick.AddListener(OpenApp);
    }

    public void OpenApp()
    {
        string packageName = "npnlab.smart.algriculture.kiosskdashboard"; 
        Application.OpenURL($"android-app://{packageName}");
    }
}