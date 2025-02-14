using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class SpawnerLevel : MonoBehaviour
{
    [SerializeField] private Level[] allLevel;
    [SerializeField] private PlayerRunner _player;

    private int _levelIndex;
    private Level _currentLevel;

    private void Start()
    {
        _levelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
        int convertedIndex = _levelIndex - (int)(_levelIndex / allLevel.Length) * allLevel.Length;
        allLevel[convertedIndex].gameObject.SetActive(true);
        _currentLevel = allLevel[convertedIndex];
        _player.SetInfo(_currentLevel.GetPathCreator);
    }
}
