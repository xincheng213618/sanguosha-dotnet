#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYunBoundaryMobileRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IMobileZhaoYunProgramHost.GiveRevealedCard(ProgramSkillFrame frame, string sourceBind) => GiveMobileZhaoYunRevealedCard(frame, sourceBind);
    internal SkillProgramStepOutcome GiveMobileZhaoYunRevealedCard(ProgramSkillFrame supplied, string sourceBind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, sourceBind);
        if (frame.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || binding.CardIds.Count != 1 || binding.SourceLocations is not [var source] || source != CardLocation.Processing || _cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != source || frame.PendingMovementContinuation is not null)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "涯角展示牌或受牌角色已失效。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(source).Single(c => c.Id == binding.CardIds[0]);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Hand(recipient), new CardMoveReason($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.GiveMobileYajiaoRevealedCard)}"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, frame.OwnerSeat, recipient, binding.CardIds, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence), disclosure: CardMovementDisclosure.FaceUp);
        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return SkillProgramStepOutcome.AwaitChild;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }
}
