#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanShaoBoundaryMobileRuntime
{
    internal const string MobileYuanShaoLuanji = "boundary:luanji-mobile", MobileYuanShaoResponderDraw = "skill-program.boundary:luanji-mobile.responder-draw", MobileYuanShaoNoDamageDraw = "skill-program.boundary:luanji-mobile.undamaged-draw";
    internal static string MobileYuanShaoSuitKey(Suit suit) => "mobile-luanji:suit:" + suit;
    internal bool MobileYuanShaoSuitAvailable(int owner, Suit suit) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner, MobileYuanShaoLuanji, MobileYuanShaoSuitKey(suit), SkillUsageScope.Turn) == 0;
    internal bool IsMobileYuanShaoViewAsAllowed(CharacterState owner, IReadOnlyList<int> cardIds, CardConversionSource source) => source.SkillId != MobileYuanShaoLuanji || source.OwnerSeat == owner.Seat && source.BindingId == "unused-suit-pair-as-arrow-barrage" && cardIds.Count == 2 && cardIds.Distinct().Count() == 2 && cardIds.All(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner.Seat) && MobileYuanShaoSuitAvailable(owner.Seat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 66001)]
    internal void RecordMobileYuanShaoSuits(ProgramViewAsConvertedEvent fact)
    {
        if (fact.SkillId != MobileYuanShaoLuanji || !fact.IsUse)
            return;
        // The real paid conversion is always issued. CardActionAccepted for an
        // ordinary trick depends on whether a finalized-target observer exists.
        if (fact.BindingId != "unused-suit-pair-as-arrow-barrage" || fact.OutputKind != CardKind.ArrowBarrage || _coreCardUseLifecycleOperations.LifecycleCardUse(fact.FrameId)is not { SourceSeat: var owner, CardKind: CardKind.ArrowBarrage, Action: { Type: CardActionType.Use } action } || owner != fact.OwnerSeat || action.ActorSeat != owner || action.EffectiveKind != CardKind.ArrowBarrage || !action.ConversionChain.Any(c => c.SkillId == fact.SkillId && c.BindingId == fact.BindingId && c.OwnerSeat == owner) || fact.PhysicalCardIds.Count != 2 || fact.PhysicalCardIds.Distinct().Count() != 2 || action.PhysicalCards.Count != 2 || !action.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(fact.PhysicalCardIds.Order()) || action.PhysicalCards.Any(c => c.From != CardLocation.Hand(owner) || _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Processing))
            throw new InvalidOperationException("Mobile Luanji lost its exact paid native Arrow conversion and two original hand materials.");
        foreach (var suit in action.PhysicalCards.Select(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).Suit).Distinct())
        {
            var key = MobileYuanShaoSuitKey(suit);
            if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(action.ActorSeat, MobileYuanShaoLuanji, key, SkillUsageScope.Turn, 1))
                throw new InvalidOperationException("Mobile Luanji cannot repeat a material suit in the same actual turn.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(action.ActorSeat, MobileYuanShaoLuanji, key, SkillUsageScope.Turn, 1));
        }
    }

    SkillProgramStepOutcome global::CardGame.Core.IMobileYuanShaoProgramHost.Draw(ProgramSkillFrame frame, bool responder) => DrawMobileYuanShao(frame, responder);
    bool global::CardGame.Core.IMobileYuanShaoSelectionQueries.SuitAvailable(int owner, Suit suit) => MobileYuanShaoSuitAvailable(owner, suit);
    internal int? MobileYuanShaoResponder(ProgramSkillWindowContext context, int owner)
    {
        if (context.Window != SkillProgramTriggerWindow.CardResponseCompleted || context.CardUse is not { } action || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ResponseCompletion is not { CostsDrained: true, OriginalContinuation: ProgramCardContinuation.GroupResponse or ProgramCardContinuation.FactionDefenseDodge } || window.Action.ActionId != action.CardActionId || window.Action.EffectiveKind != CardKind.Dodge || window.Action.ProviderSeat == owner || window.Action.ParentActionId is not { } parentAction || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } parent } use || parent.ActionId != parentAction || parent.EffectiveKind != CardKind.ArrowBarrage || parent.ActorSeat != owner || use.SourceSeat != owner)
            return null;
        return window.Action.ProviderSeat;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66002, SkillProgramEffectOp.DrawMobileLuanjiResponder, SkillProgramEffectOp.DrawMobileLuanjiNoDamage)]
    internal bool CanOfferMobileYuanShaoDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MobileYuanShaoLuanji || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        if (trigger.Id == "actual-arrow-dodge-draw")
            return MobileYuanShaoResponder(context, candidate.OwnerSeat)is { } recipient && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive;
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted && context.CardUse is { EffectiveKind: CardKind.ArrowBarrage, ActorSeat: var actor } && actor == candidate.OwnerSeat && context.Facts?.CardUseCausedDamage == false;
    }

    internal SkillProgramStepOutcome DrawMobileYuanShao(ProgramSkillFrame supplied, bool responder)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileYuanShaoLuanji || frame.InstructionIndex != 1 || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Mobile Luanji draw lost its actual response or completed use.");
        var recipient = responder ? MobileYuanShaoResponder(context, frame.OwnerSeat) : frame.OwnerSeat;
        if (recipient is not { } seat || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var count = responder ? 1 : context.Facts?.CardUseDesignatedTargetCount ?? 0;
        if (!responder && (context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.Facts?.CardUseCausedDamage != false))
            throw new InvalidOperationException("Mobile Luanji's whole-use draw cannot follow damage or a per-target boundary.");
        return count > 0 ? _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, seat, count, new(responder ? MobileYuanShaoResponderDraw : MobileYuanShaoNoDamageDraw)) : SkillProgramStepOutcome.Continue;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawMobileLuanjiResponder, SkillProgramEffectOp.DrawMobileLuanjiNoDamage)]
    internal void ReturnMobileYuanShaoDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        var responder = frame.TriggerId == "actual-arrow-dodge-draw";
        var expected = responder ? MobileYuanShaoResponder(frame.WindowContext!, frame.OwnerSeat) : frame.OwnerSeat;
        var count = responder ? 1 : frame.WindowContext?.Facts?.CardUseDesignatedTargetCount ?? 0;
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
