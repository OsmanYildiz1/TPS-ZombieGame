using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
{
    [Header("Zombie Health and Damage Var")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;    // zombie gezinme aracý
    public Transform LookPoint;         // görüþ noktasý
    public Camera AttackingRaycastArea;     // saldýrý için
    public LayerMask PlayerLayer;
    public Transform playerBody;

    [Header("Zombie Standing Variables")]
    public float zombieSpeed;

    [Header("Zombie Attacking Variables")]
    public float timeBtwAttack;     // saldýrmadan önceki süre
    bool previouslyAttack;  // ataktan önce

    [Header("Zombie Animation")]
    public Animator anim;



    [Header("Zombie Moods/States")]
    public float visionRadius;  // görüþ alaný
    public float attackingRadius;   // saldýrma mesafesi
    public bool playerInvisionRadius;   // oyuncu görüþ mesafesi
    public bool playerInattackingRadius; // oyuncu saldýrma mesafesi

    private void Awake()
    {
        presentHealth = zombieHealth;
        healthBar.GiveFullHealth(zombieHealth); // zombinin can barýda baslangýcta full olsun
        zombieAgent = GetComponent<NavMeshAgent>(); // navmeshi yakala
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        // oyuncu görüþ mesafesi oyunculayerý görüþ açýsý ve posizyonuna göre ayarlandý.
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);
        // oyuncu saldýrý mesafesi  oyunculayerý saldarma açýsý ve pozisyonuna göre ayarlandý.

        if (!playerInattackingRadius && !playerInvisionRadius)  // eðer oyuncu saldýrý ve ya görüþ açýsýnda deðilse guard fonksiyonu calýssýn
            Idle();
        if (playerInvisionRadius && !playerInattackingRadius)   // eðer oyuncu görüþ açýsýndaysa
            Pursueplayer(); // takip et
        if (playerInvisionRadius && playerInattackingRadius) // saldýrý mesafesindeyse
            AttackPlayer(); // saldýr
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }

    private void Pursueplayer() // oyuncu takibi
    {
        if (zombieAgent.SetDestination(playerBody.position))   // oyunucunun görüþ açýsýndaysa takip et
        {
            // animations
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
        }
        else
        {
            //anim.SetBool("Walking", false);
            //anim.SetBool("Running", false);
            //anim.SetBool("Attacking", false);
            //anim.SetBool("Died", true);
        }
    }
    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);    // zombi saldýrýrken oyuncuya dönsün
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking " + hitInfo.transform.name);   // lazere fizik ekledik kameranýn baslangýcýndan ilerisine doðru, vurduðumuz hedefi görmek için

                PlayerScripts playerBody = hitInfo.transform.GetComponent<PlayerScripts>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage); // oyuncuya vurunca hasar ver
                }

                anim.SetBool("Attacking", true);
                anim.SetBool("Running", false);

            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack); // saldýrmadan önceki süreden sonra saldýr.(1 saniye dur ve saldýr tekrar et)
        }
    }

    private void ActiveAttacking()  // saldýrma
    {
        previouslyAttack = false;  // saldýrmadan önceki süre dursun
    }

    public void zombieHitDamge(float takeDamage)    // zombi hasar alma
    {
        presentHealth -= takeDamage;
        healthBar.SetHalth(presentHealth);  // hasar alýndýðýnda health bar þimdiki can neyse o olsun

        if (presentHealth <= 0)    // zombi hasar alsýn ve caný sýfýrdan azsa ölsün
        {
            anim.SetBool("Died", true);

            zombieDie();
        }
    }

    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;                  // zombi ölürse deðiþkenleri 0 olup yok olsun
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 5.0f);

    }
}
