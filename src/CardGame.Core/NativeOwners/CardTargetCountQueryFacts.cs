#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct CardTargetCountQueryFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundFirstSlashTargetsEntries> getCoreGameEngineRoundFirstSlashTargetsRuntime, global::System.Func<global::CardGame.Core.TurnCardUseEffectStore> getTurnCardUseEffectStore, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.ProgramTargetRules> getProgramTargetRules, CharacterState player, CardKind effectiveCardKind, long? declaredUseFrameId) : IMatchCardTargetCountQuery
{
    public CardKind EffectiveCardKind => effectiveCardKind;
    public IReadOnlyList<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.CardTargetCount, effectiveCardKind);
    public IEnumerable<RuleQueryContribution> FirstSlashContributions => getCoreGameEngineRoundFirstSlashTargetsRuntime().RoundFirstSlashTargetContributions(player, effectiveCardKind, declaredUseFrameId);
    public IReadOnlyList<TurnRuleModifier> TurnModifiers => getTurnCardUseEffectStore().GetRuleModifiers(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, player.Seat, SkillRuleQuery.CardTargetCount, effectiveCardKind);

    public RuleQueryContribution[] ProjectContributions(RuleQueryContribution[] contributions) => getProgramTargetRules().AppendTargetCount(new(player, effectiveCardKind), contributions);
}
