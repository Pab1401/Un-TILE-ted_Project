using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private MapRender renderer;
    [SerializeField] private GameObject player;
    [SerializeField] private MovementHandler movementHandler;
    public void SpawnPlayer()
    {
        movementHandler.SetPlayerPosition();
        player.transform.position = renderer.playerSpawnPosition;
    }
}
