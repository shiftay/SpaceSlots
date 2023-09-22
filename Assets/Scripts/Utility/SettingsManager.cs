using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public RouletteManager rouletteManager;
    public List<SettingsToggle> toggles;
    public Animator anim;

    void Start()
    {
        toggles.ForEach(n=> n.Setup(rouletteManager.currentData, this));
    }


    public void OpenSettings() => anim.SetTrigger("Open");

    public void CloseSettings() => anim.SetTrigger("Close");

}
