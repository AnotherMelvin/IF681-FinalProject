using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeSelect : MonoBehaviour
{
    public MazePlayer player;
    public Transform cameraCanvas;
    public Transform previousTransform;
    public CameraController playerCamera;
    public FollowPlayer followPlayer;
    public Collider mazeCollider;
    public Outline outline;
    public MeshFilter mapFilter;
    public MeshCollider mapCollider;
    public Sprite mapSprite;
    private Coroutine transition;

    void Awake()
    {
        outline.enabled = false;
        mazeCollider.enabled = true;

        mapFilter.mesh = SpriteToMesh(mapSprite);
        mapCollider.sharedMesh = mapFilter.mesh;
    }

    public void Select()
    {
        if (player.isOpened || player.isFinished)
        {
            outline.enabled = false;
            return;
        }

        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }

    public void Interact()
    {
        if (!player.isOpened && !player.isFinished)
        {
            OpenMaze();
        }
    }

    internal void OpenMaze()
    {
        playerCamera.OnMaze = true;
        followPlayer.OnMaze = true;

        mazeCollider.enabled = false;
        outline.enabled = false;

        previousTransform.position = Camera.main.transform.position;
        previousTransform.rotation = Camera.main.transform.rotation;
        transition ??= StartCoroutine(CameraTransition(cameraCanvas));
    }

    internal void CloseMaze()
    {
        mazeCollider.enabled = true;
        transition ??= StartCoroutine(CameraTransition(previousTransform));
    }

    private IEnumerator CameraTransition(Transform position)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, position.position, 5f * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, position.rotation, 5f * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.isOpened = !player.isOpened;
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = CursorLockMode.Locked;

        if (!player.isOpened)
        {
            playerCamera.OnMaze = false;
            followPlayer.OnMaze = false;
        }

        transition = null;
    }

    private Mesh SpriteToMesh(Sprite sprite)
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(Array.ConvertAll(sprite.vertices, i => (Vector3)i).ToList());
        mesh.SetUVs(0, sprite.uv.ToList());
        mesh.SetTriangles(Array.ConvertAll(sprite.triangles, i => (int)i), 0);

        return mesh;
    }
}
