#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsRuntime
{
    internal void AssertOlMoRumoStates()
    {
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoEnteredEvent>().ToArray();
        if (entries.Any(item => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(item.OwnerSeat) || item.EntryRound > _coreActualRoundProgressRoundNumberAccess._roundNumber || item.ActualTurn > _coreActualTurnProgressTurnNumberAccess._turnNumber))
            throw new InvalidOperationException("OL Rumo cannot retain an entry from another owner or a future boundary.");
        foreach (var owner in entries.Select(item => item.OwnerSeat).Distinct())
            _ = OlMoRumoEntry(owner);
        var starts = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoRoundStartedEvent>().ToArray();
        if (starts.GroupBy(item => (item.OwnerSeat, item.RoundNumber)).Any(group => group.Count() != 1) || starts.GroupBy(item => item.FrameId).Any(group => group.Count() != 1) || starts.Any(item => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(item.OwnerSeat) || OlMoRumoEntry(item.OwnerSeat)?.EntryId != item.EntryId || _coreProgramEventHistoryOperations.ProgramEventHistory<RoundEndingStartedEvent>().Count(boundary => boundary.BoundaryFrameId == item.BoundaryId && boundary.RoundNumber == item.RoundNumber && boundary.ActualTurnNumber == item.ActualTurn && boundary.ActorSeat == item.TurnOwner) != 1 || OlMoRumoDamageBefore(item) != item.HadRoundDamage))
            throw new InvalidOperationException("OL Rumo must issue at most one exact sampled obligation per old round and owner.");
        var finished = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoRoundCompletedEvent>().ToArray();
        if (finished.GroupBy(item => item.FrameId).Any(group => group.Count() != 1))
            throw new InvalidOperationException("OL Rumo cannot finish its original obligation twice.");
        foreach (var item in finished)
        {
            var started = starts.SingleOrDefault(start => start.FrameId == item.FrameId);
            var losses = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoHpLossAppliedEvent>().Where(loss => loss.FrameId == item.FrameId).ToArray();
            if (started is null || started.EntryId != item.EntryId || started.BoundaryId != item.BoundaryId || started.OwnerSeat != item.OwnerSeat || started.RoundNumber != item.RoundNumber || started.HadRoundDamage != item.HadRoundDamage || item.ActualLoss is < 0 or > 1 || losses.Length > 1 || losses.Any(loss => loss.OwnerSeat != item.OwnerSeat || loss.RequestedAmount != 1 || loss.ActualAmount != item.ActualLoss || loss.AppearancePrevented != item.AppearancePrevented) || losses.Length == 0 && (item.ActualLoss != 0 || item.AppearancePrevented) || item.HadRoundDamage && losses.Length != 0 || losses.Any(loss => !OlMoRumoNativeLossProof(loss)))
                throw new InvalidOperationException("OL Rumo's completed result must retain its original sampled and native-loss facts.");
        }
    }

    internal OlMoRumoEnteredEvent? OlMoRumoEntry(int ownerSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat))
            return null;
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoEnteredEvent>().Where(item => item.OwnerSeat == ownerSeat).ToArray();
        if (entries.Length > 1 || entries.Any(item => item.EntryId <= 0 || item.Origin.OwnerSeat != ownerSeat || item.ActualTurn < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(item.TurnOwner) || !(item.Origin.SkillId == "ol:mo-qiangang" && item.EntryRound >= 1 && _characterSunQuanOlMagicOperations.IsPaidQiangangRumoOrigin(item.EntryId, item.Origin, item.OriginGameplayHash) || _characterLuBuOlMagicOperations.IsMoLuBuRumoOrigin(item) || _characterDiaoChanOlMagicOperations.IsMoDcQingshiRumoOrigin(item) || _programPipelineHooksOperations._programPipelineHooks.RumoOrigin(item.EntryId, item.Origin, item.OriginGameplayHash))))
            throw new InvalidOperationException("OL Rumo must retain one exact permanent paid entry per owner.");
        return entries.SingleOrDefault();
    }

    internal static ProgramTriggerCandidate OlMoRumoCandidate(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules__shared_OlMoRumo_Proofs.OlMoRumoCandidate(frame);
    internal bool OlMoRumoCandidateSource(ProgramTriggerCandidate candidate) => candidate.SkillId == "ol:mo-rumo" && candidate.BindingId == "no-damage-round-ending" && candidate.Priority == 0 && candidate.OccurrenceIndex == 0 && OlMoRumoEntry(candidate.OwnerSeat)is { } entry && entry.EntryRound <= _coreActualRoundProgressRoundNumberAccess._roundNumber && candidate.SkillInstanceId == global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumoInstance(entry.EntryId) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault("ol:mo-rumo")?.Program is { } program && program.GameplayHash == candidate.GameplayHash && program.Triggers is [{ Id: "no-damage-round-ending", Window: SkillProgramTriggerWindow.RoundEnding, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ResolveOlMoRumoRoundPenalty }] }];
    internal bool OlMoRumoParentMatches(ProgramSkillFrame frame) => frame.TriggerId == "no-damage-round-ending" && frame.ActivationId == "no-damage-round-ending" && frame.SelectedCardIds.Count == 0 && frame.SelectedTargetSeats.Count == 0 && frame.WindowContext is { } context && OlMoRumoCandidateSource(OlMoRumoCandidate(frame)) && _coreRoundEndingOperations.IsExactRoundEndingCandidate(OlMoRumoCandidate(frame), context) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Where(item => item.FrameId == frame.Id).ToArray()is [var started] && started == new ProgramBindingStartedEvent(frame.Id, "ol:mo-rumo", "no-damage-round-ending", frame.SkillInstanceId, frame.OwnerSeat, SkillProgramTriggerWindow.RoundEnding);
    internal bool OlMoRumoDamageBefore(OlMoRumoRoundStartedEvent started)
    {
        var all = _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(item => item.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).ToArray();
        var index = Array.FindIndex(all, item => item is OlMoRumoRoundStartedEvent sample && sample == started);
        if (index < 0)
            throw new InvalidOperationException("OL Rumo lost its original no-damage sample fact.");
        var round = Array.FindLastIndex(all, index, item => item is RoundStartedEvent);
        if (round < 0 || all[round] is not RoundStartedEvent { RoundNumber: var number } || number != started.RoundNumber)
            throw new InvalidOperationException("OL Rumo cannot substitute another round's damage history.");
        return all.Skip(round + 1).Take(index - round - 1).OfType<DamageAppliedEvent>().Any(item => item.Amount > 0 && !item.SourceLess && item.SourceSeat == started.OwnerSeat);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28940)]
    internal void AssertOlMoRumo(ProgramSkillFrame frame)
    {
        if (frame.OlMoRumo is not { } receipt)
            return;
        if (!OlMoRumoParentMatches(frame) || frame.InstructionIndex != 1 || receipt.InstructionIndex != frame.InstructionIndex || receipt.Source != new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || OlMoRumoEntry(frame.OwnerSeat)is not { } entry || entry.EntryId != receipt.EntryId || !Enum.IsDefined(receipt.Stage) || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame boundary || boundary.Id != receipt.BoundaryId || boundary.RoundNumber != receipt.RoundNumber || boundary.ActualTurnNumber != receipt.ActualTurn || boundary.OwnerSeat != receipt.TurnOwner || _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoRoundStartedEvent>().Where(item => item.FrameId == frame.Id).ToArray()is not [var started] || started != new OlMoRumoRoundStartedEvent(frame.Id, receipt.EntryId, receipt.BoundaryId, frame.OwnerSeat, receipt.RoundNumber, receipt.ActualTurn, receipt.TurnOwner, receipt.HadRoundDamage) || _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoRoundStartedEvent>().Count(item => item.OwnerSeat == frame.OwnerSeat && item.RoundNumber == receipt.RoundNumber) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoRoundCompletedEvent>().Any(item => item.FrameId == frame.Id) || OlMoRumoDamageBefore(started) != receipt.HadRoundDamage)
            throw new InvalidOperationException("OL Rumo lost its exact paid entry, current old-round parent or frozen actual-damage sample.");
        var applied = _coreProgramEventHistoryOperations.ProgramEventHistory<OlMoRumoHpLossAppliedEvent>().Where(item => item.FrameId == frame.Id).ToArray();
        var losses = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Where(item => item.FrameId == frame.Id).ToArray();
        if (!receipt.LossReturned)
        {
            if (applied.Length != 0 || losses.Length != 0 || receipt.ActualLoss != 0 || receipt.AppearancePrevented || receipt.HpAfter != 0 || receipt.Stage != (receipt.HadRoundDamage ? OlMoRumoStage.LossChildren : OlMoRumoStage.LossIssuing))
                throw new InvalidOperationException("OL Rumo cannot invent a returned HP deduction.");
            return;
        }

        if (receipt.HadRoundDamage || receipt.Stage != OlMoRumoStage.LossChildren || receipt.ActualLoss is < 0 or > 1 || receipt.HpAfter < 0 || applied is not [var paid] || paid != new OlMoRumoHpLossAppliedEvent(frame.Id, frame.OwnerSeat, 1, receipt.ActualLoss, receipt.HpAfter, receipt.AppearancePrevented))
            throw new InvalidOperationException("OL Rumo lost its once-only native HP-loss result.");
        if (!OlMoRumoNativeLossProof(paid))
            throw new InvalidOperationException("OL Rumo's returned HP loss must retain its exact native producer proof.");
    }

    internal bool OlMoRumoNativeLossProof(OlMoRumoHpLossAppliedEvent paid)
    {
        var losses = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Where(item => item.FrameId == paid.FrameId).ToArray();
        if (!paid.AppearancePrevented)
            return losses is [var loss] && loss == new ProgramSkillHpLostEvent(paid.FrameId, "ol:mo-rumo", paid.OwnerSeat, paid.ActualAmount, paid.HpAfter);
        return paid.ActualAmount == 0 && losses.Length == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<IdentityHpDeductionPreventedEvent>().Count(item => item.OwnerSeat == paid.OwnerSeat && item.Entry == IdentityAppearanceEntry.ProgramHpLoss && item.RequestedDeduction == 1 && (_coreProgramEventHistoryOperations.ProgramEventHistory<IdentityGeneralAppearedEvent>().Any(start => start.AppearanceFrameId == item.AppearanceFrameId && start.Receipt.ParentFrameId == paid.FrameId && start.Receipt.OwnerSeat == paid.OwnerSeat && start.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && start.Receipt.RequestedDeduction == 1 && start.Receipt.OriginalInstructionIndex == 1) || _resolutionStackOperations._resolutionStack.Any(frame => frame is IdentityAppearanceFrame appearance && appearance.Id == item.AppearanceFrameId && appearance.Receipt.ParentFrameId == paid.FrameId && appearance.Receipt.OwnerSeat == paid.OwnerSeat && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == 1))) == 1;
    }
}
