using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusGame : MonoBehaviour
{
    public List<BonusPlanet> bonusPlanets;
    int currentTier = 0;
    RouletteManager rm;
    
    public void Setup(RouletteManager rm) {
        // this.rm = rm;
        // if(rm.selectedPlanets.Count > 0) {
        //     ship.transform.localPosition = rm.selectedPlanets[rm.selectedPlanets.Count-1].checkmark.transform.localPosition;
        //     ship.transform.eulerAngles = Vector3.zero;
        // }

        // currentTier = rm.LEVEL / 2;

        // bonusPlanets.ForEach(n => {
        //     n.button.interactable = n.tier == currentTier;

        //     n.checkmark.SetActive(rm.selectedPlanets.Contains(n));

        //     if(n.tier == currentTier) {
        //         n.anim.SetTrigger("Flashing");
        //     } else {
        //         n.anim.SetTrigger("Default");
        //     }
        // });

        // currentMultiplierTxt.text = rm.bonus_CurrentMultiplier.ToString();
        // currentMultiplier.SetActive(rm.bonus_CurrentMultiplier > 1);     

        
    }

    public GameObject ship;
    public Animator rewardAnim;
    public GameObject currentMultiplier;
    public TextMeshProUGUI currentMultiplierTxt;
    public TextMeshProUGUI currentReward;
    const int BASECOINS = 150;

    public bool multiplier;

    public void SelectPlanet(int id) {
        
        // Even tiers == multipliers
        // Odd tiers == multipliers / coins
        int rMulti = 1;

        // Setting up numbers
        if(currentTier % 2 == 1) {
            if(Random.Range(0, 100) % 2 == 0) {
                secondNum.sprite = numbers[rMulti];
                multiplier = true;
            } else {
                float coins = BASECOINS * currentTier;
                thirdNum.sprite = numbers[(int)(coins % 10)];
                secondNum.sprite = numbers[(int)((coins % 100) / 10)];
                firstNum.sprite = numbers[(int)(coins / 100)];
                multiplier = false;
            }
        } else {
            rMulti = 2;

            firstNum.gameObject.SetActive(false);
            thirdNum.gameObject.SetActive(false);
            secondNum.sprite = numbers[rMulti];
            multiplier = true;
        }


        rm.bonus_CurrentMultiplier += rMulti;

        // Turn off the buttons and flashing
        bonusPlanets.ForEach(n => {
            n.button.interactable = false;
            n.anim.SetTrigger("Default");
        });

        // setup moving the shiop
        target = bonusPlanets.Find(n=> n.id == id).button.transform.localPosition;
        movingShip = true;
        startingPos = ship.transform.localPosition;
        
        // rm.selectedPlanets.Add(bonusPlanets.Find(n=> n.id == id));
        // Set Rewards
        close = true;
    }


    private bool movingShip;
    private bool close;
    private Vector3 target, startingPos;
    private float timeElapsed = 0;
    private float duration = 2;
    public List<Sprite> numbers;
    public Image firstNum, secondNum, thirdNum;
    void Update()
    {
        if(movingShip) {
            if(timeElapsed > duration) {
                movingShip = false;
                rewardAnim.SetTrigger("Open");
            } else {
                // ship.transform.rotation = Quaternion.LookRotation(target - ship.transform.localPosition, Vector3.forward);
                ship.transform.localPosition = Vector3.Lerp(startingPos, target, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
            }
        }
    }

    public void OpenScreen() {
        // rewardAnim.SetTrigger("Open");
        Invoke("Close", 1.5f);
    }

    public void OpenComputerForScreen() {
        rewardAnim.SetTrigger("Open");
        currentReward.text = multiplier ? "Multiplier." : "Coins.";
    }


    public void Close() {
        rewardAnim.SetTrigger("Close");

        if(close) {
            Invoke("CloseOut", 1f);    
        }
    }

    public void CloseOut() {
        rewardAnim.SetTrigger("Close");
        rm.canvasAnim.SetTrigger("Game");
        rm.ContinueBonus();
    }

    // public void CloseScreen() {
    //     rewardAnim.SetTrigger("Close");
    // }

    public void LeaveSpins(){}
    // Select Planet -> Move Ship -> Reward -> Close back into spins.
}



[System.Serializable]
public class BonusPlanet {
    public Animator anim;
    public GameObject checkmark;
    public Button button;
    public int tier;
    public int id;
}