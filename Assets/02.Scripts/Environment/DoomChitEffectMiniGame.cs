namespace CreateMap
{
    public class DoomChitEffectMiniGame : DoomChitEffect
    {
        public Gauge target;
        protected override void Update()
        {
            lerpSpeed = target.GetPercent() * 3f;
            base.Update();
        }
    }
}
