using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AntiVirusStrength : MonoBehaviour
{

    public TextMeshProUGUI powerNumber;

    GameRoot gameRoot;
    // Start is called before the first frame update
    void Start()
    {
        gameRoot = GameRoot.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRoot.isAntiSystemKilling)
        {

             powerNumber.text = (gameRoot.antivirusSystem.antivirusSpeed * 100).ToString("0");
        }
        else
        {
            powerNumber.text = (gameRoot.antiVirusKillingSpeed * 100).ToString("0");
        }
    }
}
