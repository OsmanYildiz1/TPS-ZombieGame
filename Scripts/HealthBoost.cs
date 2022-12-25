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
        if (Vector3.Distance(transform.position, player.transform.position) < radius)   // eðer oyuncunun pozisyonu healthboxýnkinden küçükse (yani ona yakýnsa)
        {
            if (Input.GetKeyDown("f"))  // ve f ' e basarsa
            {
                animator.SetBool("Open", true);
                player.presentHealth = healthToGive;    // animasyon oynasýn ve oyuncunun su anki caný 100'e yükselsin

                // sound effect
                audioSource.PlayOneShot(HealthBoostSound);
                healthBar.SetHalth(player.presentHealth);
                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
