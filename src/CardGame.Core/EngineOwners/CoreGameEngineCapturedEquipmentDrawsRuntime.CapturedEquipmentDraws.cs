#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCapturedEquipmentDrawsRuntime
{
    internal bool CanPlaceCapturedEquipmentCost(int owner, int recipient, int id)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        return recipient != owner && !card.IsGeneralWeapon && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(owner, recipient, card, from) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.SkillId ?? "", _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.SkillInstanceId ?? "", card);
    }

    internal bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        return recipient != owner && !card.IsGeneralWeapon && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(owner, recipient, card, from) && !_coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.Any(b => b.IsEnabled && b.SkillId == skillId && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, b.SkillInstanceId, card));
    }

    internal SkillProgramStepOutcome BeginCapturedEquipmentAndDraw(ProgramSkillFrame f, int recipient, string bind, int requested)
    {
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (f.CapturedEquipmentDraw is not null || f.TriggerId is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || set.CardIds is not [var id] || set.SourceLocations is not [var from] || !set.CardIds.SequenceEqual(f.SelectedCardIds) || f.SelectedTargetSeats is not [var target] || target != recipient || target == f.OwnerSeat || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Captured equipment placement lost its unpaid exact active source/material/recipient.");
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !CanPlaceCapturedEquipmentCost(f.OwnerSeat, recipient, id) || _cardZonesOperations._cardZones.GetLocation(id) != from || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "直置前参与者、所选实体或技能来源已失效，未支付装备。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[recipient]).Where(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c) == slot).ToArray();
        var replaced = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentSlotCapacity(slot) ? equipped[0] : null;
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var receipt = new ProgramCapturedEquipmentDrawReceipt(f.InstructionIndex, bind, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, recipient, id, from, slot, replaced?.Id, replaced?.IsGeneralWeapon ?? false, before, before, 0, CapturedEquipmentDrawStage.Placed);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), CapturedEquipmentDraw = receipt });
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(recipient), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(recipient), CardMoveReasons.EquipmentEnter);
        var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
        var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == id && m.From == from && m.To == CardLocation.Equipment(recipient) && m.Reason == CardMoveReasons.EquipmentEnter);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        receipt = receipt with
        {
            SequenceAfter = after,
            EntryMovementSequence = entry.Sequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { CapturedEquipmentDraw = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, recipient, slot, id, card.Kind, replaced?.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CapturedEquipmentPlacedEvent(f.Id, source, f.GameplayHash, recipient, id, from, slot, replaced?.Id, replaced?.IsGeneralWeapon ?? false, before, after, entry.Sequence));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 将【{(card.DisplayName)}】置入 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)} 的装备区。", recipient);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 450)]
    internal bool ResumeCapturedEquipmentAndDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CapturedEquipmentDraw is not { } r)
            return false;
        AssertCapturedEquipmentAndDraw(f);
        if (_coreActualDiscardRecoveriesOperations.DrainCapturedEquipmentOrDiscardChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.CapturedEquipmentDraw!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == CapturedEquipmentDrawStage.Placed && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
            var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), CapturedEquipmentDraw = r with { Stage = CapturedEquipmentDrawStage.Drawn, DrawSequenceBefore = before } });
            var reason = new CardMoveReason($"skill-program.{(f.SkillId)}.captured-equipment-reward");
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], effect.Amount, true, reason);
            var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
            var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason == reason);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { CapturedEquipmentDraw = active.CapturedEquipmentDraw!with { DrawSequenceAfter = after, ActualDrawCount = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CapturedEquipmentRewardDrawnEvent(id, f.OwnerSeat, effect.Amount, actual, before, after));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CapturedEquipmentDraw = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 530)]
    internal void AssertCapturedEquipmentAndDraw(ProgramSkillFrame f)
    {
        if (f.CapturedEquipmentDraw is not { } r)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var op = plan.GetPausedInstruction(f.InstructionIndex).Effect;
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, r.SourceBind);
        if (op.Op != SkillProgramEffectOp.PlaceCapturedEquipmentAndDraw || op.SourceBind != r.SourceBind || r.InstructionIndex != f.InstructionIndex || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || f.TriggerId is not null || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || f.SelectedTargetSeats is not [var target] || target != r.RecipientSeat || target == f.OwnerSeat || !set.CardIds.SequenceEqual([r.CardId]) || !set.SourceLocations.SequenceEqual([r.OriginalFrom]) || !f.SelectedCardIds.SequenceEqual(set.CardIds) || r.OriginalFrom.OwnerSeat != f.OwnerSeat || r.OriginalFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).IsGeneralWeapon || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind) || _coreEquipmentContentOperations.EquipmentRules.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind).Slot != r.Slot || !Enum.IsDefined(r.Stage) || r.SequenceAfter <= r.SequenceBefore || r.SequenceBefore < 0 || r.Stage == CapturedEquipmentDrawStage.Placed && (r.DrawSequenceBefore != 0 || r.DrawSequenceAfter != 0 || r.ActualDrawCount != 0) || r.EntryMovementSequence <= r.SequenceBefore || r.EntryMovementSequence > r.SequenceAfter || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == r.EntryMovementSequence && m.CardId == r.CardId && m.From == r.OriginalFrom && m.To == CardLocation.Equipment(target) && m.Reason == CardMoveReasons.EquipmentEnter) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CapturedEquipmentPlacedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.RecipientSeat == target && e.CardId == r.CardId && e.OriginalFrom == r.OriginalFrom && e.Slot == r.Slot && e.ReplacedCardId == r.ReplacedCardId && e.ReplacedGeneralWeapon == r.ReplacedGeneralWeapon && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter && e.EntryMovementSequence == r.EntryMovementSequence) != 1)
            throw new InvalidOperationException("Captured equipment placement lost its exact actual cost/entry and frozen original recipient.");
        var replacements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.Equipment(target) && m.Reason == CardMoveReasons.EquipmentReplace).ToArray();
        if (r.ReplacedCardId is { } replaced)
        {
            if (replacements is not [var movement] || movement.CardId != replaced || replaced == r.CardId || _coreAdvancedLifecycleOperations.GetAdvancedCard(replaced).IsGeneralWeapon != r.ReplacedGeneralWeapon || movement.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.ReplacedCardId!.Value), CardLocation.Equipment(target), (r.ReplacedGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile)) || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(movement.CardKind) || _coreEquipmentContentOperations.EquipmentRules.Get(movement.CardKind).Slot != r.Slot)
                throw new InvalidOperationException("Captured equipment placement lost its first replaced physical/generated identity.");
        }
        else if (replacements.Length > 0 || r.ReplacedGeneralWeapon)
            throw new InvalidOperationException("An unoccupied original slot cannot carry a replacement receipt.");
        var payment = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        var movedOx = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind == CardKind.WoodenOx && r.OriginalFrom == CardLocation.Equipment(f.OwnerSeat);
        var replacedOx = r.ReplacedCardId is { } oldOx && _coreAdvancedLifecycleOperations.GetAdvancedCard(oldOx).Kind == CardKind.WoodenOx;
        if (payment.Any(m => !(m.Sequence == r.EntryMovementSequence && m.CardId == r.CardId && m.From == r.OriginalFrom && m.To == CardLocation.Equipment(target) && m.Reason == CardMoveReasons.EquipmentEnter) && !(m.CardId == r.ReplacedCardId && m.From == CardLocation.Equipment(target) && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.ReplacedCardId!.Value), CardLocation.Equipment(target), (r.ReplacedGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile)) && m.Reason == CardMoveReasons.EquipmentReplace) && !(movedOx && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.WoodenOxGrain(target) && m.Reason == CardMoveReasons.WoodenOxTransfer) && !(replacedOx && m.From == CardLocation.WoodenOxGrain(target) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard)))
            throw new InvalidOperationException("Captured equipment placement contains a movement outside its exact equipment/WoodenOx payment.");
        if (r.Stage == CapturedEquipmentDrawStage.Drawn)
        {
            if (r.ActualDrawCount < 0 || r.ActualDrawCount > op.Amount || r.DrawSequenceBefore < r.SequenceAfter || r.DrawSequenceAfter < r.DrawSequenceBefore || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawSequenceBefore && m.Sequence <= r.DrawSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.captured-equipment-reward") != r.ActualDrawCount || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CapturedEquipmentRewardDrawnEvent>().Count(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Requested == op.Amount && e.Actual == r.ActualDrawCount && e.SequenceBefore == r.DrawSequenceBefore && e.SequenceAfter == r.DrawSequenceAfter) != 1 || _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.DrawSequenceBefore && m.Sequence <= r.DrawSequenceAfter && !(m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.captured-equipment-reward") && !(m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
                throw new InvalidOperationException("Captured equipment placement lost its one-time real draw tail.");
        }
    }
}
