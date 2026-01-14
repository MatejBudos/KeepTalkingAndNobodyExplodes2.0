// MazeButton.cs
using UnityEngine;

public class MazeButton : MonoBehaviour
{
    public MazeModule targetModule;

    public Vector2Int direction;
    public MazePlayer targetPlayer;


    public void OnMouseDown()
    {
        Debug.Log($"Button clicked. targetModule={targetModule}, targetModule.player={targetModule?.player}");
        targetModule?.MovePlayer(direction);
    }
}
