// MazeButton.cs
using UnityEngine;

public class MazeButton : MonoBehaviour
{
    public MazeModule targetModule;

    public Vector2Int direction; // direction this button moves the player
    public MazePlayer targetPlayer;


    void OnMouseDown()
    {
        Debug.Log($"Button clicked. targetModule={targetModule}, targetModule.player={targetModule?.player}");
        targetModule?.MovePlayer(direction);
    }
}
