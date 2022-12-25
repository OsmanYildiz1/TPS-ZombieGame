using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheels Colliders")]
    public WheelCollider frontRightWheelCollider;       // tekerlek colliderlar�n� tan�mlad�k
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    public Transform frontRightWheelTransform;      // tekerlek transformlar�n� tan�mlad�k
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    public float accelerationForce= 100f;   // h�zg�c�
    public float breakingForce = 200f; // fren g�c�
    private float presentBreakForce = 0f; // su anki fren h�z�
    private float presentAcceleration = 0f; // su anki h�z

    [Header("Vehicle Steering")]    // direksiyon 
    public float wheelsTorque = 20f;    // tekerlek d�nmesi
    private float presentTurnAngle = 0f;    // d�n�� a��s�
        
    [Header("Vehicle Security")]    
    public PlayerScripts player;
    private float radius = 5f;          // ara� kap�s� i�in
    private bool isOpened = false;

    [Header("Disable Things")] // araca binince player controller�n etkisiz olmas� i�in
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public GameObject PlayerCharacter;

    [Header("Vehicle Hit Variables")]
    public Camera cam;
    public float hitRange = 2f;
    private float giveDamageOf = 100f;
    // public ParticleSystem hitSpark;
    public GameObject goreEffect;
    public GameObject DestroyEffect;


    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position)< radius)    // oyuncu posisyonu vehicle radiustan k���kse yani yak�nsa
        {
            if (Input.GetKeyDown(KeyCode.F))    // f'e basm��sa
            {
                isOpened = true;
                radius = 5000f;
                // objecttive completed
                ObjectivesC.occurence.GetObjectivesDone(true, true, true, false); // ara� bulundu ���nc� g�rev tamamland� sonuncusu hala false hala false
            }
            else if (Input.GetKeyDown(KeyCode.G))   // Oyuncu G'ye bas�p ara�tan inebilsin
            {
                player.transform.position = vehicleDoor.transform.position; // kap�n�n oldugu yere c�ks�n
                isOpened = false;
                radius = 5f;
            }
        }
        if (isOpened == true)
        {
            ThirdPersonCam.SetActive(false);     
            ThirdPersonCanvas.SetActive(false);         // ara�tayken aim cameralar� ve ni�angah g�r�nmemesi i�in
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            PlayerCharacter.SetActive(false);

            MoveVehicle();
            VehicleSteering();
            ApplyBreaks();
            HitZombies();
        }
        else if (isOpened == false)
        {
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);         // ara�tan ininc aim cameralar� ve ni�angah g�r�nmesi i�in
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            PlayerCharacter.SetActive(true);
        }
        
    }
    void MoveVehicle()
    {
        //FWD
        frontRightWheelCollider.motorTorque = presentAcceleration; // sa� �n teker, mevcut h�z de�eri kadar �eksin(tork g�c�)
        frontLeftWheelCollider.motorTorque = presentAcceleration;   // di�erleri i�inde ayn�s�
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;


        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");   // �imdiki h�z, h�z g�c� kadar ve ileri veya geri tu�una bas�ld��� de�eri als�n
    }

    void VehicleSteering()  // ara� direksiyonu y�netimi
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;   // sa� ve sol �n tekerlek i�in su anki girilen a�� verildi


        //animate wheels
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);

    }
    void SteeringWheels(WheelCollider WC, Transform WT) // tekerlerleri y�netmek i�in wheel collider ve transform de�erleriyle fonksiyon olusturdum
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);    // ger�ek d�nya a��s�na g�re
                                                    
        WT.position = position;   //vector3 pozisyonu wheel transformunun pozisyonuna atand�
        WT.rotation = rotation;   // quaternion y�n� wheel transformun y�n�ne atand�
    }

    void ApplyBreaks()  // fren uygulama
    {
        if (Input.GetKey(KeyCode.Space))
        {
            presentBreakForce = breakingForce;  // e�er space bas�l�rsa mevcut fren g�c� belirtilen fren g�c� olsun
        }
        else
        {
            presentBreakForce = 0f;
        }
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;      // space 'e bas�l�yorsa tekerlek �arp��malar�na mevcut fren g�c� eklensin
        backRightWheelCollider.brakeTorque = presentBreakForce;
    }
    void HitZombies()
    {
        RaycastHit hitInfo; // hit i�in raycast (g�r�nmeyen bir lazer) olu�turdum. 

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, hitRange)) ;
        // lazere fizik ekledik kameran�n baslang�c�ndan ilerisine do�ru, vurdu�umuz hedefi g�rmek i�in
        {
            // Debug.Log(hitInfo.transform.name);  // vurdu�unda vurulan nesnenin ismi konsolda g�z�ks�n

            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();    // vurulan zombinin bilgisi
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();    // vurulan zombinin bilgisi
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); // vuralan objenin bilgisi getirdik

            if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);   // objeye  zarar ver
                zombie1.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamge(giveDamageOf);   // objeye  zarar ver
                zombie2.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(goreEffectGo, 1f);
            }

            else if (objectToHit != null) // vurulan obje bo� de�ilse 
            {
                objectToHit.objectHitDamege(giveDamageOf);   // objeye 100 zarar ver
                GameObject DestroyGo = Instantiate(DestroyEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // woodefekti oynatmakk i�in hitinfo ile konumu ald�k.
                Destroy(DestroyGo, 1f); 
            }
        }
    }
}
