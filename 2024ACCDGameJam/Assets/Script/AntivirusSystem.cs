using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntivirusSystem : MonoBehaviour
{
    public float antivirusSpeed = 5f; // The infection progress reduced by each antivirus. !!!Modify it!!!
    public bool isAntivirusRunning = false; // Mark whether it is running
    private GameObject currentTarget = null; // Current target file

    public void RunAntivirus(Dictionary<string, GameObject> computerFileDictionary)
    {
        // If antivirus software is not running, start a scan
        if (!isAntivirusRunning)
        {
            StartCoroutine(CheckAndReduceInfection(computerFileDictionary));
        }
    }

    private IEnumerator CheckAndReduceInfection(Dictionary<string, GameObject> computerFileDictionary)
    {
        isAntivirusRunning = true;

        Dictionary<string, GameObject>.ValueCollection valueColl = computerFileDictionary.Values;

        // Traverse all files and look for infected files
        foreach (GameObject fileEntry in valueColl)
        {
            Debug.Log("can access dictionary");

            IsFile fileComponent = fileEntry.GetComponent<IsFile>();

            // Check if the file is currently infected
            if (fileComponent != null && fileComponent.hasVirus && fileComponent.currentProcess > 0 && fileComponent.currentProcess < 1)
            {
                currentTarget = fileEntry;

                // Gradually reduce the progress of infection
                while (fileComponent.currentProcess > 0)
                {
                    fileComponent.currentProcess -= antivirusSpeed * Time.deltaTime;

                    //Make sure progress doesn't drop to negative values
                    if (fileComponent.currentProcess < 0)
                        fileComponent.currentProcess = 0;

                    yield return null; // Update every frame
                }

                // Clear virus markers
                fileComponent.hasVirus = false;
                currentTarget = null;
            }
        }

        isAntivirusRunning = false; // Complete the check and turn off the antivirus system
    }
}
