using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winPanelPin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
    }

    public void JumpToStartScene()
    {
        SceneManager.LoadScene("01_CoverScene");
    }
}
