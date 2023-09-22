using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierController : MonoBehaviour
{
    public Image first, second;

    public List<Sprite> numbers;

    public Animator anim;

    public void SetMultiplier(int value) {
        int secondVal = value % 10;
        int firstVal = (value - secondVal) / 10;
        
        first.gameObject.SetActive(firstVal > 0);

        first.sprite = numbers[firstVal];
        second.sprite = numbers[secondVal];
    }

    public void ShowMultiplier() => anim.SetTrigger("Open");

    public void HideMultiplier() => anim.SetTrigger("Close");
}
