namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileYuanShaoLuanji = "boundary:luanji-mobile", MobileYuanShaoResponderDraw = "skill-program.boundary:luanji-mobile.responder-draw", MobileYuanShaoNoDamageDraw = "skill-program.boundary:luanji-mobile.undamaged-draw";
    private static string MobileYuanShaoSuitKey(Suit suit) => "mobile-luanji:suit:" + suit;
    private bool MobileYuanShaoSuitAvailable(int owner, Suit suit) => _skillRuntimeState.GetUsage(owner, MobileYuanShaoLuanji, MobileYuanShaoSuitKey(suit), SkillUsageScope.Turn) == 0;

    private bool IsMobileYuanShaoViewAsAllowed(CharacterState owner, IReadOnlyList<int> cardIds, CardConversionSource source) =>
        source.SkillId != MobileYuanShaoLuanji || source.OwnerSeat == owner.Seat && source.BindingId == "unused-suit-pair-as-arrow-barrage" &&
        cardIds.Count == 2 && cardIds.Distinct().Count() == 2 && cardIds.All(id =>
            _cardZones.GetLocation(id) == CardLocation.Hand(owner.Seat) && MobileYuanShaoSuitAvailable(owner.Seat, GetAdvancedCard(id).Suit));

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 66001)]
    private void RecordMobileYuanShaoSuits(ProgramViewAsConvertedEvent fact)
    {
        if (fact.SkillId != MobileYuanShaoLuanji || !fact.IsUse) return;
        // The real paid conversion is always issued. CardActionAccepted for an
        // ordinary trick depends on whether a finalized-target observer exists.
        if (fact.BindingId != "unused-suit-pair-as-arrow-barrage" || fact.OutputKind != CardKind.ArrowBarrage ||
            LifecycleCardUse(fact.FrameId) is not { SourceSeat: var owner, CardKind: CardKind.ArrowBarrage, Action: { Type: CardActionType.Use } action } ||
            owner != fact.OwnerSeat || action.ActorSeat != owner || action.EffectiveKind != CardKind.ArrowBarrage ||
            !action.ConversionChain.Any(c => c.SkillId == fact.SkillId && c.BindingId == fact.BindingId && c.OwnerSeat == owner) ||
            fact.PhysicalCardIds.Count != 2 || fact.PhysicalCardIds.Distinct().Count() != 2 || action.PhysicalCards.Count != 2 ||
            !action.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(fact.PhysicalCardIds.Order()) ||
            action.PhysicalCards.Any(c => c.From != CardLocation.Hand(owner) || _cardZones.GetLocation(c.CardId) != CardLocation.Processing))
            throw new InvalidOperationException("Mobile Luanji lost its exact paid native Arrow conversion and two original hand materials.");
        foreach (var suit in action.PhysicalCards.Select(c => GetAdvancedCard(c.CardId).Suit).Distinct())
        {
            var key = MobileYuanShaoSuitKey(suit);
            if (!_skillRuntimeState.TryConsumeUsage(action.ActorSeat, MobileYuanShaoLuanji, key, SkillUsageScope.Turn, 1))
                throw new InvalidOperationException("Mobile Luanji cannot repeat a material suit in the same actual turn.");
            AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(action.ActorSeat, MobileYuanShaoLuanji, key, SkillUsageScope.Turn, 1));
        }
    }

    [ProgramCapability]
    private sealed class MobileYuanShaoProgramCapability(GameEngine engine) : IMobileYuanShaoProgramHost, IMobileYuanShaoSelectionQueries
    {
        public bool SuitAvailable(int owner, Suit suit) => engine.MobileYuanShaoSuitAvailable(owner, suit);
        public SkillProgramStepOutcome Draw(ProgramSkillFrame frame, bool responder) => engine.DrawMobileYuanShao(frame, responder);
    }

    private int? MobileYuanShaoResponder(ProgramSkillWindowContext context, int owner)
    {
        if (context.Window != SkillProgramTriggerWindow.CardResponseCompleted || context.CardUse is not { } action ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is not { } window ||
            window.ResponseCompletion is not { CostsDrained: true, OriginalContinuation: ProgramCardContinuation.GroupResponse or ProgramCardContinuation.FactionDefenseDodge } ||
            window.Action.ActionId != action.CardActionId || window.Action.EffectiveKind != CardKind.Dodge || window.Action.ProviderSeat == owner ||
            window.Action.ParentActionId is not { } parentAction || LifecycleCardUse(window.ParentFrameId) is not { Action: { Type: CardActionType.Use } parent } use ||
            parent.ActionId != parentAction || parent.EffectiveKind != CardKind.ArrowBarrage || parent.ActorSeat != owner || use.SourceSeat != owner) return null;
        return window.Action.ProviderSeat;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66002, SkillProgramEffectOp.DrawMobileLuanjiResponder, SkillProgramEffectOp.DrawMobileLuanjiNoDamage)]
    private bool CanOfferMobileYuanShaoDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MobileYuanShaoLuanji || !_players[candidate.OwnerSeat].IsAlive) return false;
        if (trigger.Id == "actual-arrow-dodge-draw") return MobileYuanShaoResponder(context, candidate.OwnerSeat) is { } recipient && _players[recipient].IsAlive;
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted && context.CardUse is { EffectiveKind: CardKind.ArrowBarrage, ActorSeat: var actor } && actor == candidate.OwnerSeat &&
            context.Facts?.CardUseCausedDamage == false;
    }

    private SkillProgramStepOutcome DrawMobileYuanShao(ProgramSkillFrame supplied, bool responder)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileYuanShaoLuanji || frame.InstructionIndex != 1 || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Mobile Luanji draw lost its actual response or completed use.");
        var recipient = responder ? MobileYuanShaoResponder(context, frame.OwnerSeat) : frame.OwnerSeat;
        if (recipient is not { } seat || !_players[seat].IsAlive) return SkillProgramStepOutcome.Continue;
        var count = responder ? 1 : context.Facts?.CardUseDesignatedTargetCount ?? 0;
        if (!responder && (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.Facts?.CardUseCausedDamage != false))
            throw new InvalidOperationException("Mobile Luanji's whole-use draw cannot follow damage or a per-target boundary.");
        return count > 0 ? BeginOperationProgramDraw(frame, seat, count, new(responder ? MobileYuanShaoResponderDraw : MobileYuanShaoNoDamageDraw)) : SkillProgramStepOutcome.Continue;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawMobileLuanjiResponder, SkillProgramEffectOp.DrawMobileLuanjiNoDamage)]
    private void ReturnMobileYuanShaoDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        var responder = frame.TriggerId == "actual-arrow-dodge-draw";
        var expected = responder ? MobileYuanShaoResponder(frame.WindowContext!, frame.OwnerSeat) : frame.OwnerSeat;
        var count = responder ? 1 : frame.WindowContext?.Facts?.CardUseDesignatedTargetCount ?? 0;
        if (frame.SkillId != MobileYuanShaoLuanji || frame.InstructionIndex != 1 || outcome.RecipientSeat != expected || outcome.RequestedCount != count)
            throw new InvalidOperationException("Mobile Luanji lost its original paid draw recipient or target count.");
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 66002)]
    private bool MobileYuanShaoDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op is not (SkillProgramEffectOp.DrawMobileLuanjiResponder or SkillProgramEffectOp.DrawMobileLuanjiNoDamage) || frame.SkillId != MobileYuanShaoLuanji || frame.InstructionIndex != 1 ||
            pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || frame.WindowContext is not { } context) return false;
        var responder = effect.Op == SkillProgramEffectOp.DrawMobileLuanjiResponder;
        var recipient = responder ? MobileYuanShaoResponder(context, frame.OwnerSeat) : frame.OwnerSeat;
        var reason = responder ? MobileYuanShaoResponderDraw : MobileYuanShaoNoDamageDraw;
        return recipient is { } seat && _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == frame.Id && batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId &&
            batch.Movements is [var movement] && movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(seat) && movement.Reason.Value == reason && _cardMovements.Contains(movement));
    }
}
