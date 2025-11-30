using System.Collections.Generic;
using UnityEngine;

public class MazeWallParser : MonoBehaviour
{
    public static Dictionary<Vector2Int, List<Vector2Int>> ParseWalls(string[] map)
    {
        Dictionary<Vector2Int, List<Vector2Int>> blocked = new Dictionary<Vector2Int, List<Vector2Int>>();
        int totalRows = map.Length;

        int gridRowCount = 0; // only incremented for actual cell rows

        for (int row = 0; row < totalRows; row++)
        {
            string line = map[row];
            // Skip horizontal walls
            if (!line.Contains("0")) continue;

            int gridRow = -gridRowCount; // top-left = (0,0)
            gridRowCount++;

            int size = 4; // always 4x4
            for (int col = 0; col < size; col++)
            {
                int gridCol = -col;
                Vector2Int pos = new Vector2Int(gridRow, gridCol);

                if (!blocked.ContainsKey(pos))
                    blocked[pos] = new List<Vector2Int>();

                int charIndex = col * 2;

                // CHECK RIGHT
                if (col + 1 < size)
                {
                    if (charIndex + 1 >= line.Length || line[charIndex + 1] == '|')
                    {
                        Vector2Int right = new Vector2Int(gridRow, -(col + 1));
                        blocked[pos].Add(right);
                    }
                }

                // CHECK LEFT
                if (col - 1 >= 0)
                {
                    if (charIndex - 1 < 0 || line[charIndex - 1] == '|')
                    {
                        Vector2Int left = new Vector2Int(gridRow, -(col - 1));
                        blocked[pos].Add(left);
                    }
                }

                // CHECK ABOVE
                int hRowAbove = row - 1;
                if (hRowAbove >= 0)
                {
                    string aboveLine = map[hRowAbove];
                    if (aboveLine[charIndex] == '_')
                    {
                        Vector2Int up = new Vector2Int(gridRow + 1, gridCol);
                        blocked[pos].Add(up);
                    }
                }

                // CHECK BELOW
                int hRowBelow = row + 1;
                if (hRowBelow < totalRows)
                {
                    string belowLine = map[hRowBelow];
                    if (belowLine[charIndex] == '_')
                    {
                        Vector2Int down = new Vector2Int(gridRow - 1, gridCol);
                        blocked[pos].Add(down);
                    }
                }
            }
        }

        return blocked;
    }

    private void PrintWalls(Dictionary<Vector2Int, List<Vector2Int>> blocked)
    {
        foreach (var kv in blocked)
        {
            Vector2Int cell = kv.Key;
            var b = kv.Value;

            string s = "";
            foreach (var p in b)
                s += $"({p.x},{p.y}) ";

            Debug.Log($"Cell ({cell.x},{cell.y}) blocks -> {s}");
        }
    }
}
