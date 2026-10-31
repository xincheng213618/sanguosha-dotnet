namespace CardGame.Core;

// Owns detached observer values. This pure boundary never reads or changes live match state.
internal static class PlayerViewFreezer
{
    // Freeze detached projection values once, before the command commit exposes them
    // to both its result and every observer. Never freeze live content/state in place.
    internal static GameSnapshot Freeze(GameSnapshot snapshot) => snapshot with
    {
        TableOrder = FreezeViewList(snapshot.TableOrder),
        BenevolenceCards = FreezeViewList(snapshot.BenevolenceCards),
        CardDeclarations = snapshot.CardDeclarations is { } declarations
            ? Array.AsReadOnly(declarations.Select(item => item with { TargetSeats = FreezeViewList(item.TargetSeats)! }).ToArray()) : null,
        Players = Array.AsReadOnly(snapshot.Players.Select(FreezePlayer).ToArray()),
        PendingDecision = snapshot.PendingDecision is { } decision ? RuntimeSkills.RuntimeRules_Core_CommonRules.CloneDecision(decision) : null,
        PublicRevealedCards = FreezeViewList(snapshot.PublicRevealedCards)!,
        PrivateRevealedCards = FreezeViewList(snapshot.PrivateRevealedCards),
        FriendPredictions = snapshot.FriendPredictions is { } forecasts
            ? Array.AsReadOnly(forecasts.Select(forecast => forecast with { Values = FreezeViewList(forecast.Values)! }).ToArray()) : null,
        ProgramResponseExchangeStates = FreezeViewList(snapshot.ProgramResponseExchangeStates),
        BoundarySidiPredictions = FreezeViewList(snapshot.BoundarySidiPredictions),
        TurnProhibitedPhysicalCards = snapshot.TurnProhibitedPhysicalCards is { } restrictions
            ? Array.AsReadOnly(restrictions.Select(item => item with { CardIds = FreezeViewList(item.CardIds)! }).ToArray()) : null
    };

    private static IReadOnlyList<T>? FreezeViewList<T>(IReadOnlyList<T>? values) =>
        values is null ? null : values.Count == 0 ? EmptyViewList<T>.Value : Array.AsReadOnly(values.ToArray());

    private static class EmptyViewList<T>
    {
        internal static readonly IReadOnlyList<T> Value = Array.AsReadOnly(Array.Empty<T>());
    }

