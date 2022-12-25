using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public Text ammoText;
    public Text magText;

    public static AmmoCount occurence;  // mermi sayacý deðiþkeni

    private void Awake()
    {
        occurence = this;
    }

    public void UpdateAmmoText(int presentAmmuniton)
    {
        ammoText.text = "mermi " + presentAmmuniton;
    }

    public void UpdateMagText(int mag)
    {
        magText.text = "sarjor " + mag;
    }
}
