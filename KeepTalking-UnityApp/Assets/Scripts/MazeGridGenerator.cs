using UnityEngine;

public class MazeGridGenerator : MonoBehaviour
{
    public Transform screen;       
    public GameObject cellPrefab;    
    public Transform cellsRoot;        // Empty GameObject under the maze to hold the cells

    public MazeModule modulePrefab;  
    public MazePlayer playerPrefab;    // White dot prefab
    public GameObject endDotPrefab;    // Red dot for ending position

    public GameObject mazeTypePrefab;  

    public GameObject buttonUp;
    public GameObject buttonDown;
    public GameObject buttonLeft;
    public GameObject buttonRight;
    public GameObject moduleLEDPrefab;


    public int gridSize = 4;
    public GameObject[,] cells;     

    private float fullCellSize;

    void Start()
    {
        cells = new GameObject[gridSize, gridSize];
        GenerateGrid();
        SpawnModuleWithPlayer();
    }

    void GenerateGrid()
    {
        float screenHeight = screen.localScale.y;
        float screenWidth  = screen.localScale.z;

        fullCellSize = screenHeight / gridSize;
        float padding = 0.95f;
        float cellSize = fullCellSize * padding;
        float surfaceOffset = 0.7f;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject cell = Instantiate(cellPrefab, cellsRoot);
                cell.transform.localScale = new Vector3(0.04f, cellSize, cellSize);

                float y = (screenHeight / 2f) - (row * fullCellSize) - (fullCellSize / 2f);
                float z = (-screenWidth / 2f) + (col * fullCellSize) + (fullCellSize / 2f);

                cell.transform.localPosition = new Vector3(surfaceOffset, y, z);
                cells[row, col] = cell;
            }
        }

        if (endDotPrefab != null)
        {
            Vector2Int endPos = new Vector2Int(gridSize - 1, gridSize - 1);
            GameObject endDot = Instantiate(endDotPrefab, cellsRoot);
            Vector3 endPosWorld = cells[endPos.x, endPos.y].transform.localPosition;
            endDot.transform.localPosition = new Vector3(endPosWorld.x + 0.05f, endPosWorld.y, endPosWorld.z);
        }
    }

    void SpawnModuleWithPlayer()
    {
        // Instantiate the maze module prefab
        if (modulePrefab == null)
        {
            Debug.Log("moduleprefab is null");
        }
        Vector2Int[] possiblePositions = new Vector2Int[]
        {
            new Vector2Int(2, 3),
            new Vector2Int(3, 2),
            new Vector2Int(3, 1)
        };

        Vector2Int randomPos = possiblePositions[Random.Range(0, possiblePositions.Length)];

        if (mazeTypePrefab != null)
        {
            GameObject mazeObj = Instantiate(mazeTypePrefab, cellsRoot); 
            Vector3 worldPos = cells[randomPos.x, randomPos.y].transform.localPosition;
            mazeObj.transform.localPosition = new Vector3(
                worldPos.x + 0.05f,
                worldPos.y,
                worldPos.z
            );
        }
        MazeModule module = Instantiate(modulePrefab, transform);
        module.InitializeGrid(cells, gridSize, fullCellSize, randomPos);

        if (playerPrefab == null)
        {
            Debug.LogError("MazeGridGenerator: playerPrefab is null!");
            return;
        }

       
        MazePlayer playerObj = Instantiate(playerPrefab, module.transform);
        playerObj.SetParentModule(module);
        
        Vector2Int startPos = new Vector2Int(0, 0);
        playerObj.Initialize(module, startPos);
        playerObj.transform.SetParent(cellsRoot);
        Vector3 startPosWorld = cells[startPos.x, startPos.y].transform.localPosition;
        playerObj.transform.localPosition = new Vector3(
            startPosWorld.x + 0.06f,
            startPosWorld.y,
            startPosWorld.z
        );

       
        module.SetPlayer(playerObj);

        if (moduleLEDPrefab != null)
        {
            GameObject ledObj = Instantiate(moduleLEDPrefab, module.transform);
            module.LEDObject = ledObj; 
            ledObj.transform.localPosition = new Vector3(0.2f, -1.7f, -0.25f);
            ledObj.transform.localRotation = Quaternion.identity;
        }

         

        AssignButton(buttonUp, module, playerObj, Vector2Int.up);
        AssignButton(buttonDown, module, playerObj, Vector2Int.down);
        AssignButton(buttonLeft, module, playerObj, Vector2Int.right);
        AssignButton(buttonRight, module, playerObj, Vector2Int.left);
        
    }

    void AssignButton(GameObject buttonObj, MazeModule module, MazePlayer player, Vector2Int dir)
    {
        if (buttonObj == null) return;

        MazeButton btn = buttonObj.GetComponent<MazeButton>();
        if (btn == null)
        {
            Debug.LogWarning("MazeButton component missing on " + buttonObj.name);
            return;
        }

        btn.targetModule = module;
        btn.targetPlayer = player;
        btn.direction = dir;
    }

}
