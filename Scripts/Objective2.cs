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
            ObjectivesC.occurence.GetObjectivesDone(true, true, false, false); // sa� kalanlar bulundu ikinci g�rev tamamland� di�erleri hala false

            Destroy(gameObject, 2f); 
        }
    }
}
