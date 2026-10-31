#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.TargetCountContributions, 20)]
    internal RuleQueryContribution? ShenwuJuyuanTargetCountContribution(ProgramTargetCountQuery query) => ShenwuJuyuanExtraTarget(query.Actor, query.Kind) ? new FiniteRuleQueryContribution($"turn:{(query.Actor.Seat)}:shenwu:juyuan", SkillRuleOperation.Add, 1) : null;
    internal int ShenwuReplacementDrawBonus(int seat) => _coreShenwuActivitiesOperations.Value.ShenwuBattleRules?.ReplacementDrawBonus(seat) ?? 0;
    internal bool ShenwuZhaohuoDisablesArmor(int targetSeat) => _coreMatchParticipantsPlayersAccess._players.Count > _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && targetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && _coreShenwuNpcProgramsOperations.ShenwuEnabled(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], "shenwu:zhaohuo");
    internal bool ShenwuSipuForbidsAction(int actorSeat)
    {
        if (actorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ShenwuFirstTwoPlayCardsProhibitOthers))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(history, fact => fact is PhaseChangedEvent { Phase: TurnPhase.Play } phase && phase.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        return start >= 0 && history.Skip(start + 1).OfType<CardUseDeclaredEvent>().Count(use => use.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) <= 2;
    }

    internal bool ShenwuJuyuanExtraTarget(CharacterState player, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == player.Seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ShenwuPreviousEndHealthSlashTarget) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShenwuTurnEndHealthEvent>().LastOrDefault(fact => fact.OwnerSeat == player.Seat)is { } previous && player.Hp < previous.Hp;

    internal bool IsShenwuHonglianHandExempt(CharacterState owner, Card card) => _contentRegistryOperations._contentRegistry.Skills.ContainsKey("shenwu:honglian") && _coreShenwuNpcProgramsOperations.ShenwuEnabled(owner, "shenwu:honglian") && CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 80)]
    internal bool ShenwuSipuCardUseForbidden(ProgramCardUseQuery query) => ShenwuSipuForbidsAction(query.ActorSeat);
}
