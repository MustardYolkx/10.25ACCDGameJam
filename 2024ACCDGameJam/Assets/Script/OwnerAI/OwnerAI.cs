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
    //[Header("Alerted Variables")]
    //public bool isOnAlert;

    [Header("References")]
    public AntivirusSystem antivirusSystem;
    public GameRoot gameRoot;
    public Animator animator;

    void Start()
    {
        currentState = OwnerStates.AFK;

        if ( antivirusSystem == null)
        {
            antivirusSystem = FindObjectOfType<AntivirusSystem>();
        }
        if ( gameRoot == null)
        {
            gameRoot = GameRoot.GetInstance();
        }
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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

        // check if virus is running (visibly)
        IsVirusRunning();

        // if owner isnt alerted to something goin on, proceed with usual routine
        if (!IsVirusRunning())
        {
            // start timer to switch back to AFK
            if (switchToAFKCoroutine == null)
            {
                Debug.Log("Current State:" + currentState.ToString());
                switchToAFKCoroutine= StartCoroutine(SwitchToAFKCoroutine());
            }

            // do regular business
        }
        else 
        {
            Debug.Log("Owner is on alert!!!");
            animator.SetTrigger("Alert");
            currentState = OwnerStates.OnAlert;
        }
    }
    private void UpdateOnAlert()
    {
        // activate antivirus software
        antivirusSystem.RunAntivirus(gameRoot.computerFile_Dictionary);
    }
    // functional yaaaaaay party
    private bool IsVirusRunning()
    {
        //GameRoot gameRoot = GameRoot.GetInstance();
        bool virusFound = false;

        if (gameRoot.currentOpenFile_Dictionary != null)
        {
            Dictionary<string, GameObject>.ValueCollection valueColl = gameRoot.currentOpenFile_Dictionary.Values;

            // Traverse all files and look for infected files (cannot access foreach loop)
            foreach (GameObject entry in valueColl)
            {
                //GameObject fileObject = entry;
                PageInfo fileComponent = entry.GetComponent<PageInfo>();

                if (fileComponent == null)
                {
                    Debug.Log("fileComponent null!! cannot find PageInfo script");
                }

                // Check if the file is currently infected
                if (fileComponent != null && fileComponent.fileInfo.hasVirus)
                {
                    virusFound = true;
                }
                else { virusFound = false; }
            }

            if (virusFound) {Debug.Log("virus detected---return true"); return true; }    
            else { return false; }
        }
        else { return false; }
    }

    public void AntivirusTaskCompleted(bool taskComplete)
    {
        if (!taskComplete) 
        {
            return;
        }
        else
        {
            animator.SetTrigger("Active");
            currentState = OwnerStates.Active;
            Debug.Log("antivirus finished running");

            taskComplete = false;
        }
    }

    # region coroutines
    public IEnumerator SwitchToActiveCoroutine()
    {
        // switch to active state after random interval of time
        yield return new WaitForSeconds(Random.Range(minAFKTime, maxAFKTime));

        animator.SetTrigger("Active");
        currentState = OwnerStates.Active;
    }
    public IEnumerator SwitchToAFKCoroutine() 
    {
        yield return new WaitForSeconds(Random.Range(minActiveTime, maxAFKTime));

        animator.SetTrigger("AFK");
        currentState = OwnerStates.AFK;
    }
    #endregion
}
