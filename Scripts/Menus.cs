using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;    // olu�turdu�um go'lerin tan�m�
    public GameObject objectiveMenuUI;

    public static bool GameIsStopped = false;   // oyun durdurulmu� mu? anlamak i�in bool


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   // gameisstopped baslang�cta false oldu�u i�in ters bir i�leyi� olu�turuldu yani �nce men�den c�k�lan durumlar kontrol edildi
            if (GameIsStopped)  // esc'ye bas�ld���nda oyun devam ediyorsa yani men�den ��k�ld�ysa resume
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else    // esc ile oyun durduruluyorsa
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;     // menudeyken imlec g�r�n�r olsun
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (GameIsStopped)// o'ya bas�ld���nda oyun devam ediyorsa yani g�revlerden ��k�ld�ysa resume
            {
                removeObjectives();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else  // o ile g�revlere bak�lacaksa
            {
                ShowObjectives();
                Cursor.lockState = CursorLockMode.None;     // g�revler ekran�nda imlec g�r�n�r olsun
            }
        }
    }
    public void ShowObjectives()
    {
        objectiveMenuUI.SetActive(true);    // g�revlerin g�r�n�rl��� ac�k olsun
        Time.timeScale = 0f;    // zaman dursun
        GameIsStopped = true;   // oyun dursun
    }

    public void removeObjectives()
    {
        objectiveMenuUI.SetActive(false);    // g�revlerin g�r�n�rl��� kapal� olsun
        Time.timeScale = 1f;    // zaman normal devam etsin
        Cursor.lockState = CursorLockMode.Locked;   // oyun i�inde imle� kilitli olsun
        GameIsStopped = false;   // oyun devam etsin
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);   // pause menu g�r�nmesin
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;   // oyun i�inde imle� kilitli olsun
        GameIsStopped = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);    // pause men� g�r�ns�n ve oyun dursun
        Time.timeScale = 0f;
        GameIsStopped = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("ZombieLand");    // yeniden oynaya bas�ld���nda 1 sahnesini y�kle
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");    // men�ye bas�ld���nda ana men�y� sahnesini y�kle
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..");   // quite t�klay�nca oyundan ��k
        Application.Quit();
    }
}
