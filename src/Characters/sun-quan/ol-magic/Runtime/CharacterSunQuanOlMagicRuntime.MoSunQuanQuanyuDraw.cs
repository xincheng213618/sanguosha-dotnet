#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool DrainMoQuanyu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMoQuanyu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQuanyu: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == MoQuanyuStage.Choosing)
        {
            while (r.Votes.Count < r.Participants.Count)
            {
                var participant = r.Participants[r.Votes.Count];
                if (participant.AvailableMask != 0 && _coreMatchParticipantsPlayersAccess._players[participant.Seat].IsAlive)
                {
                    PublishMoQuanyu(f);
                    return;
                }

                r = r with
                {
                    Votes = r.Votes.Append(new(participant.Seat, MoQuanyuEffect.None)).ToArray()
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyu = r });
            }

            var ownerChoice = r.Kind == MoQuanyuInvocationKind.Round ? r.Votes.Single(v => v.Seat == f.OwnerSeat).Effect : r.OwnerChoiceBefore;
            var matching = ownerChoice == MoQuanyuEffect.None ? 0 : r.Votes.Count(v => v.Effect == ownerChoice);
            r = r with
            {
                DrawCount = Math.Min(3, matching),
                Stage = MoQuanyuStage.Drawing
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyu = r });
            foreach (var(vote, i)in r.Votes.Select((v, i) => (v, i)))
            {
                var participant = r.Participants[i];
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuChoiceRevealedEvent(id, i, f.OwnerSeat, vote.Seat, r.RoundNumber, vote.Effect, participant.HistoryMask, participant.HistoryMask | MoQuanyuBit(vote.Effect)));
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的【权御】：{(_coreMatchParticipantsPlayersAccess._players[vote.Seat].Name)} 选择了{(MoQuanyuEffectName(vote.Effect))}。", f.OwnerSeat, vote.Seat);
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuRevealCompletedEvent(id, ownerChoice, matching, r.DrawCount));
        }

        if (r.Stage == MoQuanyuStage.Drawing)
        {
            if (f.NativeDraw is not null)
                return;
            if (r.DrawCount == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteMoQuanyu(f);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuDrawRequestedEvent(id, f.OwnerSeat, r.DrawCount));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.DrawCount, new(MoQuanyuDrawReason));
            return;
        }

        if (DrainMoQuanyu(f))
            return;
        CompleteMoQuanyu(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMoSunQuanQuanyuRoundChoices, SkillProgramEffectOp.ResolveMoSunQuanQuanyuTargetChoice)]
    internal void ReturnMoQuanyuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MoQuanyu is not { Stage: MoQuanyuStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.DrawCount || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Quanyu lost its one native matching-choice draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyu = r with { Stage = MoQuanyuStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueMoQuanyu(f.Id);
    }

    internal void CompleteMoQuanyu(ProgramSkillFrame f)
    {
        var r = f.MoQuanyu!;
        if (r.Stage == MoQuanyuStage.Choosing || f.NativeDraw is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuChoicesCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Quanyu cannot complete before every private choice and native child returns.");
        var count = r.DrawOutcome?.RequestedCount ?? 0;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuChoicesCompletedEvent(f.Id, r.Kind, count));
        var returned = r.TargetRequest is { } request ? new MoQuanyuTargetChoiceReturn(f.Id, request, r.Votes.Single().Effect, count) : null;
        if (returned is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuTargetChoiceReturnedEvent(returned));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        if (returned is not null)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame parent || parent.Id != returned.Request.ParentFrameId || !IsMoSunQuanTianenQuanyuRequest(parent, returned.Request))
                throw new InvalidOperationException("Quanyu target-only completion lost its exact paid Tianen parent.");
            ReturnMoSunQuanTianenQuanyu(parent, returned);
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28910)]
    internal bool ResumeMoQuanyu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQuanyu: not null } f || f.Id != id)
            return false;
        ContinueMoQuanyu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28910)]
    internal bool ReturnMoQuanyuMovement(ProgramSkillFrame f)
    {
        if (f.MoQuanyu is null)
            return false;
        ContinueMoQuanyu(f.Id);
        return true;
    }
}
