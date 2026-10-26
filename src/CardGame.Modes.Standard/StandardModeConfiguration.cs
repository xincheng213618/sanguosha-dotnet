using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Standard;

public sealed partial class StandardModeSetupFactory
{
    public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase)
    {
        if (phase == ModeConfigurationValidationPhase.Adapter)
        {
            if (options.PlayerCount is not (4 or 5 or 6 or 8) &&
                !(options.ModeId == WushuangMode.ModeId && options.PlayerCount == 2) &&
                !(options.ShanheBattle is not null && options.PlayerCount is >= 2 and <= 8))
                throw new ArgumentOutOfRangeException(nameof(options.PlayerCount),
                    "The built-in adapters currently support 4, 5, 6, or 8 players.");
        }
        else if (phase == ModeConfigurationValidationPhase.BeforeModeResolution)
        {
            if (options.HumanGuanduRole is { } role && (!Enum.IsDefined(role) || options.HumanSeat < 0))
                throw new ArgumentException("A Guandu role requires a valid human seat and role.", nameof(options.HumanGuanduRole));
        }
        else throw new ArgumentOutOfRangeException(nameof(phase));
    }

    public string? ResolveDefaultModeId(GameOptions options, ContentRegistry registry) =>
        options.PlayerCount == 4 ? "team:standard-2v2" : $"identity:standard-{options.PlayerCount}";
}

