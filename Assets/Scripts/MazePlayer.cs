using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazePlayer : MonoBehaviour, IPointerDownHandler
{
    public MeshRenderer cylinder;
    public Material start;
    public Material finish;
    public Transform spawnPoint;
    public Transform finishPoint;
    public GameObject indicator;
    public MazeSelect mazeSelect;
    public PlayerController playerController;
    public GhostController ghostController;
    public Transform[] ghostSpawnPoints;
    internal bool isSelected;
    internal bool isFinished;
    internal bool isOpened;
    private Image point;
    private static bool isFirst;
    private Coroutine finished;
    private static float timeLimit = 10f;
    private float elapsedTime;

    void Awake()
    {
        transform.position = spawnPoint.position;
        cylinder.material = start;
        point = GetComponent<Image>();
        isFirst = true;
    }

    void Update()
    {
        if (ghostController.GameOver) return;

        if (isFirst)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }

        if (!isOpened)
        {
            timeLimit = 10f;
            elapsedTime = 0f;
            return;
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeLimit)
        {
            if (!ghostController.IsImmune && !ghostController.IsRunning && !ghostController.IsWalking && !isFinished)
            {
                ghostController.transform.position = ghostSpawnPoints[Random.Range(0, ghostSpawnPoints.Length)].position;
                ghostController.StartGhostFollow();
            }

            elapsedTime = 0f;
        }

        Cursor.lockState = CursorLockMode.None;

        if (isSelected && !isFinished)
        {
            point.raycastTarget = false;
            Cursor.visible = false;

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector3.Lerp(transform.position, worldPosition, 10f * Time.deltaTime);

            isFirst = false;
        }
        else if (!isFinished)
        {
            point.raycastTarget = true;
            Cursor.visible = true;

            transform.position = Vector3.Lerp(transform.position, spawnPoint.position, 10f * Time.deltaTime);

            timeLimit = Mathf.Clamp(timeLimit - 1f, 5f, 10f);
        }

        if (isFinished)
        {
            point.raycastTarget = true;
            Cursor.visible = true;
            cylinder.material = finish;

            transform.position = Vector3.Lerp(transform.position, finishPoint.position, 10f * Time.deltaTime);

            finished ??= StartCoroutine(FinishMaze());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isSelected = false;
            mazeSelect.CloseMaze();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
    }

    private IEnumerator FinishMaze()
    {
        playerController.SolveMaze();
        CoroutineManager.Instance.StopWalkCoroutine();
        ghostController.ResetGhost();
        yield return new WaitForSeconds(3f);
        isSelected = false;
        mazeSelect.CloseMaze();
    }
}
