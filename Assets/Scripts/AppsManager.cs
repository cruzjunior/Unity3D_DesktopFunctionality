using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> apps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
