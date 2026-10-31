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
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, actor, candidates, policy.SkillId, IsActiveUse: true, f.OwnerSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateMouJwFactionParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.MouJiangWei is not { FactionPolicy: { } policy } r)
            return false;
        if (!ValidMouJw(f) || !r.Accepted || r.Cursor >= r.Targets.Count || r.Stage is not (MouJiangWeiStage.FactionRequest or MouJiangWeiStage.Issuing or MouJiangWeiStage.NativeUse or MouJiangWeiStage.NativeReturned) || !pending.IsAssistedProgramUse || pending.OwnerFrameId != f.Id || pending.ResolutionId != f.Id || pending.ProgramSkillFrameId != f.Id || pending.OwnerSeat != r.Targets[r.Cursor] || pending.TargetSeat != f.OwnerSeat || pending.PolicySource != policy || pending.ProviderFactionId != policy.FactionId || pending.AssistedResultBind != MouJwFactionBind || _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiFactionRequestedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Cursor && e.ActorSeat == pending.OwnerSeat && e.TargetSeat == f.OwnerSeat && e.Policy == policy) != 1)
            throw new InvalidOperationException("Tiaoxin faction use changed its actual requester, original policy or owning target occurrence.");
        return true;
    }

    internal bool MouJwFactionSupplyLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind)
    {
        if (pending.ProgramSkillFrameId is not { } id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is not { MouJiangWei.FactionPolicy: not null } f)
            return true;
        return ValidateMouJwFactionParent(pending, f) && !_coreCardUseModulesOperations.IsCardUseForbidden(pending.OwnerSeat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(pending.OwnerSeat, f.OwnerSeat, kind) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(pending.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly);
    }

    internal bool HasMouJwFactionDistance(int actor, int target) => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsAssistedProgramUse: true, ProgramSkillFrameId: { } id } p && p.OwnerSeat == actor && p.TargetSeat == target && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is { MouJiangWei.FactionPolicy: not null } f && f.OwnerSeat == target && ValidateMouJwFactionParent(p, f);
    internal bool PrepareMouJwFactionNative(ProgramSkillFrame f, NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> cards, CardKind kind, CardConversionSource? conversion)
    {
        if (!ValidateMouJwFactionParent(pending, f))
            return false;
        var r = f.MouJiangWei!;
        var actor = r.Targets[r.Cursor];
        if (r.Stage != MouJiangWeiStage.FactionRequest || cards.Count == 0 || cards.Select(c => c.Id).Distinct().Count() != cards.Count || provider.Seat == actor || !pending.CandidateSeats.Contains(provider.Seat) || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat != provider.Seat) || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(actor, kind, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, f.OwnerSeat, kind))
            throw new InvalidOperationException("Tiaoxin's real faction provider lost its paid material or the actual actor's target legality.");
        var action = new LegalAction(LegalActionKind.Slash, cards[0].Id, f.OwnerSeat, "通过实际势力响应使用杀", kind)
        {
            ConversionSource = conversion
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouJiangWei = r with { Stage = MouJiangWeiStage.Issuing, NativeAction = action, Materials = cards.Select(c => c.Id).ToArray(), ProviderSeat = provider.Seat } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiFactionProvidedEvent(f.Id, r.Cursor, actor, provider.Seat, kind, cards.Count));
        return true;
    }

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
