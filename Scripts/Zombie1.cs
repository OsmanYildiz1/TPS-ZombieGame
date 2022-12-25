using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie1 : MonoBehaviour
{
    [Header("Zombie Health and Damage Var")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;    // zombie gezinme arac�
    public Transform LookPoint;         // g�r�� noktas�
    public Camera AttackingRaycastArea;     // sald�r� i�in
    public LayerMask PlayerLayer;
    public Transform playerBody;

    [Header("Zombie Guarding Variables")]
    public GameObject[] walkPoints;     // y�r�me noktalar� dizi halinde
    int currentZombiePosition = 0;      // mevcut zombi pozisyonu
    public float zombieSpeed;
    float walkingPointRadius = 2;   // y�r�me alan�

    [Header("Zombie Attacking Variables")]
    public float timeBtwAttack;     // sald�rmadan �nceki s�re
    bool previouslyAttack;  // ataktan �nce

    [Header("Zombie Animation")]
    public Animator anim;



    [Header("Zombie Moods/States")]
    public float visionRadius;  // g�r�� alan�
    public float attackingRadius;   // sald�rma mesafesi
    public bool playerInvisionRadius;   // oyuncu g�r�� mesafesi
    public bool playerInattackingRadius; // oyuncu sald�rma mesafesi

    private void Awake()
    {
        presentHealth = zombieHealth;   // ful can basla
        healthBar.GiveFullHealth(zombieHealth); // zombinin can bar�da baslang�cta full olsun
        zombieAgent = GetComponent<NavMeshAgent>(); // navmeshi yakala
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        // oyuncu g�r�� mesafesi oyunculayer� g�r�� a��s� ve posizyonuna g�re ayarland�.
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);
        // oyuncu sald�r� mesafesi  oyunculayer� saldarma a��s� ve pozisyonuna g�re ayarland�.

        if (!playerInattackingRadius && !playerInvisionRadius)  // e�er oyuncu sald�r� ve ya g�r�� a��s�nda de�ilse guard fonksiyonu cal�ss�n
            Guard();
        if (playerInvisionRadius && !playerInattackingRadius)   // e�er oyuncu g�r�� a��s�ndaysa
            Pursueplayer(); // takip et
        if (playerInvisionRadius && playerInattackingRadius) // sald�r� mesafesindeyse
            AttackPlayer(); // sald�r
    }

    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentZombiePosition].transform.position, transform.position) < walkingPointRadius)//  y�r�me noktas�na ula��lmam��sa
        {
            currentZombiePosition = Random.Range(0, walkPoints.Length); // walk pointse y�r�
            if (currentZombiePosition >= walkPoints.Length) // walk pointse ula��lm��sa 
            {
                currentZombiePosition = 0;  // ilk konuma d�n
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        //change zombie facing
        transform.LookAt(walkPoints[currentZombiePosition].transform.position); 
    }

    private void Pursueplayer() // oyuncu takibi
    {   
        if(zombieAgent.SetDestination(playerBody.position))   // oyunucunun pozisyonunu takip et
        {
            // animations
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", false);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
        }
    }
    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);    // zombi sald�r�rken oyuncuya d�ns�n
        if (!previouslyAttack)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius)) 
            {
                Debug.Log("Attacking " + hitInfo.transform.name);   // lazere fizik ekledik kameran�n baslang�c�ndan ilerisine do�ru, vurdu�umuz hedefi g�rmek i�in

                PlayerScripts playerBody = hitInfo.transform.GetComponent<PlayerScripts>();

                if (playerBody != null )
                {
                    playerBody.playerHitDamage(giveDamage); // oyuncuya vurunca hasar ver
                }

                anim.SetBool("Attacking", true);
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Died", false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack); // sald�rmadan �nceki s�reden sonra sald�r.(1 saniye dur ve sald�r tekrar et)
        }
    }

    private void ActiveAttacking()  // sald�rma
    {
        previouslyAttack = false;  // sald�rmadan �nceki s�re dursun
    }

    public void zombieHitDamage(float takeDamage)    // zombi hasar alma
    {
        presentHealth -= takeDamage;
        healthBar.SetHalth(presentHealth);  // hasar al�nd���nda health bar �imdiki can neyse o olsun

        if (presentHealth <= 0 )    // zombi hasar als�n ve can� s�f�rdan azsa �ls�n
        {
            anim.SetBool("Attacking", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Died", true);

            zombieDie();
        }
    }

    private void zombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;                  // zombi �l�rse de�i�kenleri 0 olup yok olsun
        playerInattackingRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 5.0f);

    }
}
