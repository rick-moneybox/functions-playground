namespace Functions.Tutorials.Core.Domain
{
    public class TutorialTip
    {
        public string Key { get; }

        public TutorialTipTargetAudience TargetAudience { get; }

        public int TimeDelayInDays { get; }

        public TutorialTip(TutorialTipState state)
        {
            TimeDelayInDays = state.TimeDelayInDays;
            Key = state.Key;
            TargetAudience = state.TargetAudience;
        }
    }
    
    public class TutorialTipState
    {
        public string Key { get; }

        public TutorialTipTargetAudience TargetAudience { get; set; }

        public int TimeDelayInDays { get; set; }
    }
}
