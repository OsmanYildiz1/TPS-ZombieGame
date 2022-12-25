using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Variables")]
    public Camera cam;
    public float giveDamageOf = 5f;
    public float punchingRange = 3.5f;

    [Header("Punch Effects")]
    public GameObject woodEffect;
    public GameObject goreEffect;
    public void Punch()
    {
        RaycastHit hitInfo; // yumruk için raycast  oluþturdum. 

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchingRange)) ;
        // lazere fizik ekledik kameranýn baslangýcýndan ilerisine doðru, vurduðumuz hedeften bilgi almak için
        {
            Debug.Log(hitInfo.transform.name);  // vurduðunda vurulan nesnenin ismi konsolda gözüksün

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); // vuralan objenin transformunu getirdik
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();    // vurulan zombinin bilgisi
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (objectToHit != null) // vurulan obje boþ deðilse 
            {
                objectToHit.objectHitDamege(giveDamageOf);   // objeye 10 zarar ver
               
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);   // objeye 10 zarar ver
             
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamge(giveDamageOf);   // objeye 10 zarar ver
         
            }
        }
    }

}
