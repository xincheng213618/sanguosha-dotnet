#nullable enable

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
    SkillProgramStepOutcome IYiJiProgramHost.GiveDrawPileBottomCard(ProgramSkillFrame frame) => BeginGiveDrawPileBottomCard(frame);
}
