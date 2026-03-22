using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapGeneration generator;
    public MapRender renderer;

    void Start()
    {
        // The Sequence: Logic first, then Visuals
        generator.GenerateMap();
        renderer.RenderMap();
    }
}