using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject mainMenu;

    public void onBackButton()
    {
        selectCharacter.SetActive(false);    // backa basýlýnca ana menüye dön
        mainMenu.SetActive(true);
    }
    public void onCharacter1()
    {
        SceneManager.LoadScene("ZombieLand");
    }

    public void onCharacter2()
    {
        SceneManager.LoadScene("ZombieLand1");
    }

    public void onCharacter3()
    {
        SceneManager.LoadScene("ZombieLand2");
    }
}