/// <summary>Selected-mode qualification, executed before any setup plan is applied.</summary>
public static class StandardModeConfigurationRules
{
    public static void ValidateOptions(GameOptions options, ContentModeDefinition mode, ContentRegistry registry)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(mode);
        ArgumentNullException.ThrowIfNull(registry);
        ValidateModeOptions(options, mode);
        ValidateShanheBattleOptions(options, registry, mode);
    }

    private static void ValidateModeOptions(
        GameOptions options,
        ContentModeDefinition mode)
    {
        if (options.HumanSeats is not null && (mode.ModeKind != ContentModeKind.Identity || options.ShanheBattle is not null))
            throw new ArgumentException("Multiplayer currently supports identity tables only.", nameof(options));
        if (options.HumanGuanduRole is not null && mode.GuanduActivity is null)
            throw new ArgumentException("Guandu role is only valid in Guandu.", nameof(options.HumanGuanduRole));
        if (options.WenheAspirationRank is { } goal &&
            (mode.WenheActivity is null || options.HumanSeat < 0 || goal is not (1 or 2 or 4 or 6)))
            throw new ArgumentException("Wenhe aspiration requires a human seat in Wenhe and a rank of 1, 2, 4 or 6.", nameof(options));
        if (mode.ModeKind == ContentModeKind.Survival)
        {
            if (options.HumanRole is not null || options.HumanTeamId is not null)
                throw new ArgumentException("Independent survival contestants have neither identity choices nor teams.", nameof(options));
            return;
        }
        if (mode.ModeKind == ContentModeKind.Team)
        {
            if (options.HumanRole is not null)
            {
                throw new ArgumentException(
                    "HumanRole must be null in a public-team mode.",
                    nameof(options.HumanRole));
            }

            if (mode.TeamCounts is null || mode.TeamCounts.Count != (mode.DragonBoatActivity is null ? 2 : 4) ||
                mode.TeamCounts.Values.Sum() != options.PlayerCount)
            {
                throw new InvalidOperationException(
                    $"Mode '{mode.Id}' is not supported by the two-team adapter for {options.PlayerCount} players.");
            }

            if (options.HumanSeat < 0 && options.HumanTeamId is not null)
            {
                throw new ArgumentException(
                    "HumanTeamId requires a human seat.",
                    nameof(options.HumanTeamId));
            }

            if (options.HumanTeamId is not null &&
                !mode.TeamCounts.ContainsKey(options.HumanTeamId))
            {
                throw new ArgumentException(
                    $"Team '{options.HumanTeamId}' is not defined by mode '{mode.Id}'.",
                    nameof(options.HumanTeamId));
            }
            if (mode.BossActivity is { } boss && options.HumanTeamId is { } selectedTeam && selectedTeam != boss.PlayerTeamId)
                throw new ArgumentException("The local player must join the allied team in a boss activity.", nameof(options.HumanTeamId));
            if (mode.ShenwuActivity is { } shenwu && options.HumanTeamId is { } selectedShenwuTeam && selectedShenwuTeam != shenwu.PlayerTeamId)
                throw new ArgumentException("The local player must join the allied team in Shenwu.", nameof(options.HumanTeamId));
        }
        else if (mode.ModeKind == ContentModeKind.NationalWarLite)
        {
            if (options.HumanRole is not null)
            {
                throw new ArgumentException(
                    "HumanRole must be null in a national-war mode.",
                    nameof(options.HumanRole));
            }

            if (options.HumanTeamId is not null)
            {
                throw new ArgumentException(
                    "HumanTeamId is not valid in a national-war mode.",
                    nameof(options.HumanTeamId));
            }

            if (mode.FactionCounts is null ||
                mode.FactionCounts.Count < 2 ||
                mode.FactionCounts.Values.Sum() != options.PlayerCount)
            {
                throw new InvalidOperationException(
                    $"Mode '{mode.Id}' must define at least two factions totaling {options.PlayerCount} players.");
            }
        }
        else if (options.HumanTeamId is not null)
        {
            throw new ArgumentException(
                "HumanTeamId is only valid in a public-team mode.",
                nameof(options.HumanTeamId));
        }
    }

    private static void ValidateShanheBattleOptions(GameOptions options, ContentRegistry registry, ContentModeDefinition mode)
    {
        var isCampaignMode = mode.Id == ShanheBattleSetup.BattleModeId;
        if (options.ShanheBattle is not { } setup)
        {
            if (isCampaignMode) throw new ArgumentException("A Shanhe encounter requires its campaign opening recipe.");
            return;
        }
        GodFactionSelectionRules.ValidateDefault(setup.DefaultGodFactionId);
        if (setup.TerrainSkillId is { } terrain && (!registry.Skills.TryGetValue(terrain, out var terrainSkill) ||
            terrainSkill.ImplementationStatus != SkillImplementationStatus.Complete || terrainSkill.Program is null))
            throw new ArgumentException("The Shanhe terrain requires complete registered content.");
        _ = ShanheGeneralRanks.GetBonuses(setup.PlayerRank ?? 1);
        if (setup.SharedAllyRank != 0 && setup.SharedAllyRank != Math.Max(3, setup.PlayerRank ?? 1))
            throw new ArgumentException("Shared ally rank must match the expedition rank.");
        if (setup.ExtraSkillIds.Contains(ShanheTeamRankPrograms.SharedRebirthSkillId, StringComparer.Ordinal) ||
            setup.TacticSkillIds.Contains(ShanheTeamRankPrograms.SharedRebirthSkillId, StringComparer.Ordinal) ||
            setup.EnemyRoster.Concat(setup.AllyRoster).Any(member =>
                (member.ExtraSkillIds ?? []).Contains(ShanheTeamRankPrograms.SharedRebirthSkillId, StringComparer.Ordinal)))
            throw new ArgumentException("Allied rank rebirth must be granted by the actual qualified school initialization.");
        if (setup.RebirthAvailable && setup.PlayerRank != 5 ||
            setup.RebirthAvailable != setup.ExtraSkillIds.Contains(ShanheGeneralRanks.RebirthSkillId, StringComparer.Ordinal) ||
            setup.TacticSkillIds.Contains(ShanheGeneralRanks.RebirthSkillId, StringComparer.Ordinal))
            throw new ArgumentException("The Shanhe rebirth skill must match its fifth-rank expedition allowance.");
        var playerCount = 1 + setup.EnemyRoster.Count + setup.AllyRoster.Count;
        if (!isCampaignMode || mode.ModeKind != ContentModeKind.Team || options.Seed != setup.BattleSeed ||
            options.PlayerCount != playerCount || options.HumanSeat is not (0 or -1) || options.UseInteractiveSetup ||
            mode.DualGenerals || mode.BossActivity is not null || mode.TeamActivity is not null ||
            mode.ShenwuActivity is not null || mode.NianActivity is not null || mode.HellActivity is not null ||
            mode.WenheActivity is not null || mode.DragonBoatActivity is not null ||
            options.HumanRole is not null || options.HumanTeamId is not (null or ShanheBattleSetup.PlayerTeamId) ||
            mode.TeamCounts is null || mode.TeamCounts.Count != 2 ||
            mode.TeamCounts.GetValueOrDefault(ShanheBattleSetup.PlayerTeamId) != 1 + setup.AllyRoster.Count ||
            mode.TeamCounts.GetValueOrDefault(ShanheBattleSetup.EnemyTeamId) != setup.EnemyRoster.Count ||
            setup.EnemyRoster.Count == 0 || setup.EncounterOrdinal < 1 || string.IsNullOrWhiteSpace(setup.NodeId) ||
            string.IsNullOrWhiteSpace(setup.CampaignHash))
            throw new ArgumentException("The Shanhe opening recipe does not match this encounter mode.");

        ValidateMember(new(setup.PlayerGeneralId, setup.MaxHpBonus, setup.Hp, setup.InitialHandBonus,
            setup.DrawBonus, setup.SlashBonus, setup.ExtraSkillIds), isPlayer: true);
        var tacticIdentities = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in setup.TacticSkillIds)
        {
            if (string.IsNullOrWhiteSpace(id) || !registry.Skills.TryGetValue(id, out var skill) ||
                skill.ImplementationStatus != SkillImplementationStatus.Complete || skill.Program is null)
                throw new ArgumentException("Shanhe tactics require complete registered skill programs.");
            var acquisition = skill.CampaignTacticAcquisition ?? new CampaignTacticAcquisitionRule(skill.Id);
            if (!tacticIdentities.Add(acquisition.IdentityId) && !acquisition.Repeatable)
                throw new ArgumentException("A unique campaign tactic cannot be acquired more than once.");
        }
        foreach (var member in setup.EnemyRoster) ValidateMember(member);
        foreach (var member in setup.AllyRoster) ValidateMember(member, isAlly: true);
        if (setup.BossTransition is { } transition &&
            (transition.EnemyIndex < 0 || transition.EnemyIndex >= setup.EnemyRoster.Count ||
             !registry.Generals.ContainsKey(transition.GeneralId) || !registry.IsGeneralPlayable(transition.GeneralId) ||
             mode.GeneralPoolIds?.Contains(transition.GeneralId, StringComparer.Ordinal) != true ||
             transition.MaxHp is < 1 or > 1000 || transition.Hp < 1 || transition.Hp > transition.MaxHp ||
             transition.DrawCards is < 0 or > 40))
            throw new ArgumentException("The Shanhe second form must be a reserved playable general with bounded health and draw count.");
        if (setup.BossTransition is { Combatant: { } stage } sourceTransition)
        {
            if (stage.GeneralId != sourceTransition.GeneralId) throw new ArgumentException("The source second-form numeric recipe has a different general.");
            if (stage.StartingEquipment is { Count: > 0 }) throw new ArgumentException("A reserved form must declare equipment on the boss transition, not its numeric combatant.");
            ValidateMember(stage);
        }
        foreach (var card in setup.StartingCards.Concat(setup.StartingEquipment))
        {
            if (!Enum.IsDefined(card.Kind) || !Enum.IsDefined(card.Suit) || card.Suit == Suit.None ||
                card.Rank is < 1 or > 13 || !registry.Cards.Values.Any(c => c.LegacyKind == card.Kind))
                throw new ArgumentException("A Shanhe starting card is not registered physical content.");
        }
        if (setup.StartingCards.Count > 24 || setup.StartingEquipment.Count > 5 ||
            setup.StartingEquipment.Any(c => !EquipmentCatalog.IsEquipment(c.Kind)) ||
            setup.StartingEquipment.Select(c => EquipmentCatalog.Get(c.Kind).Slot).Distinct().Count() != setup.StartingEquipment.Count)
            throw new ArgumentException("Shanhe starting equipment needs one registered card per equipment slot.");

        void ValidateMember(ShanheCombatantDefinition member, bool isPlayer = false, bool isAlly = false)
        {
            if (!registry.Generals.TryGetValue(member.GeneralId, out var general) ||
                !registry.IsGeneralPlayable(member.GeneralId) || mode.GeneralPoolIds?.Contains(member.GeneralId) != true ||
                member.MaxHpBonus is < 0 or > 40 || member.Hp is <= 0 || member.Hp > general.BaseHp + member.MaxHpBonus ||
                member.InitialHandBonus is < 0 or > 20 || member.DrawBonus is < 0 or > 10 || member.SlashBonus is < 0 or > 10 ||
                member.InitialArmor is < 0 or > 1000 || member.ArmorRecoveryPerRound is < 0 or > 1000 ||
                member.ArmorRecoveryCap is < 0 or > 1000 || member.ExtraDamage is < 0 or > 1000 ||
                (member.ExtraSkillIds?.Count ?? 0) > 8 + (isPlayer && setup.RebirthAvailable || isAlly && setup.SharedAllyRank == 5 ? 1 : 0) ||
                !isPlayer && (member.ExtraSkillIds ?? []).Contains(ShanheGeneralRanks.RebirthSkillId, StringComparer.Ordinal) ||
                (member.ExtraSkillIds ?? []).Distinct(StringComparer.Ordinal).Count() != (member.ExtraSkillIds?.Count ?? 0) ||
                (member.ExtraSkillIds ?? []).Any(id => !registry.Skills.TryGetValue(id, out var skill) ||
                    skill.ImplementationStatus != SkillImplementationStatus.Complete))
                throw new ArgumentException("A Shanhe combatant has invalid general, health, bonuses or skills.");
            ValidateShanhePrintedLordQualifications(member, general, registry);
            foreach (var cards in new[] { member.StartingHand, member.StartingCards, member.StartingEquipment })
                if (cards is { } hand && (hand.Count > 24 || hand.Any(card => card is null ||
                        !Enum.IsDefined(card.Kind) || !Enum.IsDefined(card.Suit) || card.Suit == Suit.None ||
                        card.Rank is < 1 or > 13 || !registry.Cards.Values.Any(c => c.LegacyKind == card.Kind))))
                    throw new ArgumentException("A Shanhe combatant has invalid fixed physical opening cards.");
            if (member.StartingEquipment is { } equipment && (equipment.Count > 5 ||
                equipment.Any(card => !EquipmentCatalog.IsEquipment(card.Kind)) ||
                equipment.Select(card => EquipmentCatalog.Get(card.Kind).Slot).Distinct().Count() != equipment.Count))
                throw new ArgumentException("Shanhe combatant starting equipment needs one registered card per equipment slot.");
        }
    }

    private static void ValidateShanhePrintedLordQualifications(ShanheCombatantDefinition member,
        ContentGeneralDefinition general, ContentRegistry registry)
    {
        if (member.QualifiedPrintedLordSkillIds is not { } requested) return;
        if (requested.Distinct(StringComparer.Ordinal).Count() != requested.Count || requested.Any(id =>
                string.IsNullOrWhiteSpace(id) || !general.SkillIds.Contains(id, StringComparer.Ordinal) ||
                !registry.Skills.TryGetValue(id, out var skill) || !skill.Tags.HasFlag(SkillTag.Lord) ||
                skill.ImplementationStatus != SkillImplementationStatus.Complete || skill.Program is null))
            throw new ArgumentException("Scenario Lord qualification requires distinct complete Lord skills printed on the original primary general.");
    }

}

internal sealed partial class StandardModeSetupFeature : IMatchModeConfigurationValidator
{
    public void ValidateOptions() => StandardModeConfigurationRules.ValidateOptions(_options, _mode, _registry);
}
