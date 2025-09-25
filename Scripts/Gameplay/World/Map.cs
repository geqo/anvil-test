// Scripts/Gameplay/World/Map.cs
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Tile[,] _grid; // Двумерный массив для хранения клеток
    private List<Entity> _allEntities; // Общий список всего, что есть на карте

    public Map(int width, int height)
    {
        Width = width;
        Height = height;

        _grid = new Tile[width, height];
        _allEntities = new List<Entity>();

        // Инициализируем каждую клетку в сетке
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid[x, y] = new Tile();
            }
        }

        Debug.Log($"Создана карта размером {width}x{height}");
    }

    /// <summary>
    /// Основной метод для добавления чего-либо на карту.
    /// </summary>
    public void SpawnEntity(Entity entity, Vector2Int position)
    {
        // Устанавливаем позицию объекта
        entity.Position = position;

        // Добавляем в общие списки
        _allEntities.Add(entity);
        _grid[position.x, position.y].AddEntity(entity);

        Debug.Log($"На карту в точке {position} добавлен объект {entity.GetType().Name} (ID: {entity.UniqueId})");
    }

    /// <summary>
    /// Удаление чего-либо с карты
    /// </summary>
    public void RemoveEntity(Entity entity)
    {
        _allEntities.Remove(entity);
        _grid[entity.Position.x, entity.Position.y].RemoveEntity(entity);
        Debug.Log($"С карты удален объект {entity.GetType().Name} (ID: {entity.UniqueId})");
    }
}
