using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    public static PlayerRunner Instance;

    public UiManager uiManager;
    public PathFollower pathFollower;
    public Animator animator;
    public float moveSpeed = 5f; // �������� ����������� �������
    public float leftLimitLocal = -2f; // ��������� ����� ����������� �� ��� X
    public float rightLimitLocal = 2f; // ��������� ������ ����������� �� ��� 


    public Camera _mainCamera;
    private Vector3 _offset;
    private bool _dead = false;
    private float _startSpeed;

    private void Awake()
    {
        Instance = this;
    }

    public void SetInfo(PathCreator pathCreator)
    {
        pathFollower.pathCreator = pathCreator;
        animator.SetInteger("State", 0);
        _startSpeed = pathFollower.speed;
        pathFollower.speed = 0;
        // �������� ������ �� ������� ������
        _mainCamera = Camera.main;
    }

    public void StartGame()
    {
        pathFollower.speed = _startSpeed;
        animator.SetInteger("State", 1);
    }

    void Update()
    {
        if (_dead)
        {
            return;
        }

        // ��� ������� ������ ���� ��������� �������� �� ������� �� ������� �����
        if (Input.GetMouseButtonDown(0))
        {
            StoreOffset();
        }

        // ��� ��������� ������ ���� ���������� ������
        if (Input.GetMouseButton(0))
        {
            MovePlayer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + leftLimitLocal, transform.position.y, transform.position.z), 0.2f);

        Gizmos.DrawWireSphere(new Vector3(transform.position.x + rightLimitLocal, transform.position.y, transform.position.z), 0.2f);
    }

    private void StoreOffset()
    {
        // �������� ��� �� ������ �� ����� ����� �� ������
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out distance))
        {
            // �������� ������� ������� ����� � ��������� �������� �� ������� �� ���� �������
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            _offset = transform.position - mouseWorldPosition;
            _offset.y = 0;
            _offset.z = 0;
        }
    }

    private void MovePlayer()
    {
        // �������� ��� �� ������ �� ������� ������� ����� �� ������
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out distance))
        {
            // �������� ������� ������� ����� � ��������� ��������, ����� �������� ������� �������
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            Vector3 targetPosition = mouseWorldPosition + _offset;

            // ������ ���������� ������ � ������� ���� � �������������� Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��������� ����������� ������ � ��������� ��� X ����� �����������
            Vector3 clampedLocalPosition = transform.localPosition;
            clampedLocalPosition.x = Mathf.Clamp(clampedLocalPosition.x, leftLimitLocal, rightLimitLocal);
            clampedLocalPosition.z = 0; // ������������� z ���������� � 0, ����� ������ ��������� �� ����� ������
            transform.localPosition = clampedLocalPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Border>())
        {
            uiManager.LoseGame();
            _dead = true;
            pathFollower.speed = 0;
            animator.SetTrigger("Dead");
        }

        if (other.GetComponent<Finish>())
        {
            uiManager.WinGame();
            pathFollower.speed = 0;
            animator.SetInteger("State", 2);
        }
    }
}
