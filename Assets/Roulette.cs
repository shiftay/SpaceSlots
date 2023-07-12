using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    private const int BONUS_SPIN = 9;
    private const int EXPERIENCE = 10;

    public Transform area_parent;
    public List<Sprite> potentialSprites;
    public List<Sprite> experienceSprites;
    protected float time;
    public bool toggleRoulette;
    public float speed;
    int currentChoice;
    public RouletteManager rm;
    public int index = 0;


    public void Spin() {
        for(int i = 0; i < area_parent.childCount; i++){
            currentItem = area_parent.GetChild(i);
            if(!currentItem.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default")){
                currentItem.GetComponent<Animator>().SetTrigger("Default");
            }
        }

        index = 0;
    }

    public void RunAnimation(int index) {
        currentItem = area_parent.GetChild(index);
        currentItem.GetComponent<Animator>().SetTrigger(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite).ToString());

    }

    public void SetSlots() {
        for(int i = 0; i < area_parent.childCount; i++) {
            currentItem = area_parent.GetChild(i);

            currentChoice = rm.doingBonusSpins ? Randomizer.BonusRoll() : Randomizer.BasicRoll();

            currentItem.GetComponent<Image>().sprite = potentialSprites[currentChoice];

            Transform expLevel = currentItem.GetChild(0);
            expLevel.gameObject.SetActive(currentChoice == EXPERIENCE);
            if(currentChoice == EXPERIENCE) {
                expLevel.GetComponent<Image>().sprite = experienceSprites[Random.Range(0,experienceSprites.Count)];
            } 
        }
    }

    private Transform currentItem;
    public void UpdateAll() {
        for(int i = 0; i < area_parent.childCount; i++) {
            currentChoice = Randomizer.BasicRoll();
            currentItem = area_parent.GetChild(i);
            currentItem.GetComponent<Image>().sprite = potentialSprites[currentChoice];
            // currentItem.GetComponent<Animator>().SetTrigger("Default");
            // Debug.Log("Inside UpdateAll");
            currentItem.GetChild(0).gameObject.SetActive(currentChoice == EXPERIENCE);
        }
    }

    public List<int> Results() {
        List<int> retVal = new List<int>();
        
        for(int i = 0; i < area_parent.childCount; i++) {
            currentItem = area_parent.GetChild(i);
            retVal.Add(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite));
        }

        return retVal;
    }

    public List<ExperienceStar> Experience() {
        
        List<ExperienceStar> retVal = new List<ExperienceStar>();

        for(int i = 0; i < area_parent.childCount; i++) {
            currentItem = area_parent.GetChild(i);
            if(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite) == EXPERIENCE) {
                retVal.Add(new ExperienceStar(i - 2, experienceSprites.FindIndex(n => n == currentItem.GetChild(0).GetComponent<Image>().sprite)+1));
            }
        }

        return retVal;
    }

    public int BonusSpin() {
        int retVal = 0;

        for(int i = 0; i < area_parent.childCount; i++) {
            currentItem = area_parent.GetChild(i);
            if(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite) == BONUS_SPIN) {
                retVal++;
                currentItem.GetComponent<Animator>().SetTrigger(BONUS_SPIN.ToString());
            }
        }

        return retVal;
    }


}

public class ExperienceStar {
    public int index, value;

    public ExperienceStar(int index, int value) {
        this.index = index;
        this.value = value;
    }
}



