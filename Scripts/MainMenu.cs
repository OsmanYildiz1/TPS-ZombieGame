using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject mainMenu;


    public void OnSelectCharacter()
    {
        selectCharacter.SetActive(true);    // karakter seçiminde ana menü gözükmesin
        mainMenu.SetActive(false);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("ZombieLand");    // oynaya basýldýðýnda 1 sahnesini yükle
    }
      
    public void OnQuitButton()
    {
        Debug.Log("Quitting game..");   // quite týklayýnca oyundan çýk
        Application.Quit();
    }
}
