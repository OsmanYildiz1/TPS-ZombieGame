using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheels Colliders")]
    public WheelCollider frontRightWheelCollider;       // tekerlek colliderlarýný tanýmladýk
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    public Transform frontRightWheelTransform;      // tekerlek transformlarýný tanýmladýk
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    public float accelerationForce= 100f;   // hýzgücü
    public float breakingForce = 200f; // fren gücü
    private float presentBreakForce = 0f; // su anki fren hýzý
    private float presentAcceleration = 0f; // su anki hýz

    [Header("Vehicle Steering")]    // direksiyon 
    public float wheelsTorque = 20f;    // tekerlek dönmesi
    private float presentTurnAngle = 0f;    // dönüþ açýsý
        
    [Header("Vehicle Security")]    
    public PlayerScripts player;
    private float radius = 5f;          // araç kapýsý için
    private bool isOpened = false;

    [Header("Disable Things")] // araca binince player controllerýn etkisiz olmasý için
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
        if (Vector3.Distance(transform.position, player.transform.position)< radius)    // oyuncu posisyonu vehicle radiustan küçükse yani yakýnsa
        {
            if (Input.GetKeyDown(KeyCode.F))    // f'e basmýþsa
            {
                isOpened = true;
                radius = 5000f;
                // objecttive completed
                ObjectivesC.occurence.GetObjectivesDone(true, true, true, false); // araç bulundu üçüncü görev tamamlandý sonuncusu hala false hala false
            }
            else if (Input.GetKeyDown(KeyCode.G))   // Oyuncu G'ye basýp araçtan inebilsin
            {
                player.transform.position = vehicleDoor.transform.position; // kapýnýn oldugu yere cýksýn
                isOpened = false;
                radius = 5f;
            }
        }
        if (isOpened == true)
        {
            ThirdPersonCam.SetActive(false);     
            ThirdPersonCanvas.SetActive(false);         // araçtayken aim cameralarý ve niþangah görünmemesi için
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
            ThirdPersonCanvas.SetActive(true);         // araçtan ininc aim cameralarý ve niþangah görünmesi için
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            PlayerCharacter.SetActive(true);
        }
        
    }
    void MoveVehicle()
    {
        //FWD
        frontRightWheelCollider.motorTorque = presentAcceleration; // sað ön teker, mevcut hýz deðeri kadar çeksin(tork gücü)
        frontLeftWheelCollider.motorTorque = presentAcceleration;   // diðerleri içinde aynýsý
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;


        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");   // þimdiki hýz, hýz gücü kadar ve ileri veya geri tuþuna basýldýðý deðeri alsýn
    }

    void VehicleSteering()  // araç direksiyonu yönetimi
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;   // sað ve sol ön tekerlek için su anki girilen açý verildi


        //animate wheels
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);

    }
    void SteeringWheels(WheelCollider WC, Transform WT) // tekerlerleri yönetmek için wheel collider ve transform deðerleriyle fonksiyon olusturdum
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);    // gerçek dünya açýsýna göre
                                                    
        WT.position = position;   //vector3 pozisyonu wheel transformunun pozisyonuna atandý
        WT.rotation = rotation;   // quaternion yönü wheel transformun yönüne atandý
    }

    void ApplyBreaks()  // fren uygulama
    {
        if (Input.GetKey(KeyCode.Space))
        {
            presentBreakForce = breakingForce;  // eðer space basýlýrsa mevcut fren gücü belirtilen fren gücü olsun
        }
        else
        {
            presentBreakForce = 0f;
        }
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;      // space 'e basýlýyorsa tekerlek çarpýþmalarýna mevcut fren gücü eklensin
        backRightWheelCollider.brakeTorque = presentBreakForce;
    }
    void HitZombies()
    {
        RaycastHit hitInfo; // hit için raycast (görünmeyen bir lazer) oluþturdum. 

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, hitRange)) ;
        // lazere fizik ekledik kameranýn baslangýcýndan ilerisine doðru, vurduðumuz hedefi görmek için
        {
            // Debug.Log(hitInfo.transform.name);  // vurduðunda vurulan nesnenin ismi konsolda gözüksün

            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();    // vurulan zombinin bilgisi
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();    // vurulan zombinin bilgisi
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); // vuralan objenin bilgisi getirdik

            if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);   // objeye  zarar ver
                zombie1.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamge(giveDamageOf);   // objeye  zarar ver
                zombie2.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // goreefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(goreEffectGo, 1f);
            }

            else if (objectToHit != null) // vurulan obje boþ deðilse 
            {
                objectToHit.objectHitDamege(giveDamageOf);   // objeye 100 zarar ver
                GameObject DestroyGo = Instantiate(DestroyEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)); // woodefekti oynatmakk için hitinfo ile konumu aldýk.
                Destroy(DestroyGo, 1f); 
            }
        }
    }
}
