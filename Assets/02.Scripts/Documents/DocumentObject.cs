using UnityEngine;
using CreateMap;

namespace DocumentGame
{
    public class DocumentObject : DoomChitEffect
    {
        public VFXType VFXType;

        private int _excuteCount = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("GameOne"))
            {
                VFXPoolManager.Instance.VFX(VFXType, transform.position);
                _excuteCount++;
            }
        }

        void Update()
        {
            lerpSpeed = 0.3f * _excuteCount;

            base.Update();
        }
    }
}
