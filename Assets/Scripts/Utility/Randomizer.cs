using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Randomizer 
{
    private int weight, index;

    private Randomizer(int w, int i) { weight = w; index = i; }

/*
        Satellite
        Red Planet
        Earth
        Green Alien
        Android
        Space Girl
        Captain
        WildCard
        BonusSpin
        Experience
*/


    public static Randomizer SATELLITE = new Randomizer(75, 0);
    public static Randomizer RED_PLANET = new Randomizer(50, 1);
    public static Randomizer EARTH = new Randomizer(35, 2);
    public static Randomizer GREEN_ALIEN = new Randomizer(20, 3);
    public static Randomizer ANDROID = new Randomizer(15, 4);
    public static Randomizer SPACE_GIRL = new Randomizer(10, 5);
    public static Randomizer GIRL_ALIEN = new Randomizer(7,6);
    public static Randomizer CAPTAIN = new Randomizer(5, 7);
    public static Randomizer WILDCARD = new Randomizer(12, 8);
    public static Randomizer BONUS_SPIN = new Randomizer(4, 9);
    public static Randomizer EXPERIENCE = new Randomizer(16, 10);

    // Randomizer.SATELLITE
    // Randomizer.RED_PLANET
    // Randomizer.EARTH
    // Randomizer.GREEN_ALIEN
    // Randomizer.ANDROID
    // Randomizer.SPACE_GIRL
    // Randomizer.CAPTAIN
    // Randomizer.WILDCARD
    // Randomizer.BONUS_SPIN
    // Randomizer.EXPERIENCE
    // Randomizer.GIRL_ALIEN

    static public int BonusRoll() {
        List<Randomizer> test = new List<Randomizer>() {    Randomizer.BONUS_SPIN, Randomizer.CAPTAIN, Randomizer.GIRL_ALIEN, Randomizer.SPACE_GIRL, Randomizer.ANDROID, Randomizer.WILDCARD, 
                                                            Randomizer.GREEN_ALIEN, Randomizer.EXPERIENCE, Randomizer.EARTH, Randomizer.RED_PLANET, Randomizer.SATELLITE
                                                        };


        int totalWeight = 0;
        test.ForEach(n => totalWeight += n.weight);

        int result = 0, total = 0;
        int randVal = Random.Range( 0, totalWeight + 1 );

        for ( result = 0; result < test.Count; result++ ) {
            total += test[result].weight;
            if ( total >= randVal ) break;
        }


        return test[result].index;
    }


    public static int BasicRoll() {
        List<Randomizer> test = new List<Randomizer>() {    Randomizer.CAPTAIN, Randomizer.GIRL_ALIEN, Randomizer.SPACE_GIRL, Randomizer.ANDROID, 
                                                            Randomizer.GREEN_ALIEN, Randomizer.WILDCARD, Randomizer.EARTH, Randomizer.RED_PLANET, Randomizer.SATELLITE
                                                        };

        int totalWeight = 0;
        test.ForEach(n => totalWeight += n.weight);

        int result = 0, total = 0;
        int randVal = Random.Range( 0, totalWeight + 1 );


        for ( result = 0; result < test.Count; result++ ) {
            total += test[result].weight;
            if ( total >= randVal ) 
            {
                break;
            }
        }


        return test[result].index;
    }


}
