using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{

    public TextMeshProUGUI antiVirusKillingSpeed;

    public TextMeshProUGUI virusInfectSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        antiVirusKillingSpeed.text =( GameRoot.GetInstance().antiVirusKillingSpeed*100).ToString("0");

        virusInfectSpeed.text= (GameRoot.GetInstance().virusOriginSpeed * 100).ToString("0");
    }
}
