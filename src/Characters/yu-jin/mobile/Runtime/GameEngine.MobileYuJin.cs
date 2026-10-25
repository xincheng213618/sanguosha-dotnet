namespace CardGame.Core;

public sealed partial class GameEngine
{
    private PublicPersistentPileSource? MobileYuJinPile(int seat, string instance) =>
        _publicPersistentPiles.GetValueOrDefault((seat, MobileYuJinJieyue.Skill, instance));
    private bool HasMobileYuJinPile(int seat, string instance) =>
        HasRuntimeSkillInstance(_players[seat], MobileYuJinJieyue.Skill, instance) && MobileYuJinPile(seat, instance) is { } pile && PublicPileCards(pile).Count > 0;

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 76300)]
    private bool MobileYuJinConversionUnavailable(ProgramConfiguredViewAsQuery query) =>
        query.Instance.SkillId == MobileYuJinJieyue.Skill && !HasMobileYuJinPile(query.Owner.Seat, query.Instance.SkillInstanceId);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 76300)]
    private bool MobileYuJinSubmittedConversionUnavailable(ProgramViewAsLegalityQuery query) =>
        query.Source.SkillId == MobileYuJinJieyue.Skill &&
        (!HasMobileYuJinPile(query.Owner.Seat, query.Source.SkillInstanceId) ||
         query.CardIds.Count != 1 || _cardZones.GetLocation(query.CardIds[0]) != CardLocation.Hand(query.Owner.Seat));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 76300, SkillProgramEffectOp.CollectMobileYuJinJieyue)]
    private bool MobileYuJinPreparationHasCards(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && HasMobileYuJinPile(candidate.OwnerSeat, candidate.SkillInstanceId);

    [ProgramCapability]
    private sealed class MobileYuJinHost(GameEngine engine) : IMobileYuJinHost
    {
        public SkillProgramStepOutcome Transfer(ProgramSkillFrame frame, SkillProgramEffect effect) => engine.BeginMobileYuJinTransfer(frame, effect);
        public bool MatchesChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) =>
            engine.MatchesMobileYuJinChoiceResult(frame, binding);
    }
    private static string MobileYuJinTransferReason(SkillProgramEffectOp op) =>
        op == SkillProgramEffectOp.ResolveMobileYuJinJieyue ? "skill.mobile-yu-jin.jieyue.store" : "skill.mobile-yu-jin.jieyue.retrieve";

    private SkillProgramStepOutcome BeginMobileYuJinTransfer(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var store = effect.Op == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        if (frame.SkillId != MobileYuJinJieyue.Skill || frame.InstructionIndex != (store ? 3 : 1) ||
            frame.TriggerId != (store ? MobileYuJinJieyue.Ending : MobileYuJinJieyue.Preparation) ||
            frame.WindowContext is not { } context || context.Window != (store ? SkillProgramTriggerWindow.TurnEnding : SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) ||
            frame.OwnerSeat != _currentSeat || frame.MobileYuJinTransfer is not null ||
            !_players[frame.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("Jieyue requires its original own actual ending or preparation instruction.");
        if (store) return BeginMobileYuJinChoice(frame);
        var pile = MobileYuJinPile(frame.OwnerSeat, frame.SkillInstanceId);
        if (pile is null || PublicPileCards(pile).Count == 0) return SkillProgramStepOutcome.Continue;
        var ids = PublicPileCards(pile).Select(c => c.Id).ToArray();
        return PayMobileYuJinTransfer(frame, effect.Op, pile, ids, ids.Select(_cardZones.GetLocation).ToArray());
    }
    private SkillProgramStepOutcome PayMobileYuJinTransfer(ProgramSkillFrame frame, SkillProgramEffectOp operation,
        PublicPersistentPileSource pile, int[] ids, CardLocation[] from)
    {
        var store = operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        var originalId = frame.Id;
        var receipt = new MobileYuJinTransferReceipt
        {
            Operation = operation, Source = new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId),
            GameplayHash = frame.GameplayHash, Pile = pile, ActualTurn = _turnNumber, ParentId = frame.WindowContext!.ParentFrameId,
            InstructionIndex = frame.InstructionIndex, Cards = ids, From = from,
            Destination = store ? pile.Location : CardLocation.Hand(frame.OwnerSeat), Before = _cardMovements.Count, After = _cardMovements.Count
        };
        ReplaceRuntimeTop(frame = frame with { MobileYuJinTransfer = receipt, MobileYuJinChoice = null, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        AdvanceEventRulesAndQueueFact(new MobileYuJinTransferStartedEvent(frame.Id, receipt.Operation, receipt.Source,
            receipt.GameplayHash, receipt.ActualTurn, receipt.ParentId, receipt.InstructionIndex, pile, ids.Length, receipt.Before));
        MoveProgramCardsFromMultipleSources(ids, receipt.Destination, new(MobileYuJinTransferReason(operation)), (_, _) =>
        {
            var paid = GetActiveProgramFrame(originalId);
            var invoice = paid.MobileYuJinTransfer! with { After = _cardMovements.Count };
            ReplaceRuntimeTop(paid with { MobileYuJinTransfer = invoice });
            foreach (var item in invoice.Cards.Select((id, index) => (id, from: invoice.From[index])))
            {
                var move = _cardMovements.Single(m => m.Sequence > invoice.Before && m.Sequence <= invoice.After &&
                    m.CardId == item.id && m.From == item.from && m.Reason.Value == MobileYuJinTransferReason(invoice.Operation));
                AdvanceEventRulesAndQueueFact(new MobileYuJinCardTransferredEvent(paid.Id, item.id, item.from, move.To, move.Sequence));
            }
        });
        AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool ValidMobileYuJinTransfer(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinTransfer is not { } r || frame.SkillId != MobileYuJinJieyue.Skill ||
            r.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            r.GameplayHash != frame.GameplayHash || r.ActualTurn != _turnNumber || r.ParentId != frame.WindowContext?.ParentFrameId ||
            r.InstructionIndex != frame.InstructionIndex || !ExactMobileYuJinParent(frame, r.Operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue) || r.Pile.OwnerSeat != frame.OwnerSeat || r.Pile.SkillId != frame.SkillId ||
            r.Pile.SkillInstanceId != frame.SkillInstanceId || r.Pile.Capacity != int.MaxValue || r.Cards.Count == 0 ||
            r.Cards.Count != r.From.Count || r.Cards.Distinct().Count() != r.Cards.Count || r.Before < 0 || r.After <= r.Before || r.After > _cardMovements.Count)
            return false;
        var store = r.Operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        if (!store && r.Operation != SkillProgramEffectOp.CollectMobileYuJinJieyue ||
            frame.TriggerId != (store ? MobileYuJinJieyue.Ending : MobileYuJinJieyue.Preparation) ||
            frame.InstructionIndex != (store ? 3 : 1) || frame.WindowContext?.Window != (store ? SkillProgramTriggerWindow.TurnEnding : SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) ||
            r.Destination != (store ? r.Pile.Location : CardLocation.Hand(frame.OwnerSeat)) ||
            store && (r.Cards.Count != 1 || frame.SelectedTargetSeats is not [var target] ||
                r.From.Any(l => l.OwnerSeat != target || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))) ||
            !store && r.From.Any(l => l != r.Pile.Location)) return false;
        var history = CompleteProgramEventHistory().ToArray();
        if (history.OfType<MobileYuJinTransferStartedEvent>().Count(e => e == new MobileYuJinTransferStartedEvent(frame.Id,
            r.Operation, r.Source, r.GameplayHash, r.ActualTurn, r.ParentId, r.InstructionIndex, r.Pile, r.Cards.Count, r.Before)) != 1 ||
            history.OfType<MobileYuJinCardTransferredEvent>().Count(e => e.FrameId == frame.Id) != r.Cards.Count ||
            history.OfType<MobileYuJinTransferReturnedEvent>().Any(e => e.FrameId == frame.Id)) return false;
        return r.Cards.Select((card, index) => (card, from: r.From[index])).All(item =>
            _cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == item.card && m.From == item.from &&
                (m.To == r.Destination || m.To == CardLocation.OutsideGame) && m.Reason.Value == MobileYuJinTransferReason(r.Operation) &&
                history.OfType<MobileYuJinCardTransferredEvent>().Count(e => e.FrameId == frame.Id && e.CardId == item.card && e.From == item.from && e.To == m.To && e.Sequence == m.Sequence) == 1) == 1);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 76300)]
    private bool ResumeMobileYuJinTransfer(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileYuJinTransfer: { } r } frame || frame.Id != id) return false;
        if (!ValidMobileYuJinTransfer(frame)) throw new InvalidOperationException("Jieyue lost its original paid entity transfer.");
        if (PreparationGameEnded()) return true;
        if (TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id)) return true;
        AdvanceEventRulesAndQueueFact(new MobileYuJinTransferReturnedEvent(frame.Id, r.Cards.Count, r.After));
        ReplaceRuntimeTop(frame with { MobileYuJinTransfer = null, PendingMovementContinuation = null });
        AdvanceRuntimeProgram(frame.Id);
        return true;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 76300)]
    private bool MobileYuJinPaidMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        frame.MobileYuJinTransfer is { } r && effect?.Op == r.Operation && pending == frame.PendingMovementContinuation &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidMobileYuJinTransfer(frame);
    [ProgramContinuation(ProgramContinuationKind.Movement, 76300)]
    private bool ReturnMobileYuJinMovement(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinTransfer is null || frame.PendingMovementContinuation is null) return false;
        if (!ValidMobileYuJinTransfer(frame)) throw new InvalidOperationException("Jieyue movement returned without its original paid receipt.");
        AdvanceRuntimeProgram(frame.Id); return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 76300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 76300)]
    private bool KeepMobileYuJinPaidTransfer(ProgramSkillFrame frame) => frame.MobileYuJinTransfer is not null && ValidMobileYuJinTransfer(frame);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 76300)]
    private void AssertMobileYuJinTransfer(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinChoice is not null && !ValidMobileYuJinChoice(frame))
            throw new InvalidOperationException("Jieyue's unpaid choice must stay on its original participant's ending frame.");
        if (frame.MobileYuJinTransfer is not null)
        {
            if (!ValidMobileYuJinTransfer(frame)) throw new InvalidOperationException("Jieyue's paid transfer must remain on its exact original program frame.");
            return;
        }
        if (CompleteProgramEventHistory().OfType<MobileYuJinTransferStartedEvent>().Any(e => e.FrameId == frame.Id) &&
            !CompleteProgramEventHistory().OfType<MobileYuJinTransferReturnedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("A paid Jieyue transfer cannot lose its original owning receipt.");
    }
}
