using UnityEngine;

namespace MiniGameTWo
{
    public class UIMiniTwo : MonoBehaviour
    {
        public GameObject TilePrefab;
        public Transform GridParent;
        public GameTwoManager GameManager;

        public SpritePair[] SpritePairs;
        SpritePair _currentPair;
        public void InitGame()
        {
            _currentPair = SpritePairs[Random.Range(0, SpritePairs.Length)];

            for (int i = 0; i < 16; i++)
            {
                Debug.Log("InitGame");
                GameObject tile = Instantiate(TilePrefab, GridParent);
                TileButton tileBtn = tile.GetComponent<TileButton>();

                bool isRed = Random.value < 0.5f;

                tileBtn.Init(isRed, GameManager, _currentPair.RedSprite, _currentPair.WhiteSprite);
                GameManager.RegisterTile(isRed);
            }
        }
    }
}
