#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime
{
    internal const string MobileZhuRongSkill = "boundary:lieren-mobile", MobileZhuRongSource = "mobile-lieren-source", MobileZhuRongOpponent = "mobile-lieren-opponent", MobileZhuRongExchange = "skill-program.boundary:lieren-mobile.exchange";
    internal static bool IsMobileZhuRongTargetTrigger(string skillId, SkillProgramTrigger trigger) => skillId == MobileZhuRongSkill && trigger.Id == "contest-on-slash-target";
    internal void CaptureMobileZhuRongPindianReturn(ProgramSkillFrame parent, PindianFrame native)
    {
        if (parent.SkillId != MobileZhuRongSkill)
            return;
        if (parent.InstructionIndex != 1 || parent.TriggerId != "contest-on-slash-target" || native.ParentFrameId != parent.Id || native.ProgramResultBind != "contest" || native.Result is not { } result || result.SourceSeat != parent.OwnerSeat || parent.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: var target } || target != result.OpponentSeat || parent.PindianResultBindings is not [{ Name: "contest", SourceSeat: var source, OpponentSeat: var opponent }] || source != result.SourceSeat || opponent != result.OpponentSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == native.Id && e.SkillId == parent.SkillId && e.Result == result) != 1)
            throw new InvalidOperationException("Mobile Lieren lost its exact public native Pindian return.");
        _coreProgramLifecycleOperations.SetProgramCardSet(parent.Id, MobileZhuRongSource, [result.SourceCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
        _coreProgramLifecycleOperations.SetProgramCardSet(parent.Id, MobileZhuRongOpponent, [result.OpponentCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
    }

    SkillProgramStepOutcome global::CardGame.Core.IMobileZhuRongProgramHost.Exchange(ProgramSkillFrame frame) => ExchangeMobileZhuRongCards(frame);
    internal SkillProgramStepOutcome ExchangeMobileZhuRongCards(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileZhuRongSkill || frame.InstructionIndex != 3 || frame.PendingMovementContinuation is not null || frame.PindianResultBindings is not [{ Name: "contest", SourceSeat: var owner, OpponentSeat: var opponent, SourceWon: false }] || owner != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Lieren exchange lost its non-winning original contest.");
        var sourceCard = _coreProgramLifecycleOperations.GetProgramCardSet(frame, MobileZhuRongSource).CardIds.Single();
        var opponentCard = _coreProgramLifecycleOperations.GetProgramCardSet(frame, MobileZhuRongOpponent).CardIds.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        foreach (var(id, recipient)in new[]
        {
            (sourceCard, opponent),
            (opponentCard, owner)
        }

        )
        {
            if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DiscardPile)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == id);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(recipient), new(MobileZhuRongExchange), disclosure: CardMovementDisclosure.FaceUp);
        }

        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return SkillProgramStepOutcome.AwaitChild;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }
}
