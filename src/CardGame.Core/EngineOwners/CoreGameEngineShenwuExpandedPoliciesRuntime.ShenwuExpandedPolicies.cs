#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.TargetCountContributions, 20)]
    internal RuleQueryContribution? ShenwuJuyuanTargetCountContribution(ProgramTargetCountQuery query) => ShenwuJuyuanExtraTarget(query.Actor, query.Kind) ? new FiniteRuleQueryContribution($"turn:{(query.Actor.Seat)}:shenwu:juyuan", SkillRuleOperation.Add, 1) : null;
    internal int ShenwuReplacementDrawBonus(int seat) => _coreShenwuActivitiesOperations.ShenwuBattleRules?.ReplacementDrawBonus(seat) ?? 0;
    internal long CaptureShenwuFengdongStamp()
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey("shenwu:fengdong") || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) || _coreMatchParticipantsPlayersAccess._players.Count <= _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return 0;
        var current = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
        var hasSource = current.SkillGrants.Grants.Any(grant => grant.SkillId == "shenwu:fengdong");
        var dependencies = hasSource ? (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, current.SkillGrants.Revision, current.IsAlive, current.GeneralRevealed) : (0, -1, TurnPhase.NotStarted, 0L, false, false);
        if (_coreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongDependencies != dependencies)
        {
            _coreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongDependencies = dependencies;
            _coreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongStamp++;
        }

        return _coreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess._shenwuFengdongStamp;
    }

    internal bool ShenwuFengdongQualifiesGrant(CharacterState owner, SkillGrant grant) => !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ShenwuOwnTurnFreezeNonLocked) || owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is TurnPhase.NotStarted or TurnPhase.Finished || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ShenwuOwnTurnFreezeNonLocked);
    internal bool ShenwuZhaohuoDisablesArmor(int targetSeat) => _coreMatchParticipantsPlayersAccess._players.Count > _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && targetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && _coreShenwuNpcProgramsOperations.ShenwuEnabled(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], "shenwu:zhaohuo");
    internal bool ShenwuSipuForbidsAction(int actorSeat)
    {
        if (actorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ShenwuFirstTwoPlayCardsProhibitOthers))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(history, fact => fact is PhaseChangedEvent { Phase: TurnPhase.Play } phase && phase.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        return start >= 0 && history.Skip(start + 1).OfType<CardUseDeclaredEvent>().Count(use => use.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) <= 2;
    }

    internal bool ShenwuJuyuanExtraTarget(CharacterState player, CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == player.Seat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.ShenwuPreviousEndHealthSlashTarget) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShenwuTurnEndHealthEvent>().LastOrDefault(fact => fact.OwnerSeat == player.Seat)is { } previous && player.Hp < previous.Hp;

    internal bool IsShenwuHonglianHandExempt(CharacterState owner, Card card) => _contentRegistryOperations._contentRegistry.Skills.ContainsKey("shenwu:honglian") && _coreShenwuNpcProgramsOperations.ShenwuEnabled(owner, "shenwu:honglian") && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 80)]
    internal bool ShenwuSipuCardUseForbidden(ProgramCardUseQuery query) => ShenwuSipuForbidsAction(query.ActorSeat);
}
