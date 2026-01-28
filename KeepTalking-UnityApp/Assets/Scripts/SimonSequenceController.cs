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

    public Renderer LED;

    public SimonTile[] tiles;

    [Header("Sequence Settings")]
    public int sequenceLength = 5;
    public float glowDuration = 0.8f;
    public float delayBetweenGlows = 0.4f;
    public float glowIntensity = 2.5f;
    public float sequenceRepeatDelay = 7.5f;

    private List<int> sequence = new List<int>();
    private bool inputEnabled = false;
    private Coroutine sequenceLoopCoroutine;
    private float replayTimer = 0f;


    // Rule sets: index = shown tile (up, right, down, left), value = expected tile
    private readonly int[] rulesNoStrikes = { 3, 2, 0, 1 };
    private readonly int[] rulesOneStrike  = { 1, 3, 2, 0 };
    private readonly int[] rulesTwoStrikes  = { 2, 0, 1, 3 };

    private List<int> expectedSequence = new List<int>();
    private int playerInputIndex = 0;

    private bool defused = false;


    void Start()
    {
        DisableAllGlow();
        GenerateSequence();
        sequenceLoopCoroutine = StartCoroutine(SequenceLoop());
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

    public void GenerateExpectedSequence()
    {
        expectedSequence.Clear();

        int[] activeRules;

        if (BombManager.strikes == 0)
            activeRules = rulesNoStrikes;
        else if (BombManager.strikes == 1)
            activeRules = rulesOneStrike;
        else
            activeRules = rulesTwoStrikes;

        foreach (int shown in sequence)
        {
            expectedSequence.Add(activeRules[shown]);
        }

        playerInputIndex = 0;
    }


    IEnumerator SequenceLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!defused)
        {
            // Lock input while showing sequence
            inputEnabled = false;
            GenerateExpectedSequence();
            // Play sequence
            foreach (int index in sequence)
            {
                GlowTile(index, true);
                yield return new WaitForSeconds(glowDuration);
                GlowTile(index, false);
                yield return new WaitForSeconds(delayBetweenGlows);
            }

            // Enable input after sequence finishes
            inputEnabled = true;

            // Wait until it's time to replay
            replayTimer = 0f;
            while (replayTimer < sequenceRepeatDelay)
            {
                replayTimer += Time.deltaTime;
                yield return null;
            }
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
        if (!inputEnabled)
            return;

        ResetSequenceTimer();
        StartCoroutine(GlowOnce(tileIndex));

        // Check input
        if (tileIndex == expectedSequence[playerInputIndex])
        {
            Debug.Log(">>>>Clicked on index: " + tileIndex + "expected " + expectedSequence[playerInputIndex]);
            playerInputIndex++;
           

            // Sequence completed successfully
            if (playerInputIndex >= expectedSequence.Count)
            {
                Debug.Log($"Module '{gameObject.name}' defused input sequence!");
                inputEnabled = false;
                defused = true;
                BombManager.SolvedModule?.Invoke();
                LED.material.color = Color.lightGreen;
            }
        }
        else
        {
            Debug.Log($"Wrong input on module '{gameObject.name}' with sequence: {string.Join(", ", sequence)}! and expectedseq: " + string.Join(", ", expectedSequence));
            Debug.Log("Clicked on index: " + tileIndex + "but expected " + expectedSequence[playerInputIndex]);
            Debug.Log("playerInputIndex was: " + playerInputIndex);

            BombManager.OnWrongClick?.Invoke();

            inputEnabled = false;
            playerInputIndex = 0;
        }
    }

    void ResetSequenceTimer()
    {
        replayTimer = 0f;
    }

    IEnumerator GlowOnce(int tileIndex)
    {
        GlowTile(tileIndex, true);
        yield return new WaitForSeconds(glowDuration);
        GlowTile(tileIndex, false);
    }

    
}
