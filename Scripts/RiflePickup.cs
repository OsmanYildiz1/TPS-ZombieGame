using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;  // silah toplamak için    
    public PlayerPunch playerPunch;
    public GameObject rifleUI;

    [Header("Rifle Assign Things")]
    public PlayerScripts player;
    private float radius = 2.5f;    // silahi toplamak için çevresinde bir alan oluþturmak için
    public Animator animator;
    private float nextTimeToPunch =0f;  // sonraki yumruk zamaný
    public float punchCharge = 15f; // sonraki yumruk yükleme süresi



    private void Awake()
    {
        PlayerRifle.SetActive(false);   // silahsýz baslamak için
        rifleUI.SetActive(false);       // cephane sayýsý silah yokken görünmesin.
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)   
        {
            animator.SetBool("Punch",true); // punch animasyonu oynasýn
            animator.SetBool("Idle", false);    // idle animasyonu dursun
            nextTimeToPunch = Time.time + 1f / punchCharge; 

            playerPunch.Punch();    // punch fonksiyonu calýssýn
        }
        else
        {
            animator.SetBool("Punch", false);   // basýlmadýysa idle'a geçsin
            animator.SetBool("Idle", true);
        }
        if (Vector3.Distance(transform.position, player.transform.position) < radius)   // oyuncu silaha radius deðerinde yakýnsa
        {
            if (Input.GetKeyDown("f"))  // ve o mesafede f tuþuna basarsa
            {
                PlayerRifle.SetActive(true);    // silahý alsýn.
                PickupRifle.SetActive(false);   // toplanan yerdeki silah kapansýn.
                // ses
                // silah bulma görevi tamamlanacak
                ObjectivesC.occurence.GetObjectivesDone(true, false, false, false); // silah bulundu ilk görev tamamlandý diðerleri hala false

            }
        }
    }
}
