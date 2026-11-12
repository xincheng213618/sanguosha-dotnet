#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaTieAnniversaryRuntime
{
    internal ProgramSkillFrame? AnniversaryMaTieSlotParent(EquipmentSlotNamedUseDraft slot)
    {
        if (slot.Intent != EquipmentSlotNamedUseIntent.Play || slot.OriginalParentId is not { } id || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is not ProgramSkillFrame { AnniversaryMaTie: { Stage: AnniversaryMaTieStage.Issuing, UseId: 0, SelectedAction: { } action, OriginalDecision: { } original } r } f || !ValidMaTie(f) || slot.Source != action.ConversionSource || slot.EffectiveKind != action.PlayedCardKind || r.Actors[r.Cursor] != slot.Source.OwnerSeat || slot.OriginalDecision.PromptId != original.PromptId || slot.PlayAction is null || System.Text.Json.JsonSerializer.Serialize(slot.PlayAction) != System.Text.Json.JsonSerializer.Serialize(action))
            return null;
        return f;
    }

    internal bool MaTiePaymentParent(long paymentId, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == paymentId);
        var accepted = _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == paymentId) switch
        {
            DrawFundedDistinctBasicFrame draw => draw.Payment.RequestAcceptance,
            ChainedStateBasicFrame chain => chain.Payment.RequestAcceptance,
            _ => null
        };
        return accepted is not null && index > 0 &&
            _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame f && f.Id == accepted.Identity.FrameId &&
            f.InstructionIndex == accepted.Identity.InstructionIndex && f.AnniversaryMaTie is
                { Stage: AnniversaryMaTieStage.SlashChoice or AnniversaryMaTieStage.Issuing, UseId: 0 } receipt &&
            accepted.Identity.IssuanceSlot == receipt.Cursor && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) &&
            targets.Contains(receipt.Target) && _slashRequests.OwnsAcceptance(f, accepted, source.OwnerSeat, receipt.Target);
    }

    internal bool IsAnniversaryMaTiePaymentFrame(DrawFundedDistinctBasicFrame paid) => MaTiePaymentParent(paid.Id, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal bool IsAnniversaryMaTiePaymentFrame(ChainedStateBasicFrame paid) => MaTiePaymentParent(paid.Id, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal const string MaTieFactionBind = "anniversary-ma-tie-faction";
    internal bool CanRequestMaTieFaction(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaTie is not { Stage: AnniversaryMaTieStage.SlashChoice, FactionTried: false } r || r.Cursor >= r.Actors.Count || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !ValidMaTie(f))
            return false;
        return _coreAssistedFactionSlashOperations.CanRequestAssistedProgramFactionSlash(r.Actors[r.Cursor], r.Target) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(r.Actors[r.Cursor], SkillProgramCardTargetRestriction.SelfOnly);
    }

    internal void BeginMaTieFaction(ProgramSkillFrame f)
    {
        if (!CanRequestMaTieFaction(f))
            throw new InvalidOperationException("Quxian requires the scheduled actor's real faction supply policy.");
        var r = f.AnniversaryMaTie!;
        var actor = r.Actors[r.Cursor];
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actor], CardKind.Slash)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.Issuing, FactionPolicy = policy, FactionTried = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieFactionEvent(f.Id, r.Cursor, actor, r.Target, policy));
        var candidates = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actor, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, actor, candidates, policy.FactionId, targetSeat: r.Target, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: MaTieFactionBind);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.AuthorizeProgramRequest(f.InstructionIndex, requestIdentity: _slashRequests.GetIdentity(f));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, actor, candidates, policy.SkillId, IsActiveUse: true, r.Target));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateAnniversaryMaTieFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.AnniversaryMaTie is not { FactionPolicy: { } policy } r || f.SkillId != AnniversaryMaTieComposition.Quxian)
            return false;
        if (r.Cursor >= r.Actors.Count || r.Stage is not (AnniversaryMaTieStage.Issuing or AnniversaryMaTieStage.NativeUse or AnniversaryMaTieStage.NativeReturned) || !pending.IsAssistedProgramUse || pending.OwnerFrameId != f.Id || pending.ResolutionId != f.Id || pending.ProgramSkillFrameId != f.Id || pending.OwnerSeat != r.Actors[r.Cursor] || pending.TargetSeat != r.Target || pending.PolicySource != policy || pending.ProviderFactionId != policy.FactionId || pending.AssistedResultBind != MaTieFactionBind || pending.AssistedAuthorization?.RequestIdentity is not { } identity || !_slashRequests.OwnsIssuedIdentity(f, identity, pending.OwnerSeat))
            throw new InvalidOperationException("Quxian faction source lost its exact native policy, actor or original target occurrence.");
        return true;
    }

    internal bool FinishAnniversaryMaTieFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaTie.FactionPolicy: not null } f || f.Id != id)
            return false;
        if (!ValidateAnniversaryMaTieFactionParent(pending, f))
            throw new InvalidOperationException("Quxian faction refusal lost its original request.");
        var r = f.AnniversaryMaTie!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.SlashChoice, FactionPolicy = null } });
        ContinueMaTie(f.Id);
        return true;
    }
}
