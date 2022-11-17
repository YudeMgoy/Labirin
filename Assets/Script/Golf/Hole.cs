using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    [SerializeField]GameObject gameOverPanel;
    bool entered = false;
    public bool Entered {get => entered;}
    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        entered=true;
        gameOverPanel.SetActive(true);
    }
}
