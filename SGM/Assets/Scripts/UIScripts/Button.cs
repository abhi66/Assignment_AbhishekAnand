using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject startUI;
   public void StartButton()
    {
        startUI.SetActive(false);
      //  Debug.Log("clicked");
    }
   
}
