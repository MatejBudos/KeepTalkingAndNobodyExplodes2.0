using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource explosion;
    public AudioSource wrong;
    public AudioSource victory;
    public Image blackOverlay;
    public GameObject bombPrefab;
    public Transform bombSpawnPos;
    public GameObject bombGO;
    public Timer timer;

    [SerializeField] private TeleportationProvider teleportProvider;
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform xrOrigin;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.IsZero()) LooseEffect();
        
    }


    public void WinEffect()
    {
        victory.Play();
        StartCoroutine(WinSequence());
        
        

    }
    public void LooseEffect()
    {

        explosion.Play();
        StartCoroutine(LooseSequence());
        ResetGame();
    }

    public void ResetGame()
    {
        Destroy(bombGO);
        bombGO = null;
        timer.resetTimer();
    }

    IEnumerator LooseSequence()
    {
        blackOverlay.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        Teleport();
        blackOverlay.gameObject.SetActive(false);
    }
    IEnumerator WinSequence()
    {
        
        yield return new WaitForSeconds(5f);
        blackOverlay.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Teleport();
        blackOverlay.gameObject.SetActive(false);
        ResetGame();
    }

    public void MistakeEffect()
    {
        wrong.Play();
    }

    public void Teleport()
    {
        TeleportRequest request = new TeleportRequest
        {
            destinationPosition = teleportTarget.position,
            destinationRotation = teleportTarget.rotation
        };

        teleportProvider.QueueTeleportRequest(request);
    }

    public void StartGame( int moduleCount)
    {
        GameObject go = Instantiate(bombPrefab,bombSpawnPos);
        BombManager bm = go.GetComponent<BombManager>();
        bm.GenerateModules(moduleCount);
        bm.evnt += HandleBombEvent;
        bombGO = go;
        timer.startTimer();
    }

    public void HandleBombEvent(string evnt)
    {
        if(evnt == "Win")
        WinEffect();
        if(evnt == "Loose")
        LooseEffect();
        if(evnt == "Mistake")
        MistakeEffect();
    }

}
