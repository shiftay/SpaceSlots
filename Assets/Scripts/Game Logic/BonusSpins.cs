using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BonusSpins : MonoBehaviour
{

    private const int NULL = -1;
    private List<BonusSpinInfo> spinInfos = new List<BonusSpinInfo>() { 
        new BonusSpinInfo(0, 4, 1, 4, 100, 15), 
        new BonusSpinInfo(1, 6, 2, 5, 250, 25),
        new BonusSpinInfo(2, 8, 2, 6, 500, 45),
        new BonusSpinInfo(3, 10, 3, 7, 1000, 65),
        new BonusSpinInfo(4, 15, 3, 11, 2500, 100),
    };
    private bool makingScreen;
    RouletteManager rm;
    private float currentBet;
    public Animator anim;
    public bool leavingSpins = false;

    public Sprite highlight, unhighlighted;
    /* 
        Amount Spins, Additional Range, Price
        4, 1-3, 100
        6, 2-4, 250
        8, 2-5, 500
        10, 3-6, 1000
        15, 3-10, 2500
    */

    public void Open(RouletteManager rm) {
        this.rm = rm;
        anim.SetTrigger("Open");
    }

    public void OpenComputerForScreen() {

        makingScreen = true;
    }

    public void CloseScreen() {
        anim.SetTrigger("Close");
    }

    public void LeaveSpins() {
       
    }

    public List<Button> shopButtons;
    private int currentSelected = -1;

    public void Click(Button buttonPressed) {
        int clickedIndex = shopButtons.FindIndex(n => n == buttonPressed);

        if(currentSelected != NULL) 
            shopButtons[currentSelected].image.sprite = unhighlighted;

        if(currentSelected == clickedIndex) {
            // Selected the same one.
            BonusSpinInfo temp = spinInfos.Find(n => n.id == clickedIndex);

            if(rm.currentData.coinAmount < temp.price) {
                // TODO: Play sfx + vfx for not having the money to purchase
                shopButtons[clickedIndex].image.sprite = highlight;
                currentSelected = clickedIndex;
                return;
            }

            anim.SetTrigger("Close");
            rm.currentData.coinAmount -= temp.price;
            rm.currentData.currentBet = temp.betInfo;
            rm.SetCoinValue();
            rm.SetSpins(temp.baseSpin + Random.Range(temp.additionalMin, temp.additionalMax));
           
        } else  {
            shopButtons[clickedIndex].image.sprite = highlight;
            currentSelected = clickedIndex;
        }
    } 


    private class BonusSpinInfo {
        public int baseSpin, additionalMin, additionalMax, price, id, betInfo;
        public BonusSpinInfo(int id, int baseSpin, int min, int max, int price, int betInfo) {
            this.id = id; this.baseSpin = baseSpin; additionalMin = min; additionalMax = max; this.price = price; this.betInfo = betInfo;
        }
    }
}
