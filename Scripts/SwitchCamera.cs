using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;    // Aim kamerasý ve canvas tanýmý
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;        // TPS canvas ve cam

    [Header("Camera Animator")]
    public Animator animator;


    private void Update()
    {
        if (Input.GetButton("Fire2")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))   // eeðr sað týk ve w basýlý ise
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", true);    // niþanile yürüme animasyonlarý calýssýn
            animator.SetBool("Walk", true);

            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false); // niþan alýnýnca tps öðeleri kapansýn
            AimCam.SetActive(true); 
            AimCanvas.SetActive(true);  // niþan alýnýnca aim öðeleri aktif olsun
        }
        else if (Input.GetButton("Fire2")) // sadece sað týk basýlý ise
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", false);    // sadece dururken niþan alma animasyonu calýssýn
            animator.SetBool("Walk", false);

            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false); // niþan alýnýnca tps öðeleri kapansýn
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);  // niþan alýnýnca aim öðeleri aktif olsun
        }
        else
        {
            animator.SetBool("Idle", true); //sað týk basýlý deðil ise idleda kalsýn
            animator.SetBool("IdleAim", false); 
            animator.SetBool("RifleWalk", false);

            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true); 
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }
}
