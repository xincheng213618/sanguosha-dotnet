#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYunBoundaryMobileRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 60501)]
    internal bool MobileZhaoYunYajiaoGiftMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect is not { Op: SkillProgramEffectOp.GiveMobileYajiaoRevealedCard, SourceBind: "revealed" } || frame.SkillId != "boundary:yajiao-mobile" || frame.InstructionIndex != 3 || pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || frame.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || frame.WindowContext is not { CardUse: { } action, Facts: { OwnerIsTurnPlayer: false, CardActionFromOwnerHand: true } } context || context.Window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || window.Action.ActorSeat != frame.OwnerSeat || window.Action.ActionId != action.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            return false;
        var binding = frame.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind);
        if (binding is not { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var cardId], SourceLocations: [var source] } || source != CardLocation.Processing)
            return false;
        var reveals = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Where(item => item.FrameId == frame.Id).ToArray();
        if (reveals is not [var reveal] || reveal.SkillId != frame.SkillId || reveal.BindingId != frame.TriggerId || reveal.OwnerSeat != frame.OwnerSeat || reveal.Bind != binding.Name || reveal.Cards is not [var card] || card.Id != cardId)
            return false;
        // The native batch is the payment receipt. The delivered entity may move
        // again inside a gain observer, so prove its actual transfer rather than
        // requiring it to remain in the recipient's Hand throughout the child.
        var expectedDisclosure = _modeRulesOperations._modeRules.PublishesCardMovementDisclosure ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.Unspecified;
        return _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Concat(_resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(item => item.Batch)).Any(batch => batch.ParentFrameId == frame.Id && batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId && batch.Movements is [var movement] && movement.CardId == cardId && movement.CardKind == card.Kind && movement.From == source && movement.To == CardLocation.Hand(recipient) && movement.Reason.Value == "skill-program.boundary:yajiao-mobile.GiveMobileYajiaoRevealedCard" && movement.Disclosure == expectedDisclosure && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement));
    }
}
