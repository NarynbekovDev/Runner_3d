using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public float countdownTime = 5f; // Время таймера
    public TextMeshProUGUI timerText;          // Текст UI для отображения таймера
    public bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Останавливаем игровое время
        StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0)
        {
            timerText.text = Mathf.Ceil(remainingTime).ToString(); // Обновляем текст таймера
            yield return new WaitForSecondsRealtime(1f);           // Ждем реальное время
            remainingTime--;
        }

        timerText.text = "GO!"; // Финальное сообщение
        yield return new WaitForSecondsRealtime(1f);

        timerText.gameObject.SetActive(false); // Скрываем таймер
        Time.timeScale = 1f;                  // Возобновляем игровой процесс
        gameStarted = true;                   // Флаг начала игры
    }
}
