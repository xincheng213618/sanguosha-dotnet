namespace CardGame.Core.Campaigns;

/// <summary>The user-authorized local completion of the public client calculation.</summary>
public static class ShanheSourceDifficultyRuntime
{
    public const string PolicyId = "client-growth-encounter-day-hp-delta-maxhp-round-two-armor-cap-maxhp";

    internal static void ValidateContract(ShanheCampaignDefinition definition)
    {
        if (definition.SourceDifficultyId is not { } difficulty)
        {
            ShanheCampaignContent.Require(definition.SourceEnemyGrowth is null && definition.SourceEnemyGrowthHash is null && definition.SourceGrowthPolicy is null,
                "A local campaign cannot carry an unattached source execution contract.");
            return;
        }
        var calculator = definition.SourceEnemyGrowth;
        ShanheCampaignContent.Require(calculator is not null && calculator.Difficulties.SeasonId == 5 && calculator.Difficulties.BySourceId.ContainsKey(difficulty) &&
            definition.SourceEnemyGrowthHash == calculator.ContentHash && definition.SourceGrowthPolicy == PolicyId,
            "A source difficulty requires its exact frozen calculator data hash and local completion contract.");
    }
}

public sealed partial class ShanheCampaignSession
{
    private ShanheEncounterDefinition PrepareEncounter(ShanheNodeDefinition node, int ordinal, RunState state)
    {
        var encounter = WithRecruitedCompanions(WithOpeningCompanion(SelectEncounter(node, ordinal), node), node);
        return ApplySourceEnemyGrowth(encounter, node, ordinal, state);
    }

    private ShanheEncounterDefinition ApplySourceEnemyGrowth(ShanheEncounterDefinition encounter, ShanheNodeDefinition node, int ordinal, RunState state)
    {
        if (Definition.SourceDifficultyId is not { } sourceDifficulty) return encounter;
        var prepared = encounter with
        {
            TerrainSkillId = sourceDifficulty >= 11 ? encounter.TerrainSkillId : null,
            HealAfterVictory = sourceDifficulty >= 6 ? 0 : encounter.HealAfterVictory
        };
        // Ordinary 1 deliberately preserves the authored base recipe, including explicit HP and fixed cards.
        if (sourceDifficulty == 1) return prepared;
        Require(node.ChapterNumber is >= 1 and <= 3, "A source battle requires its actual chapter number.");
        var calculator = Definition.SourceEnemyGrowth!;
        var probe = ShanheBattleSetup.Freeze(new(_options.Seed, ordinal, node.Id, _options.PlayerGeneralId,
            state.Hp, state.Bonuses.MaxHp, state.Bonuses.InitialHand, state.Bonuses.Draw, state.Bonuses.Slash,
            state.ExtraSkillIds, state.StartingCards, state.StartingEquipment, prepared.Enemies, prepared.Allies ?? [], ContentHash)
        { DefaultGodFactionId = Definition.DefaultGodFactionId, TerrainSkillId = prepared.TerrainSkillId, BossTransition = prepared.BossTransition });
        var registry = _battleRegistryFactory(probe) ?? throw new InvalidOperationException("The source growth registry is unavailable.");
        var deckId = registry.Modes[ShanheBattleSetup.BattleModeId].DeckId
            ?? throw new InvalidOperationException("A source battle requires its base deck statistics.");
        var deck = registry.Decks[deckId];
        var category = node.Kind == ShanheNodeKind.BossBattle ? ShanheSourceEncounterCategory.Boss :
            node.Kind == ShanheNodeKind.EliteBattle ? ShanheSourceEncounterCategory.Elite : ShanheSourceEncounterCategory.Ordinary;
        var context = new ShanheSourceEnemyGrowthContext(sourceDifficulty, ordinal, node.ChapterNumber,
            prepared.SourceVisibleEnemyCount ?? prepared.Enemies.Count, category);
        ShanheCombatantDefinition Project(ShanheCombatantDefinition member, int? maximumOverride = null, int? hpOverride = null, int? handOverride = null)
        {
            var general = registry.Generals[member.GeneralId];
            var baseMaximum = maximumOverride ?? checked(general.BaseHp + member.MaxHpBonus);
            var baseHp = hpOverride ?? member.Hp ?? general.InitialHp ?? baseMaximum;
            var baseHand = checked((handOverride ?? member.StartingHand?.Count ?? deck.InitialHandSize) + member.InitialHandBonus);
            var basis = new ShanheSourceEnemyAttributes(baseHp, checked(deck.DrawPerTurn + member.DrawBonus), member.SlashBonus,
                member.ArmorRecoveryPerRound, baseHand, member.InitialArmor, member.ExtraDamage);
            var grown = calculator.Calculate(basis, context, member.SourceDifficultyAttributes).Attributes;
            var hpDelta = checked(grown.Hp - baseHp);
            var maximum = checked(baseMaximum + hpDelta);
            var cap = grown.ArmorGain > 0 && member.ArmorRecoveryCap == 0 ? maximum : member.ArmorRecoveryCap;
            return member with
            {
                MaxHpBonus = checked(maximum - general.BaseHp), Hp = grown.Hp,
                InitialHandBonus = checked(member.InitialHandBonus + grown.StartingCards - baseHand),
                DrawBonus = checked(member.DrawBonus + grown.Draw - basis.Draw), SlashBonus = grown.ExtraSlash,
                InitialArmor = grown.Armor, ArmorRecoveryPerRound = grown.ArmorGain, ArmorRecoveryCap = cap, ExtraDamage = grown.ExtraDamage
            };
        }
        var transition = prepared.BossTransition;
        if (transition is { Combatant: { } stage })
        {
            var grown = Project(stage, transition.MaxHp, transition.Hp, transition.DrawCards);
            var stageMaximum = checked(transition.MaxHp + grown.Hp!.Value - transition.Hp);
            // The stage's explicit maximum can be below the printed general's HP. NumericMember does not own that maximum.
            grown = grown with { MaxHpBonus = Math.Max(0, checked(stageMaximum - registry.Generals[grown.GeneralId].BaseHp)) };
            transition = transition with
            {
                MaxHp = stageMaximum, Hp = grown.Hp!.Value,
                DrawCards = checked(transition.DrawCards + grown.InitialHandBonus - stage.InitialHandBonus), Combatant = grown
            };
        }
        return prepared with { Enemies = ShanheCampaignContent.Freeze(prepared.Enemies.Select(member => Project(member))), BossTransition = transition };
    }
}
