using UnityEngine;

namespace DocumentGame
{
    public class DisplaySlot : MonoBehaviour
    {
        public float Magnitude;

        public void DisplayDocument(Document document)
        {
            if (document == null)
            {
                return;
            }
            document.transform.position = transform.position;
            document.transform.localScale *= Magnitude;
            document.gameObject.SetActive(true);
        }
    }
}
