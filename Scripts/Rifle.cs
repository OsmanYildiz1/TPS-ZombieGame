using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;      
    public float giveDamageOf = 10f; //verilen zarar i�in
    public float shootingRange = 100f;  // at�� menzili i�in
    public float fireCharge = 15f;      // at�� y�kleme s�resi
    private float nextTimeToShoot = 0f; // sonraki at�� zaman� i�in
    public Animator animator;   
    public PlayerScripts player;
    public Transform hand;  // elin pozisyonu
    public GameObject rifleUI;


    [Header("Rifle Ammunation and Shooting")]
    private int maximumAmmo = 30;   //maks mermi
    public int mag = 5;    // �arjor
    public int presentAmmuniton;   // mevcut mermi
    public float reloadingTime = 1.3f;  // reload s�resi
    private bool setRealoading = false;     // animasyon i�in


    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;  // silah patlamas� i�in
    public GameObject woodEffect;       // topra�a mermi de�di�inde
    public GameObject goreEffect;   // kan efekti

    [Header("Sounds and UI")]
    public GameObject AmmoOutUI;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;

    private void Awake()
    {
        transform.SetParent(hand);  // elin pozisyonunu takip etmek i�in
        rifleUI.SetActive(true);       // cephane say�s� silah� al�nca g�z�ks�n
        presentAmmuniton = maximumAmmo; // oyun baslad���nda full mermi olsun
       
    }

    private void Update()
    {
        if (setRealoading)
         return;

        if (presentAmmuniton <=0)   // e�er su anki mermi bittiyse
        {
            StartCoroutine(Reload());   //reload fonksiyonu cal�ss�n
            return;
        }

        
        if (Input.GetButton("Fire1" ) && Time.time >= nextTimeToShoot) // sol t�ka bas�ld���nda ve zaman sonraki at�� de�i�keninden b�y�kse
        {
            animator.SetBool("Fire", true); // ate� etme animasyonu oynas�n idle dursun
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + 1f / fireCharge;  //sonraki at�� zaman + 1/15 saniye sonra olsun
            Shoot();    
        }
        else if (Input.GetButton("Fire1")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // sol t�k ve w ya basl�yorsa veya yukar� y�n tu�una
        {
            animator.SetBool("Idle", false);    
            animator.SetBool("FireWalk", true); // y�r�rken ate� etme animasyonu cal�ss�n
        }
        else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))  // sol ve sa� t�k bas�ld�g�nda
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);      // aim animasyonu cal�ss�n
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);     
            animator.SetBool("Reloading",false);    // reload yapamas�n
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
            StartCoroutine(ShowAmmoOut());  // mermi bittiyse uyar� ver
            return;
        }

        presentAmmuniton--;
        if (presentAmmuniton ==0)   // su anki mermi s�f�rsa
        {
            mag--;  // �arjor azals�n
        }

        // UI g�ncellemesi olacak
        AmmoCount.occurence.UpdateAmmoText(presentAmmuniton);
        AmmoCount.occurence.UpdateMagText(mag);


        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo; // hit i�in raycast (g�r�nmeyen bir lazer) olu�turdum. 

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange));
        // lazere fizik ekledik kameran�n baslang�c�ndan ilerisine do�ru, vurdu�umuz hedefi g�rmek i�in
        {
            Debug.Log(hitInfo.transform.name);  // vurdu�unda vurulan nesnenin ismi konsolda g�z�ks�n

            ObjectToHit objectToHit =hitInfo.transform.GetComponent<ObjectToHit>(); // vuralan objenin bilgisi getirdik
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();    // vurulan zombinin bilgisi
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();    // vurulan zombinin bilgisi

            if (objectToHit != null) // vurulan obje bo� de�ilse 
            {
                objectToHit.objectHitDamege(giveDamageOf);   // objeye 10 zarar ver
                GameObject woodGo = Instantiate(woodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // woodefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(woodGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);   // objeye 10 zarar ver
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamge(giveDamageOf);   // objeye 10 zarar ver
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(goreEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;   // reload yaparken karakterin hareket etmemei i�in
        setRealoading = true;
        Debug.Log("Reloading");
        animator.SetBool("Reloading", true);    // reload anim oynas�n
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime); // reload s�resi kadar bekle
        animator.SetBool("Reloading", false);   // reload s�resi bitince animasyon bitsin
        presentAmmuniton = maximumAmmo;     // reload edince mermi maks olsun
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;   // h�z de�erleri normale d�ns�n
        setRealoading = false;
    }

    IEnumerator ShowAmmoOut()   // mermi bitince mermi bitti canvas�n� g�stermesi i�in
    {
        AmmoOutUI.SetActive(true);    
        yield return new WaitForSeconds(2f);
        AmmoOutUI.SetActive(false);
    }
}
