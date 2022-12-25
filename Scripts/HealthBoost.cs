using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("Health Boost")]
    public PlayerScripts player;
    private float healthToGive = 100f;
    private float radius = 2.5f;
    public HealthBar healthBar;

    [Header("Sounds")]
    public AudioClip HealthBoostSound;
    public AudioSource audioSource;

    [Header("HealthBox Animator")]
    public Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radius)   // e�er oyuncunun pozisyonu healthbox�nkinden k���kse (yani ona yak�nsa)
        {
            if (Input.GetKeyDown("f"))  // ve f ' e basarsa
            {
                animator.SetBool("Open", true);
                player.presentHealth = healthToGive;    // animasyon oynas�n ve oyuncunun su anki can� 100'e y�kselsin

                // sound effect
                audioSource.PlayOneShot(HealthBoostSound);
                healthBar.SetHalth(player.presentHealth);
                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
