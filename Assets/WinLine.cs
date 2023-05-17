using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLine : MonoBehaviour
{
    private Animator anim;
    private Image image;
    public List<Sprite> sprites;
    private bool isRunning = false;
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
    }


    public void TurnOn(int id) {
        image.sprite = sprites[id];
        anim.SetTrigger("On");
        isRunning = true;
    }

    public void TurnOff() {
        if(isRunning) {
            anim.SetTrigger("Off");
            isRunning = false;
        }
    }

}
