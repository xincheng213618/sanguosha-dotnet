namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private sealed partial record RunState
    {
        public IReadOnlyList<string> AllyRankRebirthUsedKeys { get; init; } = Array.Empty<string>();
    }

    private RunState ApplyAutomaticTeamRankSet(RunState state)
    {
        if (Definition.TeamRankSetRewardId is not { } setId || state.TacticIds.Contains(setId, StringComparer.Ordinal)) return state;
        var owned = state.TacticIds.Select(id => _rewards[id]).Where(reward => reward.TacticSchoolId == ShanheTeamTactics.SchoolId).ToArray();
        // Thresholds are per printed quality. A higher-quality tactic does not
        // silently replace one of the explicitly required common/rare members.
        if (owned.Count(reward => reward.Rarity == 1) < 2 || owned.Count(reward => reward.Rarity == 2) < 2 ||
            owned.Count(reward => reward.Rarity == 3) < 1) return state;
        return state with { TacticIds = ShanheCampaignContent.Freeze(state.TacticIds.Append(setId)) };
    }

    private int GetSharedAllyRank(RunState state) => Definition.TeamRankSetRewardId is { } setId &&
        state.TacticIds.Contains(setId, StringComparer.Ordinal) ? Math.Max(3, _options.PlayerRank ?? 1) : 0;

    private string AllyRankRebirthKey(RunState state, string encounterId, ShanheCombatantDefinition member, int index) =>
        state.StartingCompanions.Concat(state.RecruitedCompanions).Any(companion => companion.GeneralId == member.GeneralId)
            ? "companion:" + member.GeneralId
            : "encounter:" + encounterId + ":" + index.ToString(System.Globalization.CultureInfo.InvariantCulture) + ":" + member.GeneralId;

    private ShanheEncounterDefinition ApplyTeamRankGrowth(ShanheEncounterDefinition encounter, RunState state)
    {
        var rank = GetSharedAllyRank(state);
        if (rank == 0 || encounter.Allies is not { Count: > 0 } allies) return encounter;
        var bonuses = ShanheGeneralRanks.GetBonuses(rank);
        return encounter with { Allies = ShanheCampaignContent.Freeze(allies.Select((member, index) =>
        {
            var extra = member.ExtraSkillIds ?? [];
            var rebirth = rank == 5 && !state.AllyRankRebirthUsedKeys.Contains(
                AllyRankRebirthKey(state, encounter.Id, member, index), StringComparer.Ordinal);
            return member with
            {
                MaxHpBonus = checked(member.MaxHpBonus + bonuses.MaxHp),
                Hp = member.Hp is { } hp ? checked(hp + bonuses.MaxHp) : null,
                InitialHandBonus = checked(member.InitialHandBonus + bonuses.InitialHand),
                DrawBonus = checked(member.DrawBonus + bonuses.Draw),
                SlashBonus = checked(member.SlashBonus + bonuses.Slash),
                ExtraSkillIds = rebirth ? ShanheCampaignContent.Freeze(extra.Append(ShanheTeamTactics.AllyRebirthSkillId).Distinct(StringComparer.Ordinal)) : extra
            };
        })) };
    }

    private RunState ApplyTeamRebirthUsage(RunState state, GameEngine battle, ShanheBattleSetup setup)
    {
        if (setup.SharedAllyRank != 5 || state.EncounterId is not { } encounterId) return state;
        var used = battle.Events.Select(envelope => envelope.Payload).OfType<SkillUsageConsumedEvent>()
            .Where(usage => usage.SkillId == ShanheTeamTactics.AllyRebirthSkillId && usage.Scope == SkillUsageScope.Game &&
                usage.Count > 0 && usage.SkillOwnerSeat > 0 && usage.SkillOwnerSeat <= setup.AllyRoster.Count)
            .Select(usage => AllyRankRebirthKey(state, encounterId, setup.AllyRoster[usage.SkillOwnerSeat - 1], usage.SkillOwnerSeat - 1));
        return state with { AllyRankRebirthUsedKeys = ShanheCampaignContent.Freeze(state.AllyRankRebirthUsedKeys.Concat(used).Distinct(StringComparer.Ordinal)) };
    }
}
