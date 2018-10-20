using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Functions.Tutorials.Core.Domain
{
    public class Tutorial
    {
        readonly TutorialState _state;

        // Clone the state when fetching it externally
        public TutorialState State =>
            JsonConvert.DeserializeObject<TutorialState>(JsonConvert.SerializeObject(_state));

        public Tutorial(string userId) : this(new TutorialState())
        {
            _state.UserId = userId;
        }

        public Tutorial(TutorialState state)
        {
            _state = state;
        }
        
        public void SyncTutorialTips(params TutorialTip[] tutorialTips)
        {
            // Add new
            foreach (var tutorialTip in tutorialTips)
            {
                if (!_state.AllTutorialTips.Any(tt => tt.Key == tutorialTip.Key))
                {
                    _state.AllTutorialTips.Add(new UserTutorialTipState
                    {
                        DateCreated = DateTime.UtcNow,
                        Key = tutorialTip.Key,
                        Target = tutorialTip.TargetAudience,
                        TimeDelayInDays = tutorialTip.TimeDelayInDays
                    });
                }
            }

            // Remove old
            var toRemoveKeys = new List<string>();
            foreach (var userTutorialTip in _state.AllTutorialTips)
            {
                if (!tutorialTips.Any(tt => tt.Key == userTutorialTip.Key))
                {
                    toRemoveKeys.Add(userTutorialTip.Key);
                }
            }

            _state.AllTutorialTips.RemoveAll(tt => toRemoveKeys.Contains(tt.Key));
        }

        public List<UserTutorialTip> GetTutorialTips()
        {
            var unread = _state.AllTutorialTips
                .Where(tt => !_state.ReadUserTutorialTips.Contains(tt.Key));

            var applicable = unread
                .Where(tt => tt.Target == TutorialTipTargetAudience.AllUsers
                    || (tt.Target == TutorialTipTargetAudience.NewUsers && tt.DateCreated < _state.DateCreated)
                    || (tt.Target == TutorialTipTargetAudience.ExistingUsers && tt.DateCreated > _state.DateCreated));

            return applicable.Select(tt => new UserTutorialTip(
                tt.Key,
                _state.DateCreated.AddDays(tt.TimeDelayInDays) > DateTime.UtcNow))
                .ToList();
        }

        public void ReadTutorialTip(string key)
        {
            var tutorialTip = _state.AllTutorialTips.SingleOrDefault(tt => tt.Key == key);

            if (tutorialTip == null)
            {
                throw new ArgumentException($"Tutorial tip with key '{key}' does not exist");
            }

            _state.ReadUserTutorialTips.Add(key);
        }
    }

    public class TutorialState
    {
        [JsonProperty("id")]
        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public List<UserTutorialTipState> AllTutorialTips { get; set; }

        public List<string> ReadUserTutorialTips { get; set; }

        public TutorialState()
        {
            AllTutorialTips = new List<UserTutorialTipState>();
            ReadUserTutorialTips = new List<string>();
        }
    }

    public class UserTutorialTipState
    {
        [JsonProperty("id")]
        public string Key { get; set; }

        public DateTime DateCreated { get; set; }

        public int TimeDelayInDays { get; set; }

        public TutorialTipTargetAudience Target { get; set; }
    }
}
