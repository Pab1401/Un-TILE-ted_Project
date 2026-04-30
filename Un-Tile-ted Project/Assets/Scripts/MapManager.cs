using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapGeneration generator;
    public MapRender render;
    public PlayerSpawner playerSpawner;
    [SerializeField] private AIplacementManager aiPlacementManager;

    void Start()
    {
        // The Sequence: Logic first, then Visuals
        generator.GenerateMap();
        Debug.Log("Map Generated");
        render.RenderMap();
        Debug.Log("Map Rendered");
        playerSpawner.SpawnPlayer();
        aiPlacementManager.SpawnEnemies();
    }
}