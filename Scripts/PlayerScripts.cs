using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    [Header("Player Movement")] // sahnede hareket ozelliklerini iceren baslik
    public float playerSpeed = 1.9f;    //karakterin hizi için degisken
    public float playerSprint = 3f;

    [Header("Player Health Things")]    // oyuncu saðlýk deðiskenleri
    private float playerHealth = 100f;
    public float presentHealth;
    public GameObject playerDamage;
    public HealthBar healthBar;


    [Header("Player Script Cameras")]
    public Transform playerCamera;
    public GameObject endGameMenuUI;

    [Header("Player Animator and Gravity")] // sahnede animator ve yer cekimi ozelliklerini iceren baþlik
    public CharacterController cC;  // kullandýgýmýz character controllerý tanýmladým
    public float gravity = -9.81f;  // yer cekimi icin degisken
    public Animator animator; // animator tanýmlama

    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f; // zýplama degiskeni
    Vector3 velocity; // hýz icin bir vektor
    public Transform surfaceCheck; // yüzey  için transform
    bool onSurface; // zeminde olup olmadýðýnýn kontrolü için bool
    public float surfaceDistance = 0.4f; // yüzeye olan mesafe
    public LayerMask surfaceMask;   //zeminin tutulacagý katman


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // oyun basladýðýnda mouse imleci kapalý olsun
        presentHealth = playerHealth; // baslangýcta ful can
        healthBar.GiveFullHealth(playerHealth); // baþlangýcta health bar ful oyuncu canýyla baþlasýn
    }
    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        // zeminde olup olmadýgýmý belirlemek icin zemin pozisyonu, mesafesi ve layerý kontrol ediyorum.

        if (onSurface && velocity.y <0) // eðer zeminde deðilsem
        {
            velocity.y = -2f;   // hýzým eksiye düþsün yani karakterim zemine çarpana kadar düþssün

        }

        velocity.y += gravity * Time.deltaTime; // yerdeysem normal yer çekimi deðeri uygulansýn
        cC.Move(velocity * Time.deltaTime); 

        playerMove();
        Jump();
        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal"); // Yatay eksende hareketi algilamak icin
        float vertical_axis = Input.GetAxisRaw("Vertical");     // Dikey eksende hareketi algýlamak icin

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized; // yatay ve dikey eksen icin vector tanimladim

        if (direction.magnitude >= 0.1f)    // Eðer herhangi bir yonde hareket varsa
        {

            animator.SetBool("Idle", false); // idledan çýksýn
            animator.SetBool("Walk", true);     // walk animasyonuna geçsin
            animator.SetBool("Running", false);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("IdleAim", false);


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            //oyuncunun kamera açýsýna göre ayarladým ve radyan cinsinden acýya donusturdum 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnCalmVelocity, turnCalmTime);
            // yumuþak dönüþler yapabilmek için
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f )*Vector3.forward; 
            // cC'de kullanmak için hedef açý tutuldu(kamera açýsý). Mouse hareketindeki yöne doðru karakterimiz ilerleyecek
            cC.Move(moveDirection.normalized * playerSpeed* Time.deltaTime);    // cC(Karakter), playerSpeed'deki hiz ile istenilen yone hareket
                                                                              //  etsin ve deltatime fonksiyonuyla gercek zamana gore olsun
        }
        else
        {
            animator.SetBool("Idle", true); // hareket yoksa idle animasyonunda kalsýn
            animator.SetBool("Walk", false); // yürümesin
            animator.SetBool("Running", false); // koþmasýn
        }
    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump")&& onSurface) // eðer zemindeyse ve space'e basýldýysa
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump"); // zýplama animasyonu tetiklensin

            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);  // gravity deðerini eksi verdiðimiz için - ile çarptýk
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");  // jump animasyonu resetlensin
        }
    }

    void Sprint()   // koþmak için
    {
        if (Input.GetButton("Sprint")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            // YUKARIDAKÝ HAREKET KODUNUN AYNISI FAKAT KARAKTERÝN HIZI SPRÝNT HIZIYLA ÇARPILDI
            float horizontal_axis = Input.GetAxisRaw("Horizontal"); // Yatay eksende hareketi algilamak icin
            float vertical_axis = Input.GetAxisRaw("Vertical");     // Dikey eksende hareketi algýlamak icin

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized; // yatay ve dikey eksen icin vector tanimladim

            if (direction.magnitude >= 0.1f)    // Eðer herhangi bir yonde hareket varsa
            {

                animator.SetBool("Walk", false);    // yürümesin
                animator.SetBool("Running", true);  // koþma animasyonu çalýþsýn

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                //oyuncunun kamera açýsýna göre ayarladým ve radyan cinsinden acýya donusturdum 
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                // yumuþak dönüþler yapabilmek için
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // cC'de kullanmak için hedef açý tutuldu(kamera açýsý). Mouse hareketindeki yöne doðru karakterimiz ilerleyecek
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);    // cC(Karakter), playerSpeed'deki hiz ile istenilen yone hareket
                                                                                     //  etsin ve deltatime fonksiyonuyla gercek zamana gore olsun
            }
            else
            {
                animator.SetBool("Walk", true); // yürüsün
                animator.SetBool("Running", false); 
            }
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;    //oyuncuya vurulduysa hasar alsýn
        StartCoroutine(PlayerDamage());

        healthBar.SetHalth(presentHealth);  // hasar alýndýðýnda health bar þimdiki can neyse o olsun

        if (presentHealth <= 0)
        {
            PlayerDie();    // can 0 dan azsa ölsün
        }
    }

    private void PlayerDie()
    {
        endGameMenuUI.SetActive(true);  
        Cursor.lockState = CursorLockMode.None; // cursor yok olsun
        Object.Destroy(gameObject, 3.0f);   // 3 saniye sonra yok olsun
    }

    IEnumerator PlayerDamage()  // oyuncu hasar aldýgýn hasar yeme efekti uygulanmasý için 
    {
        playerDamage.SetActive(true);       
        yield return new WaitForSeconds(2f);
        playerDamage.SetActive(false);
    }
}
