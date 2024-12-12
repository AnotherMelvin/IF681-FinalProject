using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform _orientation;
    [SerializeField]
    private GameObject _flashlight;
    [SerializeField]
    private GameObject _flashlightModel;
    [SerializeField]
    private GameObject _shatter;
    [SerializeField]
    private TMP_Text _itemCount;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _deceleration;

    [SerializeField]
    private FollowPlayer _followPlayer;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private GhostController _ghostController;
    [SerializeField]
    private CanvasGroup _hud;
    [SerializeField]
    private CanvasGroup _controls;
    [SerializeField]
    private CanvasGroup _objectives;
    [SerializeField]
    private Transform[] _spawnPoints;
    [SerializeField]
    private AudioSource walkSource;
    public AudioSource collectSource;
    public string nextScene;
    public int chapter;

    public int MissingGhost;
    public bool IsSeen => isSeen;
    public int ItemCount => itemCount;

    private Vector3 currentVelocity = Vector3.zero;
    private Rigidbody rb;
    private bool isSeen;
    private int itemCount = 0;
    private Coroutine playerWalk;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (_itemCount != null && chapter == 1)
        {
            _itemCount.text = $"Collect {itemCount}/10 Books to Escape";
        }
        else if (_itemCount != null && chapter == 2)
        {
            _itemCount.text = $"Solve {itemCount}/10 Mazes to Escape";
        }

        _shatter.SetActive(false);
    }

    void Start()
    {
        transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;

        if (chapter == 1)
        {
            _controls.alpha = 1;
            _objectives.alpha = 0;
        }
        else
        {
            _controls.alpha = 0;
            _objectives.alpha = 1;
        }
    }

    void Update()
    {
        if (_ghostController.GameOver || Time.timeScale == 0)
        {
            if (playerWalk != null)
            {
                StopCoroutine(playerWalk);
                playerWalk = null;
                walkSource.Stop();
            }
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 15f))
        {
            if (!isSeen)
            {
                if (hit.collider.CompareTag("Ghost") && MissingGhost < 4)
                {
                    isSeen = true;

                    if (chapter == 1 || chapter == 3)
                    {
                        StartCoroutine(DisableFlashlight());
                    }
                    else if (chapter == 2 && !_cameraController.OnMaze)
                    {
                        int i = Random.Range(0, 2);

                        if (i == 0)
                        {
                            StartCoroutine(DisableFlashlight());
                        }
                        else
                        {
                            StartCoroutine(Immune());
                        }
                    }
                }
            }
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            _controls.alpha = 0;
            _objectives.alpha = 1;
            playerWalk ??= StartCoroutine(PlayerWalk());
        }
        else
        {
            if (playerWalk != null)
            {
                StopCoroutine(playerWalk);
                playerWalk = null;
                walkSource.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        if (_ghostController.GameOver || _cameraController.OnMaze)
        {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 targetVelocity = (_orientation.forward * z + _orientation.right * x).normalized * _moveSpeed;
        targetVelocity.y = 0;

        if (targetVelocity.magnitude > 0)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, _deceleration * Time.fixedDeltaTime);
        }

        rb.velocity = currentVelocity;
    }

    private IEnumerator PlayerWalk()
    {
        AudioManager.Instance.PlayLocalAudio(8, walkSource);

        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator DisableFlashlight()
    {
        MissingGhost = Mathf.Max(0, MissingGhost - Mathf.Clamp(Random.Range(-10, 10), 0, 1));
        AudioManager.Instance.PlayAudio(1);

        _ghostController.SetCollider(false);
        _ghostController.SetAnimation("IsWalking", false);
        _ghostController.SetAnimation("IsRunning", false);
        CoroutineManager.Instance.StopWalkCoroutine();

        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayAudio(2);

        yield return new WaitForSeconds(0.05f);

        _ghostController.transform.SetParent(null);
        _ghostController.transform.position = new Vector3(0, -100f, 0);

        _ghostController.SetAnimation("IsHanging", false);

        _ghostController.IsWalking = false;
        _ghostController.IsRunning = false;

        _shatter.SetActive(true);
        _flashlight.SetActive(false);
        yield return new WaitForSeconds(2.75f);
        _flashlight.SetActive(true);
        yield return new WaitForSeconds(1f);
        _flashlight.SetActive(false);
        yield return new WaitForSeconds(1f);
        _flashlight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _flashlight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _flashlight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _flashlight.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _flashlight.SetActive(true);
        _shatter.SetActive(false);

        isSeen = false;
    }

    private IEnumerator Immune()
    {
        CoroutineManager.Instance.StopGhostCoroutine(null, null);
        CoroutineManager.Instance.StopWalkCoroutine();
        _ghostController.SetAnimation("IsWalking", false);
        _ghostController.IsWalking = false;
        _ghostController.IsImmune = true;
        _ghostController.transform.SetParent(null);

        yield return new WaitForSeconds(3f);

        float angle = Vector3.Angle(_orientation.forward, _ghostController.transform.position - transform.position);

        if (angle < 60f)
        {
            _ghostController.StartImmuneChase();
            yield break;
        }

        _ghostController.transform.position = new Vector3(0, -100f, 0);
        _ghostController.IsImmune = false;

        isSeen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            AudioManager.Instance.PlayLocalAudio(0, collectSource);
            itemCount++;
            _itemCount.text = $"Collect {itemCount}/10 Books to Escape";
            other.gameObject.SetActive(false);

            if (itemCount == 10)
            {
                StartCoroutine(FinishGame());
            }
        }
    }

    public void SolveMaze()
    {
        AudioManager.Instance.PlayLocalAudio(0, collectSource);
        itemCount++;
        _itemCount.text = $"Solve {itemCount}/10 Mazes to Escape";

        if (itemCount == 10)
        {
            StartCoroutine(FinishGame());
        }
    }

    public void UnlockDoor()
    {
        StartCoroutine(FinishGame());
    }

    private IEnumerator FinishGame()
    {
        CoroutineManager.Instance.StopGhostCoroutine(null, null);
        CoroutineManager.Instance.StopWalkCoroutine();
        CoroutineManager.Instance.StopRunCoroutine();

        _ghostController.GameOver = true;
        _ghostController.transform.SetParent(null);
        _ghostController.transform.position = new Vector3(0, -100f, 0);

        yield return new WaitForSeconds(3f);

        if (chapter != 3)
        {
            SceneSwitcher.Instance.ChangeScene(nextScene, new Color32(0, 0, 0, 255), 1f);
        }
        else
        {
            SceneSwitcher.Instance.ChangeScene(nextScene, new Color32(255, 255, 255, 255), 1f);
        }
    }

    public void GameOver()
    {
        _hud.alpha = 0;
        _flashlightModel.SetActive(false);
        _followPlayer.OnMaze = false;
        _followPlayer.GameOver = true;
        _cameraController.OnMaze = false;
        _cameraController.GameOver = true;
        _ghostController.GameOver = true;
    }
}
