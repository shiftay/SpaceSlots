using System.Collections;
using System.Collections.Generic;

public sealed class WinDefinitions {
    
    public List<RouletteManager.Coordinates> winningCoords; 
    public int id;
    public float WinMulti;

    private WinDefinitions(List<RouletteManager.Coordinates> coords, int identifier, float Multi) {
        winningCoords = new List<RouletteManager.Coordinates>(coords);
        id = identifier;
        WinMulti = Multi;
    }

    public static WinDefinitions LINE_1 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,1),
    }, 0, 5);

    public static WinDefinitions LINE_2 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,0),new RouletteManager.Coordinates(2,0),
        new RouletteManager.Coordinates(3,0), new RouletteManager.Coordinates(4,0),
    }, 1, 5);

    public static WinDefinitions LINE_3 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,2),
    }, 2, 5);

    public static WinDefinitions LINE_4 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,0),
    }, 3, 4);
    
    public static WinDefinitions LINE_5 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,0),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,2),
    }, 4, 4);
        
    public static WinDefinitions LINE_6 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,0),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,0), new RouletteManager.Coordinates(4,1),
    }, 5, 3);
        
    public static WinDefinitions LINE_7 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,1),
    }, 6, 3);
        
    public static WinDefinitions LINE_8 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,0),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,2),
    }, 7, 3);
        
    public static WinDefinitions LINE_9 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,0), new RouletteManager.Coordinates(4,0),
    }, 8, 3);
        
    public static WinDefinitions LINE_10 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,0), new RouletteManager.Coordinates(4,1),
    }, 9, 3);
        
    public static WinDefinitions LINE_11 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,0),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,1),
    }, 10, 2);
        
    public static WinDefinitions LINE_12 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,0),
    }, 11, 2);
        
    public static WinDefinitions LINE_13 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,1),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,2),
    }, 12, 2);
        
    public static WinDefinitions LINE_14 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,0),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,0),
    }, 13, 2);
        
    public static WinDefinitions LINE_15 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,2),
    }, 14, 2);
        
    public static WinDefinitions LINE_16 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,0),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,1),
    }, 15, 1);
        
    public static WinDefinitions LINE_17 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,1), new RouletteManager.Coordinates(1,1),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,1), new RouletteManager.Coordinates(4,1),
    }, 16, 1);
        
    public static WinDefinitions LINE_18 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,0),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,0), new RouletteManager.Coordinates(4,0),
    }, 17, 1);
        
    public static WinDefinitions LINE_19 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,2), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,0),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,2),
    }, 18, 1);
        
    public static WinDefinitions LINE_20 = new WinDefinitions(new List<RouletteManager.Coordinates>() {
        new RouletteManager.Coordinates(0,0), new RouletteManager.Coordinates(1,2),new RouletteManager.Coordinates(2,2),
        new RouletteManager.Coordinates(3,2), new RouletteManager.Coordinates(4,0),
    }, 19, 1);
        
    

    public static List<WinDefinitions> ReturnDefs() {
        return new List<WinDefinitions> {  LINE_1, LINE_2, LINE_3, LINE_4, LINE_5,
                                                                LINE_6, LINE_7, LINE_8, LINE_9, LINE_10,
                                                                LINE_11, LINE_12, LINE_13, LINE_14, LINE_15,
                                                                LINE_16, LINE_17, LINE_18, LINE_19, LINE_20
                                                             };
    }



}
