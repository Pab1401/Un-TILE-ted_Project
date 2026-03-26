using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapGeneration generator;
    public MapRender renderer;
    public PlayerSpawner playerSpawner;
    [SerializeField] private AIplacementManager aiPlacementManager;

    void Start()
    {
        // The Sequence: Logic first, then Visuals
        generator.GenerateMap();
        renderer.RenderMap();
        playerSpawner.SpawnPlayer();
        aiPlacementManager.SpawnEnemies();
    }
}