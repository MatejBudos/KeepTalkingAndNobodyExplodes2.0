using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireModule : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<WireNodePair> wires;
    int wireCount; // 3–6
    public List<Color> potentialColors; 
    public List<WireNodePair> ActiveWires;
    public int correctIndex;
    public Renderer LED;
    public bool detonated = false;
    void Start()
    {
        potentialColors = new List<Color>{Color.red, 
                                        Color.blue, 
                                        Color.yellow, 
                                        Color.white, 
                                        Color.green};
        ActiveWires = new List<WireNodePair>();
        wireCount = Random.Range(3, 7);
        Debug.Log("Wire Count" + wireCount);
        for ( int i = 0; i < wires.Count; i++)
        {
            if( i < wireCount)
            {
                WireNodePair wire = wires[i];
                Color col = potentialColors[Random.Range(0,5)];
                wire.SetColor(col);
                // Debug.Log(i + "color" + wire.color.ToString());
                ActiveWires.Add( wire );
            }
            else
            {
                wires[i].DisablePair();
                Debug.Log("Disabled wire" + i);
            }
        }
        correctIndex = CorrectWireIndex();
        Debug.Log("Intended index : " + correctIndex);


    }

    // Update is called once per frame
    void Update()
    {
        if (detonated)
        {
            LED.material.color = Color.lightGreen;
        }
    }

    public int getAnswer()
    {
        return correctIndex >= 0 ? correctIndex : -1;
    }


    public bool Has(Color col)
    {
        foreach(WireNodePair pair in ActiveWires)
        {
            if (pair.color == col)
            {
                return true;
            }
        }
        return false;
    }
    public int Count(Color col)
    {
        int count = 0;
        foreach(WireNodePair pair in ActiveWires)
        {
            if (pair.color == col)
            {
                count++;
            }
        }
        return count;
    }

    public int LastIndexOf(Color col)
    {
        int res = -1;
        for ( int i = 0; i < ActiveWires.Count; i++)
        {
            if (ActiveWires[i].color == col)
            {
                res = i;
            }
        }
        return res;
    }
    public int CorrectWireIndex()
    {
        
        int count = this.wireCount;

        // ================= 3 WIRES =================
        if (count == 3)
        {
            if (!Has(Color.red))
                return 1;

            if (ActiveWires[2].color == Color.white)
                return 2;

            if (Count(Color.blue) > 1)
                return LastIndexOf(Color.blue);

            return 2;
        }

        // ================= 4 WIRES =================
        if (count == 4)
        {
            if (Count(Color.red) > 1)
                return LastIndexOf(Color.red);

            if (ActiveWires[3].color == Color.yellow && !Has(Color.red))
                return 0;

            if (Count(Color.blue) == 1)
                return 0;

            if (Count(Color.yellow) > 1)
                return 3;

            return 1;
        }

        // ================= 5 WIRES =================
        if (count == 5)
        {
            if (ActiveWires[4].color == Color.green)
                return 3;

            if (Count(Color.red) == 1 &&
                Count(Color.yellow) > 1)
                return 0;

            if (!Has(Color.green))
                return 1;

            return 0;
        }

        // ================= 6 WIRES =================
        if (count == 6)
        {
            if (!Has(Color.yellow))
                return 2;

            if (Count(Color.yellow) == 1 &&
                Count(Color.white) > 1)
                return 3;

            if (!Has(Color.red))
                return 5;

            return 3;
        }

        return -1;
    }
}

