using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePickups : MonoBehaviour
{
    
    void Start()
    {
        Invoke("Deactivate", Random.Range(3f, 6f));
    }

    // Update is called once per frame
    void Deactivate()
    {
        if (!PlayerController.instance.playerDead)
            Destroy(gameObject);
    }
}
