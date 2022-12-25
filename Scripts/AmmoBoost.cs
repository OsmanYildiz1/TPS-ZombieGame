using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoost : MonoBehaviour
{
    [Header("Ammo Boost")]
    public Rifle rifle;
    private int magToGive = 6;
    private float radius = 2.5f;
   

    [Header("Sounds")]
    public AudioClip AmmoBoostSound;
    public AudioSource audioSource;

    [Header("AmmoBox Animator")]
    public Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, rifle.transform.position) < radius)   // e�er oyuncunun pozisyonu ammobox�nkinden k���kse (yani ona yak�nsa)
        {
            if (Input.GetKeyDown("f"))  // ve f ' e basarsa
            {
                animator.SetBool("Open", true);
                rifle.mag += magToGive;   // animasyon oynas�n ve oyuncunun su anki mermisi arts�n 

                // sound effect
                audioSource.PlayOneShot(AmmoBoostSound);
               //  healthBar.SetHalth(player.presentHealth);
                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
