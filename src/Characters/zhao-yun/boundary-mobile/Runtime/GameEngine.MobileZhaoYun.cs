namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileZhaoYunProgramCapability(GameEngine engine) : IMobileZhaoYunProgramHost
    {
        public SkillProgramStepOutcome GiveRevealedCard(ProgramSkillFrame frame, string sourceBind) =>
            engine.GiveMobileZhaoYunRevealedCard(frame, sourceBind);
    }

    private SkillProgramStepOutcome GiveMobileZhaoYunRevealedCard(ProgramSkillFrame supplied, string sourceBind)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var binding = GetProgramCardSet(frame, sourceBind);
        if (frame.SelectedTargetSeats is not [var recipient] || !IsValidPlayerSeat(recipient) ||
            !_players[recipient].IsAlive || !_players[frame.OwnerSeat].IsAlive || binding.CardIds.Count != 1 ||
            binding.SourceLocations is not [var source] || source != CardLocation.Processing ||
            _cardZones.GetLocation(binding.CardIds[0]) != source || frame.PendingMovementContinuation is not null)
        {
            CancelProgramBindingAndCleanup(frame, "涯角展示牌或受牌角色已失效。");
            return SkillProgramStepOutcome.AwaitChild;
        }
        var card = _cardZones.CardsAt(source).Single(c => c.Id == binding.CardIds[0]);
        ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        MoveCard(card, source, CardLocation.Hand(recipient),
            new CardMoveReason($"skill-program.{frame.SkillId}.{SkillProgramEffectOp.GiveMobileYajiaoRevealedCard}"),
            NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, binding.CardIds, _movementSequence),
            disclosure: CardMovementDisclosure.FaceUp);
        if (TryBeginCardsMovedProgramWindow(frame.Id)) return SkillProgramStepOutcome.AwaitChild;
        ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id) with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }
}
