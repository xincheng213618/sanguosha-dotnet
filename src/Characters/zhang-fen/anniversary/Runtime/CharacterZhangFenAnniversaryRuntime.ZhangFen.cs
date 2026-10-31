#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFenAnniversaryRuntime
{
    internal static CardConversionSource ZhangFenSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryZhangFenHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginZhangFen(f, op);
    internal bool ZhangFenSlashDamage(int owner, ProgramSkillWindowContext w) => w.SourceSeat == owner && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is global::CardGame.Core.NativeOwners.CardAttackHandle a && !a.IsSourceLess && a.SourceSeat == owner && w.TargetSeat == a.TargetSeat && a.DamageWasApplied && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveCardKind ?? CardKind.Slash);
    internal int ZhangFenDestroyedCount(int owner, ProgramSkillWindowContext w) => w.MovementBatch is { } batch ? batch.Movements.Where((m, index) => (w.MovementIndex is null || w.MovementIndex == index) && m.From == CardLocation.Equipment(owner) && IsDagongCheDestroyedMaterial(m)).Sum(m => _coreProgramEventHistoryOperations.ProgramEventHistory<DagongCheDestroyedEvent>().Single(e => e.CardId == m.CardId).Upgrades) : 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 43500, SkillProgramEffectOp.ResolveAnniversaryZhangFenWanglu, SkillProgramEffectOp.ResolveAnniversaryZhangFenXianzhu, SkillProgramEffectOp.ResolveAnniversaryDagongCheAttack, SkillProgramEffectOp.ResolveAnniversaryZhangFenChaixie)]
    internal bool CanOfferZhangFen(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id switch
    {
        "preparation-cart-or-play" => HeldDagongChe(c.OwnerSeat)is not null || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].EquipmentSlotCapacity(EquipmentSlot.Treasure) > 0,
        "slash-upgrade" => ZhangFenSlashDamage(c.OwnerSeat, w) && HeldDagongChe(c.OwnerSeat)is { } cart && DagongUpgrades(cart.Id).Length < 5,
        "destroyed-cart-draw" => ZhangFenDestroyedCount(c.OwnerSeat, w) > 0,
        "play-start-slash" => HeldDagongChe(c.OwnerSeat)is { } held && DagongGrant(held.Id) == c.SkillInstanceId && DagongLegalTargets(c.OwnerSeat, DagongUpgrades(held.Id).Any(e => e.Choice == "range")).Length > 0,
        "cart-slash-discard" => ZhangFenSlashDamage(c.OwnerSeat, w) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DagongCheSlash is { } origin && origin.Source.OwnerSeat == c.OwnerSeat && origin.Source.SkillInstanceId == c.SkillInstanceId && w.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(c.OwnerSeat, target),
        _ => false
    };
    internal SkillProgramStepOutcome BeginZhangFen(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var w = f.WindowContext ?? throw new InvalidOperationException("Zhang Fen requires the original native trigger.");
        if (f.ZhangFen is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Zhang Fen requires his original unpaid instruction.");
        var cart = HeldDagongChe(f.OwnerSeat);
        var count = op == SkillProgramEffectOp.ResolveAnniversaryZhangFenChaixie ? ZhangFenDestroyedCount(f.OwnerSeat, w) : 0;
        var stage = op switch
        {
            SkillProgramEffectOp.ResolveAnniversaryZhangFenWanglu => cart is null ? ZhangFenStage.PlaceChildren : ZhangFenStage.ExtraPhase,
            SkillProgramEffectOp.ResolveAnniversaryZhangFenXianzhu => ZhangFenStage.Upgrade,
            SkillProgramEffectOp.ResolveAnniversaryZhangFenChaixie => ZhangFenStage.Drawing,
            _ => w.Window == SkillProgramTriggerWindow.PlayPhaseStarting ? ZhangFenStage.Targets : ZhangFenStage.DiscardChoice
        };
        var source = ZhangFenSource(f);
        if (op == SkillProgramEffectOp.ResolveAnniversaryZhangFenWanglu && cart is null)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].EquipmentSlotCapacity(EquipmentSlot.Treasure) <= 0)
                return SkillProgramStepOutcome.Continue;
            var cardId = _cardZonesOperations._cardZones.CreateDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1;
            cart = new(cardId, CardKind.DagongChe, Suit.Spade, 9);
            _cardZonesOperations._cardZones.AddGeneratedCard(cart);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(cardId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DagongCheCreatedEvent(f.Id, source, f.GameplayHash, cart.Id, f.OwnerSeat));
        }

        var r = new ZhangFenReceipt(source, f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage)
        {
            CartId = cart?.Id ?? 0,
            TargetSeat = w.TargetSeat ?? -1,
            Count = count,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (stage == ZhangFenStage.Targets)
            r = r with
            {
                Count = 1 + DagongUpgrades(r.CartId).Count(e => e.Choice == "target"),
                IgnoreRangeArmor = DagongUpgrades(r.CartId).Any(e => e.Choice == "range")
            };
        if (stage == ZhangFenStage.DiscardChoice)
        {
            var origin = _coreCardUseLifecycleOperations.LifecycleCardUse(_coreProgramAttackAttemptOperations.CurrentDamageAttempt!.ResolutionId)!.DagongCheSlash!;
            r = r with
            {
                CartId = origin.CartId,
                Count = 1 + DagongUpgrades(origin.CartId).Count(e => e.Choice == "discard")
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangFen = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangFenStartedEvent(f.Id, source, f.GameplayHash, op, r.ParentId, r.Turn, r.CartId, r.TargetSeat, r.Count));
        if (stage == ZhangFenStage.ExtraPhase)
            return _coreProgramLifecycleOperations.ScheduleProgramPhase(f, TurnPhase.Play, SkillProgramPhaseContinuation.BeforeNormalPreparation);
        if (stage == ZhangFenStage.PlaceChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreAdvancedWeaponsOperations.PlaceAdvancedEquipment(cart!, f.OwnerSeat);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangFen = f.ZhangFen!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueZhangFen(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == ZhangFenStage.Drawing)
        {
            if (count == 0)
            {
                FinishZhangFen(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("anniversary.chaixie.draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishZhangFen(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainZhangFen(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 43500)]
    internal bool ContinueZhangFen(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangFen: { } r } f || f.Id != id)
            return false;
        if (r.Stage == ZhangFenStage.NativeUse)
        {
            if (f.VirtualBasicDraft is not null)
                return true;
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
                throw new InvalidOperationException("Dagong Che cannot return an unfinished native Use.");
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardId == 0 && e.CardKind == CardKind.Slash) != 1)
                throw new InvalidOperationException("Dagong Che must retain its once-completed native Use before attack cleanup.");
            // PopFinishedCardUse retires the frame that owns ActiveCardAttack.
            // Its typed attack completion is still on the native call stack;
            // this parent must remain until that exact receipt returns.
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangFen = r with { Stage = ZhangFenStage.NativeReturned } });
            return true;
        }

        if (r.Stage == ZhangFenStage.Drawing)
            return true;
        if (r.Stage is ZhangFenStage.PlaceChildren or ZhangFenStage.DiscardChildren or ZhangFenStage.DrawChildren)
        {
            if (DrainZhangFen(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ZhangFen!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage != ZhangFenStage.DiscardChildren || r.Paid >= r.Count || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(f.OwnerSeat, r.TargetSeat))
            {
                FinishZhangFen(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangFen = r with { Stage = ZhangFenStage.DiscardChoice } });
        }

        if (r.Stage == ZhangFenStage.ExtraPhase)
        {
            FinishZhangFen(f);
            return true;
        }

        PublishZhangFen(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 43500)]
    internal bool ReturnZhangFenMovement(ProgramSkillFrame f) => f.ZhangFen is not null && ContinueZhangFen(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryZhangFenChaixie)]
    internal void ReturnZhangFenDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ZhangFen!;
        if (!ValidZhangFen(f) || r.Stage != ZhangFenStage.Drawing || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Count || f.NativeDraw is not null)
            throw new InvalidOperationException("Chaixie lost its original destroyed-cart native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangFen = r with { Stage = ZhangFenStage.DrawChildren, Draw = draw, Before = draw.SequenceBefore, After = draw.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangFenDrawReturnedEvent(f.Id, draw.RequestedCount, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        ContinueZhangFen(f.Id);
    }

    internal void FinishZhangFen(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, ZhangFen = f.ZhangFen!with { Stage = ZhangFenStage.Done } });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
