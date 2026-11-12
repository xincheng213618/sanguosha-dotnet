#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouMobileRuntime
{
    internal const string MouJwFactionBind = "mou-jiang-wei-tiaoxin-faction";
    internal bool CanRequestMouJwFaction(ProgramSkillFrame f)
    {
        if (f.MouJiangWei is not { Stage: MouJiangWeiStage.Responding, Accepted: true, FactionTried: false } r || r.Cursor >= r.Targets.Count || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !ValidMouJw(f))
            return false;
        var actor = r.Targets[r.Cursor];
        var source = _coreMatchParticipantsPlayersAccess._players[actor];
        var target = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        return source.IsAlive && target.IsAlive && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(source, CardKind.Slash)is not null && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target.Seat, kind) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, kind, CardActionType.Use) && _coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(source, target, kind, null, null)) && _coreRuleQueriesOperations.CanSpendSlashUse(source, target, ignoresCount: true, kind));
    }

    internal void BeginMouJwFaction(ProgramSkillFrame f)
    {
        if (!CanRequestMouJwFaction(f))
            throw new InvalidOperationException("Tiaoxin requires the responder's current legal native faction-request ability.");
        var r = f.MouJiangWei!;
        var actor = r.Targets[r.Cursor];
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actor], CardKind.Slash)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Stage = MouJiangWeiStage.FactionRequest, FactionPolicy = policy, FactionTried = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiFactionRequestedEvent(f.Id, r.Cursor, actor, f.OwnerSeat, policy));
        var candidates = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actor, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, actor, candidates, policy.FactionId, targetSeat: f.OwnerSeat, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: MouJwFactionBind);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.AuthorizeProgramRequest(f.InstructionIndex, ignoreDistance: true, requestIdentity: _slashRequests.GetIdentity(f));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, actor, candidates, policy.SkillId, IsActiveUse: true, f.OwnerSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateMouJwFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f) =>
        f.MouJiangWei is { FactionPolicy: { } policy } r && r.Cursor < r.Targets.Count &&
        pending.AssistedAuthorization?.RequestIdentity is { } identity &&
        _slashRequests.OwnsIssuedIdentity(f, identity, r.Targets[r.Cursor]) && pending.TargetSeat == f.OwnerSeat &&
        pending.ProgramSkillFrameId == f.Id && pending.PolicySource == policy && pending.AssistedResultBind == MouJwFactionBind;


    internal bool FinishMouJwFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangWei.FactionPolicy: not null } f || f.Id != id)
            return false;
        if (!ValidateMouJwFactionParent(pending, f) || f.MouJiangWei is not { Stage: MouJiangWeiStage.FactionRequest } r)
            throw new InvalidOperationException("Tiaoxin faction failure has no matching unfinished request.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiFactionDeclinedEvent(f.Id, r.Cursor, r.Targets[r.Cursor]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Stage = MouJiangWeiStage.Responding, FactionPolicy = null } });
        ContinueMouJw(f.Id);
        return true;
    }
}
