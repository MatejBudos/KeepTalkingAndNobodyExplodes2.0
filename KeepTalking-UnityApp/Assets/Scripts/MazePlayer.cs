using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    private MazeModule parentModule;

    private Vector2Int gridPos;            
    private int gridSize;                  

    private float cellStep;   

    public void Initialize(MazeModule module, Vector2Int startPos)
    {
        parentModule = module;
        gridSize = module.gridSize;
        cellStep = module.cellStep;
        gridPos = startPos;
    }

    public void SetParentModule(MazeModule module)
    {
        this.parentModule = module;
    }

    public void Move(Vector2Int direction)
    {
        if (parentModule == null) return;
        
        if (!CanStep(direction)) return;

    
        Vector2Int newPos = gridPos + direction;

        gridPos = newPos;

        // pohyb iba po y/z
        Vector3 move = new Vector3(
            0f,                             // X zostava rovnake 
            direction.y * parentModule.cellStep,  
            -direction.x * parentModule.cellStep 
        );

        transform.localPosition += move;
        if (IsAtGoal())
        {
            Debug.Log("you reached the goal");
            parentModule.setdefused();
        }
    }


    public bool IsAtGoal()
    {
        Debug.Log(gridPos + " " + -(gridSize-1));
        return gridPos.x == -(gridSize - 1) && gridPos.y == -(gridSize - 1);
    }

     private bool CanStep(Vector2Int direction)
    {
        Vector2Int newPos = gridPos + direction;

        // OUT OF BOUNDS CHECK
        if (newPos.x > 0 || newPos.x < -3 || newPos.y > 0 || newPos.y < -3)
        {
            Debug.Log("out of bounds");
            return false;
        }

        // map and player (x,y) inverted
        Vector2Int mapPos     = new Vector2Int(gridPos.y, gridPos.x);
        Vector2Int mapNextPos = new Vector2Int(newPos.y, newPos.x);

        // WALL CHECK
        if (parentModule.walls.ContainsKey(mapPos) &&
            parentModule.walls[mapPos].Contains(mapNextPos))
        {
            BombManager.OnWrongClick?.Invoke();
            Debug.Log("you have hit a wall");
            return false;
        }

        return true;
    }
}
