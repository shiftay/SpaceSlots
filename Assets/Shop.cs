using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    private const int NULL = -1;
    private List<int> COINS = new List<int>() { 250, 500, 1100, 2750, 5750, 11000 };

    public TextMeshProUGUI coinValue;
    public List<Button> shopButtons;
    public Sprite highlight, unhighlighted;
    private int currentSelected = -1;
    private Animator anim;
    private RouletteManager rm;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void Click(Button buttonPressed) {
        int clickedIndex = shopButtons.FindIndex(n => n == buttonPressed);

        if(currentSelected != NULL) 
            shopButtons[currentSelected].image.sprite = unhighlighted;

        if(currentSelected == clickedIndex) {
            // Selected the same one.
            rm.additionalcoins = COINS[clickedIndex];
            CloseShop();
            rm.coin.SetTrigger("Shake");
            rm.SetAddingMoney();



        } else  {
            shopButtons[clickedIndex].image.sprite = highlight;
            currentSelected = clickedIndex;
        }
    }

    public void OpenShop(RouletteManager data) {
        rm = data;
        coinValue.text = rm.currentData.coinAmount.ToString("F2");
        anim.SetTrigger("Open");
    }

    public void CloseShop() {
        anim.SetTrigger("Close");
        if(currentSelected != NULL)
            shopButtons[currentSelected].image.sprite = unhighlighted;
        currentSelected = NULL;
    }

}
