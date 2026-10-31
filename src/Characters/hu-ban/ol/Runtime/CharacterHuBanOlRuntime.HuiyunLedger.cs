#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuBanOlRuntime
{
    internal bool HasHuBanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack);

    internal static CardConversionSource HuiyunSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_hu_ban_ol_HuiyunLedger.HuiyunSource(f);
    internal void IssueHuiyunUse(long id, CardActionContext? a)
    {
        if (!HasHuBanCapability || a is not { Type: CardActionType.Use, EffectiveKind: CardKind.FireAttack } || a.ConversionChain.SingleOrDefault(s => s.SkillId == "ol:huiyun" && s.BindingId == "huiyun-fire-attack")is not { } source)
            return;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        HuBanComposition.ValidateProgram(source.SkillId, p);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { HuiyunOrigin: null } u || u.Action?.ActionId != a.ActionId || a.ActorSeat != source.OwnerSeat || a.ProviderSeat != a.ActorSeat || a.RequesterSeat is not null || a.PhysicalCards is not [var cost] || cost.From.OwnerSeat != a.ActorSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != cost.From || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId))
            throw new InvalidOperationException("Huiyun requires its original accepted one-HE FireAttack conversion, actor and live issuing source.");
        var r = new HuiyunUseInvoice(id, a.ActionId, source, p.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, a.ActorSeat, a.ProviderSeat, cost, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { HuiyunOrigin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunUseIssuedEvent(r));
    }

    internal bool HuiyunUseProof(CardUseFrame u)
    {
        if (u.HuiyunOrigin is not { } r || u.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.FireAttack } a || u.Id != r.UseId || u.CardId != r.Material.CardId || u.CardKind != CardKind.FireAttack || u.PhysicalCardIds is not [var id] || id != r.Material.CardId || a.ActionId != r.ActionId || a.ProviderSeat != r.ProviderSeat || a.PhysicalCards is not [var cost] || cost != r.Material || !a.ConversionChain.Contains(r.Source) || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, r.ActorSeat, r.ProviderSeat) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunUseIssuedEvent>().Count(e => e.Invoice == r) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == u.Id && e.SourceSeat == r.ActorSeat && e.CardId == r.Material.CardId && e.CardKind == CardKind.FireAttack) != 1)
            return false;
        var destination = r.Material.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(r.Material.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing;
        return HuiyunPaymentProof(r.UseId, r.ActionId, r.Material.CardId, r.Material.From, destination, CardMoveReasons.Use, r.BeforePayment, false);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27382)]
    internal void RecordHuiyunPayment(IGameEvent fact)
    {
        if (!HasHuBanCapability || fact is not CardMovedEvent e || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()is not { } m || m.CardId != e.CardId || m.From != e.From || m.To != e.To || m.Reason != e.Reason)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.Action is not null).ToArray())
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunPaymentRecordedEvent>().Any(p => p.UseId == use.Id))
                continue;
            if (use.HuiyunOrigin is { } origin && origin.Material.CardId == m.CardId && m.Sequence > origin.BeforePayment && m.From == origin.Material.From && m.Reason == CardMoveReasons.Use && m.To == (m.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.Processing))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunPaymentRecordedEvent(use.Id, origin.ActionId, m.CardId, m.From, m.To, m.Reason, m.Sequence, false));
            else if (use.HuiyunFollowup is { } native && native.CardId == m.CardId && m.Sequence > native.BeforePayment && m.From == CardLocation.Hand(native.ActorSeat) && m.To == CardLocation.Processing && m.Reason == HuiyunNativePaymentReason(native))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunPaymentRecordedEvent(use.Id, native.ActionId, m.CardId, m.From, m.To, m.Reason, m.Sequence, true));
        }
    }

    internal bool HuiyunPaymentProof(long use, long action, int card, CardLocation from, CardLocation to, CardMoveReason reason, long before, bool followup) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunPaymentRecordedEvent>().Where(e => e.UseId == use).ToArray()is [var paid] && paid.ActionId == action && paid.CardId == card && paid.From == from && paid.To == to && paid.Reason == reason && paid.Sequence > before && paid.Followup == followup && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == paid.Sequence && m.CardId == card && m.From == from && m.To == to && m.Reason == reason) == 1;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27380)]
    internal void RecordHuiyunEffect(IGameEvent fact)
    {
        if (!HasHuBanCapability)
            return;
        var id = fact switch
        {
            NullificationResolvedEvent { EffectCardKind: CardKind.FireAttack } e => e.ResolutionId,
            FireAttackCardRevealedEvent e => e.ResolutionId,
            _ => 0
        };
        if (id == 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { HuiyunOrigin: not null } use || !HuiyunUseProof(use) || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count)
            return;
        var round = _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(id);
        var target = use.TargetSeats[use.TargetIndex];
        if (fact is NullificationResolvedEvent)
        {
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunEffectEnteredEvent>().Any(e => e.UseId == id && e.EffectRound == round && e.TargetIndex == use.TargetIndex))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunEffectEnteredEvent(id, round, use.TargetIndex, target));
            return;
        }

        if (fact is FireAttackCardRevealedEvent revealed && revealed.TargetSeat == target && _cardZonesOperations._cardZones.GetLocation(revealed.CardId) == CardLocation.Hand(target))
        {
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunEffectEnteredEvent>().Any(e => e.UseId == id && e.EffectRound == round && e.TargetIndex == use.TargetIndex && e.TargetSeat == target))
                throw new InvalidOperationException("Huiyun reveal lost its exact real native effect.");
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunEffectRevealedEvent>().Any(e => e.UseId == id && e.EffectRound == round && e.TargetIndex == use.TargetIndex))
                throw new InvalidOperationException("A Huiyun effect cannot replace its original displayed entity.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunEffectRevealedEvent(id, round, use.TargetIndex, target, revealed.CardId));
        }
    }

    internal IReadOnlyList<HuiyunOpportunity> HuiyunOpportunities(CardUseFrame u) => Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunEffectEnteredEvent>().Where(e => e.UseId == u.Id && u.TargetSeats.Contains(e.TargetSeat)).Select(e => new HuiyunOpportunity(e.EffectRound, e.TargetIndex, e.TargetSeat, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunEffectRevealedEvent>().SingleOrDefault(r => r.UseId == u.Id && r.EffectRound == e.EffectRound && r.TargetIndex == e.TargetIndex && r.TargetSeat == e.TargetSeat)?.CardId)).ToArray());
    internal bool HuiyunOptionAvailable(int seat, int round, int option) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunOptionSpentEvent>().Any(e => e.Source.OwnerSeat == seat && e.Source.SkillId == "ol:huiyun" && e.RoundNumber == round && e.Option == option);
    internal bool HasHuiyunOption(int seat, int round) => Enumerable.Range(1, 3).Any(i => HuiyunOptionAvailable(seat, round, i));
    internal bool IsIssuedHuiyunCandidate(ProgramTriggerCandidate c) => HasHuBanCapability && c.SkillId == "ol:huiyun" && c.BindingId == "completed-converted-fire-attack" && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash == c.GameplayHash;
    internal CardUseFrame? HuiyunCandidateUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedHuiyunCandidate(c) || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.OwnerSeat != c.OwnerSeat || context.CardUse is not { } card || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, HuiyunOrigin: { } r, Action: { } a } u || !HuiyunUseProof(u) || r.Source.OwnerSeat != c.OwnerSeat || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || card.CardActionId != a.ActionId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != u.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var current = window.Candidates[window.CandidateIndex];
        return current.OwnerSeat == c.OwnerSeat && current.SkillId == c.SkillId && current.TriggerId == c.BindingId && current.SkillInstanceId == c.SkillInstanceId && current.GameplayHash == c.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) == context && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id) == 1 ? u : null;
    }

    internal bool CanRunIssuedHuiyunCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => HuiyunCandidateUse(c, context)is { } u && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && HasHuiyunOption(c.OwnerSeat, u.HuiyunOrigin!.RoundNumber) && HuiyunOpportunities(u).Count > 0 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuiyunStartedEvent>().Any(e => e.Origin.UseId == u.Id);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 19)]
    internal void CollectIssuedHuiyunCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { HuiyunOrigin: { } r } u || action.ActionId != r.ActionId || !HuiyunUseProof(u) || !_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsAlive || !HasHuiyunOption(r.Source.OwnerSeat, r.RoundNumber) || HuiyunOpportunities(u).Count == 0)
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.Triggers.Single();
        if (result.Any(c => c.SkillId == r.Source.SkillId && c.SkillInstanceId == r.Source.SkillInstanceId && c.OwnerSeat == r.Source.OwnerSeat))
            return;
        result.Add(new(r.Source.OwnerSeat, r.Source.OwnerSeat, r.Source.SkillId, t.Id, r.GameplayHash, r.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, r.Source.OwnerSeat, r.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat], action))));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27380)]
    internal bool QualifyHuiyun(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack }] || CanRunIssuedHuiyunCandidate(c, context);
    [ProgramIssuedTrigger(520, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger520(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedHuiyunCandidate(candidate) ? CanRunIssuedHuiyunCandidate(candidate, context) : null;
}
