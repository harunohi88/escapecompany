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
            document.transform.localScale = Vector3.one * Magnitude;
            document.gameObject.SetActive(true);
        }
    }
}
