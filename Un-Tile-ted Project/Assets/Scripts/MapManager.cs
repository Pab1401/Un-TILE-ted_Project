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
        render.RenderMap();
        playerSpawner.SpawnPlayer();
        aiPlacementManager.SpawnEnemies();
    }
}