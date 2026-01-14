using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSequenceController : MonoBehaviour
{
    [System.Serializable]
    public class SimonTile
    {
        public Renderer renderer;
        public Color glowColor;
    }

    public SimonTile[] tiles;

    [Header("Sequence Settings")]
    public int sequenceLength = 5;
    public float glowDuration = 0.8f;
    public float delayBetweenGlows = 0.4f;
    public float glowIntensity = 2.5f;
    public float sequenceRepeatDelay = 10f;

    private List<int> sequence = new List<int>();

    void Start()
    {
        DisableAllGlow();
        GenerateSequence();
        StartCoroutine(SequenceLoop());
    }

    // -----------------------------
    // Sequence logic
    // -----------------------------

    void GenerateSequence()
    {
        sequence.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, tiles.Length);
            sequence.Add(randomIndex);
        }

        Debug.Log($"{gameObject.name} sequence: {string.Join(", ", sequence)}");
    }

    IEnumerator SequenceLoop()
    {
        yield return new WaitForSeconds(1f); // initial delay

        while (true)
        {
            // Play full sequence
            foreach (int index in sequence)
            {
                GlowTile(index, true);
                yield return new WaitForSeconds(glowDuration);
                GlowTile(index, false);
                yield return new WaitForSeconds(delayBetweenGlows);
            }

            // Wait after last glow
            yield return new WaitForSeconds(sequenceRepeatDelay);
        }
    }

    // -----------------------------
    // Glow helpers
    // -----------------------------

    void GlowTile(int index, bool enable)
    {
        var mat = tiles[index].renderer.material;

        if (enable)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", tiles[index].glowColor * glowIntensity);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }

    void DisableAllGlow()
    {
        foreach (var tile in tiles)
        {
            var mat = tile.renderer.material;
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }

    public void OnTileClicked(int tileIndex)
    {
        Debug.Log(
            $"Module '{gameObject.name}' received click on tile {tileIndex}"
        );

        StartCoroutine(GlowOnce(tileIndex));
    }

IEnumerator GlowOnce(int tileIndex)
    {
        GlowTile(tileIndex, true);
        yield return new WaitForSeconds(glowDuration);
        GlowTile(tileIndex, false);
    }

    
}
