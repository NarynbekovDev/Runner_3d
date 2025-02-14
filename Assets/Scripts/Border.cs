using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private Transform player; // Ссылка на игрока

    void Start()
    {
        // Находим объект игрока по тегу
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position; // Направление к игроку
            direction.y = 0; // Убираем наклон по оси Y, чтобы противник не смотрел вверх или вниз
            transform.rotation = Quaternion.LookRotation(direction); // Устанавливаем поворот
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerRunner>())
        {
            anim.SetTrigger("Dead");
        }
    }
}
