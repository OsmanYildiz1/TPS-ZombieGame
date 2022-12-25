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
        selectCharacter.SetActive(true);    // karakter se�iminde ana men� g�z�kmesin
        mainMenu.SetActive(false);
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("ZombieLand");    // oynaya bas�ld���nda 1 sahnesini y�kle
    }
      
    public void OnQuitButton()
    {
        Debug.Log("Quitting game..");   // quite t�klay�nca oyundan ��k
        Application.Quit();
    }
}
