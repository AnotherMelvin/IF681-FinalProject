using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }
    public Coroutine GhostCoroutine;
    public Coroutine WalkCoroutine;
    public Coroutine RunCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void StartGhostCoroutine(IEnumerator coroutine, PlayerController player, GhostController ghost)
    {
        StopGhostCoroutine(player, ghost);
        GhostCoroutine = StartCoroutine(coroutine);
    }

    public void StopGhostCoroutine(PlayerController player, GhostController ghost)
    {
        if (GhostCoroutine != null)
        {
            StopCoroutine(GhostCoroutine);

            if (player != null && ghost != null)
            {
                if (ghost.IsLurking)
                {
                    player.MissingGhost++;
                }
            }

            GhostCoroutine = null;
        }
    }

    public void StartWalkCoroutine(IEnumerator coroutine)
    {
        StopWalkCoroutine();
        WalkCoroutine = StartCoroutine(coroutine);
    }

    public void StopWalkCoroutine()
    {
        if (WalkCoroutine != null)
        {
            StopCoroutine(WalkCoroutine);
            WalkCoroutine = null;
        }
    }

    public void StartRunCoroutine(IEnumerator coroutine)
    {
        StopRunCoroutine();
        RunCoroutine = StartCoroutine(coroutine);
    }

    public void StopRunCoroutine()
    {
        if (RunCoroutine != null)
        {
            StopCoroutine(RunCoroutine);
            RunCoroutine = null;
        }
    }
}
