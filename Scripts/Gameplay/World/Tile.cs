// Scripts/Gameplay/World/Tile.cs
using System.Collections.Generic;

public class Tile
{
    // В будущем здесь будет информация о типе земли (пол, стена, трава)
    // public TerrainDefinition Terrain;

    private List<Entity> _entitiesOnTitle;

    public Tile()
    {
        _entitiesOnTitle = new List<Entity>();
    }

    public void AddEntity(Entity entity)
    {
        _entitiesOnTitle.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        _entitiesOnTitle.Remove(entity);
    }
}
