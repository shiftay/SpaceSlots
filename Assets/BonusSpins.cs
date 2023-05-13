using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BonusSpins : MonoBehaviour
{
    const float BETMINIMUM = 20.0f;
    const float BETMAXIMUM = 500.0f;
    
    public List<Sprite> numbers;
    public Image firstNum, secondNum;

    public TextMeshProUGUI value, calculating;

    public Animator main, screen;
    private bool makingScreen;
    RouletteManager rm;
    private float currentBet;

    public bool leavingSpins = false;

    public void Open(RouletteManager rm) {
        this.rm = rm;
        currentBet = rm.currentData.currentBonusBet;
        calculating.text = "";
        calc = null;
        glowLoops = 0;
        leavingSpins = false;
        calculating.alignment = TextAlignmentOptions.Left;
        value.text = currentBet.ToString("F2");
        main.SetTrigger("Open");
    }

    public void OpenComputerForScreen() {
        main.SetTrigger("Open");
        makingScreen = true;
    }

    public void CloseScreen() {
        leavingSpins = true;
        screen.SetTrigger("Close");
        main.SetTrigger("Close");
    }

    public void OpenScreen() {
        if(makingScreen) {
           screen.SetTrigger("Open");
           makingScreen = false; 
        } else {
            
            // Setup Loot in the screen.
            main.SetTrigger("Open");
            calculating.text = "+" + spins.ToString() + " Bonus Spins";   
        }
    }

    public void LeaveSpins() {
        if(leavingSpins) main.SetTrigger("Close");
    }

    public void AddMoney() {
        currentBet += BETMINIMUM;
        if(currentBet > BETMAXIMUM) currentBet = BETMAXIMUM;
        SetText();
    }

    public void RemoveMoney() {
        currentBet -= BETMINIMUM;
        if(currentBet < BETMINIMUM) currentBet = BETMINIMUM;
        SetText();
    }

    private void SetText() {
        value.text = currentBet.ToString();
    }

    private int spins;
    private int glowLoops;
    private const int MAXLOOPS = 5;
    public void Glowing() {
        main.SetTrigger("Close");
        rm.SetSpins(spins);
        leavingSpins = true;
    }

    bool purchased = false;
    public void BuySpins() {
        if(rm.currentData.coinAmount < currentBet) return;

        rm.currentData.coinAmount -= currentBet;

        rm.SetCoinValue();


        spins = Random.Range(5, 10) + scale(1, 25, 1, 10, (int)(currentBet / BETMINIMUM));

        secondNum.sprite = numbers[spins % 10];
        firstNum.sprite = numbers[1];
        firstNum.gameObject.SetActive(Mathf.CeilToInt(spins / 10) > 0);

        screen.SetTrigger("Close");
        main.SetTrigger("Close");
        purchased = true;
        main.SetTrigger("Analyze");
    }

    public static int scale(int OldMin, int OldMax, int NewMin, int NewMax, int OldValue){
        int OldRange = (OldMax - OldMin);
        int NewRange = (NewMax - NewMin);
        int NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
    
        return(NewValue);
    }

    Coroutine calc = null;

    public void Analyze() {
        if(calc == null)
            calc = StartCoroutine(Calculating());
    }

    List<string> Calc = new List<string>() { "Calculating Bonus Spins .", "Calculating Bonus Spins . .", "Calculating Bonus Spins . . ."};

    IEnumerator Calculating() {
        float timer = 0;
        int index = 0;
        
        while(timer < 5f) {
            calculating.text = Calc[index];

            yield return new WaitForEndOfFrame();
            if(Time.frameCount % 20 == 0){
                index++;
                if(index > 2) index = 0;
            }            
            timer += Time.fixedDeltaTime;
        }
       
       main.SetTrigger("Open");
       calc = null;
       calculating.text = "";
       calculating.alignment = TextAlignmentOptions.Center;
    }


}
