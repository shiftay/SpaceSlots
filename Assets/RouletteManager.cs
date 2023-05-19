using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RouletteManager : MonoBehaviour
{
    private const int EXPPERLEVEL = 20;
    private const int WILDCARD = 8;
    private const float MAXTIME = 2.25f;
    public List<Roulette> roulettes;
    public Loader loader;
    public GameData currentData;
    public Shop shop;
    public BonusSpins bonusSpins;
    public WinLine winLine;
    public Animator canvasAnim;
    public Animator levelUpAnim;
    public GameObject bonusGameBtn;
    public BonusGame bonusGame;
    public List<BonusPlanet> selectedPlanets;

    private void ToggleRoulettes(bool toggle, bool setAnims = false, float speed = 0f) {
        roulettes.ForEach(n => {
            n.speed = speed;
            n.toggleRoulette = toggle;
            if(setAnims) n.Spin();
        });
    }

    private bool isRunning = false, slowDown = false;
    public float timer = 0.0f;
    private float coinValue = 10000f;
    protected List<RouletteWin> rWin = new List<RouletteWin>();
    List<List<int>> Results = new List<List<int>>();

    // DEBUG VALUES
   
    public bool continuous;
    private bool Won = false;
    public bool printValues;
    public bool writeNewFile;
    // DEBUG VALUES


    private void Awake() {
        loader = GetComponent<Loader>();

        currentData = loader.LoadFile(writeNewFile);

        roulettes.ForEach(n => {
            n.UpdateAll();
            n.rm = this;
        });

        doingBonusSpins = false;

        coinValue = currentData.coinAmount;
        SetBetValue();
        SetCoinValue();
    }

    public bool AddingMoney;
    public bool doingBonusSpins;
    public float coinTimer;
    public Animator coin;
    public Animator fakeSpin;
    private float timeElapsed = 0;
    private float duration = 2;

    private bool LevelUp = false, GainedXP = false;
/*
    Bonus Spins


    do bonus Spins


*/


    private void FixedUpdate() {

        if(isRunning) {
            timer += Time.fixedDeltaTime;
            // Stops the Roulettes to begin looking for a win.
            if(timer > MAXTIME ) {
                timer = 0f; 
                fakeSpin.SetTrigger("Stop");
                ToggleRoulettes(false);
                isRunning = slowDown = false;

                RunResults();

                if(Won) {
                    // Animates the Winning tiles
                    // Tells the Column which row is part of the winning 
                    rWin[0].WinningCoords.ForEach(n => {
                        roulettes[n.Column].RunAnimation(n.Row);
                    });
            

                    ShowWin(rWin);

                    if(doingBonusSpins) CheckSpinAmt();
                    else {
                        winLine.TurnOn(rWin[0].winId);
                        start.interactable = true;
                    }
                   
                } else {
                    // *** DEBUG MODE *** 
                    // To let constant spinning to see wins.
                    if(doingBonusSpins) CheckSpinAmt();
                    else start.interactable = true;

                    if(continuous) Spin();
                }


            }
        }


 
   
        // Create a Money adding effect of slow gain.
        if(AddingMoney) {
            if(currentData.coinAmount == TargetCoins) {
                coin.SetTrigger("Complete");
                timeElapsed = 0;
                AddingMoney = false;
            } else {
                // Option to use Mathf.SmoothStep
                currentData.coinAmount = Mathf.Lerp(InitialCoins, TargetCoins, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                SetCoinValue();
            }
        }
    }


    public void ShowBonusGame() {
        bonusGame.Setup(this);

        levelUpAnim.SetTrigger("Close");
        canvasAnim.SetTrigger("Bonus");
    }


    public void CheckSpinAmt() {
        if(spinAmount > 0 ) {
            if(!GainedXP) Invoke("Spin", 2.5f);
        } else {
            doingBonusSpins = false;
            start.interactable = true;

            xpAnim.SetTrigger("FadeOut");
            bonusAnimator.SetTrigger("Close");
        }
    }

    public void RunSpins() {
        timer += Time.fixedDeltaTime;
        // Stops the fake Spin animatiion
        if(timer > 1f && !fakeSpin.GetCurrentAnimatorStateInfo(0).IsName("RouletteComplete")) {
            fakeSpin.SetTrigger("Stop");
        } 
        // Slows down the Roulette spins
        if(timer > 2f && !slowDown) {
            slowDown = true;
            ToggleRoulettes(true, false, 0.25f);
        }
    }


    public void RunResults() {
        // Gathers the results column by column
        Results = new List<List<int>>();
        roulettes.ForEach(n => Results.Add(n.Results()));

        // TODO Check for Experience, Bonus Spins


        // *** DEBUG ***
        // Prints out the values of the roulettes
        if(printValues) {
            string temp = "";
            for(int i = 0; i < Results.Count; i++) {
                
                Results[i].ForEach(n => temp += " " + n + " ");
                temp += "\n";
            }
            Debug.Log(temp);
        }

        rWin = new List<RouletteWin>();
        
        PullCoordinates();
        CheckForWin();

        // TODO: Needs to grab all Experience 
        if(doingBonusSpins) {
            List<ExperienceStar> exp = new List<ExperienceStar>();
            roulettes.ForEach(n => {
                exp.AddRange(n.Experience());
            });


            GainedXP = exp.Count > 0;
            if(exp.Count > 0) {
                TargetXP = 0;
                // Add Exp up and add it to the bar
                for(int i = 0; i < exp.Count; i++) {
                    TargetXP += exp[i].value;
                }

                Debug.Log("Target " + TargetXP + " exp Count " + exp.Count);

                LevelUp = SetXP();
            } 

            int extraSpins = 0;
            roulettes.ForEach(n=> {
                extraSpins += n.BonusSpin();
            });
            
            spinAmount += extraSpins;
        }

        // Checks Diagonal
        // rWin.AddRange(DiagonalWin(Results));
        
        rWin.ForEach(n => {
            if(n.Won) {
                // Debug.LogError("Winner @ " + n.indexOfWin + " " + n.wintype);
                Won = true;
            } 
        });

        isRunning = false;
    }



#region Look for Wins

    List<Coordinates> coordinates = new List<Coordinates>();
    private void PullCoordinates() {
        coordinates.Clear();

        for(int i = 0; i < roulettes.Count; i++) {
            List<int> temp = roulettes[i].Results();

            for(int j = 0; j < temp.Count; j++) {
                coordinates.Add(new Coordinates(j, i, temp[j]));
            }
        }
    }


    private void CheckForWin() {
        List<WinDefinitions> defn = WinDefinitions.ReturnDefs();

        defn.ForEach(n => {
            int lookup = -1;
            int comparison = -1;
            bool setLookup = true;
            bool winning = true;
            
            for(int i = 0; i < n.winningCoords.Count; i++) {
                // Debug.Log(n.winningCoords[i].Column +", "+ n.winningCoords[i].Row + ", " + n.id);
                comparison = coordinates.Find(x => x.Column == n.winningCoords[i].Column && x.Row == n.winningCoords[i].Row).Value;


                if(comparison != WILDCARD) {
                    // Debug.Log(lookup + " == " + comparison);

                    if(setLookup) {
                        lookup = comparison;  
                        setLookup = false;
                        continue;
                    }

                    if(comparison != lookup) {
                        winning = false;
                        break;
                    }
                } 
            }

            if(winning) {
                rWin.Add(new RouletteWin(n.id, lookup, n.WinMulti, n.winningCoords));
            }
        });

        rWin = rWin.OrderByDescending(o=>o.winId).ToList();
        
        // Debug.Log(rWin.Sort((x, y) => x.winId.CompareTo(y.winId))[0].winId);
    }



    
#endregion

#region Notes
// Define
    // [x] Horizontal Win 
    // [x] Verical Win
    // [x] Diagonal Win
    // [x] X Win
    // [x] T Win
    // [x] Inverted T Win
    // [] L Win
    // [x] Square Win
    // Define 
    // [x] WildCards
    // [] Multiplier Gain + Use
    // [] Bonus Spins (Auto Spin with Cumulative win and a chance to gain more spins.)

    // Regular Roulette, will not feature bonus spins

    // Change the chance of specific
#endregion Notes

#region UI

    public Button start, bonusSpinBtn;
    public TextMeshProUGUI betValue, winValue, coinAmount, expLevel, bonusSpinValue, levelupSpinInfo;
    public Image xpSlider;


    private const float MAXBET = 10.00f;
    private const float MINBET = 2.50f;
    private int level;
    public int LEVEL {
        get { return level; }
    }

    public bool AddingXP { get; private set; }
    public float InitialXP { get; private set; }
    public float TargetXP { get; private set; }

    private float currentXP;
    private int spinAmount;
    public Animator winAnimator, bonusAnimator;
    public Animator winPopUp;
    public TextMeshProUGUI winTypeText, winAmountText;

    // Resets variables, and pays the money. If it can't pay the money, it won't spin.
    public void Spin() {
        if(LevelUp) {
            LevelUp = false;
            levelUpAnim.SetTrigger("Close");
        }


        if(!doingBonusSpins) {
            if(currentData.coinAmount - currentData.currentBet < 0) return;

            currentData.coinAmount -= currentData.currentBet;
            SetCoinValue();
        } else {
            spinAmount -= 1;
            SetSpinsVal();
        }

        winLine.TurnOff();
        timeElapsed = timer = 0.0f;
        start.interactable = false;
        Won = false;
        ToggleRoulettes(true, true);
        fakeSpin.SetTrigger("Start");
        isRunning = true;
    }

    
    private void SetBetValue() {
        betValue.text = currentData.currentBet.ToString("f2");
    }

    public void SetCoinValue() {
        coinAmount.text = currentData.coinAmount.ToString("f2");
    }

    public void MaxBet() {
        currentData.currentBet = MAXBET;
        SetBetValue();
    }

    public void AddMoney() {
        currentData.currentBet += 0.5f;
        if(currentData.currentBet > MAXBET) currentData.currentBet = MAXBET;
        
        SetBetValue();
    }

    public bool SetXP() {
        InitialXP = currentXP;

        StartCoroutine(Experience());

        return (InitialXP + TargetXP) > (level * EXPPERLEVEL);
    }


    IEnumerator Experience() {

        xpAnim.SetTrigger("Shake");

        while(TargetXP > 0) {
            xpSlider.fillAmount = currentXP / (float)(level * EXPPERLEVEL);


            currentXP ++;
            TargetXP --;
            if(currentXP >= (level * EXPPERLEVEL)) {
                level++;
                currentXP = 0;
                levelUpAnim.SetTrigger("LevelUp");
                bonusGameBtn.SetActive(level % 2 == 0);

                levelupSpinInfo.text = "+2 BONUS SPINS";
                spinAmount += 2;
                expLevel.text = level.ToString();
            }
            

                // levelUpAnim.SetTrigger("LevelUp");
                // bonusGameBtn.SetActive(level % 2 == 0);


            // timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.15f);
        }

        xpAnim.SetTrigger("Shake");

        if(level % 2 != 0) {
            Invoke("Spin", 3.5f);
        }    
    }

    public Animator xpAnim;
    public int bonus_CurrentMultiplier;

    public void SetSpins(int spins) {
        selectedPlanets = new List<BonusPlanet>();
        bonusSpinBtn.interactable = false;
        spinAmount = spins;
        start.interactable = false;
        SetSpinsVal();
        xpAnim.SetTrigger("FadeIn");
        bonusAnimator.SetTrigger("Open");
        level = 1;
        currentXP = 0;
        expLevel.text = level.ToString();
        xpSlider.fillAmount = currentXP;

        bonus_CurrentMultiplier = 1;
        isRunning = doingBonusSpins = true;
        Won = false;
        ToggleRoulettes(true, true);
        fakeSpin.SetTrigger("Start");
    }

    private void SetSpinsVal() {
        bonusSpinValue.text = spinAmount.ToString();
    }

    public void RemoveMoney() {
        currentData.currentBet -= 0.5f;
        if(currentData.currentBet < MINBET) currentData.currentBet = MINBET;
        
        SetBetValue();
    }

    public float TargetCoins, InitialCoins; 
    
    public void ShowWin(List<RouletteWin> win) {
        if(win.Count > 2) {
            winTypeText.text = "MULTI WIN";
        } else {
            // winTypeText.text = win[0].wintype.ToString();
        }

        float amt = winAmount(win);

        InitialCoins = currentData.coinAmount;
        TargetCoins = InitialCoins + amt;
        AddingMoney = true;

        
        winAmountText.text = "+ " + amt.ToString() + " COINS";
        winPopUp.SetTrigger("Won");

        Invoke("Closewin", 1.5f);
    }

    private void Closewin() { winPopUp.SetTrigger("Close"); }

    private float winAmount(List<RouletteWin> win) {
        float retVal = 0.0f;

        foreach(RouletteWin rw in win) {
            retVal += IconMultipliers.summary.Find(n => n.checkSum == rw.checkSum).multiplier 
                    * rw.WinMulti
                    * currentData.currentBet;
        }

        return retVal;
    } 


    public void OpenShop() {
        shop.OpenShop(this);
    }

    public void OpenBonusSpins() {
        bonusSpins.Open(this);
    }


    public void ShowInfo() {
        canvasAnim.SetTrigger("Info");
    }

    public void CloseInfo() {
        canvasAnim.SetTrigger("Game");
    }

#endregion

#region Save File
    private void OnApplicationPause(bool pauseStatus) {
        loader.SaveFile(currentData);
    }
    
    private void OnApplicationQuit() {
        loader.SaveFile(currentData);
    }
#endregion

#region Win Class / Enums
    [System.Serializable]
    public class RouletteWin {
        public bool Won;
        public int checkSum;
        public int WinMulti;
        public int winId;
        public List<Coordinates> WinningCoords;
        // Index of Win must have it's own definition, so as to cover which row or column, or starting diagonal.


        public RouletteWin() {
            WinningCoords = new List<Coordinates>();
            Won = false;
            winId = WinMulti = checkSum = -1;
        }

        public RouletteWin(int winId, int checkSum, int multi, List<Coordinates> WinCoords) {
            WinningCoords = new List<Coordinates>();
            WinningCoords.AddRange(WinCoords);
            Won = true;
            WinMulti = multi;
            this.checkSum = checkSum;
            this.winId = winId;
        }
    }
    
    public enum WINTYPE { HORIZONTAL, VERTICAL, DIAGONAL, X, CROSS,  T, T_INVERTED, L, L_INVERTED, SQUARE, NULL }

    [System.Serializable]
    public class Coordinates {
        public int Row, Column;
        public int Value;
        public Coordinates(int x, int y) { Column = x; Row = y; }

        public Coordinates(int x, int y, int val) { Row = x; Column = y; Value = val; }
    }

    public sealed class TypeMultipliers {
        public int multiplier;
        public WINTYPE type;
        private TypeMultipliers(int mp, WINTYPE tp) { multiplier = mp; type = tp; }
        public static TypeMultipliers Hori = new TypeMultipliers(3, WINTYPE.HORIZONTAL);
        public static TypeMultipliers Vert = new TypeMultipliers(5, WINTYPE.VERTICAL);
        public static TypeMultipliers Diag = new TypeMultipliers(10, WINTYPE.DIAGONAL);
        public static TypeMultipliers X = new TypeMultipliers(15, WINTYPE.X);
        public static TypeMultipliers Cross = new TypeMultipliers(15, WINTYPE.CROSS);
        public static TypeMultipliers T = new TypeMultipliers(10, WINTYPE.T);
        public static TypeMultipliers T_Inv = new TypeMultipliers(10, WINTYPE.T_INVERTED);
        public static TypeMultipliers L = new TypeMultipliers(10, WINTYPE.L);
        public static TypeMultipliers L_Inv = new TypeMultipliers(10, WINTYPE.L_INVERTED);
        public static TypeMultipliers Square = new TypeMultipliers(25, WINTYPE.SQUARE);
        public static List<TypeMultipliers> summary = new List<TypeMultipliers>() { Hori, Vert, Diag, X, Cross, T, T_Inv, L, L_Inv, Square };
    }

    /*
        Satellite
        Red Planet
        Earth
        Green Alien
        Android
        Space Girl
        Alien Girl
        Captain
        WildCard
        BonusSpin
        Experience
*/

    public sealed class IconMultipliers {
        public int multiplier;
        public int checkSum;
        private IconMultipliers(int mp, int cSum) { multiplier = mp; checkSum = cSum; }
        public static IconMultipliers Sat = new IconMultipliers(2, 0);
        public static IconMultipliers RedPlanet = new IconMultipliers(3, 1);
        public static IconMultipliers Earth = new IconMultipliers(4, 2);
        public static IconMultipliers GrnAlien = new IconMultipliers(5, 3);
        public static IconMultipliers Android = new IconMultipliers(6, 4);
        public static IconMultipliers SpaceGirl = new IconMultipliers(8, 5);
        public static IconMultipliers GirlAlien = new IconMultipliers(10, 6);
        public static IconMultipliers Captain = new IconMultipliers(15, 7);
        public static IconMultipliers WildCard = new IconMultipliers(2, 8);
        public static IconMultipliers BonusSpin = new IconMultipliers(2, 9);
        public static IconMultipliers Exp = new IconMultipliers(2, 10);
        public static List<IconMultipliers> summary = new List<IconMultipliers>() { Sat, RedPlanet, Earth, GrnAlien, Android, SpaceGirl,
                                                                                    GirlAlien, Captain, WildCard, BonusSpin, Exp };
    }




#endregion
}
