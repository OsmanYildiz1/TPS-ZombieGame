using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesC : MonoBehaviour
{
    [Header("Objectives to Complete")]
    public Text objective1;
    public Text objective2;
    public Text objective3;
    public Text objective4;

    public static ObjectivesC occurence;    // olay

    private void Awake()
    {
        occurence = this;
    }

    public void GetObjectivesDone(bool obj1, bool obj2, bool obj3, bool obj4)
    {
        if (obj1 == true)    
        {
            objective1.text = "1. tamamlandý";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "1. yakindaki evlerden silah bul";
            objective1.color = Color.white;         
        }

        if (obj2 == true)
        {
            objective2.text = "2. Tamamlandý";
            objective2.color = Color.green;
        }
        else
        {
            objective2.text = "2. sag kalanlari arastir";
            objective2.color = Color.white;
        }

        if (obj3 == true)
        {
            objective3.text = "3. Tamamlandý";
            objective3.color = Color.green;
        }
        else
        {
            objective3.text = "3. bir arac bul";
            objective3.color = Color.white;
        }

        if (obj4 == true)
        {
            objective4.text = "Gorev tamamlandý";
            objective4.color = Color.green;
        }
        else
        {
            objective4.text = "4. sag kalanlarla beraber kac";
            objective4.color = Color.white;
        }

    }
}
