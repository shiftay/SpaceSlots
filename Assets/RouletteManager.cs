using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RouletteManager : MonoBehaviour
{
    private const int WILDCARD = 8;
    public List<Roulette> roulettes;
    public Loader loader;
    public GameData currentData;
    public Shop shop;
    public BonusSpins bonusSpins;

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
    public float maxTIME;
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

    public void SetAddingMoney() {
        for(int i = 0; i < STEPS.Count; i++) {
            if(additionalcoins / STEPS[i] > 5) {
                removalIndex = i;
                break;
            }
        }
        
        AddingMoney = true;
    }

    public bool doingBonusSpins;
    public float additionalcoins;
    public float coinTimer;
    public Animator coin;
    public Animator fakeSpin;
    private float removal = 10;
    private int removalIndex = 0;

    private List<int> STEPS = new List<int>() { 50, 25, 10, 5, 1 };
    private List<int> DIVISOR = new List<int>() { 100, 100, 100, 10, 0 };

    private void FixedUpdate() {
        if(isRunning) {
            timer += Time.fixedDeltaTime;
            if(timer > 1f && !fakeSpin.GetCurrentAnimatorStateInfo(0).IsName("RouletteComplete")) {
                fakeSpin.SetTrigger("Stop");
            } 
            if(timer > 2f && !slowDown) {
                slowDown = true;
                ToggleRoulettes(true, false, 0.25f);
            }

            if(timer > maxTIME) {
                ToggleRoulettes(false);
                isRunning = slowDown = false;

                // Check For Win..
                Results = new List<List<int>>();
                roulettes.ForEach(n => Results.Add(n.Results()));

                // TODO Check for Experience, Bonus Spins

                if(printValues) {
                    string temp = "";
                    for(int i = 0; i < Results.Count; i++) {
                        
                        Results[i].ForEach(n => temp += " " + n + " ");
                        temp += "\n";
                    }
                    Debug.Log(temp);
                }

                rWin = new List<RouletteWin>();

                for(int i = 0; i < Results.Count; i++) {
                    rWin.Add(VerticalWin(Results, i));
                }

                for(int i = 0; i < Results.Count; i++) {
                    rWin.Add(HorizontalWin(Results, i));
                }

                rWin.RemoveAll(n => n.wintype == WINTYPE.NULL);

                if(rWin.Count > 2) {
                    rWin = CheckForSquare(rWin);
                    rWin = CheckForCross(rWin);
                    rWin = CheckForT(rWin);
                }
        
                rWin.AddRange(DiagonalWin(Results));

                rWin.ForEach(n => {
                    if(n.Won) {
                        Debug.LogError("Winner @ " + n.indexOfWin + " " + n.wintype);
                        Won = true;
                    } 
                });

                
        
                isRunning = false;

                if(Won) {
                    foreach(RouletteWin win in rWin) {
                        switch(win.wintype) {
                            case WINTYPE.VERTICAL:
                                roulettes[win.indexOfWin].VerticalWin();
                                break;
                            case WINTYPE.HORIZONTAL:
                                foreach(Roulette r in roulettes) {
                                    r.RunAnimation(win.indexOfWin);
                                }
                                break;
                            case WINTYPE.DIAGONAL:
                            case WINTYPE.CROSS:
                            case WINTYPE.X:
                            case WINTYPE.L:
                            case WINTYPE.L_INVERTED:
                            case WINTYPE.SQUARE:
                            case WINTYPE.T:
                            case WINTYPE.T_INVERTED:
                                break;
                        }
                    }

                    ShowWin(rWin);
                    start.interactable = true;
                } else {
                    if(continuous) {
                        Spin();
                        timer = 0f;         
                        
                    } else {
                        start.interactable = true;
                    }
                }


            }
        }
   
        if(AddingMoney) {
            if(additionalcoins == 0) {
                coin.SetTrigger("Complete");
                
                AddingMoney = false;
                additionalcoins = 0;
            } else {
                // Find first Step Index;
                if(additionalcoins / STEPS[removalIndex] < DIVISOR[removalIndex] && removalIndex < STEPS.Count-1) {
                    removalIndex++;
                } 


                additionalcoins -= STEPS[removalIndex];
                currentData.coinAmount += STEPS[removalIndex];
                SetCoinValue();
            }
        }
    }

#region Look for Wins
    private List<RouletteWin> CheckForCross(List<RouletteWin> results)
    {
        // 2 2
        List<RouletteWin> checkVals = new List<RouletteWin>();

        for(int i = 0; i < results.Count; i++) {
            if(results[i].wintype == WINTYPE.HORIZONTAL && results[i].indexOfWin == 2) {
                checkVals.Add(results[i]);
            } else if(results[i].wintype == WINTYPE.VERTICAL && results[i].indexOfWin == 2) {
                checkVals.Add(results[i]);
            }
        }

        if(checkVals.Count == 2) {
            int checkSum = checkVals[0].checkSum;
            checkVals.ForEach(n => results.Remove(n));
            results.Add(new RouletteWin(WINTYPE.CROSS, 2, checkSum));
        }

        return results;


    }

    private List<RouletteWin> CheckForSquare(List<RouletteWin> results)
    {
        List<RouletteWin> checkVals = new List<RouletteWin>();
        for(int i = 0; i < results.Count; i++) {
            if(results[i].wintype == WINTYPE.HORIZONTAL && results[i].indexOfWin == 0) {
                checkVals.Add(results[i]);
            } else if(results[i].wintype == WINTYPE.VERTICAL && results[i].indexOfWin == 0) {
                checkVals.Add(results[i]);
            } else if(results[i].wintype == WINTYPE.HORIZONTAL && results[i].indexOfWin == 4) {
                checkVals.Add(results[i]);
            } else if(results[i].wintype == WINTYPE.VERTICAL && results[i].indexOfWin == 4) {
                checkVals.Add(results[i]);
            }
        }

        if(checkVals.Count == 4) {
            int checkSum = checkVals[0].checkSum;
            checkVals.ForEach(n => results.Remove(n));
            results.Add(new RouletteWin(WINTYPE.SQUARE, 0, checkSum));
        }

        return results;
    }

    private List<RouletteWin> CheckForT(List<RouletteWin> results)
    {
        List<RouletteWin> checkVals = new List<RouletteWin>();

        checkVals = T_Indices(results, 0, 2);

        if(checkVals.Count == 2) {
            int checkSum = checkVals[0].checkSum;
            checkVals.ForEach(n => results.Remove(n));
            results.Add(new RouletteWin(WINTYPE.T, 0, checkSum));
        }
        checkVals.Clear();

        checkVals = T_Indices(results, 4, 2);
        if(checkVals.Count == 2) {
            int checkSum = checkVals[0].checkSum;
            checkVals.ForEach(n => results.Remove(n));
            results.Add(new RouletteWin(WINTYPE.T_INVERTED, 4, checkSum));
        }

        return results;
    }


    private List<RouletteWin> T_Indices(List<RouletteWin> results, int hori, int vert) {
        List<RouletteWin> retVal = new List<RouletteWin>();
        for(int i = 0; i < results.Count; i++) {
            if(results[i].wintype == WINTYPE.HORIZONTAL && results[i].indexOfWin == 0) {
                retVal.Add(results[i]);
            } else if(results[i].wintype == WINTYPE.VERTICAL && results[i].indexOfWin == 2) {
                retVal.Add(results[i]);
            }
        }


        return retVal;
    }

    private RouletteWin HorizontalWin(List<List<int>> Results, int index) {
        RouletteWin retVal = new RouletteWin();

        bool horizontalWin = true;
        int checkSum = Results[0][index];

        if(checkSum == WILDCARD) {
            for(int i = 0; i < Results.Count; i++) {
                if(Results[i][index] != WILDCARD) {
                    checkSum = Results[i][index];
                    break;
                }
            }
            // Full Wildcard Win
            if(checkSum == WILDCARD) {
                retVal.Won = true;
                retVal.wintype = WINTYPE.HORIZONTAL;
                retVal.indexOfWin = index;
                
                retVal.checkSum = checkSum;
                return retVal;
            }
        }


        for(int i = 0; i < Results.Count; i++) {
            
            if(Results[i][index] != checkSum && Results[i][index] != WILDCARD) {
                horizontalWin = false;
            }
        }

        if(horizontalWin) {
            retVal.Won = true;
            retVal.wintype = WINTYPE.HORIZONTAL;
            retVal.indexOfWin = index;
            
            retVal.checkSum = checkSum;
            return retVal;
        }

        return retVal;
    }

    private RouletteWin VerticalWin(List<List<int>> Results, int index) {
        RouletteWin retVal = new RouletteWin();


        bool verticalWin = true;
        int checksum = Results[index][0];
        // WildCard look for first non wildcard in the row.
        if(Results[index][0] == WILDCARD) {

            for(int i = 0; i < Results.Count; i++) {
                if(Results[index][i] != WILDCARD)  {
                    checksum = Results[index][i];
                    break;
                }
            }
        }

        for(int i = 0; i < Results.Count; i++) {
            if(Results[index][i] != checksum && Results[index][i] != WILDCARD) {
                verticalWin = false;
            }
        }

        if(verticalWin) {
            retVal.Won = true;
            retVal.wintype = WINTYPE.VERTICAL;
            retVal.indexOfWin = index;
            retVal.checkSum = checksum;
            return retVal;
        }

        return retVal;
    }

    private List<RouletteWin> DiagonalWin(List<List<int>> Results) {
        List<RouletteWin> retVal = new List<RouletteWin>();


        int checkSum = Results[0][0];

        bool diagonalWin = true;
        if(checkSum == WILDCARD) {
            for(int i = 0; i < Results.Count; i++) {
                for(int j = 0; j < Results.Count; j++) {
                    if(j == i) {
                        if(Results[i][j] != WILDCARD) {
                            checkSum = Results[i][j];
                            break;
                        }
                    }
                }
            }
        }

        if(checkSum == WILDCARD) {
            retVal.Add(new RouletteWin(WINTYPE.DIAGONAL, 0, checkSum));
        } else {
            for(int i = 0; i < Results.Count; i++) {
                for(int j = 0; j < Results.Count; j++) {
                    if(j == i) {
                        if(Results[i][j] != WILDCARD && Results[i][j] != checkSum)
                            diagonalWin = false;
                    } else
                        continue;
                }
            }

            if(diagonalWin) {
                retVal.Add(new RouletteWin(WINTYPE.DIAGONAL, 0, checkSum));
            }
        }



        checkSum = Results[0][Results.Count-1];
        diagonalWin = true;
        if(checkSum == WILDCARD) {
            for(int i = 0; i < Results.Count; i++) {
                for(int j = 0; j < Results.Count; j++) {
                    if(j == (Results.Count-1) - i) {
                        if(Results[i][j] != WILDCARD) {
                            checkSum = Results[i][j];
                            break;
                        }
                    }
                }
            }
        }

        if(checkSum == WILDCARD) {
            retVal.Add(new RouletteWin(WINTYPE.DIAGONAL, 5, checkSum));
        } else {
            for(int i = 0; i < Results.Count; i++) {
                for(int j = 0; j < Results.Count; j++) {
                    if(j == (Results.Count-1) - i) {
                        if(Results[i][j] != WILDCARD && Results[i][j] != checkSum)
                            diagonalWin = false;
                    } else
                        continue;
                }
            }

            if(diagonalWin) {
                retVal.Add(new RouletteWin(WINTYPE.DIAGONAL, 5, checkSum));
            }
        }

        if(retVal.Count == 2) {
            // X WON
            retVal.Clear();
            retVal.Add(new RouletteWin(WINTYPE.X, 0, checkSum));
        }


        return retVal;
    }

    // Define
    // [x] Horizontal Win 
    // [x] Verical Win
    // [x] Diagonal Win
    // [x] X Win
    // [x] T Win
    // [x] Inverted T Win
    // [] L Win
    // [x] Square Win
#endregion

    // Define 
    // [x] WildCards
    // [] Multiplier Gain + Use
    // [] Bonus Spins (Auto Spin with Cumulative win and a chance to gain more spins.)

    // Regular Roulette, will not feature bonus spins

    // Change the chance of specific

#region UI

    public Button start, bonusSpinBtn;
    public TextMeshProUGUI betValue, winValue, coinAmount, expLevel, bonusSpinValue;
    public Image xpSlider;

    private const float MAXBET = 10.00f;
    private const float MINBET = 2.50f;
    private int level;
    private int spinAmount;
    public Animator winAnimator, bonusAnimator;
    public Animator winPopUp;
    public TextMeshProUGUI winTypeText, winAmountText;


    public void Spin() {
        if(currentData.coinAmount - currentData.currentBet < 0) return;

        if(!doingBonusSpins) {
            currentData.coinAmount -= currentData.currentBet;
            SetCoinValue();
        }

        start.interactable = false;
        Won = false;
        ToggleRoulettes(true, true);
        isRunning = true;
        fakeSpin.SetTrigger("Start");
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

    public Animator xpAnim;

    public void SetSpins(int spins) {
        bonusSpinBtn.interactable = false;
        spinAmount = spins;
        start.interactable = false;
        SetSpinsVal();
        xpAnim.SetTrigger("FadeIn");
        bonusAnimator.SetTrigger("Open");
        level = 1;
        expLevel.text = level.ToString();
        doingBonusSpins = true;
        
        // Initiate Bonus Spins
        // Fade in Win counter on win
    }

    private void SetSpinsVal() {
        bonusSpinValue.text = spinAmount.ToString();
    }


    public void RemoveMoney() {
        currentData.currentBet -= 0.5f;
        if(currentData.currentBet < MINBET) currentData.currentBet = MINBET;
        
        SetBetValue();
    }

    public void UpdateWin(string winAmount) {
        winAnimator.SetTrigger("FadeIn");

        winValue.text = winAmount;
    }

    public void ShowWin(List<RouletteWin> win) {
        if(win.Count > 2) {
            winTypeText.text = "MULTI WIN";
        } else {
            winTypeText.text = win[0].wintype.ToString();
        }

        float amt = winAmount(win);

        additionalcoins = amt;
        SetAddingMoney();
        
        winAmountText.text = "+ " + amt.ToString() + " COINS";
        winPopUp.SetTrigger("Won");
    }

    private float winAmount(List<RouletteWin> win) {
        float retVal = 0.0f;

        foreach(RouletteWin rw in win) {
            Debug.Log(rw.checkSum + " " + rw.wintype.ToString());
            retVal += IconMultipliers.summary.Find(n => n.checkSum == rw.checkSum).multiplier 
                    * TypeMultipliers.summary.Find(n => n.type == rw.wintype).multiplier 
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
        public WINTYPE wintype;
        public int indexOfWin;
        // Index of Win must have it's own definition, so as to cover which row or column, or starting diagonal.


        public RouletteWin() {
            Won = false;
            wintype = WINTYPE.NULL;
            indexOfWin = -1;
            checkSum = -1;
        }

        public RouletteWin(WINTYPE type, int indexOfWin, int checkSum) {
            Won = true;
            wintype = type;
            this.checkSum = checkSum;
            this.indexOfWin = indexOfWin;
        }
    }
    
    public enum WINTYPE { HORIZONTAL, VERTICAL, DIAGONAL, X, CROSS,  T, T_INVERTED, L, L_INVERTED, SQUARE, NULL }


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
