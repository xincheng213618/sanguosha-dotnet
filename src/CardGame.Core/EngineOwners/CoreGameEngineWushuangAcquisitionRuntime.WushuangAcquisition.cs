#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangAcquisitionRuntime
{
    internal bool TryBeginWushuangAcquisition(long windowId, WushuangShopOffer offer)
    {
        if (offer.TacticId is not { } id || WushuangTactics.ById[id].SkillId is not { } skillId || _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program is not { } program || program.Triggers.SingleOrDefault(t => t.Id == "acquired" && t.Window == SkillProgramTriggerWindow.SkillsChanged)is not { } trigger)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not WushuangIntermissionFrame parent || parent.Id != windowId || parent.Acquisition is not null || !_coreWushuangOperations.RequireWushuangFeature.HasPurchasedOffer(parent.SeatCursor, offer.Id))
            throw new InvalidOperationException("A tactic acquisition lost its paid shop window.");
        var instance = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[parent.SeatCursor], skillId);
        var receipt = new WushuangAcquisitionReceipt(parent.SeatCursor, id, offer.Id, skillId, instance, program.GameplayHash, parent.Opening || !_coreMatchParticipantsPlayersAccess._players[parent.SeatCursor].IsHuman && _coreAiBrainsAiBrainsAccess._aiBrains[parent.SeatCursor].PolicyVersion < AiDecisionPolicy.WushuangStrategyVersion);
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Acquisition = receipt });
        var candidate = new ProgramTriggerCandidate(receipt.Seat, skillId, trigger.Id, instance, program.GameplayHash, trigger.Priority);
        _coreProgramLifecycleOperations.BeginProgramBinding(candidate, new(SkillProgramTriggerWindow.SkillsChanged, windowId, receipt.Seat, SourceSeat: receipt.Seat, TargetSeat: receipt.Seat));
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 62000)]
    internal bool ReturnWushuangAcquisitionBinding(ProgramSkillFrame frame, bool completed)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.SkillsChanged } context || _resolutionStackOperations._resolutionStack.LastOrDefault()is not WushuangIntermissionFrame parent || parent.Acquisition is not { } receipt)
            return false;
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || context.ParentFrameId != parent.Id || frame.TriggerId != "acquired" || frame.OwnerSeat != receipt.Seat || parent.SeatCursor != receipt.Seat || frame.SkillId != receipt.SkillId || frame.SkillInstanceId != receipt.SkillInstanceId || frame.GameplayHash != receipt.GameplayHash || !_coreWushuangOperations.RequireWushuangFeature.HasWushuangTactic(receipt.Seat, receipt.TacticId) || !_coreWushuangOperations.RequireWushuangFeature.HasPurchasedOffer(receipt.Seat, receipt.OfferId, receipt.TacticId))
            throw new InvalidOperationException("A tactic acquisition lost its exact paid parent, source or offer.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { Acquisition = null });
        if (receipt.AdvanceSeat)
            _coreWushuangOperations.AdvanceWushuangShopSeat(parent);
        else
            _coreWushuangOperations.CompleteWushuangShopAction(parent.Id);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62000)]
    internal bool AllowsWushuangAcquiredBinding(ProgramTriggerLegalityContext gate)
    {
        if (gate.Candidate.BindingId != "acquired" || !gate.Candidate.SkillId.StartsWith("wushuang:tactic-", StringComparison.Ordinal))
            return true;
        return gate.Context.Window == SkillProgramTriggerWindow.SkillsChanged && _resolutionStackOperations._resolutionStack.OfType<WushuangIntermissionFrame>().LastOrDefault()is { Acquisition: { } receipt } parent && gate.Context.ParentFrameId == parent.Id && receipt.Seat == gate.Candidate.OwnerSeat && receipt.SkillId == gate.Candidate.SkillId && receipt.SkillInstanceId == gate.Candidate.SkillInstanceId && receipt.GameplayHash == gate.Candidate.GameplayHash;
    }
}
