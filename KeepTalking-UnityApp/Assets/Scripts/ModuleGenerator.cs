using UnityEngine;

public class ModuleGenerator : MonoBehaviour
{
    public GameObject bomb;
    public GameObject[] modulePrefabs;

    public float xOffset = 0.5f;

    public float zOffset = 0.75f;

    void Start()
    {
        GenerateModules();
    }

    void GenerateModules()
    {
        if (bomb == null || modulePrefabs.Length == 0)
            return;

        // The bomb center
        Vector3 center = bomb.transform.position;

        Vector3[] worldPositions =
        {
            new Vector3(center.x + xOffset, center.y, center.z + zOffset), // front 1
            new Vector3(center.x + xOffset, center.y, center.z - zOffset), // front 2

            new Vector3(center.x + xOffset - 1, center.y, center.z + zOffset),  // back 1
            new Vector3(center.x + xOffset - 1, center.y, center.z - zOffset)   // back 2
        };

        Quaternion[] rotations =
        {
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, -180, 0),
            Quaternion.Euler(0, -180, 0)
        };

        for (int i = 0; i < 4; i++)
        {
            int randomInt = Random.Range(0, 2);
            GameObject prefab = modulePrefabs[randomInt];
            GameObject module = Instantiate(prefab);

            module.transform.position = worldPositions[i];
            module.transform.rotation = rotations[i];

            module.transform.SetParent(bomb.transform, true);
        }
    }
}
