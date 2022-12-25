using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;      
    public float giveDamageOf = 10f; //verilen zarar için
    public float shootingRange = 100f;  // atýþ menzili için
    public float fireCharge = 15f;      // atýþ yükleme süresi
    private float nextTimeToShoot = 0f; // sonraki atýþ zamaný için
    public Animator animator;   
    public PlayerScripts player;
    public Transform hand;  // elin pozisyonu
    public GameObject rifleUI;


    [Header("Rifle Ammunation and Shooting")]
    private int maximumAmmo = 30;   //maks mermi
    public int mag = 5;    // þarjor
    public int presentAmmuniton;   // mevcut mermi
    public float reloadingTime = 1.3f;  // reload süresi
    private bool setRealoading = false;     // animasyon için


    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;  // silah patlamasý için
    public GameObject woodEffect;       // topraða mermi deðdiðinde
    public GameObject goreEffect;   // kan efekti

    [Header("Sounds and UI")]
    public GameObject AmmoOutUI;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;

    private void Awake()
    {
        transform.SetParent(hand);  // elin pozisyonunu takip etmek için
        rifleUI.SetActive(true);       // cephane sayýsý silahý alýnca gözüksün
        presentAmmuniton = maximumAmmo; // oyun basladýðýnda full mermi olsun
       
    }

    private void Update()
    {
        if (setRealoading)
         return;

        if (presentAmmuniton <=0)   // eðer su anki mermi bittiyse
        {
            StartCoroutine(Reload());   //reload fonksiyonu calýssýn
            return;
        }

        
        if (Input.GetButton("Fire1" ) && Time.time >= nextTimeToShoot) // sol týka basýldýðýnda ve zaman sonraki atýþ deðiþkeninden büyükse
        {
            animator.SetBool("Fire", true); // ateþ etme animasyonu oynasýn idle dursun
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + 1f / fireCharge;  //sonraki atýþ zaman + 1/15 saniye sonra olsun
            Shoot();    
        }
        else if (Input.GetButton("Fire1")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // sol týk ve w ya baslýyorsa veya yukarý yön tuþuna
        {
            animator.SetBool("Idle", false);    
            animator.SetBool("FireWalk", true); // yürürken ateþ etme animasyonu calýssýn
        }
        else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))  // sol ve sað týk basýldýgýnda
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);      // aim animasyonu calýssýn
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);     
            animator.SetBool("Reloading",false);    // reload yapamasýn
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
        }
    }
    private void Shoot()
    {
        // sarjor kontrol et
        if (mag==0)
        {
            StartCoroutine(ShowAmmoOut());  // mermi bittiyse uyarý ver
            return;
        }

        presentAmmuniton--;
        if (presentAmmuniton ==0)   // su anki mermi sýfýrsa
        {
            mag--;  // þarjor azalsýn
        }

        // UI güncellemesi olacak
        AmmoCount.occurence.UpdateAmmoText(presentAmmuniton);
        AmmoCount.occurence.UpdateMagText(mag);


        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo; // hit için raycast (görünmeyen bir lazer) oluþturdum. 

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange));
        // lazere fizik ekledik kameranýn baslangýcýndan ilerisine doðru, vurduðumuz hedefi görmek için
        {
            Debug.Log(hitInfo.transform.name);  // vurduðunda vurulan nesnenin ismi konsolda gözüksün

            ObjectToHit objectToHit =hitInfo.transform.GetComponent<ObjectToHit>(); // vuralan objenin bilgisi getirdik
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();    // vurulan zombinin bilgisi
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();    // vurulan zombinin bilgisi

            if (objectToHit != null) // vurulan obje boþ deðilse 
            {
                objectToHit.objectHitDamege(giveDamageOf);   // objeye 10 zarar ver
                GameObject woodGo = Instantiate(woodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // woodefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(woodGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);   // objeye 10 zarar ver
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamge(giveDamageOf);   // objeye 10 zarar ver
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(goreEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;   // reload yaparken karakterin hareket etmemei için
        setRealoading = true;
        Debug.Log("Reloading");
        animator.SetBool("Reloading", true);    // reload anim oynasýn
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime); // reload süresi kadar bekle
        animator.SetBool("Reloading", false);   // reload süresi bitince animasyon bitsin
        presentAmmuniton = maximumAmmo;     // reload edince mermi maks olsun
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;   // hýz deðerleri normale dönsün
        setRealoading = false;
    }

    IEnumerator ShowAmmoOut()   // mermi bitince mermi bitti canvasýný göstermesi için
    {
        AmmoOutUI.SetActive(true);    
        yield return new WaitForSeconds(2f);
        AmmoOutUI.SetActive(false);
    }
}
