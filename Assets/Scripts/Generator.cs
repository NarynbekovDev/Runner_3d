using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : PathSceneTool
{
  public GameObject[] prefab;         // Префабы игровых объектов, которые будут генерироваться
    public GameObject holder;           // Родительский объект, куда будут помещаться сгенерированные объекты
    public float spacing = 3;           // Расстояние между сгенерированными объектами по пути
    public float distanceFromStart = 0; // Расстояние от начала пути до первой генерации объекта
    public float distanceFromFinish = 0; // Расстояние от конца пути до последней генерации объекта

    public Vector3[] trapsPoint;

    const float minSpacing = 0.1f;

    void Generate()
    {
        if (pathCreator != null && prefab != null && holder != null)
        {
            DestroyObjects();

            VertexPath path = pathCreator.path;

            spacing = Mathf.Max(minSpacing, spacing);
            float dst = distanceFromStart;

            while (dst < (path.length - distanceFromFinish))
            {
                Vector3 point = path.GetPointAtDistance(dst);
                Quaternion rot = path.GetRotationAtDistance(dst);
                Vector3 position = point + trapsPoint[Random.Range(0, trapsPoint.Length)];

                // Добавляем поворот на 90 градусов вокруг оси Z к текущему повороту
                rot *= Quaternion.Euler(0, 0, 90);

                // Создаем объект из случайного префаба и помещаем его в указанные координаты и поворот, внутри родительского объекта
                Instantiate(prefab[Random.Range(0, prefab.Length)], position, rot, holder.transform);

                dst += spacing;
            }
        }
    }

    void DestroyObjects()
    {
        // Удаляем все сгенерированные объекты, находящиеся внутри родительского объекта
        int numChildren = holder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
        }
    }

    protected override void PathUpdated()
    {
        // Вызывается, когда путь был обновлен (например, изменена его форма)
        // Запускаем генерацию объектов по обновленному пути
        if (pathCreator != null && !Application.isPlaying)
        {
            Generate();
        }
    }

}
