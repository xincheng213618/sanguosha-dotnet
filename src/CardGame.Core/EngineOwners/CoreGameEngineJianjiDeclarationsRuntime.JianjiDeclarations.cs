#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJianjiDeclarationsRuntime
{
    internal bool HasJianjiNativeUseCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.DrawAndOfferJianjiExactCard);

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 70)]
    internal bool JianjiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasJianjiNativeUseCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 80)]
    internal bool JianjiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasJianjiNativeUseCapability;
    internal bool MatchesJianjiDeclaration(ProgramSkillFrame root, CardDeclarationFrame declaration)
    {
        if (root.HuangQuan is not { Kind: "jianji", Stage: "issuing", NativeUse: { UseId: 0, ActionId: 0 } r } p || !_characterHuangQuanOlOperations.ValidJianjiReceipt(r, root, false) || r.ActorSeat != p.RecipientSeat || declaration.OwnerSeat != r.ActorSeat || declaration.TurnNumber != p.ActualTurn || p.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || p.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || p.ActualPhase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || declaration.Return is not { Purpose: CardDeclarationPurpose.Use, UseAction: { } a } ret || ret.ParentFrameId != root.Id || ret.ParentKind != root.Kind || ret.ParentStep != root.Step || ret.ActorSeat != r.ActorSeat || !_characterHuangQuanOlOperations.SameJianjiReceipt(r, r with { Action = a }) || declaration.Payment is not { Claimed: false } payment || payment.DeclarationId != declaration.Id || payment.OwnerFrameId != declaration.Id || payment.ProviderSeat != r.ActorSeat || payment.ActorSeat != r.ActorSeat || payment.Cost.CardId != r.CardId || payment.Cost.From != CardLocation.Hand(r.ActorSeat) || r.Action.ConversionSource is not { } source || payment.Source != source || source.OwnerSeat != r.ActorSeat || _corePlayCardHistoryOperations.ViewAsRule(source)is not { DeclarationValidation: not null } rule || rule.OutputKind != payment.DeclaredKind || !declaration.TargetSeats.SequenceEqual(a.TargetSeats))
            return false;
        // The generic native declaration invariant checks quota and challenge children.
        // Require the actual unique native commit and material movement as our join proof.
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardDeclarationCommittedEvent>().Count(e => e.DeclarationId == declaration.Id && e.OwnerSeat == r.ActorSeat && e.ActorSeat == r.ActorSeat && e.DeclaredKind == payment.DeclaredKind && e.TargetSeats.SequenceEqual(a.TargetSeats)) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > p.After && m.CardId == r.CardId && m.From == payment.Cost.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay") == 1;
    }

    internal bool IsJianjiDeclarationQuery(CharacterState actor, int? selectedId)
    {
        if (selectedId is not { } id || !actor.IsAlive || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardDeclarationFrame { Stage: CardDeclarationStage.Applying, Succeeded: true } declaration || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame root || !MatchesJianjiDeclaration(root, declaration) || declaration.OwnerSeat != actor.Seat || declaration.Payment.Cost.CardId != id || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing || _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, id) != declaration.Payment)
            return false;
        return true;
    }

    internal bool IsJianjiDeclarationChild(ProgramSkillFrame root, ResolutionFrame child) => child is CardDeclarationFrame declaration && MatchesJianjiDeclaration(root, declaration);
    internal bool HuangQuanDeclarationObserverSuffix(int rootIndex) => _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (index) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || this.IsJianjiDeclarationObserverEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index, dying)));
    internal bool IsJianjiDeclarationObserverEdge(int index)
    {
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardDeclarationFrame d || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == d.Return.ParentFrameId)is not ProgramSkillFrame root || !MatchesJianjiDeclaration(root, d))
            return false;
        if (_resolutionStackOperations._resolutionStack[index] is CardDeclarationChallengeFrame challenge)
            return d.Stage == CardDeclarationStage.Challenging && challenge.ParentFrameId == d.Id && challenge.SeatIndex >= 0 && challenge.SeatIndex < challenge.EligibleSeats.Count && challenge.EligibleSeats.Distinct().Count() == challenge.EligibleSeats.Count && challenge.EligibleSeats.All(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != d.OwnerSeat);
        if (_resolutionStackOperations._resolutionStack[index] is CardsMovedTriggerWindowFrame moved)
            return d.Stage is CardDeclarationStage.Paying or CardDeclarationStage.Cleaning && d.ActiveChildFrameId == moved.Id && moved.ResumeDeclarationFrameId == d.Id && moved.Batch.ParentFrameId == d.Id && moved.Batch.Movements.Count == 1 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.CardId == d.Payment.Cost.CardId && (d.Stage == CardDeclarationStage.Paying ? m.From == d.Payment.Cost.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay" : m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value is "conversion.declaration.failed" or "conversion.declaration.cancelled"));
        return _resolutionStackOperations._resolutionStack[index] is ProgramLifecycleTriggerWindowFrame skills && d.Stage == CardDeclarationStage.Granting && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == d.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeCardDeclaration;
    }

    internal bool TryReturnJianjiDeclaration(CardDeclarationFrame declaration, bool success)
    {
        if (success || declaration.Return.Purpose != CardDeclarationPurpose.Use || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == declaration.Return.ParentFrameId)is not ProgramSkillFrame { HuangQuan: { Kind: "jianji", Stage: "issuing" } p } root)
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == declaration.Id) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != root.Id || declaration.Stage != CardDeclarationStage.Returning || !MatchesJianjiDeclaration(root, declaration) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiUseIssuedEvent>().Any(e => e.Receipt.ProgramFrameId == root.Id) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HuangQuanCompletedEvent>().Any(e => e.FrameId == root.Id))
            throw new InvalidOperationException("Jianji declaration failure lost its original paid request or returned twice.");
        if (declaration.ChallengerSeat is { } challenger && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardDeclarationRevealedEvent>().Count(e => e.DeclarationId == declaration.Id && e.ChallengerSeat == challenger && e.Succeeded == declaration.Succeeded && e.Card.Id == declaration.Payment.Cost.CardId) != 1)
            throw new InvalidOperationException("Jianji declaration failure lost its actual challenged reveal.");
        // A failed challenge produced no Use. Clear only this preliminary use intent;
        // FinishHuangQuan then emits the normal once-only completion with a null action.
        _coreRuntimeOperations.ReplaceRuntimeTop(root = root with { HuangQuan = p with { Stage = "offer", NativeUse = null } });
        _characterHuangQuanOlOperations.FinishHuangQuan(root);
        return true;
    }
}
