#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct CardTargetCountQueryFacts(Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime>> getCoreGameEngineRoundFirstSlashTargetsRuntime, Func<TurnCardUseEffectStore> getTurnCardUseEffectStore, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<ProgramTargetRules> getProgramTargetRules, CharacterState player, CardKind effectiveCardKind, long? declaredUseFrameId) : IMatchCardTargetCountQuery
{
    public CardKind EffectiveCardKind => effectiveCardKind;
    public IReadOnlyList<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.CardTargetCount, effectiveCardKind);
    public IEnumerable<RuleQueryContribution> FirstSlashContributions => getCoreGameEngineRoundFirstSlashTargetsRuntime().Value.RoundFirstSlashTargetContributions(player, effectiveCardKind, declaredUseFrameId);
    public IReadOnlyList<TurnRuleModifier> TurnModifiers => getTurnCardUseEffectStore().GetRuleModifiers(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, player.Seat, SkillRuleQuery.CardTargetCount, effectiveCardKind);

    public RuleQueryContribution[] ProjectContributions(RuleQueryContribution[] contributions) => getProgramTargetRules().AppendTargetCount(new(player, effectiveCardKind), contributions);
}
