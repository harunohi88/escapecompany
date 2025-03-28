namespace CreateMap
{
    public class DoomChitEffectMiniGame : DoomChitEffect
    {
        public Gauge target;
        void Update()
        {
            lerpSpeed = target.GetPercent() * 3f;
            base.Update();
        }
    }
}
