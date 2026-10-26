namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public IReadOnlyList<ShanheCombatantDefinition> StartingCompanions { get; init; } = Array.Empty<ShanheCombatantDefinition>();
    }

    private RunState GrantOpeningCompanion(RunState state, ShanheRewardDefinition reward, bool atExpeditionStart)
    {
        if (reward.OpeningCompanionPool is not { } pool) return state;
        Require(atExpeditionStart && state.StartingCompanions.Count + state.RecruitedCompanions.Count < 6 &&
            !state.TacticIds.Contains(reward.Id, StringComparer.Ordinal),
            "An opening companion tactic must be chosen once before the expedition.");
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
            Definition.Id + ":companion:" + reward.Id, 0));
        var member = pool[random.Next(pool.Count)];
        Require(!state.StartingCompanions.Any(existing => existing.GeneralId == member.GeneralId),
            "Opening companion tactics cannot grant the same companion twice.");
        return state with { StartingCompanions = ShanheCampaignContent.Freeze(state.StartingCompanions.Append(member)) };
    }

    private ShanheEncounterDefinition WithOpeningCompanion(ShanheEncounterDefinition encounter, ShanheNodeDefinition node)
    {
        if (_state.StartingCompanions.Count == 0) return encounter;
        var existing = encounter.Allies ?? [];
        var added = _state.StartingCompanions.Where(member => !existing.Any(ally => ally.GeneralId == member.GeneralId)).ToArray();
        if (added.Length == 0) return encounter;
        // Preserve the authored roster and fill only the remaining seats, in grant order.
        var admitted = added.Take(Math.Max(0, 7 - encounter.Enemies.Count - existing.Count)).ToArray();
        return encounter with
        {
            Allies = ShanheCampaignContent.Freeze(admitted.Concat(existing)),
            StoryText = admitted.Length == added.Length ? encounter.StoryText :
                (encounter.StoryText ?? node.StoryText) + (admitted.Length == 0
                    ? "\n\n本场席位已满，开局队友暂不参战。" : "\n\n本场席位已满，部分开局队友暂不参战。")
        };
    }
}
