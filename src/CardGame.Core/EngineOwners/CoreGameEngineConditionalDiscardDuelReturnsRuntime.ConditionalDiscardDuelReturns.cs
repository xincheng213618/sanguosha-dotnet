#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConditionalDiscardDuelReturnsRuntime
{
    internal bool ValidConditionalDuelPayments(ProgramSkillFrame f)
    {
        if (!_coreConditionalDiscardDuelsOperations.Value.ConditionalDuelParentMatches(f) || f.ConditionalDiscardDuel is not { } d || !Enum.IsDefined(d.Stage))
            return false;
        bool Paid(ConditionalDiscardDuelPayment? p, bool owner)
        {
            if (p is null)
                return false;
            var seat = owner ? f.OwnerSeat : d.TargetSeat;
            var reason = owner ? "skill-program.conditional-discard-duel.owner-cost" : "skill-program.conditional-discard-duel.target-cost";
            var moved = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter).ToArray();
            return p.PayerSeat == seat && p.CardId > 0 && p.From.OwnerSeat == seat && p.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && p.SequenceBefore >= 0 && p.SequenceAfter > p.SequenceBefore && moved is [var actual] && actual.CardId == p.CardId && actual.CardKind == p.PrintedKind && actual.From == p.From && actual.Reason.Value == reason && actual.TurnNumber == d.TurnNumber && (actual.To == CardLocation.DiscardPile || p.From.Zone == CardZoneKind.Equipment && actual.To == CardLocation.OutsideGame && (_coreCardAttackStateOperations.GetAttackCard(p.CardId).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(actual))) && (!owner || CoreNativeCardClassificationsRuntime.IsSlashCard(p.PrintedKind) && !_coreCardAttackStateOperations.GetAttackCard(p.CardId).IsGeneralWeapon) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ConditionalDiscardDuelPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.PayerSeat == seat && e.OwnerCost == owner && e.SequenceBefore == p.SequenceBefore && e.SequenceAfter == p.SequenceAfter) == 1;
        }

        if (d.Stage == ConditionalDiscardDuelStage.OwnerChoice)
            return d.OwnerPayment is null && d.TargetPayment is null && d.CardUseFrameId is null && d.OwnerHpAtIssue is null && d.TargetHpAtIssue is null;
        if (!Paid(d.OwnerPayment, true))
            return false;
        if (d.Stage is ConditionalDiscardDuelStage.OwnerChildren or ConditionalDiscardDuelStage.TargetChoice)
            return d.TargetPayment is null && d.CardUseFrameId is null && d.OwnerHpAtIssue is null && d.TargetHpAtIssue is null;
        return Paid(d.TargetPayment, false) && (d.Stage == ConditionalDiscardDuelStage.TargetChildren ? d.CardUseFrameId is null && d.OwnerHpAtIssue is null && d.TargetHpAtIssue is null : d.CardUseFrameId is> 0 && d.OwnerHpAtIssue is { } ownerHp && d.TargetHpAtIssue >= ownerHp && !CoreNativeCardClassificationsRuntime.IsSlashCard(d.TargetPayment!.PrintedKind));
    }

    internal bool MatchesConditionalDuelOrigin(ProgramSkillFrame f, ConditionalDiscardDuelOrigin o) => ValidConditionalDuelPayments(f) && f.ConditionalDiscardDuel is { Stage: ConditionalDiscardDuelStage.DuelIssued } d && f.Id == o.ParentProgramFrameId && o.InstructionIndex == d.InstructionIndex && d.CardUseFrameId == o.CardUseFrameId && o.Source == d.Source && o.GameplayHash == d.GameplayHash && o.TurnNumber == d.TurnNumber && o.TurnOwnerSeat == d.TurnOwnerSeat && o.InitialActorSeat == f.OwnerSeat && o.InitialTargetSeat == d.TargetSeat && o.OwnerHpAtIssue == d.OwnerHpAtIssue && o.TargetHpAtIssue == d.TargetHpAtIssue && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ConditionalDiscardDuelIssuedEvent>().Count(e => e.Origin == (o with { AttackStarted = false })) == 1;
    internal bool IsConditionalDiscardDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ConditionalDiscardDuelOrigin: not null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    [NativeUseReturn(250)]
    internal void ReturnConditionalDiscardDuel(CardUseFrame use)
    {
        if (use.ConditionalDiscardDuelOrigin is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || use.Id != origin.CardUseFrameId || !MatchesConditionalDuelOrigin(f, origin))
            throw new InvalidOperationException("Conditional Duel lost its exact no-attack typed return.");
        _coreConditionalDiscardDuelsOperations.Value.FinishConditionalDiscardDuel(f, true);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 50)]
    internal bool ReturnRegisteredCompleteConditionalDiscardDuelAttackReturn(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ConditionalDiscardDuelReturn is null)
            return false;
        CompleteConditionalDiscardDuelAttackReturn(completion);
        return true;
    }

    internal void CompleteConditionalDiscardDuelAttackReturn(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ConditionalDiscardDuelReturn is not { AttackStarted: true } origin || completion.ResolutionId != origin.CardUseFrameId || completion.ProgramFrameId != origin.ParentProgramFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != origin.ParentProgramFrameId || !MatchesConditionalDuelOrigin(f, origin))
            throw new InvalidOperationException("Conditional Duel lost its exact completed attack return.");
        _coreConditionalDiscardDuelsOperations.Value.FinishConditionalDiscardDuel(f, true);
    }
}
