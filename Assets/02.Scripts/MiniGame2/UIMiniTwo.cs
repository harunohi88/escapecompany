using UnityEngine;

public class UIMiniTwo : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public GameTwoManager gameManager;

    public void InitGame()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject tile = Instantiate(tilePrefab, gridParent);
            TileButton tileBtn = tile.GetComponent<TileButton>();
            bool isRed = Random.value < 0.5f;
            tileBtn.Init(isRed, gameManager);
            gameManager.RegisterTile(isRed);
        }
    }
}
