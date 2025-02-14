using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private Transform player; // ������ �� ������

    void Start()
    {
        // ������� ������ ������ �� ����
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
            Vector3 direction = player.position - transform.position; // ����������� � ������
            direction.y = 0; // ������� ������ �� ��� Y, ����� ��������� �� ������� ����� ��� ����
            transform.rotation = Quaternion.LookRotation(direction); // ������������� �������
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
