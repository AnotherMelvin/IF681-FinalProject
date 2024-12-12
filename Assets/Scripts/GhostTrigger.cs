using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoint;
    [SerializeField]
    private GhostState _state;

    private GhostController ghost;
    private Collider trigger;

    void Start()
    {
        ghost = FindObjectOfType<GhostController>();
        trigger = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (ghost.IsImmune || ghost.IsRunning || ghost.IsWalking) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();
        ghost.SetCollider(true);

        if (_state.Equals(GhostState.Stand) || _state.Equals(GhostState.Walk))
        {
            ghost.SetAnimation("IsHanging", false);
        }
        else if (_state.Equals(GhostState.Hang))
        {
            ghost.SetAnimation("IsHanging", true);
        }

        if (_state.Equals(GhostState.Follow))
        {
            int i = Random.Range(0, 4);

            ghost.transform.SetParent(null);

            if (i == 0)
            {
                ghost.transform.localPosition = player.transform.TransformPoint(Vector3.forward * 20f);

            }
            else if (i == 1)
            {
                ghost.transform.localPosition = player.transform.TransformPoint(Vector3.back * 20f);
            }
            else if (i == 2)
            {
                ghost.transform.localPosition = player.transform.TransformPoint(Vector3.left * 20f);
            }
            else
            {
                ghost.transform.localPosition = player.transform.TransformPoint(Vector3.right * 20f);
            }

            ghost.transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);

            ghost.StartGhostFollow();
        }

        StartCoroutine(TriggerCountdown(player));
        CoroutineManager.Instance.StartGhostCoroutine(GhostCountdown(player), player, ghost);
    }

    private IEnumerator TriggerCountdown(PlayerController player)
    {
        if (player.MissingGhost == 5) yield break;
        trigger.enabled = false;
        AudioManager.Instance.PlayAudio(3);
        yield return new WaitForSeconds(10f);
        trigger.enabled = true;
    }

    private IEnumerator GhostCountdown(PlayerController player)
    {
        if (player.MissingGhost == 5) yield break;

        if (player.MissingGhost <= 1)
        {
            SpawnGhost(7f);
            yield return new WaitForSeconds(7f);
            HideGhost();

            if (!player.IsSeen)
            {
                player.MissingGhost++;
            }
        }
        else if (player.MissingGhost <= 3)
        {
            SpawnGhost(5f);
            yield return new WaitForSeconds(5f);
            HideGhost();

            if (!player.IsSeen)
            {
                player.MissingGhost++;
            }
        }
        else
        {
            HideGhost();
            player.MissingGhost++;
            ghost.SetAnimation("IsHanging", false);
            ghost.SetAnimation("IsWalking", false);
            yield return new WaitForSeconds(5f);
            ghost.StartGhostChase();
        }
    }

    private void SpawnGhost(float duration)
    {
        Transform selectedSpawnPoint = _spawnPoint[Random.Range(0, _spawnPoint.Length)];
        ghost.IsLurking = true;
        ghost.transform.position = selectedSpawnPoint.position;
        ghost.transform.rotation = selectedSpawnPoint.rotation;
        ghost.transform.SetParent(selectedSpawnPoint);

        if (_state.Equals(GhostState.Walk))
        {
            ghost.StartGhostMove(duration);
        }
    }

    private void HideGhost()
    {
        ghost.IsLurking = false;
        ghost.transform.SetParent(null);
        ghost.transform.position = new Vector3(0, -100f, 0);
    }
}

public enum GhostState
{
    Stand,
    Hang,
    Walk,
    Follow
}
