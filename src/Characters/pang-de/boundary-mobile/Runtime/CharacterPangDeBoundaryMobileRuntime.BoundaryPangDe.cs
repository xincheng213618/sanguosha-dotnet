#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangDeBoundaryMobileRuntime
{
    internal const string BoundaryPangDeJianchu = "boundary:jianchu-mobile", BoundaryPangDeDiscardReason = "skill-program.boundary:jianchu-mobile.SelectAndMoveOwnedCard";
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 65500)]
    internal void CaptureBoundaryPangDeDiscard(CardMovedEvent moved)
    {
        if (moved.Reason.Value != BoundaryPangDeDiscardReason || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.SkillId != BoundaryPangDeJianchu || f.TriggerId != "discard-final-target-card" || f.InstructionIndex != 1 || f.WindowContext is not { TargetSeat: { } target } context || moved.From.OwnerSeat != target || moved.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || moved.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(moved.CardId), moved.From, CardLocation.DiscardPile, moved.Reason))
            return;
        _coreCurrentUsePhysicalClaimsOperations.CurrentFinalTargetSlashOwner(f);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(window => window.Id == context.ParentFrameId);
        var candidate = RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]);
        var accepted = ProgramInvocations.AcceptCardAction(parent, candidate, context, f.Id, retainOwnerDeath: false);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { InvocationAuthorizations = [accepted] });
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { InvocationAuthorization = accepted });
    }

    internal bool IsBoundaryPangDeUnrespondable(long frameId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == frameId && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(frameId, attack.TargetSeat).Any(r => r.SkillId == BoundaryPangDeJianchu && r.PreventCancellation);
}
