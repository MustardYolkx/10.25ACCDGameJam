using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntivirusSystem : MonoBehaviour
{
    public float antivirusSpeed = 1f; // The infection progress reduced by each antivirus. !!!Modify it!!!

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
            IsFile fileComponent = fileEntry.GetComponent<PageInfo>().fileInfo;

            // Check if the file is currently infected
            if (fileComponent != null && fileComponent.hasVirus && fileComponent.currentProcess > 0 && fileComponent.currentProcess < 1)
            {
                currentTarget = fileEntry;

                GameRoot.GetInstance().isAntiSystemKilling = true;

                // Gradually reduce the progress of infection
                while (fileComponent.currentProcess > 0)
                {
                    yield return new WaitForEndOfFrame();

                    fileComponent.currentProcess -= (antivirusSpeed * Time.deltaTime+ GameRoot.GetInstance().antiVirusKillingSpeed* Time.deltaTime);

                    //Make sure progress doesn't drop to negative values
                    if (fileComponent.currentProcess < 0)
                        fileComponent.currentProcess = 0;
                        GameRoot.GetInstance().isAntiSystemKilling = false;
                    //yield return null; // Update every frame
                }

                // Clear virus markers
                fileComponent.hasVirus = false;
                string content = fileComponent.fileName;
                Sprite targetSprite = Resources.Load<Sprite>("Sprite/Icon/" + content);
                GameRoot.GetInstance().computerFile_Dictionary[content].GetComponentInChildren<IsFile>().hasVirus = false;
                GameRoot.GetInstance().computerFile_Dictionary[content].GetComponent<Image>().sprite = targetSprite;
                currentTarget = null;
            }
            //Destroy(GameRoot.GetInstance().currentOpenFile_Dictionary[fileComponent.fileName]);
            //GameRoot.GetInstance().currentOpenFile_Dictionary.Remove(fileComponent.fileName);
        }
        OwnerAI ownerAI = FindObjectOfType<OwnerAI>().GetComponent<OwnerAI>();
        if (ownerAI != null) 
        { 
            ownerAI.AntivirusTaskCompleted(true);
        }
        else
        {
            Debug.Log("ownerAI script not found!");
        }

        isAntivirusRunning = false; // Complete the check and turn off the antivirus system
    }
}
