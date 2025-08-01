using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPoint : MonoBehaviour
{
    [SerializeField] public Material[] color = new Material[0];
    public MeshRenderer MeshRenderer;
    public int ID;

    public Vector3 OriginalPoint { get; private set; }
    public Vector3 TargetPoint => new Vector3(OriginalPoint.x, OriginalPoint.y + .1f, OriginalPoint.z);

    private Coroutine _moveCoroutine;
    private void Start()
    {
        OriginalPoint = transform.position;
    }

    public void SetColor(int id)
    {
        if (id < 0 || id >= color.Length)
        {
            Debug.LogError($"Invalid color ID: {id}. Must be between 0 and {color.Length - 1}.");
            return;
        }
        MeshRenderer.material = color[id];
    }

    public void NewOriginalPoint(Vector3 newPos) => OriginalPoint = newPos;

    public IEnumerator MoveOnEnumerator(Vector3 point, float speed = 1)
    {
        while (transform.position != point)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                point,
                speed * Time.deltaTime
            );
            yield return null;
        }
    }
    public void MoveOn(Vector3 point, float speed = 1)
    {
        StopMoveIfAlready();
        _moveCoroutine = StartCoroutine(MoveOnEnumerator(point, speed));
    }

    public void MoveOut(Vector3 point, float speed = 1)
    {
        StopAllCoroutines();
        _moveCoroutine = StartCoroutine(MoveOutEnumerator(point, speed));
    }
    public IEnumerator MoveOutEnumerator(Vector3 targetPosition, float moveSpeed = 10f, float rotateSpeed = 360f, float restoreRotationSpeed = 720f)
    {
        // Lưu rotation ban đầu
        Quaternion originalRotation = transform.rotation;

        float totalRotation = 0f;
        float targetRotation = Random.Range(720f, 1080f); // Xoay 2-3 vòng

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Di chuyển tới target
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Xoay khi đang di chuyển
            float rotationThisFrame = rotateSpeed * Time.deltaTime;
            if (totalRotation < targetRotation)
            {
                transform.Rotate(Vector3.up, rotationThisFrame);
                totalRotation += rotationThisFrame;
            }

            yield return null;
        }

        // Snap vị trí chính xác
        transform.position = targetPosition;

        // Xoay lại về rotation ban đầu
        while (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                originalRotation,
                restoreRotationSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Snap rotation
        transform.rotation = originalRotation;
    }

    private void StopMoveIfAlready()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
    }
}
