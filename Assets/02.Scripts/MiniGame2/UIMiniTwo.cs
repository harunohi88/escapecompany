using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameTWo
{
    public class UIMiniTwo : MonoBehaviour
    {
        public GameObject TilePrefab;
        public Transform GridParent;
        public GameTwoManager GameManager;

        public SpritePair[] SpritePairs;
        private SpritePair _currentPair;

        public void InitGame()
        {
            _currentPair = SpritePairs[Random.Range(0, SpritePairs.Length)];

            for (int i = 0; i < 16; i++)
            {
                GameObject tile = Instantiate(TilePrefab, GridParent);
                TileButton tileBtn = tile.GetComponent<TileButton>();

                bool isRed = Random.value < 0.5f;

                tileBtn.Init(isRed, GameManager, _currentPair.RedSprite, _currentPair.WhiteSprite);
                GameManager.RegisterTile(isRed);
            }
        }
    }
}