    private static IReadOnlyDictionary<TKey, TValue>? FreezeViewDictionary<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue>? values) where TKey : notnull => values is null ? null :
        values.Count == 0 ? System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>.Empty :
            new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(values.ToDictionary(item => item.Key, item => item.Value));

    private static PlayerSnapshot FreezePlayer(PlayerSnapshot player) => player with
    {
        OriginalHandEntities = player.OriginalHandEntities is { } originalHand
            ? Array.AsReadOnly(originalHand.Select(item => item with { CardIds = FreezeViewList(item.CardIds) }).ToArray()) : null,
        SourceCurses = FreezeViewList(player.SourceCurses),
        Hand = FreezeViewList(player.Hand)!,
        Equipment = FreezeViewList(player.Equipment)!,
        Judgment = FreezeViewList(player.Judgment)!,
        PrivateReserveCards = FreezeViewList(player.PrivateReserveCards),
        NamedAmbushPiles = player.NamedAmbushPiles is { } ambushes
            ? Array.AsReadOnly(ambushes.Select(p => p with { Entries = FreezeViewList(p.Entries) }).ToArray()) : null,
        PrivateWhirlPiles = player.PrivateWhirlPiles is { } whirls
            ? Array.AsReadOnly(whirls.Select(p => p with { Cards = FreezeViewList(p.Cards) }).ToArray()) : null,
        Markers = FreezeViewList(player.Markers),
        WushuangTacticIds = FreezeViewList(player.WushuangTacticIds),
        ActiveEffects = FreezeViewList(player.ActiveEffects),
        WoodenOxGrain = FreezeViewList(player.WoodenOxGrain),
        BuquWounds = FreezeViewList(player.BuquWounds),
        AuthorityCards = FreezeViewList(player.AuthorityCards),
        ChunlaoCards = FreezeViewList(player.ChunlaoCards),
        PublicDeferredPileCards = FreezeViewList(player.PublicDeferredPileCards),
        PublicPersistentPileCards = FreezeViewList(player.PublicPersistentPileCards),
        PublicPersistentPiles = player.PublicPersistentPiles is { } piles
            ? Array.AsReadOnly(piles.Select(pile => pile with { Cards = FreezeViewList(pile.Cards)! }).ToArray()) : null,
        PrivateGeneralLibraries = player.PrivateGeneralLibraries is {} libraries ? Array.AsReadOnly(libraries.Select(l => l with { GeneralIds = FreezeViewList(l.GeneralIds) }).ToArray()) : null,
        PrivateTurnHolds = player.PrivateTurnHolds is { } holds ? Array.AsReadOnly(holds.Select(h => h with { Cards = FreezeViewList(h.Cards) }).ToArray()) : null,
        PojunHoldCards = FreezeViewList(player.PojunHoldCards),
        EquipmentSlotCapacities = FreezeViewDictionary(player.EquipmentSlotCapacities),
        ConfiguredConversionTiers = FreezeViewDictionary(player.ConfiguredConversionTiers),
        AlternatingChoiceStates = FreezeViewList(player.AlternatingChoiceStates),
        BeneficiarySuitShields = FreezeViewList(player.BeneficiarySuitShields),
        DeferredHandAlignments = FreezeViewList(player.DeferredHandAlignments),
        TurnHandCategoryRestrictions = FreezeViewList(player.TurnHandCategoryRestrictions),
        TurnHandLimitCardKindExemptions = player.TurnHandLimitCardKindExemptions is { } kindExemptions ?
            Array.AsReadOnly(kindExemptions.Select(policy => policy with
                { CardKinds = FreezeViewList(policy.CardKinds)! }).ToArray()) : null,
        TurnSlashSuitAllowances = FreezeViewList(player.TurnSlashSuitAllowances),
        FirstRoundGameUsageRefunds = FreezeViewList(player.FirstRoundGameUsageRefunds),
        IssuedPlayPhaseUseProhibitions = FreezeViewList(player.IssuedPlayPhaseUseProhibitions),
        IssuedPlayPhaseSuitUseAllowances = player.IssuedPlayPhaseSuitUseAllowances is null ? null :
            Array.AsReadOnly(player.IssuedPlayPhaseSuitUseAllowances.Select(a => a with { Suits = Array.AsReadOnly(a.Suits.ToArray()) }).ToArray()),
        Skills = player.Skills is { } skills ? Array.AsReadOnly(skills.Select(FreezeViewSkill).ToArray()) : null,
        SecondarySkills = player.SecondarySkills is { } secondary ? Array.AsReadOnly(secondary.Select(FreezeViewSkill).ToArray()) : null,
        SkillRuntimeStates = player.SkillRuntimeStates is { } states ? Array.AsReadOnly(states.Select(state => state with
        {
            Usages = FreezeViewList(state.Usages)!,
            BooleanStates = FreezeViewList(state.BooleanStates),
            PublicRuleStates = FreezeViewList(state.PublicRuleStates),
            DirectedPolicies = state.DirectedPolicies is { } policies ? Array.AsReadOnly(policies.Select(policy => policy with
                { CardKinds = FreezeViewList(policy.CardKinds)! }).ToArray()) : null,
            ActionProhibitions = state.ActionProhibitions is { } prohibitions ? Array.AsReadOnly(prohibitions.Select(policy => policy with
            {
                CardKinds = FreezeViewList(policy.CardKinds)!,
                ActionTypes = FreezeViewList(policy.ActionTypes)!,
                Suits = FreezeViewList(policy.Suits)
            }).ToArray()) : null
        }).ToArray()) : null
    };

    private static GeneralSkillDefinition FreezeViewSkill(GeneralSkillDefinition skill) => skill with
    {
        SelectionWeights = FreezeViewDictionary(skill.SelectionWeights),
        ViewAsOpportunities = skill.ViewAsOpportunities is { } opportunities
            ? Array.AsReadOnly(opportunities.Select(item => item with
            {
                InputKinds = FreezeViewList(item.InputKinds)!,
                InputSuits = FreezeViewList(item.InputSuits)!
            }).ToArray()) : null
    };

}
