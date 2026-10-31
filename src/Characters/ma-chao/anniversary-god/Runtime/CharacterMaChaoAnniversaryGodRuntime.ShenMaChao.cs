#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChaoAnniversaryGodRuntime
{
    SkillProgramStepOutcome IShenMaChaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenMaChao(f, op);
    internal static CardConversionSource ShenMcSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal CardActionContext? HengwuAction(ProgramSkillWindowContext? context) => context is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)?.Action;
    internal bool CanHengwu(int owner, CardActionContext? a, SkillProgramTriggerWindow? window) => a is { EffectiveSuit: { } suit } && suit != Suit.None && window == (a.Type == CardActionType.Use ? SkillProgramTriggerWindow.CardUseCommitted : SkillProgramTriggerWindow.CardResponseAccepted) && (a.ActorSeat == owner || a.ProviderSeat == owner) && !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c) == suit) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HengwuDrawIssuedEvent>().Any(e => e.OwnerSeat == owner && e.ActionId == a.ActionId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39000, SkillProgramEffectOp.DrawHengwuFieldSuitEquipment)]
    internal bool CanTriggerHengwu(ProgramTriggerLegalityContext gate) => CanHengwu(gate.OwnerSeat, HengwuAction(gate.WindowContext), gate.WindowContext?.Window);
    internal SkillProgramStepOutcome BeginShenMaChao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenMaChao is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Shen Ma Chao requires his original unpaid native instruction.");
        var r = new ShenMaChaoReceipt(ShenMcSource(f), f.GameplayHash, w.ParentFrameId, op, ShenMaChaoStage.StartingMount);
        if (op == SkillProgramEffectOp.DealShouliStartingMounts)
        {
            if (f.SkillId != ShenMaChaoComposition.Shouli || w.Window != SkillProgramTriggerWindow.GameStarting)
                throw new InvalidOperationException("Shouli's starting mounts require the actual game-start frame.");
            r = r with
            {
                Seats = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, offset)).ToArray()
            };
        }
        else
        {
            if (op != SkillProgramEffectOp.DrawHengwuFieldSuitEquipment || f.SkillId != ShenMaChaoComposition.Hengwu || !CanHengwu(f.OwnerSeat, HengwuAction(w), w.Window))
                throw new InvalidOperationException("Hengwu requires an unpaid actual use or played response and no same-suit hand card.");
            var a = HengwuAction(w)!;
            var suit = a.EffectiveSuit!.Value;
            var count = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Sum(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(p, c) == suit));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HengwuDrawIssuedEvent(f.Id, f.OwnerSeat, a.ActionId, suit, count));
            if (count == 0)
                return SkillProgramStepOutcome.Continue;
            r = r with
            {
                ActionId = a.ActionId,
                Suit = suit,
                DrawCount = count,
                Stage = ShenMaChaoStage.DrawAttempt
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenMaChao = r });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("skill-program.anniversary:hengwu.draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenMaChao = r });
        ContinueShenMaChao(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenMc(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 39000)]
    internal bool ResumeShenMc(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenMaChao: not null } f || f.Id != id)
            return false;
        ContinueShenMaChao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39000)]
    internal bool ReturnShenMcMovement(ProgramSkillFrame f) => f.ShenMaChao is not null && ResumeShenMc(f.Id);
    internal void ContinueShenMaChao(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenMaChao: { } r } f && f.Id == id)
        {
            if (r.Stage is ShenMaChaoStage.DrawAttempt or ShenMaChaoStage.EquipmentUse)
                return;
            if (DrainShenMc(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenMaChao!;
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || r.Stage == ShenMaChaoStage.DrawChildren || r.Cursor >= r.Seats.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenMaChao = null, PendingMovementContinuation = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return;
            }

            if (r.Stage == ShenMaChaoStage.EquipmentChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenMaChao = r with { Stage = ShenMaChaoStage.StartingMount, Cursor = r.Cursor + 1, CardId = 0, UseId = 0 }, PendingMovementContinuation = null });
                continue;
            }

            var seat = r.Seats[r.Cursor];
            var target = _coreMatchParticipantsPlayersAccess._players[seat];
            var candidates = target.IsAlive ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse && target.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(seat, c.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(target, c.Kind, [seat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(seat, c.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(target, c)).OrderBy(c => c.Id).ToArray() : [];
            if (candidates.Length == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenMaChao = r with { Cursor = r.Cursor + 1 } });
                continue;
            }

            var card = candidates[_randomOperations._random.Next(candidates.Length)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenMaChao = r with { CardId = card.Id, Stage = ShenMaChaoStage.EquipmentUse }, PendingMovementContinuation = new(seat, 0, null) });
            var useId = _coreNativeCardUseOperations.BeginCardUse(card, seat, [], conversionSource: r.Source);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShenMaChao = f.ShenMaChao!with { UseId = useId } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShouliStartingMountIssuedEvent(f.Id, r.Cursor, seat, card.Id, useId));
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.EquipmentUse);
            if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(target, card, useId))
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(target, card, useId);
            return;
        }
    }

    [NativeUseReturn(39000)]
    internal void ContinueShouliStartingMountReturn()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenMaChao: { Stage: ShenMaChaoStage.EquipmentUse } r } f)
            return;
        if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Any(e => e.ResolutionId == r.UseId && e.CardId == r.CardId))
            throw new InvalidOperationException("Shouli's starting mount did not return its exact native equipment use.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShouliStartingMountFinishedEvent(f.Id, r.Cursor, r.Seats[r.Cursor], r.CardId, r.UseId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenMaChao = r with { Stage = ShenMaChaoStage.EquipmentChildren } });
        ContinueShenMaChao(f.Id);
    }

    internal bool IsShouliStartingEquipmentRide(ResolutionFrame child, ResolutionFrame parent) => parent is ProgramSkillFrame { ShenMaChao: { Operation: SkillProgramEffectOp.DealShouliStartingMounts, Stage: ShenMaChaoStage.EquipmentUse } r } f && child is CardUseFrame use && use.Id == r.UseId && use.CardId == r.CardId && use.SourceSeat == r.Seats[r.Cursor] && use.Action is { } a && a.ActorSeat == use.SourceSeat && a.ConversionChain.SequenceEqual([r.Source]) && a.PhysicalCards is [{ CardId: var cardId, From: var from }] && cardId == r.CardId && from == CardLocation.DrawPile && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && _coreEquipmentContentOperations.EquipmentRules.Get(use.CardKind).Slot is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse && r.Source == ShenMcSource(f);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawHengwuFieldSuitEquipment)]
    internal void ReturnHengwuDraw(ProgramSkillFrame supplied, NativeDrawOutcome o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenMaChao ?? throw new InvalidOperationException("Hengwu lost its original draw owner.");
        if (r.Stage != ShenMaChaoStage.DrawAttempt || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.DrawCount || o.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HengwuDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Hengwu cannot change or repay its frozen native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenMaChao = r with { Stage = ShenMaChaoStage.DrawChildren, Before = o.SequenceBefore, After = o.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HengwuDrawReturnedEvent(f.Id, f.OwnerSeat, r.DrawCount, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter));
        ContinueShenMaChao(f.Id);
    }
}
