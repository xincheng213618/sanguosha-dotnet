#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal void BindMouLixianNativeUse(CardUseFrame supplied)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == supplied.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouLixian: { Stage: MouLixianStage.NativeIssuing } r } parent)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        if (use.MouLixianOrigin is not null || !ValidMouLixianStarted(parent) || r.MaterialId is not { } id || r.PrintedKind is not { } printed || r.From is not { } from || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != parent.OwnerSeat || a.ProviderSeat != parent.OwnerSeat || use.SourceSeat != parent.OwnerSeat || use.CardKind != r.OutputKind || a.EffectiveKind != r.OutputKind || a.EffectiveSuit != r.EffectiveSuit || a.EffectiveRank != r.EffectiveRank || a.PhysicalCards is not [{ } cost] || cost.CardId != id || cost.CardKind != printed || cost.From != from || !a.ConversionChain.SequenceEqual([r.Source]) || !a.TargetSeats.SequenceEqual(use.TargetSeats))
            throw new InvalidOperationException("Lixian changed its owning source, original one material or effective native appearance before acceptance.");
        var origin = new MouLixianNativeOrigin(parent.Id, r.Source, r.GameplayHash, use.Id, a.ActionId, a, id, printed, from, r.Before);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouLixian = r with { Stage = MouLixianStage.NativeUse, UseId = use.Id, ActionId = a.ActionId, Targets = use.TargetSeats } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouLixianOrigin = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianUseIssuedEvent(parent.Id, use.Id, a.ActionId, parent.OwnerSeat, use.CardKind, id, printed, from, r.EffectiveSuit, r.EffectiveRank));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianNameUsedEvent(parent.Id, use.Id, a.ActionId, parent.OwnerSeat, use.CardKind));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28960)]
    internal void RecordMouLixianPayment(CardMovedEvent e)
    {
        if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()is not { } m || m.CardId != e.CardId || m.From != e.From || m.To != e.To || m.Reason != e.Reason || m.Reason != CardMoveReasons.Use)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.MouLixianOrigin is not null).ToArray())
        {
            var o = use.MouLixianOrigin!;
            if (m.CardId != o.CardId || m.CardKind != o.PrintedKind || m.From != o.From || m.Sequence <= o.BeforePayment || m.To != CardLocation.Processing && !(m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon && m.From.Zone == CardZoneKind.Equipment || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, m.To))) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Any(p => p.UseId == use.Id))
                continue;
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == o.ProgramFrameId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouLixian = parent.MouLixian!with { MovementSequence = m.Sequence, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianMaterialPaidEvent(parent.Id, use.Id, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
        }
    }

    internal bool ValidMouLixianStarted(ProgramSkillFrame f)
    {
        if (f.MouLixian is not { } r || f.SkillId != MouLixianSkill || f.TriggerId is not ("after-damage" or "unlocked-preparation" or "unlocked-ending") || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || f.ParentFrameId != r.OriginFrameId || r.Source != MouHuangYueyingSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.Targets is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianStartedEvent>().Count(e => e == new MouLixianStartedEvent(f.Id, r.Source, r.GameplayHash, c.Window, r.OriginFrameId, r.Phase)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == c.Window) != 1)
            return false;
        return c.Window == SkillProgramTriggerWindow.AfterDamageApplied ? r.Phase is null && c.TargetSeat == f.OwnerSeat && c.Amount > 0 : r.Phase is { } phase && phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && (c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow ? phase.Kind == ActualDiscardRecoveryPhaseKind.Preparation && MouLixianPreparationUnlocked(f.OwnerSeat) : c.Window == SkillProgramTriggerWindow.TurnEnding && phase.Kind == ActualDiscardRecoveryPhaseKind.Ending && MouLixianEndingUnlocked(f.OwnerSeat));
    }

    internal bool IsIssuedMouLixianUse(CardUseFrame use)
    {
        if (use.MouLixianOrigin is not { } o || use.Id != o.UseId || use.Action is not { Type: CardActionType.Use } a || a.ActionId != o.ActionId || a.ActorSeat != o.Source.OwnerSeat || a.ProviderSeat != o.Source.OwnerSeat || use.SourceSeat != o.Source.OwnerSeat || !MouLixianNames(o.Source.OwnerSeat).Contains(use.CardKind) || a.EffectiveKind != use.CardKind || a.PhysicalCards is not [{ } cost] || cost.CardId != o.CardId || cost.CardKind != o.PrintedKind || cost.From != o.From || !a.ConversionChain.SequenceEqual([o.Source]) || _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash != o.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseIssuedEvent>().Where(e => e.UseId == use.Id).ToArray()is not [var issued] || issued.FrameId != o.ProgramFrameId || issued.ActionId != a.ActionId || issued.OwnerSeat != a.ActorSeat || issued.OutputKind != use.CardKind || issued.CardId != o.CardId || issued.PrintedKind != o.PrintedKind || issued.From != o.From || issued.EffectiveSuit != a.EffectiveSuit || issued.EffectiveRank != a.EffectiveRank || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNameUsedEvent>().Count(e => e == new MouLixianNameUsedEvent(o.ProgramFrameId, use.Id, a.ActionId, a.ActorSeat, use.CardKind)) != 1)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Where(e => e.UseId == use.Id).ToArray();
        // BeginCardUse binds before payment; the command's real movement then signs exactly this original entity.
        if (paid.Length == 0)
            return _cardZonesOperations._cardZones.GetLocation(o.CardId) == o.From && _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence == o.BeforePayment;
        return paid is [var p] && p.FrameId == o.ProgramFrameId && p.CardId == o.CardId && p.PrintedKind == o.PrintedKind && p.From == o.From && p.MovementSequence > o.BeforePayment && (p.To == CardLocation.Processing || p.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId).IsGeneralWeapon && o.From.Zone == CardZoneKind.Equipment || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId), o.From, p.To))) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == p.MovementSequence && m.CardId == p.CardId && m.CardKind == p.PrintedKind && m.From == p.From && m.To == p.To && m.Reason == CardMoveReasons.Use) == 1;
    }

    internal bool IsMouLixianRemovedMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { MouLixianOrigin: { } o } use && o.CardId == cardId && IsIssuedMouLixianUse(use) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Count(p => p.UseId == useId && p.CardId == cardId && p.To == CardLocation.OutsideGame) == 1;
    [NativeUseValidation(28960)]
    internal void ValidateMouLixianNativeReturn(CardUseFrame use)
    {
        if (use.MouLixianOrigin is not null && !IsIssuedMouLixianUse(use))
            throw new InvalidOperationException("Lixian's whole native Use lost its exact accepted source and once-paid original entity.");
    }

    [NativeUseReturn(28960)]
    internal void ReturnMouLixianNativeUse(CardUseFrame use)
    {
        if (use.MouLixianOrigin is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.MouLixian!;
        if (!IsIssuedMouLixianUse(use) || !ValidMouLixianStarted(f) || r.Stage != MouLixianStage.NativeUse || r.NativeReturned || r.UseId != use.Id || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == o.CardId && e.CardKind == use.CardKind) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Lixian cannot return before or twice after its accepted whole native Use.");
        var batches = SignMouLixianNativeBatches(f, use);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLixian = r with { Stage = MouLixianStage.NativeChildren, NativeReturned = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeMovementBatchIds = batches }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianUseReturnedEvent(f.Id, use.Id, o.ActionId));
    }

    internal IReadOnlyList<long> SignMouLixianNativeBatches(ProgramSkillFrame f, CardUseFrame use)
    {
        var r = f.MouLixian!;
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat).ToArray();
        var draws = use.CardKind == CardKind.DrawTwo ? batches.Select(b => b.NativeDrawInvocation).Where(p => p is not null && p.ParentFrameId == use.Id && p.DirectProducer is null && p.RequestedCount == 2 && (use.TargetSeats.Count == 0 ? p.OwnerSeat == use.SourceSeat : use.TargetSeats.Contains(p.OwnerSeat)) && p.Reason == CardMoveReasons.Draw && p.LastBatchId == batches.Single(b => b.NativeDrawInvocation == p).Id && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(p)).Select(p => p!).ToArray() : [];
        bool Primary(CardMovementRecord m)
        {
            if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
                return false;
            if (m.CardId == r.MaterialId && m.CardKind == r.PrintedKind && (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Count(p => p.FrameId == f.Id && p.UseId == use.Id && p.CardId == m.CardId && p.PrintedKind == m.CardKind && p.From == m.From && p.To == m.To && p.MovementSequence == m.Sequence) == 1 || m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && (m.Reason == CardMoveReasons.UseFinished || use.CardKind == CardKind.IronChain && m.Reason == CardMoveReasons.IronChainFinished)))
                return true;
            if (draws.Any(p => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.OwnerSeat) && m.Reason == p.Reason && p.Materials.Any(c => c.CardId == m.CardId && c.PrintedKind == m.CardKind && c.MovementSequence == m.Sequence) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
                return true;
            if (use.CardKind is not (CardKind.Snatch or CardKind.Dismantlement))
                return false;
            var take = use.CardKind == CardKind.Snatch;
            var action = take ? LegalActionKind.Snatch : LegalActionKind.Dismantlement;
            var entry = m.From == CardLocation.Processing ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.CardId == m.CardId && p.Sequence < m.Sequence) : m;
            if (entry is null || entry.CardKind != m.CardKind || entry.Sequence <= r.Before || entry.From.OwnerSeat is not { } target || entry.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || !use.TargetSeats.Contains(target) || entry.Reason != global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime.GetTargetCardMoveReason(action, entry.From.Zone) || entry.To != CardLocation.Processing && !(entry.To == CardLocation.OutsideGame && entry.From.Zone == CardZoneKind.Equipment && (_coreAdvancedLifecycleOperations.GetAdvancedCard(entry.CardId).IsGeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(entry.CardId), entry.From, entry.To))))
                return false;
            var actual = take ? _coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardTakenEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == f.OwnerSeat && e.TargetSeat == target && e.FromZone == entry.From.Zone && (e.PublicCardId is null || e.PublicCardId == entry.CardId)) : _coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardDiscardedEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == f.OwnerSeat && e.TargetSeat == target && e.FromZone == entry.From.Zone && (e.PublicCardId is null || e.PublicCardId == entry.CardId));
            if (actual == 0)
                return false;
            if (m == entry)
                return true;
            return entry.To == CardLocation.Processing && m.From == CardLocation.Processing && m.To == (take ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile) && m.Reason == global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime.GetTargetCardFinishedMoveReason(action, entry.From.Zone) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(p => p.CardId == entry.CardId && p.Sequence > entry.Sequence) == m;
        }

        bool OxSpill(CardMovementBatchContext batch) => batch.ParentBatchId is { } parent && batches.SingleOrDefault(b => b.Id == parent)is { } original && original.Movements.Any(m => m.CardKind == CardKind.WoodenOx && m.From.Zone == CardZoneKind.Equipment && Primary(m)) && batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From.Zone == CardZoneKind.WoodenOxGrain && original.Movements.Any(p => p.CardKind == CardKind.WoodenOx && p.From.OwnerSeat == m.From.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
        foreach (var batch in batches)
        {
            if (batch.Movements.Count == 0 || !batch.Movements.All(Primary) && !OxSpill(batch))
                throw new InvalidOperationException("Lixian's retired native Use has a movement without its exact material, target-effect pair or Wooden Ox parent invoice. " + System.Text.Json.JsonSerializer.Serialize(new { Program = f.Id, Use = use.Id, Kind = use.CardKind.ToString(), Material = r.MaterialId, PrintedKind = r.PrintedKind?.ToString(), OriginalZone = r.From?.ToString(), r.Before, r.After, Targets = use.TargetSeats, Batch = batch.Id, batch.ParentFrameId, batch.ParentBatchId, batch.AwaitingProgramFrameId, batch.OriginSkillId, batch.OriginSkillInstanceId, batch.OriginOwnerSeat, Count = batch.Movements.Count, Moves = batch.Movements.Take(8).Select(m => new { m.CardId, Kind = m.CardKind.ToString(), From = m.From.ToString(), To = m.To.ToString(), Reason = m.Reason.Value, m.Sequence, Primary = Primary(m) }).ToArray(), Draw = batch.NativeDrawInvocation is { } proof ? new { proof.InvocationId, proof.ParentFrameId, proof.OwnerSeat, proof.RequestedCount, proof.ActualCount, Reason = proof.Reason.Value, proof.SequenceBefore, proof.SequenceAfter, proof.LastBatchId, proof.DirectProducer, Valid = _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof) } : null, Pending = batches.Take(8).Select(b => new { b.Id, b.ParentBatchId, Count = b.Movements.Count, DrawId = b.NativeDrawInvocation?.InvocationId }).ToArray() }));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianNativeBatchSignedEvent(f.Id, use.Id, batch.Id, batch.ParentBatchId, batch.Movements.Count));
            foreach (var m in batch.Movements)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianNativeMovementSignedEvent(f.Id, use.Id, batch.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        }

        return Array.AsReadOnly(batches.Select(b => b.Id).ToArray());
    }

    internal bool IsMouLixianMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.MouLixian is { Stage: MouLixianStage.NativeChildren, NativeReturned: true } r && ValidMouLixianStarted(f) && effect?.Op == SkillProgramEffectOp.ResolveMouHuangYueyingLixian && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
    internal bool IsReturnedMouLixianUse(ProgramSkillFrame f) => f.MouLixian is { Stage: MouLixianStage.NativeChildren, Paid: true, NativeReturned: true, OutputKind: { } output, MaterialId: { } card, PrintedKind: { } printed, From: { } from } r && ValidMouLixianStarted(f) && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var issued] && issued == new MouLixianUseIssuedEvent(f.Id, r.UseId, r.ActionId, f.OwnerSeat, output, card, printed, from, r.EffectiveSuit, r.EffectiveRank) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseReturnedEvent>().Count(e => e == new MouLixianUseReturnedEvent(f.Id, r.UseId, r.ActionId)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNameUsedEvent>().Count(e => e == new MouLixianNameUsedEvent(f.Id, r.UseId, r.ActionId, f.OwnerSeat, output)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == card && e.CardKind == output) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianCompletedEvent>().Any(e => e.FrameId == f.Id);
    internal bool IsMouLixianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.MouLixian is { Paid: true } r && ValidMouLixianStarted(f) && recovery.Return.ResumeFrameId == r.UseId && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == CardMoveReasons.Use && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && r.PrintedKind == CardKind.SilverLion && r.From == CardLocation.Equipment(f.OwnerSeat);
    internal bool IsMouLixianNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => f.MouLixian is { Stage: MouLixianStage.NativeChildren, NativeReturned: true, MaterialId: { } cardId, PrintedKind: { } printed } r && ValidMouLixianStarted(f) && batch.ParentFrameId == r.UseId && batch.AwaitingProgramFrameId is null && batch.Movements.Count > 0 && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.OriginOwnerSeat == f.OwnerSeat && r.NativeMovementBatchIds.Contains(batch.Id) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNativeBatchSignedEvent>().Count(e => e == new MouLixianNativeBatchSignedEvent(f.Id, r.UseId, batch.Id, batch.ParentBatchId, batch.Movements.Count)) == 1 && batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNativeMovementSignedEvent>().Count(e => e == new MouLixianNativeMovementSignedEvent(f.Id, r.UseId, batch.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianUseReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28960)]
    internal void AssertMouLixian(ProgramSkillFrame f)
    {
        if (f.MouLixian is not { } r)
            return;
        if (!ValidMouLixianStarted(f) || r.NativeMovementBatchIds is not System.Collections.IList { IsReadOnly: true } || r.NativeMovementBatchIds.Distinct().Count() != r.NativeMovementBatchIds.Count || r.NativeMovementBatchIds.Count != _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianNativeBatchSignedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId) || r.Paid && r.Stage is not (MouLixianStage.NativeIssuing or MouLixianStage.NativeUse or MouLixianStage.NativeChildren) || r.OutputKind is { } output && !MouLixianNames(f.OwnerSeat).Contains(output) || r.Stage == MouLixianStage.NativeUse && (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { } use || !IsIssuedMouLixianUse(use)) || r.Stage == MouLixianStage.NativeChildren && !IsReturnedMouLixianUse(f))
            throw new InvalidOperationException("Lixian lost its original trigger/phase, readonly selection or true native use return.");
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 600)]
    internal bool WindowSettlement_IsMouLixianNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouLixianNativeSettlementBatch(frame, batch);
}
