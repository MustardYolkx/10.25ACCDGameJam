using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OwnerAI : MonoBehaviour
{
    public enum OwnerStates
    {
        AFK,
        Active,
        OnAlert
    }

    [Header("Owner's current state")]
    [SerializeField] public OwnerStates currentState;

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

    [Header("Cursor")]
    public OwnerCursor cursor;
    public string currentTargetFile;
    public float timeDuration;
    private bool timeStart = true;
    [HideInInspector]public bool isFileOpen;
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

        if (timeStart)
        {
            timeDuration += Time.deltaTime;
        }
        if(currentState == OwnerStates.Active)
        {
           
            if (timeDuration > Random.Range(2,4))
            {
                if(isFileOpen)
                {
                    ClosePageCursor();
                }
                else
                {
                    if (currentTargetFile == "")
                    {

                         RandomTargetFile();
                    }
                    
                    
                }
            }
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

    public void SpeedUpAntiVirusSystem()
    {
        GameRoot.GetInstance().antiVirusKillingSpeed +=0.05f;
    }
    private void UpdateOnAlert()
    {
        // activate antivirus software
        antivirusSystem.RunAntivirus(gameRoot.currentOpenFile_Dictionary);
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

        timeDuration = 0;
        timeStart = true;


        animator.SetTrigger("Active");

        currentState = OwnerStates.Active;
    }
    public IEnumerator SwitchToAFKCoroutine() 
    {
        yield return new WaitForSeconds(Random.Range(minActiveTime, maxAFKTime));

        if (isFileOpen)
        {
            ClosePageCursor();
        }
        timeDuration = 0;
        timeStart = false;
        animator.SetTrigger("AFK");

        currentState = OwnerStates.AFK;
    }
    #endregion

    public void RandomTargetFile()
    {
        timeStart = false;
        if (GameRoot.GetInstance().currentOpenFile_Dictionary.Count ==0)
        {
            currentTargetFile = gameRoot.fileNames[Random.Range(0, gameRoot.fileNames.Count)];
            GameObject targetFile = gameRoot.computerFile_Dictionary[currentTargetFile];
            cursor.ownerAI = this;
            cursor.targetTransform = targetFile.transform;
            cursor.cursorState = OwnerCursor.CursorState.MovetoFile;
        }
        else
        {
            ClosePageCursor();
        }
        
    }

    public void OpenTargetFile()
    {        
        GameObject UnityPanel = Resources.Load<GameObject>("UIPanel/"+ currentTargetFile+"Panel");
        GameObject unityPage = Instantiate(UnityPanel, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);
        if (GameRoot.GetInstance().currentOpenFile_Dictionary.Keys.Contains(unityPage.GetComponent<PageInfo>().fileName))
        {

        }
        else
        {

            GameRoot.GetInstance().currentOpenFile_Dictionary.Add(unityPage.GetComponent<PageInfo>().fileName, unityPage);
        }
        timeDuration= 0;
        cursor.StartCoroutine(cursor.StayForAWhile());
        cursor.targetTransform = cursor.idlePos;
        isFileOpen = true;
        timeStart = true;
    }

    public void ClosePageCursor()
    {
        
        if (GameRoot.GetInstance().currentOpenFile_Dictionary.ContainsKey(currentTargetFile))
        {
            Transform cursorTargetPos = GameRoot.GetInstance().currentOpenFile_Dictionary[currentTargetFile].transform.Find("Close");
            cursor.targetTransform = cursorTargetPos;
            cursor.cursorState = OwnerCursor.CursorState.MovetoFile;
        }
        else if(GameRoot.GetInstance().currentOpenFile_Dictionary.Count>0)
        {
            foreach(string s in GameRoot.GetInstance().currentOpenFile_Dictionary.Keys)
            {
                Transform cursorTargetPos = GameRoot.GetInstance().currentOpenFile_Dictionary[s].transform.Find("Close");
                cursor.targetTransform = cursorTargetPos;
                cursor.cursorState = OwnerCursor.CursorState.MovetoFile;
                break;
            }
        }
        timeStart= false;
        timeDuration = 0;
    }
    public void ClosePage()
    {
        cursor.StartCoroutine(cursor.StayForAWhile());
        if(currentState!=OwnerStates.OnAlert)
        {
            if (GameRoot.GetInstance().currentOpenFile_Dictionary.ContainsKey(currentTargetFile))
            {
                Destroy(GameRoot.GetInstance().currentOpenFile_Dictionary[currentTargetFile]);
                GameRoot.GetInstance().currentOpenFile_Dictionary.Remove(currentTargetFile);
            }
            else if (GameRoot.GetInstance().currentOpenFile_Dictionary.Count > 0)
            {
                foreach (string s in GameRoot.GetInstance().currentOpenFile_Dictionary.Keys)
                {
                    Destroy(GameRoot.GetInstance().currentOpenFile_Dictionary[s]);
                    GameRoot.GetInstance().currentOpenFile_Dictionary.Remove(s);
                    break;
                }
            }
        }
       
        currentTargetFile = "";
        isFileOpen= false;
        //GameRoot.GetInstance().UIManager_Root.Push(new UnityPanel());
        //Scene2 scene2 = new Scene2();
        //GameRoot.GetInstance().SceneControl_Root.LoadScene(scene2.SceneName, scene2);
    }

    public void UpdateTargetFile()
    {
        
    }
}
