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
    public float moveSpeed = 5f; // Скорость перемещения объекта
    public float leftLimitLocal = -2f; // Локальное левое ограничение по оси X
    public float rightLimitLocal = 2f; // Локальное правое ограничение по оси 


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
        // Получаем ссылку на главную камеру
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

        // При нажатии кнопки мыши сохраняем смещение от объекта до позиции клика
        if (Input.GetMouseButtonDown(0))
        {
            StoreOffset();
        }

        // При удержании кнопки мыши перемещаем объект
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
        // Получаем луч из камеры до точки клика на экране
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out distance))
        {
            // Получаем мировую позицию клика и вычисляем смещение от объекта до этой позиции
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            _offset = transform.position - mouseWorldPosition;
            _offset.y = 0;
            _offset.z = 0;
        }
    }

    private void MovePlayer()
    {
        // Получаем луч из камеры до текущей позиции клика на экране
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out distance))
        {
            // Получаем мировую позицию клика и добавляем смещение, чтобы получить целевую позицию
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            Vector3 targetPosition = mouseWorldPosition + _offset;

            // Плавно перемещаем объект к позиции мыши с использованием Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Применяем ограничения только к локальной оси X после перемещения
            Vector3 clampedLocalPosition = transform.localPosition;
            clampedLocalPosition.x = Mathf.Clamp(clampedLocalPosition.x, leftLimitLocal, rightLimitLocal);
            clampedLocalPosition.z = 0; // Устанавливаем z координату в 0, чтобы объект оставался на одной высоте
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
