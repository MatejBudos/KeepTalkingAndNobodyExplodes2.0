using UnityEngine;
using System.Collections.Generic;

public class MazeModule : MonoBehaviour
{
    public MazePlayer player;
    public GameObject[,] gridCells;

    public Vector2Int mazeTypePosition;
    public GameObject mazeTypeObject;

    public GameObject LEDObject;      
    public Vector2Int LEDPosition;
    public int gridSize;

    public float cellHeight;
    public float cellWidth;

    public float cellStep; 
    bool solved = false;

    string[] map0 = new string[] // (2,3)
    {
        "p|0 0|0",
        "0|0|0|0",
        "0|0|0|0",
        "0 0|0 g"
    };

    static string[] map1 = new string[] // (3,2)
    {
        "0|0 0|0", 
        "0 0|0 0",  
        "  _ _ _",  
        "0|0|0 0",  
        "0 0 0|0" 
    };

    string[] map2 = new string[] // (3,1)
    {
        "0 0 0 0",    
        "_ _ _  ",   
        "0 0 0 0",   
        "  _ _ _",   
        "0 0 0 0",   
        "_ _   _",  
        "0 0 0 g"    
    };

    public Dictionary<Vector2Int, List<Vector2Int>> walls;

    public void InitializeGrid(GameObject[,] cells, int gridSize, float cellStep, Vector2Int mazeType)
    {
        this.gridCells = cells;
        this.gridSize = gridSize;
        this.cellStep = cellStep;
        this.mazeTypePosition = mazeType;
        string[] selectedMap = map0;
        
        if (mazeTypePosition == new Vector2Int(2, 3))
        {
            selectedMap = map0;
        }
            
        else if (mazeTypePosition == new Vector2Int(3, 2))
        {
            selectedMap = map1;
        }
        else if (mazeTypePosition == new Vector2Int(3, 1))
        {
            selectedMap = map2;
        }
        walls = MazeWallParser.ParseWalls(selectedMap);
    }

    void Update()
    {
        if (solved)
        {
            if (LEDObject != null)
                LEDObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
    
   public void MovePlayer(Vector2Int direction)
    {
        if (solved) return;
        if (player == null)
        {
            Debug.LogWarning("MazeModule: No MazePlayer assigned!");
            return;
        }
        player.Move(direction);
    }


    public void SetPlayer(MazePlayer newPlayer)
    {
        if (newPlayer == null)
        {
            Debug.Log("PLayer is NULL (module)");
        }
        player = newPlayer;
        player.SetParentModule(this); 
    }
    public void setSolved()
    {
        solved = true;
    }
}
