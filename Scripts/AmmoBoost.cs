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
        if (Vector3.Distance(transform.position, rifle.transform.position) < radius)   // eðer oyuncunun pozisyonu ammoboxýnkinden küçükse (yani ona yakýnsa)
        {
            if (Input.GetKeyDown("f"))  // ve f ' e basarsa
            {
                animator.SetBool("Open", true);
                rifle.mag += magToGive;   // animasyon oynasýn ve oyuncunun su anki mermisi artsýn 

                // sound effect
                audioSource.PlayOneShot(AmmoBoostSound);
               //  healthBar.SetHalth(player.presentHealth);
                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
