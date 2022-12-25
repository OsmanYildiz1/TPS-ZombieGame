using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            // complete objective
            ObjectivesC.occurence.GetObjectivesDone(true, true, true, true); // sa� kalanlar kurtar�ld� d�rd�nc� g�rev tamamland� 

            SceneManager.LoadScene("MainMenu");
        }
    }
}
