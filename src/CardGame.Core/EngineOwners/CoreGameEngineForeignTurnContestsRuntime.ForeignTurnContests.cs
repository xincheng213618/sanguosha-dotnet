#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineForeignTurnContestsRuntime
{
    internal bool TryBeginForeignActualTurnStart(CharacterState turnOwner) => _foreignActualTurnWindowOperations.Get().TryBegin(turnOwner.Seat);
    internal void AdvanceForeignActualTurnStartCandidate(ForeignActualTurnStartWindowFrame parent, bool activated, bool completed) => _foreignActualTurnWindowOperations.Get().AdvanceCandidate(parent, activated, completed);
    internal void CompleteForeignActualTurnStartBinding(ProgramSkillFrame child) => _foreignActualTurnWindowOperations.Get().CompleteBinding(child);
    internal bool IsForeignActualTurnPindian(ProgramSkillFrame frame) => _closedProgramPhaseFrameQueriesSource.Get().IsForeignActualTurnPindian(frame);
    internal ForeignTurnPindianOrigin? FreezeForeignTurnPindianOrigin(ProgramSkillFrame program, PindianFrame pindian) => _foreignTurnContestOperations.Get().FreezeOrigin(program, pindian);
    internal void AssertForeignTurnContest(ProgramSkillFrame frame) => _foreignTurnContestOperations.Get().Assert(frame);
    internal bool CanUseForeignTurnContestSlash(int actor, int target) => _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor, target, Suit.None) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], ignoresCount: true);
    internal ForeignTurnContestSlashIssue CaptureForeignTurnContestSlash(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin)
    {
        var actor = origin.Result.OpponentSeat;
        var target = source.OwnerSeat;
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A foreign Slash cannot overwrite a child attack.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor, actor, null, null, null, CardKind.Slash, [target], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        return new(new(programFrameId, instructionIndex, source, origin, id, action.ActionId), action);
    }

    internal void CommitForeignTurnContestSlash(ForeignTurnContestSlashIssue issue)
    {
        var returned = issue.Return;
        var id = returned.CardUseFrameId;
        var action = issue.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[returned.Source.OwnerSeat];
        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(returned.ProgramFrameId);
        if (parent.ForeignTurnContest is not { Stage: ForeignTurnContestStage.SlashIssued } receipt || receipt.SlashReturn != returned || returned.InstructionIndex != parent.InstructionIndex || action.ActionId != returned.CardActionId || actor.Seat != returned.Origin.Result.OpponentSeat || target.Seat != parent.OwnerSeat || !action.TargetSeats.SequenceEqual([target.Seat]))
            throw new InvalidOperationException("A foreign Slash lost its prepared typed native issuance.");
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, CardKind.Slash, [target.Seat], PhysicalCardIds: []) { Action = action, ForeignTurnContestSlashReturn = returned });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ForeignTurnContestSlashIssuedEvent(returned));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [target.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), programSkillCardUseFrameId: parent.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, target.Seat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 80)]
    internal bool ReturnRegisteredCompleteForeignTurnContestSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ForeignTurnContestSlashReturn is not { } returned)
            return false;
        _foreignTurnContestOperations.Get().CompleteSlash(new(completion.ProgramFrameId, completion.ResolutionId, returned));
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IForeignTurnContestAidProgramHost.ResolveForeignTurnPindian(ProgramSkillFrame frame, string resultBind) => _foreignTurnContestOperations.Get().Resolve(frame, resultBind);
    SkillProgramStepOutcome global::CardGame.Core.IForeignTurnContestAidProgramHost.OfferSameTypeDifferentNameOrExtraTarget(ProgramSkillFrame frame) => _coreSameTypeActualUseAidOperations.BeginSameTypeAid(frame);
}
