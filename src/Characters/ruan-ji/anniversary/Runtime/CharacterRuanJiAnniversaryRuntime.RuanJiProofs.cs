#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanJiAnniversaryRuntime
{

    internal bool RuanJiDiscardPaid(ProgramSkillFrame f, RuanJiReceipt r) => r.DiscardId > 0 && r.BatchId is { } batch && r.After > r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<RuanJiDiscardPaidEvent>().Count(e => e == new RuanJiDiscardPaidEvent(f.Id, r.DiscardId, batch, r.Before, r.After)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == r.DiscardId && m.Sequence == r.After && m.From == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.anniversary:jiudun.discard" && (m.To == CardLocation.DiscardPile || _coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, CardLocation.Hand(f.OwnerSeat), new("skill-program.anniversary:jiudun.discard")))) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 42800)]
    internal bool RuanJiMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.RuanJi is { Operation: SkillProgramEffectOp.ResolveAnniversaryRuanJiJiudun, Stage: RuanJiStage.DiscardChildren } r && effect?.Op == r.Operation && f.PendingMovementContinuation == pending && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && RuanJiDiscardPaid(f, r);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42800)]
    internal bool ContinuePaidRuanJiAfterSkillLoss(ProgramSkillFrame f)
    {
        if (f.RuanJi is null)
            return false;
        return true;
    }

    [NativeUseValidation(42800)]
    internal void ValidateRuanJiWine(CardUseFrame use)
    {
        if (use.VirtualBasicReturn is not { } returned || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == returned.ParentFrameId)is not { RuanJi: { Stage: RuanJiStage.WineIssued } r } parent)
            return;
        if (use.Id != r.WineId || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || action.ActionId != r.WineActionId || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || action.ParentActionId != r.ActionId || !action.ConversionChain.SequenceEqual([r.Source]) || use.CardId != 0 || use.CardKind != CardKind.Alcohol || use.PhysicalCardIds is not { Count: 0 } || !use.TargetSeats.SequenceEqual([parent.OwnerSeat]) || returned.InstructionIndex != r.InstructionIndex || use.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("Jiudun cannot replace the exact native Alcohol Use or its original typed parent return.");
    }
}
