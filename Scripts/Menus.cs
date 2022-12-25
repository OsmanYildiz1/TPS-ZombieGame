using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;    // oluþturduðum go'lerin tanýmý
    public GameObject objectiveMenuUI;

    public static bool GameIsStopped = false;   // oyun durdurulmuþ mu? anlamak için bool


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   // gameisstopped baslangýcta false olduðu için ters bir iþleyiþ oluþturuldu yani önce menüden cýkýlan durumlar kontrol edildi
            if (GameIsStopped)  // esc'ye basýldýðýnda oyun devam ediyorsa yani menüden çýkýldýysa resume
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else    // esc ile oyun durduruluyorsa
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;     // menudeyken imlec görünür olsun
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (GameIsStopped)// o'ya basýldýðýnda oyun devam ediyorsa yani görevlerden çýkýldýysa resume
            {
                removeObjectives();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else  // o ile görevlere bakýlacaksa
            {
                ShowObjectives();
                Cursor.lockState = CursorLockMode.None;     // görevler ekranýnda imlec görünür olsun
            }
        }
    }
    public void ShowObjectives()
    {
        objectiveMenuUI.SetActive(true);    // görevlerin görünürlüðü acýk olsun
        Time.timeScale = 0f;    // zaman dursun
        GameIsStopped = true;   // oyun dursun
    }

    public void removeObjectives()
    {
        objectiveMenuUI.SetActive(false);    // görevlerin görünürlüðü kapalý olsun
        Time.timeScale = 1f;    // zaman normal devam etsin
        Cursor.lockState = CursorLockMode.Locked;   // oyun içinde imleç kilitli olsun
        GameIsStopped = false;   // oyun devam etsin
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);   // pause menu görünmesin
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;   // oyun içinde imleç kilitli olsun
        GameIsStopped = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);    // pause menü görünsün ve oyun dursun
        Time.timeScale = 0f;
        GameIsStopped = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("ZombieLand");    // yeniden oynaya basýldýðýnda 1 sahnesini yükle
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");    // menüye basýldýðýnda ana menüyü sahnesini yükle
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..");   // quite týklayýnca oyundan çýk
        Application.Quit();
    }
}
