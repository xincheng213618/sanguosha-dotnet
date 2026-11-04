#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsRuntime
{
    internal CardUseFrame CurrentFinalTargetSlashOwner(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context, TargetSeat: { } target } || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == context.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } owner || action.ActionId != context.CardActionId || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || frame.OwnerSeat != action.ActorSeat || context.ActorSeat != action.ActorSeat || !action.EffectiveDesignatedTargetSeats.Contains(target) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            throw new InvalidOperationException("A current-target operation lost its exact Slash use and target.");
        return owner;
    }

    internal SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame, ProgramParticipantReference? recipient = null)
    {
        var owner = frame.WindowContext is { Window: SkillProgramTriggerWindow.SlashFullyDodged, CardUse: { } use }
            ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(f => f.Id == use.ParentCardUseFrameId)
            : CurrentFinalTargetSlashOwner(frame);
        var target = recipient?.Kind == ProgramParticipantRef.Owner ? frame.OwnerSeat : frame.WindowContext!.TargetSeat!.Value;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
            return SkillProgramStepOutcome.Continue;
        var effectIndex = frame.InstructionIndex - 1;
        var ids = owner.Action!.PhysicalCards.Select(cost => cost.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_nativeMaterialClaims.HasRelinquishedMaterial(owner.Id, id)).ToArray();
        if (ids.Length == 0)
            return SkillProgramStepOutcome.Continue;
        if (!_nativeMaterialClaims.AcceptClaim(owner.Id, frame.Id, effectIndex, ids))
            return SkillProgramStepOutcome.Continue;
        var receipt = new ProgramCurrentUsePhysicalClaim(owner.Id, owner.Action.ActionId, frame.Id, effectIndex, frame.OwnerSeat, target, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, Array.AsReadOnly(ids));
        // Issue the owning receipt before movements: a movement observer can
        // inspect or continue the parent use without treating these entities as leaks.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCurrentUsePhysicalCardsClaimedEvent(receipt));
        var before = _materialMovements.Read().LastOrDefault()?.Sequence ?? 0;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(target), new("skill-program.current-use-physical-claim"));
        _nativeMaterialClaims.CaptureDelivery(owner.Id, _materialMovements.Read().Where(move =>
            move.Sequence > before && ids.Contains(move.CardId) && move.From == CardLocation.Processing &&
            move.To != move.From).ToArray());
        return _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    internal void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame)
    {
        var owner = CurrentFinalTargetSlashOwner(frame);
        var target = frame.WindowContext!.TargetSeat!.Value;
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return;
        var entries = owner.FinalTargetSlashReceipts ?? [];
        var effectIndex = frame.InstructionIndex - 1;
        if (entries.Any(item => item.ProducerFrameId == frame.Id && item.EffectIndex == effectIndex))
            return;
        var receipt = new ProgramTargetSlashReceipt(owner.Action!.ActionId, frame.OwnerSeat, target, owner.Id, frame.Id, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!, effectIndex, true);
        _coreRuntimeOperations.ReplaceRuntimeFrame(owner.Id, owner with { FinalTargetSlashReceipts = Array.AsReadOnly(entries.Append(receipt).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTargetSlashReceiptIssuedEvent(owner.Id, receipt));
    }

    SkillProgramStepOutcome ICurrentUsePhysicalClaimProgramHost.ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame, ProgramParticipantReference? recipient) => ClaimCurrentUsePhysicalCards(frame, recipient);
    void ICurrentUsePhysicalClaimProgramHost.PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => PreventCurrentTargetSlashCancellationByRule(frame);
}
