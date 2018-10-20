namespace Functions.Tutorials.Core.Domain
{
    public class UserTutorialTip
    {
        public string Key { get; }

        public bool IsActive { get; }

        public UserTutorialTip(string key, bool isActive)
        {
            Key = key;
            IsActive = isActive;
        }
    }
}
