using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    
    public Image mainImage;
    public Sprite _enabled, _disabled;
    private bool currentPhase = true;
    private SettingsManager settingsManager;
    public ToggleID id;

    // Called by button click
    public void Toggle() {
        currentPhase = !currentPhase;

        mainImage.sprite = currentPhase ? _enabled : _disabled;

        switch(id) {
            case ToggleID.MUSIC:
                settingsManager.rouletteManager.audioManager.UpdateMusicVolume(currentPhase);
                break;
            case ToggleID.SFX:
                settingsManager.rouletteManager.audioManager.UpdateSFXVolume(currentPhase);
                break;
        }
    }

    public void Setup(GameData gameData, SettingsManager settingsManager) {
        this.settingsManager = settingsManager;
        switch(id) {
            case ToggleID.MUSIC:
                currentPhase = gameData.currentMusic;
                break;
            case ToggleID.SFX:
                currentPhase = gameData.currentSfx;
                break;
        }

        mainImage.sprite = currentPhase ? _enabled : _disabled;
    }
}

public enum ToggleID { MUSIC, SFX }