#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct DistanceQueryFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<ModeRuleSet> getModeRuleSet, Func<RuntimeEntries.CoreNativeRuleQueriesEntries> getCoreNativeRuleQueriesRuntime, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<ProgramDistanceRules> getProgramDistanceRules, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<TurnCardUseEffectStore> getTurnCardUseEffectStore, EngineStates.CoreActualTurnProgressState actualTurnProgressState, CharacterState source, CharacterState target) : IMatchDistanceQuery
{
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public int? FixedDistance => getModeRuleSet().FixedDistance;
    public int SourceSeat => source.Seat;
    public int TargetSeat => target.Seat;

    public int AliveSeatDistance() => getCoreNativeRuleQueriesRuntime().GetAliveSeatDistance(source.Seat, target.Seat);
    public IReadOnlyList<Card> SourceEquipment => getCharacterHuCheErMouRuntime().KongwuEffectiveEquipment(source);
    public IReadOnlyList<Card> TargetEquipment => getCharacterHuCheErMouRuntime().KongwuEffectiveEquipment(target);

    public EquipmentDefinition EquipmentDefinition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
    public void AddBaseTerms(List<RuleQueryBaseTerm> terms) => getProgramDistanceRules().AddDistanceBaseTerms(new(source, target), terms);
    public IEnumerable<RuleQueryContribution> OutgoingContributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(source, SkillRuleQuery.OutgoingDistance);
    public IReadOnlyList<TurnRuleModifier> OutgoingTurnModifiers => getTurnCardUseEffectStore().GetRuleModifiers(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, source.Seat, SkillRuleQuery.OutgoingDistance);
    public IEnumerable<RuleQueryContribution> IncomingContributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(target, SkillRuleQuery.IncomingDistance);

    public void AddIncomingContributions(List<RuleQueryContribution> contributions) => getProgramDistanceRules().AddIncomingContributions(target.Seat, contributions);
    public RuleQueryEvaluation Project(RuleQueryEvaluation evaluation) => getProgramDistanceRules().ProjectDistance(source, target, evaluation);
}
