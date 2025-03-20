using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_S1 : MonoBehaviour
{
    public GameObject playerCam1Holder;
    private bool hasLvlStarted;
    // Start is called before the first frame update
    void Start()
    {
        sceneInit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E)) {
            sceneStart();
        }
    }
    void sceneInit() {
        playerCam1Holder.GetComponent<PlayerCam1>().sit();
        hasLvlStarted = false;
        Debug.Log("Game Initialized");
    }
    void sceneStart() {
            playerCam1Holder.GetComponent<PlayerCam1>().stand();
            hasLvlStarted = true;
            Debug.Log("StartGame");
    }
    void foodPrompt() {
        
    }
}
