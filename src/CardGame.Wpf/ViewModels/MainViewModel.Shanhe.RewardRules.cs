using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool EnableCurrentShanheRewardRules()
    {
        if (_shanheSession is not { HasBalancedJourneyRewards: false } session ||
            session.Snapshot.Phase is not (ShanheCampaignPhase.Map or ShanheCampaignPhase.NodeChoice) ||
            session.PendingBattleSetup is not null) return false;
        var result = session.Submit(new EnableShanheBalancedRewardsCommand(session.Revision));
        return result.Accepted;
    }
}
