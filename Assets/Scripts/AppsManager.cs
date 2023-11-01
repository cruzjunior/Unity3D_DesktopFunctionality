using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsManager : MonoBehaviour
{
    /// <summary>
    /// The list of apps in the computer
    /// </summary>
    [SerializeField] private List<GameObject> apps;

    void Start()
    {
        // sets all apps to inactive at the start of the game
        foreach (GameObject app in apps)
        {
            app.SetActive(false);
        }
    }
    /// <summary>
    /// Opens the app with the given name
    /// </summary>
    /// <param name="appName"></param>
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
