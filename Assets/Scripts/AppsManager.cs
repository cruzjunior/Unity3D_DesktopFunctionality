using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsManager : MonoBehaviour
{
    /// <summary>
    /// The list of apps in the computer
    /// </summary>
    [SerializeField] private List<GameObject> apps;

    // Update is called once per frame
    public void OpenApp(string appName)
    {
        foreach (GameObject app in apps)
        {
            if (app.name == appName)
            {
                app.SetActive(true);
            }
        }
    }
}
