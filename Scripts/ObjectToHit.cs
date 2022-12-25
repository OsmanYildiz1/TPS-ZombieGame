using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public float objectHealth = 100f; // obje can�

    public void objectHitDamege(float amount)   // miktar
    {
        objectHealth -= amount;
        if (objectHealth <= 0f)   // objenin can� 0 dan az ise
        {
            Die();      // yok olsun
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
