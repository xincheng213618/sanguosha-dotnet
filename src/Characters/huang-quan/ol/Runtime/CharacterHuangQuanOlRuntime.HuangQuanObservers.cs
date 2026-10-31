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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 270)]
    internal void RequireIssuedHuangQuan(ProgramSkillFrame f)
    {
        if (f.HuangQuan is not { } p)
            return;
        if (!ValidHuangQuanDraw(f) || p.Stage == "draw-paid" && (f.PendingMovementContinuation is not { SubjectSeat: var target, BeforeCount: 0, CoverageResultBind: null } || target != p.RecipientSeat) || p.Stage != "draw-paid" && f.PendingMovementContinuation is not null || p.Stage is "issuing" or "native-use" or "returned" && (p.NativeUse is not { } r || !ValidJianjiReceipt(r, f, p.Stage != "issuing")))
            throw new InvalidOperationException("Huang Quan lost its exact relation occurrence, actual draw payment, native actor or owning request.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 230)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 230)]
    internal bool CanContinueIssuedHuangQuan(ProgramSkillFrame f)
    {
        if (f.HuangQuan is null)
            return false;
        RequireIssuedHuangQuan(f);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 230)]
    internal bool IsHuangQuanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation continuation) => f.HuangQuan is { Stage: "draw-paid", RecipientSeat: var target } && continuation.SubjectSeat == target && continuation.BeforeCount == 0 && continuation.CoverageResultBind is null && e?.Op is SkillProgramEffectOp.DrawForDianhuActualOccurrence or SkillProgramEffectOp.DrawAndOfferJianjiExactCard && ValidHuangQuanDraw(f);
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

    internal ProgramSkillFrame? HuangQuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterHuangQuanOlOperations.HuangQuanFirstChild(r, c), (i) => _coreJianjiDeclarationsOperations2.HuangQuanDeclarationObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 190)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 300)]
    internal bool HasHuangQuanDamageObserver(long id) => HuangQuanObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 390)]
    internal bool HasHuangQuanBeforeDamageObserver(long id) => HuangQuanObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 160)]
    internal bool IsHuangQuanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HuangQuanObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 300)]
    internal bool TryAdvanceHuangQuanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuangQuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    internal void AssertHuangQuanNativeUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.JianjiUse is not null))
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == use.JianjiUse!.ProgramFrameId)is not ProgramSkillFrame parent || !JianjiNativeUseRidesOn(use, parent))
                throw new InvalidOperationException("Jianji native use lost its exact actual draw owner.");
    }
}
