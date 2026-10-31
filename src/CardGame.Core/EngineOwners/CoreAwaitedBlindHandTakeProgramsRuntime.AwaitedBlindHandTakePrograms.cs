#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAwaitedBlindHandTakeProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IAwaitedBlindHandTakeProgramHost.TakeRandomHandCardsAndAwait(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason) => BeginAwaitedBlindHandTake(frame, amountPerTarget, reason);
    internal SkillProgramStepOutcome BeginAwaitedBlindHandTake(ProgramSkillFrame frame, int amountPerTarget, CardMoveReason reason)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (amountPerTarget != 1 || frame.BlindHandTake is not null || frame.SelectedTargetSeats.Count > 2 || frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count || frame.SelectedTargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == frame.OwnerSeat))
            throw new InvalidOperationException("An awaited blind take requires zero to two distinct other participants.");
        var cards = new List<(Card Card, CardLocation From)>();
        var seats = new List<int>();
        foreach (var seat in frame.SelectedTargetSeats)
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || hand.Count == 0)
                continue;
            cards.Add((hand[_randomOperations._random.Next(hand.Count)], CardLocation.Hand(seat)));
            seats.Add(seat);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BlindHandTake = new(frame.InstructionIndex, Array.AsReadOnly(seats.ToArray()), Array.AsReadOnly(cards.Select(c => c.Card.Id).ToArray()), Array.AsReadOnly(cards.Select(c => c.From).ToArray())) });
        if (cards.Count > 0)
        {
            var destination = CardLocation.Hand(frame.OwnerSeat);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(c => c.Card.Id).ToArray(), destination, reason);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomHandCardsTakenEvent(frame.Id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, Array.AsReadOnly(seats.ToArray()), cards.Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 730)]
    internal bool ResumeAwaitedBlindHandTake(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.BlindHandTake is not { } receipt)
            return false;
        AssertAwaitedBlindHandTake(frame);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BlindHandTake = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "得牌子窗口结清后，来源失效或游戏结束，剩余结算取消。");
            return true;
        }

        return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 20)]
    internal void AssertAwaitedBlindHandTake(ProgramSkillFrame frame)
    {
        if (frame.BlindHandTake is not { } receipt)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (receipt.InstructionIndex != frame.InstructionIndex || receipt.InstructionIndex < 1 || receipt.InstructionIndex > plan.Instructions.Count || plan.Instructions[receipt.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.TakeRandomHandCardFromSelectedTargets, AwaitMovementTriggers: true } || receipt.TargetSeats.Count != receipt.CardIds.Count || receipt.CardIds.Count != receipt.SourceLocations.Count || receipt.CardIds.Count > 2 || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count || receipt.TargetSeats.Distinct().Count() != receipt.TargetSeats.Count || receipt.SourceLocations.Where((from, i) => from != CardLocation.Hand(receipt.TargetSeats[i])).Any())
            throw new InvalidOperationException("An awaited blind hand take lost its frozen exact instruction or source batch.");
    }
}
