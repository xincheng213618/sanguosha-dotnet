#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangDeBoundaryMobileRuntime
{
    internal const string BoundaryPangDeJianchu = "boundary:jianchu-mobile", BoundaryPangDeDiscardReason = "skill-program.boundary:jianchu-mobile.SelectAndMoveOwnedCard";
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 65500)]
    internal void CaptureBoundaryPangDeDiscard(CardMovedEvent moved)
    {
        if (moved.Reason.Value != BoundaryPangDeDiscardReason || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.SkillId != BoundaryPangDeJianchu || f.InstructionIndex != 1 || f.WindowContext?.TargetSeat is not { } target || moved.From.OwnerSeat != target || moved.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || moved.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(moved.CardId), moved.From, CardLocation.DiscardPile, moved.Reason))
            return;
        var use = _coreCurrentUsePhysicalClaimsOperations.CurrentFinalTargetSlashOwner(f);
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(m => m.CardId == moved.CardId && m.From == moved.From && m.To == moved.To && m.Reason == moved.Reason);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryPangDeDiscardPaidEvent(f.Id, f.SkillInstanceId, f.GameplayHash, use.Id, use.Action!.ActionId, f.OwnerSeat, target, movement.CardId, movement.CardKind, movement.From, movement.To, movement.Sequence));
    }

    internal bool HasBoundaryPangDePaidDiscard(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryPangDeJianchu || f.TriggerId != "discard-final-target-card" || f.InstructionIndex is < 1 or > 3 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } action, TargetSeat: { } target } || f.CardSetBindings.SingleOrDefault(b => b.Name == "discarded-target-card")is not { CardIds: [var cardId] } binding)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == action.ParentCardUseFrameId);
        if (use?.Action is not { Type: CardActionType.Use } native || !CoreNativeCardClassificationsRuntime.IsSlashCard(native.EffectiveKind) || native.ActionId != action.CardActionId || native.ActorSeat != f.OwnerSeat || action.ActorSeat != f.OwnerSeat || !native.EffectiveDesignatedTargetSeats.Contains(target))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryPangDeDiscardPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid.Instance == f.SkillInstanceId && paid.Hash == f.GameplayHash && paid.Hash == _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash && paid.UseId == use.Id && paid.ActionId == native.ActionId && paid.Owner == f.OwnerSeat && paid.Target == target && paid.CardId == cardId && paid.From.OwnerSeat == target && paid.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && binding.SourceLocations.SequenceEqual([paid.To]) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == paid.MovementSequence && m.CardId == paid.CardId && m.CardKind == paid.Kind && m.From == paid.From && m.To == paid.To && m.Reason.Value == BoundaryPangDeDiscardReason) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65500)]
    internal bool KeepBoundaryPangDePaidDiscard(ProgramSkillFrame f) => HasBoundaryPangDePaidDiscard(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 65500)]
    internal bool ReturnBoundaryPangDePaidDiscard(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryPangDeJianchu || f.InstructionIndex != 1 || f.PendingMovementContinuation is null)
            return false;
        if (!HasBoundaryPangDePaidDiscard(f) || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.WindowContext!.TargetSeat)
            throw new InvalidOperationException("Mobile Jianchu lost its original paid target discard movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool IsBoundaryPangDeUnrespondable(long frameId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == frameId && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(frameId, attack.TargetSeat).Any(r => r.SkillId == BoundaryPangDeJianchu && r.PreventCancellation);
}
