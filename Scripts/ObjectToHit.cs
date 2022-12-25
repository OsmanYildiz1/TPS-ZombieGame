using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public float objectHealth = 100f; // obje caný

    public void objectHitDamege(float amount)   // miktar
    {
        objectHealth -= amount;
        if (objectHealth <= 0f)   // objenin caný 0 dan az ise
        {
            Die();      // yok olsun
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
