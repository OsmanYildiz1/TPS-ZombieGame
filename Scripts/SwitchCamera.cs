using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;    // Aim kameras� ve canvas tan�m�
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;        // TPS canvas ve cam

    [Header("Camera Animator")]
    public Animator animator;


    private void Update()
    {
        if (Input.GetButton("Fire2")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))   // ee�r sa� t�k ve w bas�l� ise
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", true);    // ni�anile y�r�me animasyonlar� cal�ss�n
            animator.SetBool("Walk", true);

            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false); // ni�an al�n�nca tps ��eleri kapans�n
            AimCam.SetActive(true); 
            AimCanvas.SetActive(true);  // ni�an al�n�nca aim ��eleri aktif olsun
        }
        else if (Input.GetButton("Fire2")) // sadece sa� t�k bas�l� ise
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", false);    // sadece dururken ni�an alma animasyonu cal�ss�n
            animator.SetBool("Walk", false);

            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false); // ni�an al�n�nca tps ��eleri kapans�n
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);  // ni�an al�n�nca aim ��eleri aktif olsun
        }
        else
        {
            animator.SetBool("Idle", true); //sa� t�k bas�l� de�il ise idleda kals�n
            animator.SetBool("IdleAim", false); 
            animator.SetBool("RifleWalk", false);

            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true); 
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }
}
