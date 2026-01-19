using System;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static int strikes;

    public Renderer strike1a;
    public Renderer strike1b;
    public Renderer strike2a;
    public Renderer strike2b;
    public Renderer strike3a;
    public Renderer strike3b;
    public List<GameObject> modulePrefabs;
    public List<Transform> moduleContainers;
    public static Action OnWrongClick;
    public GameObject emptyModulePrefab;
    public static Action SolvedModule;
    public int numOfModules;
    public int solvedModules;
    public event Action<string> evnt;

    public static BombManager Instance;
 

    public void Update(){}

    public void Start()
    {
        strikes = 0;

    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void OnEnable()
    {
        OnWrongClick += HandleWrongClick;
        SolvedModule += HandleSolvedModule;
    }

    public void OnDisable()
    {
        OnWrongClick -= HandleWrongClick;
        SolvedModule -= HandleSolvedModule;
    }

    public void HandleSolvedModule()
    {
        solvedModules++;
        if (solvedModules == numOfModules)
        {
            evnt.Invoke("Win");
        }
    }
    public void HandleWrongClick()
    {
        strikes++;

        if (strikes == 1)
        {
            strike1a.material.color = Color.red;
            strike1b.material.color = Color.red;
            evnt.Invoke("Mistake");
        }
        else if (strikes == 2)
        {
            strike2a.material.color = Color.red;
            strike2b.material.color = Color.red;
            evnt.Invoke("Mistake");
        }
        else if (strikes >= 3)
        {
            strike3a.material.color = Color.red;
            strike3b.material.color = Color.red;
            evnt.Invoke("Loose");
        }
    }

    public void GenerateModules(int moduleCount)
    {
        strikes = 0;
        this.solvedModules = 0;
        ClearModules();
        this.numOfModules = moduleCount;
        moduleCount = Mathf.Min(moduleCount, moduleContainers.Count);


        List<Transform> shuffledContainers = new List<Transform>(moduleContainers);
        ShuffleContainers(shuffledContainers);

        // Place modules
        for (int i = 0; i < moduleCount; i++)
        {
            Transform container = shuffledContainers[i];
            int moduleIndex = UnityEngine.Random.Range(0, modulePrefabs.Count);

            Instantiate(
                modulePrefabs[moduleIndex],
                container.position,
                container.rotation,
                container
            );
        }

        // Fill remaining slots with empty modules
        for (int i = moduleCount; i < shuffledContainers.Count; i++)
        {
            Transform container = shuffledContainers[i];

            Instantiate(
                emptyModulePrefab,
                container.position,
                container.rotation,
                container
            );
        }
    }



    void ClearModules()
    {
        foreach (Transform container in moduleContainers)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    void ShuffleContainers(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }


}
