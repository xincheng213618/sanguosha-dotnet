namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public bool BalancedRewardsEnabled { get; init; }
    }

    public bool HasBalancedJourneyRewards => BalancedJourneyRewards(_state);

    private bool BalancedJourneyRewards(RunState state) =>
        _options.UseBalancedJourneyRewards || state.BalancedRewardsEnabled;

    private RunState EnableBalancedJourneyRewards()
    {
        Require(!HasBalancedJourneyRewards, "The expedition already uses the current reward rules.");
        Require((_state.Phase is ShanheCampaignPhase.Map or ShanheCampaignPhase.NodeChoice) &&
            _state.BattleSetup is null, "Finish the current battle and its rewards before updating future rewards.");
        // Append a command after replaying the old journal. Never reinterpret
        // historical story choices or reroll a published battle reward group.
        return ReconcileBalancedShopStock(_state with { BalancedRewardsEnabled = true });
    }

    private ShanheNodeDefinition BalancedBattleRewards(RunState state, ShanheNodeDefinition node) =>
        BalancedJourneyRewards(state) && (node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle) &&
        node.BossRewards is null && node.PrimaryTacticRarity < 2
            ? node with { PrimaryTacticRarity = 2 } : node;
}
