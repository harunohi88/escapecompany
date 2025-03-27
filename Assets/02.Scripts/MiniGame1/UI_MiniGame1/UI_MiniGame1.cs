using UnityEngine;
using TMPro;

namespace DocumentGame
{
    public class UI_MiniGame1 : MonoBehaviour
    {
        public static UI_MiniGame1 Instance;

        public TextMeshProUGUI ComboText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void UIActive()
        {
            ComboText.gameObject.SetActive(true);
        }

        public void UIInactive()
        {
            ComboText.gameObject.SetActive(false);
        }

        public void ComboRefresh(int combo)
        {
            ComboText.text = $"{combo}";
        }
    }
}
