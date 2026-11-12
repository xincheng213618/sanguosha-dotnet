#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuJingOlRuntime
{
    internal const string WuJingFactionBind = "heji-native-faction";
    internal bool WuJingFactionTarget(ProgramSkillFrame f) => f.WuJing is { TargetSeat: var target } && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && target != f.OwnerSeat && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(f.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly) && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, target, kind) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]));
    internal void BeginWuJingFaction(ProgramSkillFrame f)
    {
        var r = f.WuJing!;
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Slash);
        if (r.Stage != WuJingStage.UseChoice || policy is null || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !WuJingFactionTarget(f))
            throw new InvalidOperationException("Heji faction request lost its exact same-target Slash need.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuJing = r with { Stage = WuJingStage.FactionRequest, FactionPolicy = policy } });
        var seats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(f.OwnerSeat, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, f.OwnerSeat, seats, policy.FactionId, targetSeat: r.TargetSeat, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: WuJingFactionBind);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.AuthorizeProgramRequest(f.InstructionIndex, ignoreDistance: true, requestIdentity: _slashRequests.GetIdentity(f));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, f.OwnerSeat, seats, policy.SkillId, IsActiveUse: true, r.TargetSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateWuJingFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f) =>
        f.WuJing is { FactionPolicy: { } policy } r && pending.AssistedAuthorization?.RequestIdentity is { } identity &&
        _slashRequests.OwnsIssuedIdentity(f, identity, f.OwnerSeat) && pending.TargetSeat == r.TargetSeat &&
        pending.ProgramSkillFrameId == f.Id && pending.PolicySource == policy && pending.AssistedResultBind == WuJingFactionBind;


    internal bool FinishWuJingFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuJing: { FactionPolicy: not null } } f || f.Id != id)
            return false;
        ValidateWuJingFactionParent(pending, f);
        FinishWuJing(f, false);
        return true;
    }
}
