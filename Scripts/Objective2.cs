using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // complete objective
            ObjectivesC.occurence.GetObjectivesDone(true, true, false, false); // sað kalanlar bulundu ikinci görev tamamlandý diðerleri hala false

            Destroy(gameObject, 2f); 
        }
    }
}
