#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePindianHandReturnsRuntime
{
    internal bool IsPindianHandReturn(ProgramSkillFrame f) => f.TriggerId is null && ProgramInstructionResolver.Default.FindActivation(_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!, f.ActivationId)is { PindianHandReturn: true } a && PindianHandReturnContract.Matches(a.Effects);
    internal void RecordPindianHandContestReturn(ProgramSkillFrame f, PindianFrame native)
    {
        if (!IsPindianHandReturn(f))
            return;
        var result = native.Result!;
        if (native.ParentFrameId != f.Id || f.InstructionIndex != 1 || f.PindianHandReturn is not null || native.SourceSeat != f.OwnerSeat || f.SelectedTargetSeats is not [var target] || native.OpponentSeat != target || native.ProgramResultBind != "__active-pindian-result" || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == native.Id && e.SkillId == f.SkillId && e.Result == result) != 1)
            throw new InvalidOperationException("Hand return lost its exact native Pindian result and parent.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PindianHandReturn = new(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, native.Id, target, result.SourceRank, result.OpponentRank) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPindianHandContestReturnedEvent(f.Id, native.Id, f.OwnerSeat, target, result.SourceRank, result.OpponentRank, result.SourceWon));
    }

    internal long? BeginPindianWholeHandAcquisition(ProgramSkillFrame f, string bind, SkillProgramCardDestination destination)
    {
        if (!IsPindianHandReturn(f) || f.InstructionIndex != 3 || destination != SkillProgramCardDestination.OwnerHand)
            return null;
        var receipt = RequirePindianHandReturn(f);
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (receipt.AcquisitionPaid || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !cards.CardIds.Order().SequenceEqual(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat]).Select(c => c.Id).Order()) || cards.SourceLocations.Any(location => location != CardLocation.Hand(receipt.TargetSeat)))
            throw new InvalidOperationException("Pindian whole-hand acquisition lost its exact unpaid opponent hand.");
        return _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
    }

    internal void CommitPindianWholeHandAcquisition(long frameId, string bind, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = RequirePindianHandReturn(f);
        var ids = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind).CardIds;
        var after = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray();
        if (moves.Length != ids.Count || !moves.Select(m => m.CardId).Order().SequenceEqual(ids.Order()) || moves.Any(m => m.From != CardLocation.Hand(r.TargetSeat) || m.To != CardLocation.Hand(f.OwnerSeat) || m.Reason != new CardMoveReason($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}")))
            throw new InvalidOperationException("Pindian whole-hand acquisition must move every frozen material exactly once.");
        var paid = r with
        {
            AcquisitionPaid = true,
            SequenceBefore = before,
            SequenceAfter = after,
            Materials = moves.Select(m => new PindianHandReturnMaterial(m.CardId, m.Sequence)).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PindianHandReturn = paid });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPindianHandAcquiredEvent(paid));
    }

    internal ProgramPindianHandReturnReceipt RequirePindianHandReturn(ProgramSkillFrame f)
    {
        if (!IsPindianHandReturn(f) || f.PindianHandReturn is not { } r || r.FrameId != f.Id || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || f.SelectedTargetSeats is not [var target] || target != r.TargetSeat || f.PindianResultBindings.SingleOrDefault(b => b.Name == "__active-pindian-result")is not { SourceWon: true } result || result.SourceSeat != f.OwnerSeat || result.OpponentSeat != target || result.SourceRank != r.SourceRank || result.OpponentRank != r.OpponentRank || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramPindianHandContestReturnedEvent>().Count(e => e.FrameId == f.Id && e.PindianFrameId == r.PindianFrameId && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == target && e.SourceRank == r.SourceRank && e.OpponentRank == r.OpponentRank && e.Won) != 1)
            throw new InvalidOperationException("Pindian hand return lost its exact native winning contest.");
        if (!r.AcquisitionPaid)
            return r;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramPindianHandAcquiredEvent>().Where(e => e.Receipt.FrameId == f.Id).ToArray();
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        if (issued is not [var fact] || fact.Receipt.Source != r.Source || fact.Receipt.GameplayHash != r.GameplayHash || fact.Receipt.PindianFrameId != r.PindianFrameId || fact.Receipt.TargetSeat != r.TargetSeat || fact.Receipt.SourceRank != r.SourceRank || fact.Receipt.OpponentRank != r.OpponentRank || !fact.Receipt.AcquisitionPaid || fact.Receipt.SequenceBefore != r.SequenceBefore || fact.Receipt.SequenceAfter != r.SequenceAfter || !fact.Receipt.Materials.SequenceEqual(r.Materials) || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Materials.Count != moves.Length || !r.Materials.SequenceEqual(moves.Select(m => new PindianHandReturnMaterial(m.CardId, m.Sequence))) || moves.Any(m => m.From != CardLocation.Hand(target) || m.To != CardLocation.Hand(f.OwnerSeat) || m.Reason != new CardMoveReason($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}")))
            throw new InvalidOperationException("Pindian hand return lost its issued atomic acquisition payment.");
        return r;
    }

    internal void FreezePindianHalfReturn(ProgramSkillFrame f, int count)
    {
        if (!IsPindianHandReturn(f) || f.InstructionIndex != 4)
            return;
        var r = RequirePindianHandReturn(f);
        if (!r.AcquisitionPaid || r.ReturnCount is not null || f.PendingMovementContinuation is not null || count != _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count / 2)
            throw new InvalidOperationException("Half-hand return must freeze once after every acquisition child returns.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PindianHandReturn = r with { ReturnCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPindianHalfReturnCountEvent(f.Id, f.OwnerSeat, r.TargetSeat, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count, count));
    }

    internal bool HasPaidPindianHandReturn(ProgramSkillFrame f)
    {
        if (f.PindianHandReturn is not { AcquisitionPaid: true })
            return false;
        _ = RequirePindianHandReturn(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 7003)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 7003)]
    internal bool ContinuePaidPindianHandReturn(ProgramSkillFrame f) => HasPaidPindianHandReturn(f);

    [ProgramContinuation(ProgramContinuationKind.Resume, 7003)]
    internal bool ResumeInvalidPindianHandReturn(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || !HasPaidPindianHandReturn(f))
            return false;
        var r = f.PindianHandReturn!;
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, OwnedCardSelection = null });
        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "已付获牌子流程结清，死亡参与者不再执行赠牌。");
        return true;
    }
}
