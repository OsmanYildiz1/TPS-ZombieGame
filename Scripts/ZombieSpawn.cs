using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [Header("Zombie Spawn Variables")]
    public GameObject zombiePrefab;
    public Transform zombieSpawnPosition1;
    public Transform zombieSpawnPosition2;
    public GameObject dangerZone1;
    // public GameObject dangerZone1;
    private float repeatCycle =1f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip dangerZoneSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   // oyuncu triggera çarparsa
        {
            InvokeRepeating("enemySpawner", 1f, repeatCycle);   // 1 saniye arayla tekrar
            audioSource.PlayOneShot(dangerZoneSound);
            StartCoroutine(dangerZoneTimer());
            Destroy(gameObject, 7f);    // oyuncu colliderdan geçtikten 5 saniye sonra collider yok olsun
            gameObject.GetComponent<BoxCollider>().enabled = false; 
        }
    }

    void enemySpawner()
    {
        Instantiate(zombiePrefab, zombieSpawnPosition1.position, zombieSpawnPosition1.rotation); // zombie spawn pozisyonunda zombi üret
        Instantiate(zombiePrefab, zombieSpawnPosition2.position, zombieSpawnPosition2.rotation);
    }
    IEnumerator dangerZoneTimer()
    {
        dangerZone1.SetActive(true);
        yield return new WaitForSeconds(5f);
        dangerZone1.SetActive(false);
    }
}
