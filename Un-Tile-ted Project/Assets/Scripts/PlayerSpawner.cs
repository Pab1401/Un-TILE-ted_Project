using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private MapRender render;
    [SerializeField] private GameObject player;
    [SerializeField] private MovementHandler movementHandler;
    public void SpawnPlayer()
    {
        movementHandler.SetPlayerPosition();
        player.transform.position = render.playerSpawnPosition;
    }
}
