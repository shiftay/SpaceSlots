using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    private const int BONUS_SPIN_MODIFIER = 2;
    private const int EXPERIENCE = 10;

    public Transform area_parent;
    public List<Sprite> potentialSprites;
    public List<Sprite> experienceSprites;
    protected float time;
    public bool toggleRoulette;
    public float speed;
    int currentChoice;
    public RouletteManager rm;

    // Update is called once per frame
    void Update()
    {
        if(toggleRoulette && time > speed) {
            time = 0;

            currentItem = area_parent.GetChild(area_parent.childCount - 1);

            //
            // TODO: Update this for if it should include bonus spins.
            // currentChoice = Random.Range(0, potentialSprites.Count - BONUS_SPIN_MODIFIER);

            currentChoice = rm.doingBonusSpins ? Randomizer.BonusRoll() : Randomizer.BasicRoll();

            currentItem.GetComponent<Image>().sprite = potentialSprites[currentChoice];

            Transform expLevel = currentItem.GetChild(0);
            expLevel.gameObject.SetActive(currentChoice == EXPERIENCE);
            if(currentChoice == EXPERIENCE) {
                expLevel.GetComponent<Image>().sprite = experienceSprites[Random.Range(0,experienceSprites.Count)];
            } 
            
            currentItem.SetAsFirstSibling();
        } else if(toggleRoulette) {
            time += Time.fixedDeltaTime;       
        }
    }


    public void Spin() {
        for(int i = 0; i < area_parent.childCount; i++){
            currentItem = area_parent.GetChild(i);
            if(!currentItem.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default")){
                currentItem.GetComponent<Animator>().SetTrigger("Default");
                Debug.Log("Inside Spin on Roulette");
            }
               
            // currentItem.GetComponent<Animator>().enabled = false;
        }
    }

// 2 to 6 = 0 to 4

    public void RunAnimation(int index) {
        int childIndex = BonusSpins.scale(0, 4, 2, 6, index);

        currentItem = area_parent.GetChild(childIndex);

        Debug.LogWarning("Run Animate??? " + index + " , " + childIndex);
        



        currentItem.GetComponent<Animator>().SetTrigger(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite).ToString());
        
        // currentItem.GetComponent<Animator>().enabled = true;
    }

    public void VerticalWin() {
        for(int i = 2; i < 7; i++) {
            currentItem = area_parent.GetChild(i);
            
            currentItem.GetComponent<Animator>().SetTrigger(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite).ToString());
            // currentItem.GetComponent<Animator>().enabled = true;
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
        
        for(int i = 2; i < 7; i++) {
            currentItem = area_parent.GetChild(i);
            retVal.Add(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite));
        }

        return retVal;
    }

    public List<ExperienceStar> Experience() {
        
        List<ExperienceStar> retVal = new List<ExperienceStar>();

        for(int i = 2; i < 7; i++) {
            currentItem = area_parent.GetChild(i);
            if(potentialSprites.FindIndex(n => n == currentItem.GetComponent<Image>().sprite) == EXPERIENCE) {
                retVal.Add(new ExperienceStar(i - 2, experienceSprites.FindIndex(n => n == currentItem.GetChild(0).GetComponent<Image>().sprite)));
            }
        }

        return retVal;
    }

    public class ExperienceStar {
        public int index, value;

        public ExperienceStar(int index, int value) {
            this.index = index;
            this.value = value;
        }
    }
}
