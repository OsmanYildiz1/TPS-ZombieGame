using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    [Header("Player Movement")] // sahnede hareket ozelliklerini iceren baslik
    public float playerSpeed = 1.9f;    //karakterin hizi i�in degisken
    public float playerSprint = 3f;

    [Header("Player Health Things")]    // oyuncu sa�l�k de�iskenleri
    private float playerHealth = 100f;
    public float presentHealth;
    public GameObject playerDamage;
    public HealthBar healthBar;


    [Header("Player Script Cameras")]
    public Transform playerCamera;
    public GameObject endGameMenuUI;

    [Header("Player Animator and Gravity")] // sahnede animator ve yer cekimi ozelliklerini iceren ba�lik
    public CharacterController cC;  // kulland�g�m�z character controller� tan�mlad�m
    public float gravity = -9.81f;  // yer cekimi icin degisken
    public Animator animator; // animator tan�mlama

    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f; // z�plama degiskeni
    Vector3 velocity; // h�z icin bir vektor
    public Transform surfaceCheck; // y�zey  i�in transform
    bool onSurface; // zeminde olup olmad���n�n kontrol� i�in bool
    public float surfaceDistance = 0.4f; // y�zeye olan mesafe
    public LayerMask surfaceMask;   //zeminin tutulacag� katman


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // oyun baslad���nda mouse imleci kapal� olsun
        presentHealth = playerHealth; // baslang�cta ful can
        healthBar.GiveFullHealth(playerHealth); // ba�lang�cta health bar ful oyuncu can�yla ba�las�n
    }
    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        // zeminde olup olmad�g�m� belirlemek icin zemin pozisyonu, mesafesi ve layer� kontrol ediyorum.

        if (onSurface && velocity.y <0) // e�er zeminde de�ilsem
        {
            velocity.y = -2f;   // h�z�m eksiye d��s�n yani karakterim zemine �arpana kadar d��ss�n

        }

        velocity.y += gravity * Time.deltaTime; // yerdeysem normal yer �ekimi de�eri uygulans�n
        cC.Move(velocity * Time.deltaTime); 

        playerMove();
        Jump();
        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal"); // Yatay eksende hareketi algilamak icin
        float vertical_axis = Input.GetAxisRaw("Vertical");     // Dikey eksende hareketi alg�lamak icin

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized; // yatay ve dikey eksen icin vector tanimladim

        if (direction.magnitude >= 0.1f)    // E�er herhangi bir yonde hareket varsa
        {

            animator.SetBool("Idle", false); // idledan ��ks�n
            animator.SetBool("Walk", true);     // walk animasyonuna ge�sin
            animator.SetBool("Running", false);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("IdleAim", false);


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            //oyuncunun kamera a��s�na g�re ayarlad�m ve radyan cinsinden ac�ya donusturdum 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnCalmVelocity, turnCalmTime);
            // yumu�ak d�n��ler yapabilmek i�in
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f )*Vector3.forward; 
            // cC'de kullanmak i�in hedef a�� tutuldu(kamera a��s�). Mouse hareketindeki y�ne do�ru karakterimiz ilerleyecek
            cC.Move(moveDirection.normalized * playerSpeed* Time.deltaTime);    // cC(Karakter), playerSpeed'deki hiz ile istenilen yone hareket
                                                                              //  etsin ve deltatime fonksiyonuyla gercek zamana gore olsun
        }
        else
        {
            animator.SetBool("Idle", true); // hareket yoksa idle animasyonunda kals�n
            animator.SetBool("Walk", false); // y�r�mesin
            animator.SetBool("Running", false); // ko�mas�n
        }
    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump")&& onSurface) // e�er zemindeyse ve space'e bas�ld�ysa
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump"); // z�plama animasyonu tetiklensin

            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);  // gravity de�erini eksi verdi�imiz i�in - ile �arpt�k
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");  // jump animasyonu resetlensin
        }
    }

    void Sprint()   // ko�mak i�in
    {
        if (Input.GetButton("Sprint")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            // YUKARIDAK� HAREKET KODUNUN AYNISI FAKAT KARAKTER�N HIZI SPR�NT HIZIYLA �ARPILDI
            float horizontal_axis = Input.GetAxisRaw("Horizontal"); // Yatay eksende hareketi algilamak icin
            float vertical_axis = Input.GetAxisRaw("Vertical");     // Dikey eksende hareketi alg�lamak icin

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized; // yatay ve dikey eksen icin vector tanimladim

            if (direction.magnitude >= 0.1f)    // E�er herhangi bir yonde hareket varsa
            {

                animator.SetBool("Walk", false);    // y�r�mesin
                animator.SetBool("Running", true);  // ko�ma animasyonu �al��s�n

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                //oyuncunun kamera a��s�na g�re ayarlad�m ve radyan cinsinden ac�ya donusturdum 
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                // yumu�ak d�n��ler yapabilmek i�in
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // cC'de kullanmak i�in hedef a�� tutuldu(kamera a��s�). Mouse hareketindeki y�ne do�ru karakterimiz ilerleyecek
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);    // cC(Karakter), playerSpeed'deki hiz ile istenilen yone hareket
                                                                                     //  etsin ve deltatime fonksiyonuyla gercek zamana gore olsun
            }
            else
            {
                animator.SetBool("Walk", true); // y�r�s�n
                animator.SetBool("Running", false); 
            }
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;    //oyuncuya vurulduysa hasar als�n
        StartCoroutine(PlayerDamage());

        healthBar.SetHalth(presentHealth);  // hasar al�nd���nda health bar �imdiki can neyse o olsun

        if (presentHealth <= 0)
        {
            PlayerDie();    // can 0 dan azsa �ls�n
        }
    }

    private void PlayerDie()
    {
        endGameMenuUI.SetActive(true);  
        Cursor.lockState = CursorLockMode.None; // cursor yok olsun
        Object.Destroy(gameObject, 3.0f);   // 3 saniye sonra yok olsun
    }

    IEnumerator PlayerDamage()  // oyuncu hasar ald�g�n hasar yeme efekti uygulanmas� i�in 
    {
        playerDamage.SetActive(true);       
        yield return new WaitForSeconds(2f);
        playerDamage.SetActive(false);
    }
}
