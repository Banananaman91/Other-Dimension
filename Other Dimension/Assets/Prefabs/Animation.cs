using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator an1;
    public GameObject player;
    public GameObject targetPos;
    public bool playerClose = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // player detection\\ //.setTrigger\\


        if(player.transform.position == targetPos.transform.position)
        {
            an1.SetTrigger("player"); 
        }


    }
}
