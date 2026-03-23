using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapGeneration generator;
    public MapRender renderer;
    public PlayerSpawner playerSpawner;

    void Start()
    {
        // The Sequence: Logic first, then Visuals
        generator.GenerateMap();
        renderer.RenderMap();
        playerSpawner.SpawnPlayer();
    }
}