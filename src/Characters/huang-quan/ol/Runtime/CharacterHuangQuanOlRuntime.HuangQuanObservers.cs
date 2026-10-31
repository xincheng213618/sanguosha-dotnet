#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanOlRuntime
{
    internal bool ValidHuangQuanDraw(ProgramSkillFrame f)
    {
        if (f.SkillId == "guandu:yuanlve")
            return _coreGuanduGeneralNativeCardsOperations.ValidGuanduYuanlveOffer(f);
        if (f.SkillId == "dragonboat:jianji" && f.HuangQuan is { Stage: "drawing" } waiting)
            return _coreDragonBoatActivitiesOperations.IsDragonBoatActivity && waiting.Source == HuangQuanSource(f) && waiting.GameplayHash == f.GameplayHash && waiting.InstructionIndex == f.InstructionIndex && f.InstructionIndex == 1 && waiting.Kind == "jianji" && waiting.Before == waiting.After && waiting.CardIds.Count == 0 && waiting.Occurrence is null && f.PendingMovementContinuation is null && f.NativeDraw is { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, Operation: SkillProgramEffectOp.DrawAndOfferJianjiExactCard, RequestedCount: 1 } draw && draw.Recipients.SequenceEqual([waiting.RecipientSeat]);
        if (f.HuangQuan is not { } p || p.Source != HuangQuanSource(f) || p.GameplayHash != f.GameplayHash || p.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || p.Kind is not ("jianji" or "dianhu") || p.Stage is not ("draw-paid" or "offer" or "issuing" or "native-use" or "returned") || p.Before < 0 || p.After < p.Before || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || p.CardIds.Count > 1 || p.CardIds.Distinct().Count() != p.CardIds.Count || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (p.Kind == "jianji" ? SkillProgramEffectOp.DrawAndOfferJianjiExactCard : SkillProgramEffectOp.DrawForDianhuActualOccurrence))
            return false;
        if (p.Kind == "jianji" && (f.TriggerId is not null || p.RecipientSeat == f.OwnerSeat || f.SelectedTargetSeats is not [var target] || target != p.RecipientSeat))
            return false;
        if (p.Kind == "dianhu" && (p.Occurrence is not { } r || f.WindowContext?.Dianhu != r || p.RecipientSeat != f.OwnerSeat || r.ActualAmount <= 0 || r.TargetSeat != r.Relation.TargetSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DianhuQualifiedEvent>().Count(e => e.Receipt == r) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DianhuArmedEvent>().Count(e => e.Relation == r.Relation) != 1))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HuangQuanDrawPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.Kind == p.Kind && e.RecipientSeat == p.RecipientSeat && e.ActualTurn == p.ActualTurn && e.TurnOwner == p.TurnOwner && e.ActualPhase == p.ActualPhase && e.Before == p.Before && e.After == p.After && e.CardCount == p.CardIds.Count && e.Occurrence == p.Occurrence) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.To == CardLocation.Hand(p.RecipientSeat) && m.Reason.Value == "program." + p.Kind + ".draw") == p.CardIds.Count && p.CardIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.CardId == id && m.To == CardLocation.Hand(p.RecipientSeat) && m.Reason.Value == "program." + p.Kind + ".draw") == 1);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 230)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 230)]
    internal bool CanContinueIssuedHuangQuan(ProgramSkillFrame f)
    {
        if (f.HuangQuan is null)
            return false;
        return true;
    }
    internal bool HuangQuanFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidHuangQuanDraw(root) || root.HuangQuan is not { } p)
            return false;
        if (_coreJianjiDeclarationsOperations.IsJianjiDeclarationChild(root, child))
            return true;
        if (child is CardUseFrame use)
            return JianjiNativeUseRidesOn(use, root);
        return p.Stage == "draw-paid" && child is CardsMovedTriggerWindowFrame moved && moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > p.Before && m.Sequence <= p.After && p.CardIds.Contains(m.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    }

    internal ProgramSkillFrame? HuangQuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.HuangQuanFirstChild(r, c), (i) => _coreJianjiDeclarationsOperations.HuangQuanDeclarationObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 300)]
    internal bool TryAdvanceHuangQuanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuangQuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
