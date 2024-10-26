using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntivirusSystem : MonoBehaviour
{

    public float antivirusSpeed = 5f; // Antivirus software reduces the speed at which infections progress. !!!!!! Modify it!!!!!!!
    private bool isAntivirusRunning = false;
    private GameObject currentTarget = null; // The target file currently being processed

    public void StartAntivirus(bool isOwnerWatching, Dictionary<string, GameObject> computerFileDictionary)
    {
        if (isOwnerWatching && !isAntivirusRunning)
        {
            StartCoroutine(RunAntivirusAfterDelay(3f, computerFileDictionary));
        }
        else if (!isOwnerWatching && isAntivirusRunning)
        {
            StopAntivirus(); // Stop antivirus software
        }
    }

    private IEnumerator RunAntivirusAfterDelay(float delay, Dictionary<string, GameObject> computerFileDictionary)
    {
        yield return new WaitForSeconds(delay);

        isAntivirusRunning = true;
        StartCoroutine(CheckAndReduceInfection(computerFileDictionary));
    }

    private IEnumerator CheckAndReduceInfection(Dictionary<string, GameObject> computerFileDictionary)
    {
        while (isAntivirusRunning)
        {
            // Checks all files for files that are being infected
            currentTarget = FindInfectedFile(computerFileDictionary);

            if (currentTarget != null)
            {
                // Get the IsFile component of a file and reduce the infection progress
                var fileComponent = currentTarget.GetComponent<IsFile>();
                fileComponent.currentProcess -= antivirusSpeed * Time.deltaTime;

                // Make sure progress doesn't drop to negative values
                if (fileComponent.currentProcess < 0)
                    fileComponent.currentProcess = 0;

                // If the infection is completely cleared, reset the current target
                if (fileComponent.currentProcess == 0)
                {
                    fileComponent.hasVirus = false; // clear virus status
                    currentTarget = null;
                    // UI changes
                }
            }

            yield return null; // Update once per frame
        }
    }

    private GameObject FindInfectedFile(Dictionary<string, GameObject> computerFileDictionary)
    {
        // Iterate over all files and return the first file with an infection progress between 0-100%
        foreach (var fileEntry in computerFileDictionary)
        {
            var fileObject = fileEntry.Value;
            var fileComponent = fileObject.GetComponent<IsFile>();

            if (fileComponent != null && fileComponent.hasVirus && fileComponent.currentProcess > 0 && fileComponent.currentProcess < 100)
            {
                return fileObject;
            }
        }
        return null; // If no file being infected is found, null is returned.
    }

    private void StopAntivirus()
    {
        isAntivirusRunning = false;
        currentTarget = null; // Reset current target
        StopCoroutine(CheckAndReduceInfection(null));

    public float antivirusSpeed = 5f; // The infection progress reduced by each antivirus. !!!Modify it!!!
    private bool isAntivirusRunning = false; // Mark whether it is running
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

        // Traverse all files and look for infected files
        foreach (var fileEntry in computerFileDictionary)
        {
            var fileObject = fileEntry.Value;
            var fileComponent = fileObject.GetComponent<IsFile>();

            // Check if the file is currently infected
            if (fileComponent != null && fileComponent.hasVirus && fileComponent.currentProcess > 0 && fileComponent.currentProcess < 1)
            {
                currentTarget = fileObject;

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
