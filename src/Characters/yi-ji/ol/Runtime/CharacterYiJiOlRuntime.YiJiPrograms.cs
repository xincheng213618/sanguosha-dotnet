#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYiJiOlRuntime
{
    internal SkillProgramStepOutcome BeginGiveDrawPileBottomCard(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            throw new InvalidOperationException("Jijie lost its recipient.");
        var drawPile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
        if (drawPile.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var bottom = drawPile[0];
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 观看牌堆底的一张牌并将其交给 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}。", owner.Seat, recipient);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards([bottom], CardLocation.DrawPile, CardLocation.Hand(recipient), new CardMoveReason($"skill-program.{(frame.SkillId)}.jijie-give"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, [bottom.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    // A real Give can now open the recipient's Jiyuan before this paid instruction
    // returns. Its original cards-moved child already owns the complete invoice.
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 51000)]
    internal bool IsYiJiBottomGiftMovement(ProgramSkillFrame frame, SkillProgramEffect? paidEffect, ProgramMovementContinuation pending) => paidEffect?.Op == SkillProgramEffectOp.GiveDrawPileBottomCard && frame.PendingMovementContinuation == pending && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && frame.SelectedTargetSeats is [var recipient] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => window.ResumeProgramFrameId is null && window.Batch.ParentFrameId == frame.Id && window.Batch.AwaitingProgramFrameId == frame.Id && window.Batch.OriginSkillId == frame.SkillId && window.Batch.OriginSkillInstanceId == frame.SkillInstanceId && window.Batch.OriginOwnerSeat == frame.OwnerSeat && window.Batch.Movements is [var movement] && movement.From == CardLocation.DrawPile && (movement.To == CardLocation.Hand(recipient) || movement.To == CardLocation.OutsideGame || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, CardLocation.Hand(recipient))) && movement.Reason.Value == $"skill-program.{(frame.SkillId)}.jijie-give" && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement));
    SkillProgramStepOutcome global::CardGame.Core.IYiJiProgramHost.GiveDrawPileBottomCard(ProgramSkillFrame frame) => BeginGiveDrawPileBottomCard(frame);
}
