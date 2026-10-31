#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineIssuedPlayUsePoliciesRuntime
{
    internal bool HasFirstActualPlayUseDistance(CharacterState actor) => _coreActualPlayPhaseCardUsesOperations.TracksActualPlayPhaseCardUses && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(actor.Seat) == 0 && _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.FirstActualPlayUseDistanceUnlimited);
    internal bool HasIssuedFirstPlayUseDistance(long? frameId) => frameId is { } id && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == id)?.FirstOwnPlayUseDistanceUnlimited == true;
    internal bool HasIssuedPlayPhaseUseBan(int seat) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions.Any(policy => policy.ActorSeat == seat && policy.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && policy.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 50)]
    internal void CleanupIssuedPlayPhaseUseBans() => _coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions.RemoveAll(policy => _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || policy.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || policy.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || policy.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal bool HasIssuedPolicyForAction(long actionId) => _coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions.Any(policy => policy.CardActionId == actionId);
    internal bool IsIssuedCardUnrespondable(long frameId) => _characterZhangFeiBoundaryMobileOperations.IsBoundaryZhangFeiLiyongUnrespondable(frameId) || _characterHuangZhongBoundaryMobileOperations.IsBoundaryHuangZhongUnrespondable(frameId) || _characterMaChaoBoundaryMobileOperations.IsBoundaryMaChaoUnrespondable(frameId) || _characterPangDeBoundaryMobileOperations.IsBoundaryPangDeUnrespondable(frameId) || _characterPanZhangMaZhongBoundaryMobileOperations.IsBoundaryAnjianUnrespondable(frameId) || _characterChenDaoMobileShiOperations.IsMobileShiWanglieUnrespondable(frameId) || _characterWeiYanMobileShiOperations.IsMobileShiZhuangshiUnrespondable(frameId) || _characterHuangChongMobileShiOperations.IsShiHcSiliSlashUnrespondable(frameId) || _characterHuangGaiMouMobileOperations.IsMouZhaJiangUnrespondable(frameId) || _characterLvKuangLvXiangOlOperations.IsQigongUnrespondable(frameId) || _coreNonBasicSlashProfileOperations.HasIssuedNonBasicSlashDistance(frameId) || _characterZhangYanOlOperations.IsZhangYanLangdaoUnrespondable(frameId) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == frameId)is { Action: { } action, IssuedNoResponse: { } issued } && issued.CardActionId == action.ActionId && issued.ParentFrameId == frameId;
    void global::CardGame.Core.IIssuedPlayUseProgramHost.IssueCardNoResponseAndPlayUseBan(ProgramSkillFrame frame) => IssueCardNoResponseAndPlayUseBan(frame);
    internal void IssueCardNoResponseAndPlayUseBan(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } window || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame parent || parent.Id != window.ParentFrameId || parent.Action.ActionId != context.CardActionId || parent.Action.ActorSeat != frame.OwnerSeat || window.OwnerSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != frame.OwnerSeat || !_coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Contains(context.CardActionId) || !IsEligibleIssuedNoResponseCard(parent.Action.EffectiveKind) || HasIssuedPolicyForAction(context.CardActionId))
            throw new InvalidOperationException("Issued card policy requires its exact own Play-use committed parent.");
        var isResponse = parent.Action.Type == CardActionType.Response;
        if (isResponse && (parent.CompletedResponseReturn is not { IsCommitted: true } typed || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(parent.Action, typed.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(parent)))
            throw new InvalidOperationException("Issued response policy lost its exact typed use parent.");
        var action = parent.Action;
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId);
        var issued = new IssuedCardNoResponse(action.ActionId, parent.ParentFrameId, source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        if (!isResponse)
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == parent.ParentFrameId);
            if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame use || use.Action?.ActionId != action.ActionId || parent.Continuation is not (ProgramCardContinuation.CommittedSlash or ProgramCardContinuation.CommittedTrick or ProgramCardContinuation.CommittedSimpleCard) || use.IssuedNoResponse is not null)
                throw new InvalidOperationException("Issued card policy lost its actual owning card-use frame.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { IssuedNoResponse = issued });
        }
        else if (parent.CompletedResponseReturn?.Kind == ProgramCompletedResponseKind.Nullification)
        {
            var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("Issued counterspell lost its chain.");
            var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == pending.Id);
            if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not NullificationWindowFrame counter || counter.IssuedNoResponseNode is not null)
                throw new InvalidOperationException("Issued counterspell lost its owning frame.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(counter.Id, counter with { IssuedNoResponseNode = new(action.ActionId, pending.ChainDepth, issued) });
        }

        _coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess._issuedPlayPhaseUseProhibitions.Add(new(source, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.ActionId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(issued));
    }

    internal static bool IsEligibleIssuedNoResponseCard(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_IssuedPlayUsePolicies.IsEligibleIssuedNoResponseCard(kind);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 140)]
    internal bool DistanceCardUseExemption140HasFirstActualPlayUseDistance(ProgramCardDistanceQuery q) => HasFirstActualPlayUseDistance(q.Actor);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 150)]
    internal bool DistanceCardUseExemption150HasIssuedFirstPlayUseDistance(ProgramCardDistanceQuery q) => HasIssuedFirstPlayUseDistance(q.CardUseFrameId);
}
