using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OwnerAI : MonoBehaviour
{
    private enum OwnerStates
    {
        AFK,
        Active,
        OnAlert
    }

    [Header("Owner's current state")]
    [SerializeField] private OwnerStates currentState;

    // coroutine variables
    [HideInInspector] public Coroutine switchToActiveCoroutine;
    [HideInInspector] public Coroutine switchToAFKCoroutine;
    [HideInInspector] public Coroutine antiVirusCountdownCoroutine;

    [Header("AFK time length")]
    public float minAFKTime;
    public float maxAFKTime;

    [Header("Active time length")]
    public float minActiveTime;
    public float maxActiveTime;

    // misc
    [Header("Alerted Variables")]
    public bool isOnAlert;

    //[Header("misc unlabled")]
    //private List<Application> apps = new List<Application>();

    // list of favorite applications

    void Start()
    {
        currentState = OwnerStates.AFK;
    }

    void Update()
    {
        switch (currentState) 
        { 
            case OwnerStates.AFK:
                UpdateAFK(); break;
            case OwnerStates.Active:
                UpdateActive(); break;
            case OwnerStates.OnAlert:
                UpdateOnAlert(); break;
        }
    }

    // states
    private void UpdateAFK()
    {
        // reset switchToAFKCoroutine
        if (switchToAFKCoroutine != null) 
        {             
            StopCoroutine(switchToAFKCoroutine);
            switchToAFKCoroutine = null;
        }

        // starts countdown to switch to active state
        if (switchToActiveCoroutine == null)
        {            
            Debug.Log("Current State:" + currentState.ToString());
            switchToActiveCoroutine = StartCoroutine(SwitchToActiveCoroutine());
        }
    }
    private void UpdateActive()
    {
        // reset switchToActiveCoroutine
        if (switchToActiveCoroutine != null)
        {
            StopCoroutine(switchToActiveCoroutine);
            switchToActiveCoroutine = null;
        }

        // if owner isnt alerted to something goin on, proceed with usual routine
        if (!isOnAlert)
        {
            CheckOpenApplications();

            // start timer to switch
            if (switchToAFKCoroutine == null)
            {
                Debug.Log("Current State:" + currentState.ToString());
                switchToAFKCoroutine= StartCoroutine(SwitchToAFKCoroutine());
            }
        }
        else 
        {
            Debug.Log("Owner is on alert!!!");
            currentState = OwnerStates.OnAlert;
        }

        // if an application is open...

        // if an application is not open...
    }
    private void UpdateOnAlert()
    {
        // when owner notices something up with the computer, switch to this state

        // activate antivirus software
    }

    private void CheckOpenApplications()
    {
        // gets list of the applications open
        List<Application> apps = new List<Application>();

        Application[] openApps = FindObjectsOfType<Application>();
        foreach (Application openApp in openApps) 
        { 
            apps.Add(openApp);
        }

        if (apps.Count > 2) 
        {
            while (apps.Count > 1)
            {
                // get last app in list
                Application app = apps.First();

                app.gameObject.SetActive(false);
                apps.Remove(app);
            }
        }
    }

    // coroutines
    public IEnumerator SwitchToActiveCoroutine()
    {
        // switch to active state after random interval of time
        yield return new WaitForSeconds(Random.Range(minAFKTime, maxAFKTime));

        currentState = OwnerStates.Active;
    }
    public IEnumerator SwitchToAFKCoroutine() 
    {
        yield return new WaitForSeconds(Random.Range(minActiveTime, maxAFKTime));

        currentState = OwnerStates.AFK;
    }
}
