using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public float countdownTime = 5f; // ����� �������
    public TextMeshProUGUI timerText;          // ����� UI ��� ����������� �������
    public bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // ������������� ������� �����
        StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            timerText.text = Mathf.Ceil(remainingTime).ToString(); // ��������� ����� �������
            yield return new WaitForSecondsRealtime(1f);           // ���� �������� �����
            remainingTime--;
        }

        timerText.text = "GO!"; // ��������� ���������
        yield return new WaitForSecondsRealtime(1f);

        timerText.gameObject.SetActive(false); // �������� ������
        Time.timeScale = 1f;                  // ������������ ������� �������
        gameStarted = true;                   // ���� ������ ����
    }
}
