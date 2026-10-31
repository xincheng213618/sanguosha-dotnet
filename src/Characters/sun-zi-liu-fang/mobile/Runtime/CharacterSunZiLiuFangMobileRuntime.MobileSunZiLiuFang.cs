#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunZiLiuFangMobileRuntime
{
    internal const string MobileJiyu = "mobile:jiyu", MobileJiyuActivation = "hand-discard-restriction";
    internal SkillGrantPhaseExpiry? MobileJiyuCurrentPlay(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0 ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, owner, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal bool ValidMobileJiyuInvocation(MobileJiyuInvokedEvent invoked) => invoked.Source.SkillId == MobileJiyu && invoked.Source.BindingId == MobileJiyuActivation && _coreTableSeatQueriesOperations.IsValidPlayerSeat(invoked.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(invoked.Target) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(invoked.TurnOwner) && invoked.Phase is { TurnNumber: > 0, PhaseInstanceId: > 0 } && invoked.Phase.PhaseActorSeat == invoked.Source.OwnerSeat && invoked.Hash == _contentRegistryOperations._contentRegistry.GetSkill(MobileJiyu).Program?.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == invoked.FrameId && e.OwnerSeat == invoked.Source.OwnerSeat && e.SkillId == MobileJiyu && e.ActivationId == MobileJiyuActivation) == 1;
    internal IEnumerable<MobileJiyuInvokedEvent> MobileJiyuInvocations(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiyuInvokedEvent>().Where(e => e.Source.OwnerSeat == owner && ValidMobileJiyuInvocation(e));
    internal int MobileJiyuLimit(int owner) => MobileJiyuCurrentPlay(owner)is { } phase ? 1 + MobileJiyuInvocations(owner).Count(start => start.Phase == phase && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiyuCompletedEvent>().Any(done => done.FrameId == start.FrameId && done.EarnedExtraUse)) : 0;
    internal bool MobileJiyuMaySelect(int owner, int target, int usesBefore) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count > 0 && (usesBefore == 0 || !MobileJiyuInvocations(owner).Any(e => e.Phase.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Target == target));
    int global::CardGame.Core.IMobileJiyuSelection.PhaseLimit(int owner) => MobileJiyuLimit(owner);
    bool global::CardGame.Core.IMobileJiyuSelection.MaySelect(int owner, int target) => MobileJiyuMaySelect(owner, target, _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner, MobileJiyu, MobileJiyuActivation)));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 78300)]
    internal void CaptureMobileJiyuInvocation(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != MobileJiyu)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(started.FrameId);
        var uses = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((frame.OwnerSeat, MobileJiyu, MobileJiyuActivation));
        if (frame.TriggerId is not null || frame.ActivationId != MobileJiyuActivation || frame.InstructionIndex != 0 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var target] || MobileJiyuCurrentPlay(frame.OwnerSeat)is not { } phase || uses < 1 || uses > MobileJiyuLimit(frame.OwnerSeat) || !MobileJiyuMaySelect(frame.OwnerSeat, target, uses - 1) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiyuInvokedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Mobile Jiyu must freeze the original actual-Play allowance and target before its native discard.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiyuInvokedEvent(frame.Id, new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, phase, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, target, uses > 1));
    }

    internal bool HasMobileJiyuOriginalSelection(ProgramSkillFrame frame) => frame.SkillId == MobileJiyu && frame.TriggerId is null && frame.ActivationId == MobileJiyuActivation && frame.SelectedCardIds.Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiyuInvokedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [var invoked] && ValidMobileJiyuInvocation(invoked) && invoked.Source == new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) && invoked.Hash == frame.GameplayHash && frame.SelectedTargetSeats.SequenceEqual([invoked.Target]);
    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 78300)]
    internal bool KeepMobileJiyuInitialSelection(ProgramSkillFrame frame) => HasMobileJiyuOriginalSelection(frame);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 78301)]
    internal void CompleteMobileJiyuInvocation(ProgramSkillResolvedEvent resolved)
    {
        if (resolved.SkillId != MobileJiyu || !resolved.Completed)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(resolved.FrameId);
        if (!HasMobileJiyuOriginalSelection(frame) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiyuCompletedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Mobile Jiyu cannot award the original actual-Play allowance more than once.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiyuCompletedEvent(frame.Id, _coreProgramParticipantPhaseOperationsOperations.HasActuallyUsableHandCard(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])));
    }
}
