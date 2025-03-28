using UnityEngine;

namespace DocumentGame
{
    public class DocumentObject : MonoBehaviour
    {
        public VFXType VFXType;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("GameOne"))
            {
                VFXPoolManager.Instance.VFX(VFXType, transform.position);
            }
        }
    }
}
