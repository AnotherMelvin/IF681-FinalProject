using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private bool _isTesting;

    public bool GameOver;
    public bool IsLurking;
    public bool IsImmune;
    public bool IsWalking;
    public bool IsRunning;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (!_isTesting)
        {
            transform.position = new Vector3(0, -100f, 0);
        }
        else
        {
            StartCoroutine(GhostChase());
        }
    }

    void LateUpdate()
    {
        if (GameOver) return;

        Vector3 lookDirection = _player.position - _head.position;
        _head.rotation = Quaternion.LookRotation(lookDirection);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (!player.IsSeen && player.MissingGhost <= 4)
            {
                Jumpscare(player);
            }
        }
    }

    public void SetAnimation(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    public void StartGhostMove(float duration)
    {
        CoroutineManager.Instance.StartWalkCoroutine(MoveGhost(duration));
    }

    public void StartGhostFollow()
    {
        CoroutineManager.Instance.StartWalkCoroutine(FollowPlayer());
    }

    public IEnumerator MoveGhost(float duration)
    {
        IsWalking = true;
        SetAnimation("IsWalking", true);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.Translate(2f * Time.deltaTime * Vector3.forward);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsWalking = false;
        SetAnimation("IsWalking", false);
    }

    public IEnumerator FollowPlayer()
    {
        IsWalking = true;
        SetAnimation("IsWalking", true);
        _collider.enabled = true;
        PlayerController player = _player.GetComponentInParent<PlayerController>();

        while (Vector3.Distance(transform.position, _player.parent.position) > 2f)
        {
            transform.Translate(2f * Time.deltaTime * Vector3.forward);
            transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 lookDirection = _player.parent.position - transform.position;
            lookDirection.y = 0;
            transform.localRotation = Quaternion.LookRotation(lookDirection);
            yield return null;
        }

        IsWalking = false;
        SetAnimation("IsWalking", false);

        Jumpscare(player);

        yield return null;
    }

    public void StartGhostChase()
    {
        CoroutineManager.Instance.StartRunCoroutine(GhostChase());
    }

    public void StartImmuneChase()
    {
        CoroutineManager.Instance.StartRunCoroutine(ImmuneChase());
    }

    public IEnumerator GhostChase()
    {
        IsRunning = true;
        SetAnimation("IsRunning", true);
        PlayerController player = _player.GetComponentInParent<PlayerController>();
        _collider.enabled = false;

        if (player.ItemCount == 10) yield break;

        transform.localPosition = _player.TransformPoint(Vector3.back * 40f);
        transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);

        while (Vector3.Distance(transform.position, _player.parent.position) > 2f)
        {
            transform.Translate(10f * Time.deltaTime * Vector3.forward);
            transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 lookDirection = _player.parent.position - transform.position;
            lookDirection.y = 0;
            transform.localRotation = Quaternion.LookRotation(lookDirection);
            yield return null;
        }

        IsRunning = false;
        SetAnimation("IsRunning", false);

        Jumpscare(player);

        yield return null;
    }

    public IEnumerator ImmuneChase()
    {
        IsRunning = true;
        SetAnimation("IsRunning", true);
        PlayerController player = _player.GetComponentInParent<PlayerController>();
        _collider.enabled = false;

        while (Vector3.Distance(transform.position, _player.parent.position) > 2f)
        {
            transform.Translate(10f * Time.deltaTime * Vector3.forward);
            transform.localPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 lookDirection = _player.parent.position - transform.position;
            lookDirection.y = 0;
            transform.localRotation = Quaternion.LookRotation(lookDirection);
            yield return null;
        }

        IsRunning = false;
        SetAnimation("IsRunning", false);

        Jumpscare(player);

        yield return null;
    }

    public void WalkEvent()
    {
        AudioManager.Instance.PlayLocalAudio(6, _audioSource);
    }

    public void LeftRunEvent()
    {
        AudioManager.Instance.PlayLocalAudio(4, _audioSource);
    }

    public void RightRunEvent()
    {
        AudioManager.Instance.PlayLocalAudio(5, _audioSource);
    }

    public void Jumpscare(PlayerController player)
    {
        CoroutineManager.Instance.StopGhostCoroutine(null, null);
        CoroutineManager.Instance.StopWalkCoroutine();
        CoroutineManager.Instance.StopRunCoroutine();

        SetAnimation("IsHanging", false);
        SetAnimation("IsWalking", false);
        SetAnimation("IsRunning", false);
        SetAnimation("IsOver", true);

        player.GameOver();
        AudioManager.Instance.PlayAudio(7);

    }

    public void OverEvent()
    {
        SceneSwitcher.Instance.ChangeScene("LoseScene", new Color32(255, 0, 0, 255), 1f);
    }

    public void SetCollider(bool state)
    {
        _collider.enabled = state;
    }

    public void ResetGhost()
    {
        SetAnimation("IsHanging", false);
        SetAnimation("IsWalking", false);
        SetAnimation("IsRunning", false);

        transform.position = new Vector3(0, -100f, 0);
        _collider.enabled = true;
        IsImmune = false;
        IsWalking = false;
        IsRunning = false;
    }
}
