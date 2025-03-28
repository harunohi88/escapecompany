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

        public void ActivateCombo()
        {
            ComboText.gameObject.SetActive(true);
        }

        public void InactivateCombo()
        {
            ComboText.gameObject.SetActive(false);
        }

        public void RefreshComboText(int combo)
        {
            ComboText.text = $"{combo}";
        }

    }
}
