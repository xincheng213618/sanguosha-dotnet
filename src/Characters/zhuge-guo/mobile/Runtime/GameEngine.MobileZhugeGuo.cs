namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileQirang = "mobile:qirang", MobileYuhua = "mobile:yuhua", MobileQirangGainReason = "skill-program.mobile-qirang.deck-trick";
    private static CardConversionSource MobileZhugeGuoSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramCapability]
    private sealed class MobileZhugeGuoHost(GameEngine e) : IMobileZhugeGuoHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => e.BeginMobileZhugeGuo(f, op); }

    private CardMovementRecord? MobileQirangEquipmentEntry(int owner, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardsGained || c.MovementBatch is not { } batch || c.MovementIndex is not { } index ||
            index < 0 || index >= batch.Movements.Count || _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == c.ParentFrameId) is not { } parent ||
            parent.Batch.Id != batch.Id || !parent.Batch.Movements.SequenceEqual(batch.Movements)) return null;
        var m = batch.Movements[index];
        return _cardMovements.Contains(m) && m.To == CardLocation.Equipment(owner) && m.From != m.To && EquipmentRules.IsEquipment(GetAdvancedCard(m.CardId).Kind) ? m : null;
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 81800, SkillProgramEffectOp.ObtainMobileQirangTrick, SkillProgramEffectOp.InspectMobileYuhuaTop, SkillProgramEffectOp.Draw)]
    private bool CanOfferMobileZhugeGuo(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId == MobileQirang) return g.Trigger.Id == "equipment-entry" ? MobileQirangEquipmentEntry(g.OwnerSeat, g.Context) is not null :
            g.Trigger.Id == "acquired-trick-use" && MobileQirangCommittedGain(g.Candidate, g.Context) is not null;
        return g.SkillId != MobileYuhua || g.Trigger.Id != "ending-category-top" ||
            g.Owner.HasHpValue && GetHand(g.Owner).Count > g.Owner.Hp && g.Context.Window == SkillProgramTriggerWindow.TurnEnding && g.OwnerSeat == _currentSeat;
    }
    private SkillProgramStepOutcome BeginMobileZhugeGuo(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.MobileQirangGain is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Mobile Zhuge Guo lost her clean actual issuing source.");
        if (op == SkillProgramEffectOp.InspectMobileYuhuaTop)
        {
            var owner = _players[f.OwnerSeat]; var hand = GetHand(owner);
            if (f.SkillId != MobileYuhua || context.Window != SkillProgramTriggerWindow.TurnEnding || f.OwnerSeat != _currentSeat || !owner.HasHpValue || hand.Count <= owner.Hp)
                throw new InvalidOperationException("Mobile Yuhua requires her actual qualified own ending.");
            var count = hand.Select(c => GetProgramCardCategory(c.Kind)).Distinct().Count();
            var result = BeginProgramTopReorder(f, count, null);
            f = GetActiveProgramFrame(f.Id);
            var actual = f.TopReorder?.ViewedCardIds.Count ?? 0;
            AdvanceEventRulesAndQueueFact(new MobileYuhuaPeekStartedEvent(f.Id, MobileZhugeGuoSource(f), f.GameplayHash, context.ParentFrameId, hand.Count, owner.Hp, count, actual));
            if (f.TopReorder is { } top) ReplaceRuntimeTop(f with { TopReorder = top with {
                ViewedCardIds = Array.AsReadOnly(top.ViewedCardIds.ToArray()), TopCardIds = Array.AsReadOnly(top.TopCardIds.ToArray()), BottomCardIds = Array.AsReadOnly(top.BottomCardIds.ToArray()) } });
            return result;
        }
        if (op != SkillProgramEffectOp.ObtainMobileQirangTrick || f.SkillId != MobileQirang || MobileQirangEquipmentEntry(f.OwnerSeat, context) is not { } equipment)
            throw new InvalidOperationException("Mobile Qirang requires an actual equipment entity entering its owner's equipment zone.");
        var trick = _cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(c => GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Trick);
        if (trick is null) return SkillProgramStepOutcome.Continue;
        var r = new MobileQirangGainReceipt(MobileZhugeGuoSource(f), f.GameplayHash, context.MovementBatch!.Id, equipment.Sequence, equipment.CardId,
            trick.Id, trick.Kind, _movementSequence, _movementSequence, 0);
        ReplaceRuntimeTop(f with { MobileQirangGain = r });
        MoveProgramCardsFromMultipleSources([trick.Id], CardLocation.Hand(f.OwnerSeat), new(MobileQirangGainReason), (_, moves) =>
        {
            var current = GetActiveProgramFrame(f.Id); var movement = moves.Single();
            var paid = r with { After = _movementSequence, GainSequence = movement.Sequence };
            ReplaceRuntimeTop(current with { MobileQirangGain = paid }); AdvanceEventRulesAndQueueFact(new MobileQirangGainPaidEvent(f.Id, paid));
        });
        AdvanceRuntimeProgram(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }
    private MobileQirangGainReceipt? CurrentMobileQirangGain(int owner, int card)
    {
        if (_cardZones.GetLocation(card) != CardLocation.Hand(owner)) return null;
        return ProgramEventHistory<MobileQirangGainPaidEvent>().Select(e => e.Receipt).LastOrDefault(r => r.Source.OwnerSeat == owner && r.CardId == card &&
            r.Source.SkillId == MobileQirang && HasRuntimeSkillInstance(_players[owner], r.Source.SkillId, r.Source.SkillInstanceId) &&
            _contentRegistry.GetSkill(MobileQirang).Program!.GameplayHash == r.Hash &&
            !_cardMovements.Any(m => m.CardId == card && m.Sequence > r.GainSequence && m.From == CardLocation.Hand(owner) && m.To != m.From));
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 81800)]
    private void CaptureMobileQirangUse(IGameEvent payload)
    {
        if (payload is not CardUseDeclaredEvent declared || LifecycleCardUse(declared.ResolutionId) is not { Action: { Type: CardActionType.Use } a } use ||
            a.ActorSeat != a.ProviderSeat || a.PhysicalCards is not [var material] || material.From != CardLocation.Hand(a.ActorSeat) ||
            a.EffectiveKind != material.CardKind || GetProgramCardCategory(a.EffectiveKind) != SkillProgramCardCategory.Trick || CurrentMobileQirangGain(a.ActorSeat, material.CardId) is not { } gain) return;
        var issued = new MobileQirangUseReceipt(gain, use.Id, a, _movementSequence);
        UpdateLifecycleCardUse(use.Id, f => f with { MobileQirangUse = issued });
        AdvanceEventRulesAndQueueFact(new MobileQirangUseIssuedEvent(use.Id, a.ActionId, a.ActorSeat, material.CardId, gain.GainSequence, issued.MovementBefore));
    }
    private MobileQirangGainReceipt? MobileQirangCommittedGain(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId && w.Action.ActionId == identity.CardActionId) is not { } window)
            return null;
        var a = window.Action;
        if (a.ActorSeat != c.OwnerSeat || a.ProviderSeat != c.OwnerSeat || a.PhysicalCards is not [var material] ||
            material.From != CardLocation.Hand(c.OwnerSeat) || material.CardKind != a.EffectiveKind || GetProgramCardCategory(a.EffectiveKind) != SkillProgramCardCategory.Trick ||
            a.Type == CardActionType.Response && (a.EffectiveKind != CardKind.Nullification || a.ResponderSeat != a.ActorSeat || a.RequesterSeat is not null ||
                window.CompletedResponseReturn is not { IsCommitted: true, Kind: ProgramCompletedResponseKind.Nullification })) return null;
        if (a.Type == CardActionType.Use)
        {
            var use = _resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == a.ActionId);
            if (use?.MobileQirangUse is not { } q || !SameOverflowAction(q.Action, a) || q.Gain.Source.SkillInstanceId != c.SkillInstanceId || q.Gain.Hash != c.GameplayHash ||
                !MobileQirangUsePaid(use, q)) return null;
            return q.Gain;
        }
        // A Nullification response is a native use even though it has no ordinary
        // CardUseFrame. Its committed window retains the exact accepted material.
        return ProgramEventHistory<MobileQirangGainPaidEvent>().Select(e => e.Receipt).LastOrDefault(r => r.Source.OwnerSeat == c.OwnerSeat && r.CardId == material.CardId &&
            r.Kind == a.EffectiveKind && r.Source.SkillInstanceId == c.SkillInstanceId && r.Hash == c.GameplayHash &&
            _cardMovements.Where(m => m.CardId == r.CardId && m.Sequence > r.GainSequence && m.From == material.From && m.To != m.From).ToArray() is [var paid] &&
            paid.Reason == CardMoveReasons.Nullification);
    }
    private bool MobileQirangUsePaid(CardUseFrame use, MobileQirangUseReceipt q) =>
        q.UseId == use.Id && use.Action is { } a && SameOverflowAction(a, q.Action) && a.PhysicalCards is [var p] && p.CardId == q.Gain.CardId && p.CardKind == q.Gain.Kind &&
        ProgramEventHistory<MobileQirangUseIssuedEvent>().Count(e => e == new MobileQirangUseIssuedEvent(use.Id, a.ActionId, a.ActorSeat, p.CardId, q.Gain.GainSequence, q.MovementBefore)) == 1 &&
        _cardMovements.Count(m => m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.Processing && m.Sequence > q.MovementBefore) == 1;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 81800)]
    private bool MobileQirangDistance(ProgramCardDistanceQuery q)
    {
        if (GetProgramCardCategory(q.Kind) != SkillProgramCardCategory.Trick) return false;
        if (q.CardUseFrameId is { } id && LifecycleCardUse(id) is { MobileQirangUse: { } issued } use && use.SourceSeat == q.Actor.Seat &&
            issued.Action.ActorSeat == q.Actor.Seat && issued.Action.EffectiveKind == q.Kind && MobileQirangUsePaid(use, issued)) return true;
        return q.PhysicalCardIds is [var card] && CurrentMobileQirangGain(q.Actor.Seat, card) is { } gain && gain.Kind == q.Kind;
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 81800)]
    private bool ResumeMobileQirang(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileQirangGain: not null } f || f.Id != id) return false;
        AssertMobileQirangGain(f);
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return true;
        ReplaceRuntimeTop(f with { MobileQirangGain = null }); AdvanceRuntimeProgram(id); return true;
    }
}
