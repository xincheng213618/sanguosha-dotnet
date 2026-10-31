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

    internal bool MaTiePaymentParent(long paymentId, long? parentId, long? requestId, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == paymentId);
        return index > 0 && parentId == requestId && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame f && f.Id == parentId && f.InstructionIndex == cursor && f.AnniversaryMaTie is { Stage: AnniversaryMaTieStage.Issuing, SelectedAction: { } action, OriginalDecision: { } original, UseId: 0, ActionId: 0 } r && ValidMaTie(f) && r.Actors[r.Cursor] == source.OwnerSeat && action.ConversionSource == source && action.PlayedCardKind == kind && action.TargetSeats.SequenceEqual(targets) && targets.Contains(r.Target) && original.PlayerSeat == source.OwnerSeat && original.PromptId.Value > 0 && original.SkillPrompt?.SkillId == f.SkillId;
    }

    internal bool IsAnniversaryMaTiePaymentFrame(DrawFundedDistinctBasicFrame paid) => paid.Payment.Intent == DrawFundedDistinctBasicIntent.Play && MaTiePaymentParent(paid.Id, paid.Payment.ParentFrameId, paid.Payment.RequestFrameId, paid.Payment.Cursor, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal bool IsAnniversaryMaTiePaymentFrame(ChainedStateBasicFrame paid) => paid.Payment.Intent == ChainedStateBasicIntent.Play && MaTiePaymentParent(paid.Id, paid.Payment.ParentFrameId, paid.Payment.RequestFrameId, paid.Payment.Cursor, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal bool TryCancelAnniversaryMaTiePayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaTie: { Stage: AnniversaryMaTieStage.Issuing, UseId: 0 } } f || f.Id != id || !ValidMaTie(f))
            return false;
        NextMaTieActor(f);
        return true;
    }

    internal bool TryDispatchAnniversaryMaTieDeckSlash(ProgramSkillFrame f, Card card, IReadOnlyList<int> targets)
    {
        if (f.AnniversaryMaTie is not { Stage: AnniversaryMaTieStage.SlashChoice } r || f.SkillId != AnniversaryMaTieComposition.Quxian)
            return false;
        var actor = r.Actors[r.Cursor];
        if (targets is not [var target] || target != r.Target || !AnniversaryMaTieRequestActor(f, actor) || f.RequestedDeckBasicMaterial is not { Claiming: true } paid || !_coreRequestedDeckBasicOwnershipOperations.HasValidRequestedDeckBasicMaterial(f) || paid.OwnerFrameId != f.Id || paid.ActorSeat != actor || paid.Cursor != f.InstructionIndex || paid.SelectedCardId != card.Id || paid.SelectedKind != card.Kind)
            throw new InvalidOperationException("Quxian lost its exact selected requested-deck entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        PrepareAnniversaryMaTieSuppliedSlash(f, actor, actor, target, card.Kind, [card], null);
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], card, card.Kind, actor, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: f.Id);
        return true;
    }

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
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, actor, candidates, policy.SkillId, IsActiveUse: true, r.Target));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateAnniversaryMaTieFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.AnniversaryMaTie is not { FactionPolicy: { } policy } r || f.SkillId != AnniversaryMaTieComposition.Quxian)
            return false;
        if (!ValidMaTie(f) || r.Cursor >= r.Actors.Count || r.Stage is not (AnniversaryMaTieStage.Issuing or AnniversaryMaTieStage.NativeUse or AnniversaryMaTieStage.NativeReturned) || !pending.IsAssistedProgramUse || pending.OwnerFrameId != f.Id || pending.ResolutionId != f.Id || pending.ProgramSkillFrameId != f.Id || pending.OwnerSeat != r.Actors[r.Cursor] || pending.TargetSeat != r.Target || pending.PolicySource != policy || pending.ProviderFactionId != policy.FactionId || pending.AssistedResultBind != MaTieFactionBind || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryMaTieFactionEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Cursor && e.Actor == pending.OwnerSeat && e.Target == r.Target && e.Policy == policy) != 1)
            throw new InvalidOperationException("Quxian faction source lost its exact native policy, actor or original target occurrence.");
        return true;
    }

    internal bool AnniversaryMaTieFactionSupplyLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind)
    {
        if (pending.ProgramSkillFrameId is not { } id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is not { AnniversaryMaTie.FactionPolicy: not null } f)
            return true;
        return ValidateAnniversaryMaTieFactionParent(pending, f) && !_coreCardUseModulesOperations.IsCardUseForbidden(pending.OwnerSeat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(pending.OwnerSeat, f.AnniversaryMaTie!.Target, kind) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(pending.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly);
    }

    internal bool PrepareAnniversaryMaTieFactionNative(ProgramSkillFrame f, NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> cards, CardKind kind, CardConversionSource? conversion)
    {
        if (!ValidateAnniversaryMaTieFactionParent(pending, f))
            return false;
        var r = f.AnniversaryMaTie!;
        if (r.Stage != AnniversaryMaTieStage.Issuing || cards.Count == 0 || !pending.CandidateSeats.Contains(provider.Seat) || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat != provider.Seat))
            throw new InvalidOperationException("Quxian faction provider changed its real source materials.");
        return PrepareAnniversaryMaTieSuppliedSlash(f, r.Actors[r.Cursor], provider.Seat, r.Target, kind, cards, conversion);
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
