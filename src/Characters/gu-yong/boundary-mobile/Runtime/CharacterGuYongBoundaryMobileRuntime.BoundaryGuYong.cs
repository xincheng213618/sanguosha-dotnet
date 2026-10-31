#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongBoundaryMobileRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryGuYongHost.Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing ? FreezeBoundaryGuYongColors(frame, effect) : PrepareBoundaryGuYongBingyi(frame, effect);
    internal static int BoundaryGuYongColor(Suit suit) => suit == Suit.None ? -1 : global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(suit) ? 1 : 0;
    internal SkillProgramStepOutcome FreezeBoundaryGuYongColors(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "boundary:shenxing-mobile" || frame.TriggerId is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 2 || frame.SelectedTargetSeats.Count != 0 || frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind))
            throw new InvalidOperationException("Mobile Shenxing requires its original unpaid two-card activation.");
        var colors = frame.SelectedCardIds.Select(id =>
        {
            var location = _cardZonesOperations._cardZones.GetLocation(id);
            if (location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Mobile Shenxing lost an original owned hand/equipment payment entity.");
            var card = _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == id);
            return BoundaryGuYongColor(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card));
        }).Distinct().ToArray();
        var outcome = colors.Length > 1 ? "different" : "same";
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, outcome, frame.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryGuYongColorsFrozenEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash, effect.ResultBind!, outcome));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsBoundaryGuYongChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => frame.SkillId == "boundary:shenxing-mobile" && binding.ChooserSeat == frame.OwnerSeat && binding.OptionId is "same" or "different" && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryGuYongColorsFrozenEvent>().Count(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat && fact.SkillInstanceId == frame.SkillInstanceId && fact.GameplayHash == frame.GameplayHash && fact.ResultBind == binding.Name && fact.Outcome == binding.OptionId) == 1 && ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Take(frame.InstructionIndex).Count(effect => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing && effect.ResultBind == binding.Name) == 1;
    internal SkillProgramStepOutcome PrepareBoundaryGuYongBingyi(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        if (frame.SkillId != "boundary:bingyi-mobile" || frame.TriggerId != "ending-reveal-share" || frame.InstructionIndex != 2 || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || hand.Count == 0 || !binding.CardIds.Order().SequenceEqual(hand.Select(card => card.Id).Order()))
            throw new InvalidOperationException("Mobile Bingyi requires its original complete nonempty hand at its own ending window.");
        var sameColor = hand.Select(card => BoundaryGuYongColor(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card))).Distinct().Count() == 1;
        var sameType = hand.Select(card => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind)).Distinct().Count() == 1;
        _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(frame, effect.SourceBind!);
        if (!sameColor && !sameType)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }
}
