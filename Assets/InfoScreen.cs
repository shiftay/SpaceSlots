using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    private int currentPage;
    public List<GameObject> pages;
    RouletteManager rm;

    public void Init(RouletteManager rouletteManager) { 
        rm = rouletteManager; 
        Setup();
    }

    public void Setup() {
        // Setup / Init
        currentPage = 0;
        for(int i = 0; i < pages.Count; i++) pages[i].SetActive(currentPage == i);
    }

    public void Next() {
        currentPage++;
        if(currentPage >= pages.Count) currentPage = 0;

        for(int i = 0; i < pages.Count; i++) pages[i].SetActive(currentPage == i);
    }


    public void Back() {
        currentPage++;
        if(currentPage < 0) currentPage = pages.Count - 1;

        for(int i = 0; i < pages.Count; i++) pages[i].SetActive(currentPage == i);
    }

    public void Close() {
        rm.CloseInfo();
    }

}
