using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;  // silah toplamak i�in    
    public PlayerPunch playerPunch;
    public GameObject rifleUI;

    [Header("Rifle Assign Things")]
    public PlayerScripts player;
    private float radius = 2.5f;    // silahi toplamak i�in �evresinde bir alan olu�turmak i�in
    public Animator animator;
    private float nextTimeToPunch =0f;  // sonraki yumruk zaman�
    public float punchCharge = 15f; // sonraki yumruk y�kleme s�resi



    private void Awake()
    {
        PlayerRifle.SetActive(false);   // silahs�z baslamak i�in
        rifleUI.SetActive(false);       // cephane say�s� silah yokken g�r�nmesin.
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)   
        {
            animator.SetBool("Punch",true); // punch animasyonu oynas�n
            animator.SetBool("Idle", false);    // idle animasyonu dursun
            nextTimeToPunch = Time.time + 1f / punchCharge; 

            playerPunch.Punch();    // punch fonksiyonu cal�ss�n
        }
        else
        {
            animator.SetBool("Punch", false);   // bas�lmad�ysa idle'a ge�sin
            animator.SetBool("Idle", true);
        }
        if (Vector3.Distance(transform.position, player.transform.position) < radius)   // oyuncu silaha radius de�erinde yak�nsa
        {
            if (Input.GetKeyDown("f"))  // ve o mesafede f tu�una basarsa
            {
                PlayerRifle.SetActive(true);    // silah� als�n.
                PickupRifle.SetActive(false);   // toplanan yerdeki silah kapans�n.
                // ses
                // silah bulma g�revi tamamlanacak
                ObjectivesC.occurence.GetObjectivesDone(true, false, false, false); // silah bulundu ilk g�rev tamamland� di�erleri hala false

            }
        }
    }
}
