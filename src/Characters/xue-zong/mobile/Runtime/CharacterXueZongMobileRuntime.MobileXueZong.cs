#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXueZongMobileRuntime
{
    // Ordinary tricks already request accepted-action identity for the shared
    // exchange operation. The remade entity reward also needs the native
    // acceptance boundary for basic/equipment uses before their effects.
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 80800)]
    internal bool MobileFunanAcceptsOtherCategoryUses(ProgramTargetCardWindowQuery query) => CardCatalog.Get(query.Kind).CategoryName is "基本牌" or "装备牌" && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && (_coreNationalSkillsOperations.GetSkillBindingShard(p)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted) ?? []).Any(b => b.SkillId == MobileXueZongComposition.Funan));
    internal bool IsMobileFunanActualUse(CardActionContext action)
    {
        if (action.Type == CardActionType.Use)
            return true;
        if (action.EffectiveKind == CardKind.Nullification)
            return _coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Nullification);
        if (!_coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Dodge))
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } kind } attack } && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.TargetSeat == action.ActorSeat;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 80800)]
    internal void CaptureMobileFunanAcceptedAction(CardActionAcceptedEvent accepted)
    {
        var action = accepted.Action;
        if (!_coreMatchParticipantsPlayersAccess._players.Any(p => (_coreNationalSkillsOperations.GetSkillBindingShard(p)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardResponseAccepted) ?? []).Any(b => b.SkillId == MobileXueZongComposition.Funan)))
            return;
        // Capture the direct edge while the real counterspell chain still exists.
        // ParentActionId deliberately retains the outer trick for older consumers.
        if (action.Type == CardActionType.Response && _coreResponseEntityExchangeOperations.ActualRespondedAction(action)is { } answered && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanDirectResponseObservedEvent>().Any(e => e.ResponseActionId == action.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileFunanDirectResponseObservedEvent(action.ActionId, answered.ActionId, action.ActorSeat));
        if (!IsMobileFunanActualUse(action) || action.PhysicalCards.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive)
            return;
        foreach (var binding in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCompleted).Where(b => b.SkillId == MobileXueZongComposition.Funan && b.Trigger.Id == MobileXueZongComposition.Reward))
        {
            if (!_coreResponseEntityExchangeOperations.ResponseExchangeUpgraded(action.ActorSeat, binding.SkillId, binding.SkillInstanceId, MobileXueZongComposition.State))
                continue;
            foreach (var material in action.PhysicalCards)
            {
                var claim = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramResponseEntityClaimedEvent>().LastOrDefault(e => e.OwnerSeat == action.ActorSeat && e.RecipientSeat == action.ActorSeat && e.SkillId == binding.SkillId && e.SkillInstanceId == binding.SkillInstanceId && !e.Restricted && e.ClaimedActionId == e.ResponseActionId && e.CardIds.Contains(material.CardId));
                if (claim is null)
                    continue;
                var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == material.CardId && m.To == CardLocation.Hand(action.ActorSeat) && m.Reason.Value == "skill-program.response-entity-exchange");
                if (movement is null)
                    continue;
                if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanUseEntityCapturedEvent>().Any(e => e.ActionId == action.ActionId && e.ActorSeat == action.ActorSeat && e.SkillInstanceId == binding.SkillInstanceId && e.CardId == material.CardId))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileFunanUseEntityCapturedEvent(action.ActionId, action.ActorSeat, binding.SkillInstanceId, binding.Program.GameplayHash, material.CardId, claim.ClaimedActionId, movement.Sequence));
            }
        }
    }

    internal bool HasMobileFunanUnansweredUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MobileXueZongComposition.Funan || candidate.BindingId != MobileXueZongComposition.Reward || context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } card } || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCompleted || window.Action.ActionId != card.CardActionId || window.Action.ActorSeat != candidate.OwnerSeat || !IsMobileFunanActualUse(window.Action) || window.Action.Type == CardActionType.Response && (window.CompletedResponseReturn is not { IsCommitted: false } || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window)))
            return false;
        if (window.Action.Type == CardActionType.Response && window.Action.EffectiveKind == CardKind.Nullification && (_resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().SingleOrDefault(f => f.Id == window.ParentFrameId)?.MobileFunanCompletions is not { } deferred || deferred.ActiveActionId != window.Action.ActionId || deferred.Cursor < 1 || deferred.Cursor > deferred.Items.Count || deferred.Items[deferred.Cursor - 1] is not { } original || original.Action.ActionId != window.Action.ActionId || original.Source != new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) || original.GameplayHash != candidate.GameplayHash))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanUseEntityCapturedEvent>().Any(e => e.ActionId == window.Action.ActionId && e.ActorSeat == candidate.OwnerSeat && e.SkillInstanceId == candidate.SkillInstanceId && e.GameplayHash == candidate.GameplayHash && window.Action.PhysicalCards.Any(c => c.CardId == e.CardId)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanDirectResponseObservedEvent>().Any(e => e.RespondedActionId == window.Action.ActionId && e.ResponderSeat != candidate.OwnerSeat);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 80800, SkillProgramEffectOp.Draw)]
    internal bool QualifyMobileFunanCompletedUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId != MobileXueZongComposition.Funan || candidate.BindingId != MobileXueZongComposition.Reward || HasMobileFunanUnansweredUse(candidate, context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 80800)]
    internal bool PreserveMobileFunanPaidReward(ProgramSkillFrame frame) => frame is { SkillId: MobileXueZongComposition.Funan, TriggerId: MobileXueZongComposition.Reward, InstructionIndex: 1 or 2, PendingMovementContinuation: not null, WindowContext: { } context } && HasMobileFunanUnansweredUse(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 80800)]
    internal void DeferMobileFunanCounterspellReward(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Nullification || !IsMobileFunanActualUse(action) || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } parent)
            return;
        var own = candidates.Where(c => c.SkillId == MobileXueZongComposition.Funan && c.TriggerId == MobileXueZongComposition.Reward).ToArray();
        candidates.RemoveAll(c => c.SkillId == MobileXueZongComposition.Funan && c.TriggerId == MobileXueZongComposition.Reward);
        var receipt = parent.MobileFunanCompletions ?? new([]);
        var entries = receipt.Items.ToList();
        foreach (var candidate in own)
        {
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanUseEntityCapturedEvent>().Any(e => e.ActionId == action.ActionId && e.ActorSeat == candidate.OwnerSeat && e.SkillInstanceId == candidate.SkillInstanceId && e.GameplayHash == candidate.GameplayHash) || entries.Any(e => e.Action.ActionId == action.ActionId && e.Source.SkillInstanceId == candidate.SkillInstanceId))
                continue;
            entries.Add(new(action, new(candidate.SkillId, candidate.TriggerId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash));
        }

        if (entries.Count != receipt.Items.Count)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MobileFunanCompletions = receipt with { Items = Array.AsReadOnly(entries.ToArray()) } });
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeforeNullificationFinished, 80800)]
    internal bool BeginMobileFunanSettledCounterspellReward(NullificationWindowFrame parent)
    {
        if (parent.MobileFunanCompletions is not { } receipt || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || receipt.Cursor < 0 || receipt.Cursor > receipt.Items.Count)
            throw new InvalidOperationException("Funan's deferred counterspell completion lost its original native window cursor.");
        while (receipt.Cursor < receipt.Items.Count)
        {
            var item = receipt.Items[receipt.Cursor];
            receipt = receipt with
            {
                Cursor = receipt.Cursor + 1,
                ActiveActionId = item.Action.ActionId
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { MobileFunanCompletions = receipt });
            if (item.Source.SkillId != MobileXueZongComposition.Funan || item.Source.BindingId != MobileXueZongComposition.Reward || item.Action.ActorSeat != item.Source.OwnerSeat || item.Action.EffectiveKind != CardKind.Nullification || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(item.Action, ProgramCompletedResponseKind.Nullification) || item.Action.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(parent) || item.Action.OpponentSeat != parent.SourceSeat || _contentRegistryOperations._contentRegistry.GetSkill(item.Source.SkillId).Program is not { } program || program.GameplayHash != item.GameplayHash)
                throw new InvalidOperationException("Funan's deferred completion changed its exact accepted response, source or content.");
            if (!_coreMatchParticipantsPlayersAccess._players[item.Source.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[item.Source.OwnerSeat], item.Source.SkillId, item.Source.SkillInstanceId) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanDirectResponseObservedEvent>().Any(e => e.RespondedActionId == item.Action.ActionId && e.ResponderSeat != item.Source.OwnerSeat))
                continue;
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanUseEntityCapturedEvent>().Any(e => e.ActionId == item.Action.ActionId && e.ActorSeat == item.Source.OwnerSeat && e.SkillInstanceId == item.Source.SkillInstanceId && e.GameplayHash == item.GameplayHash))
                throw new InvalidOperationException("Funan's deferred completion lost its commitment-time acquired entity proof.");
            var trigger = program.Triggers.Single(t => t.Id == item.Source.BindingId);
            var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(item.Action, SkillProgramTriggerWindow.CardUseCompleted, id, item.Source.OwnerSeat, -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[item.Source.OwnerSeat], item.Action));
            context = context with
            {
                CardUse = context.CardUse!with
                {
                    ParentCardUseFrameId = parent.Id
                }
            };
            var candidate = new ProgramCardTriggerCandidate(item.Source.OwnerSeat, -1, item.Source.SkillId, item.Source.BindingId, item.GameplayHash, item.Source.SkillInstanceId, trigger.Priority, context);
            // Other completion observers already ran at their original native
            // boundary. Push only this frozen source's deferred reward, using
            // the existing typed Nullification return to the same parent.
            _coreRuntimeOperations.PushRuntimeFrame(new ProgramCardTriggerWindowFrame(id, parent.Id, item.Action, null, Array.AsReadOnly(new[] { candidate }), CompletedResponseReturn: new(ProgramCompletedResponseKind.Nullification)));
            _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
            return true;
        }

        if (receipt.ActiveActionId is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileFunanCompletions = receipt with { ActiveActionId = null } });
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80800)]
    internal void AssertMobileFunanDeferredCompletions()
    {
        foreach (var parent in _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>())
        {
            if (parent.MobileFunanCompletions is not { } receipt)
                continue;
            if (receipt.Cursor < 0 || receipt.Cursor > receipt.Items.Count || receipt.Items.Count == 0 || receipt.Items.Select(item => (item.Action.ActionId, item.Source)).Distinct().Count() != receipt.Items.Count || receipt.ActiveActionId is { } active && (receipt.Cursor == 0 || receipt.Items[receipt.Cursor - 1].Action.ActionId != active))
                throw new InvalidOperationException("Deferred Funan counterspell rewards lost their unique original response cursor.");
            foreach (var item in receipt.Items)
                if (item.Source.SkillId != MobileXueZongComposition.Funan || item.Source.BindingId != MobileXueZongComposition.Reward || item.Source.OwnerSeat != item.Action.ActorSeat || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(item.Action, ProgramCompletedResponseKind.Nullification) || item.Action.ParentActionId != _coreDelayedNullificationOperations.NullificationParentActionId(parent) || item.Action.OpponentSeat != parent.SourceSeat || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileFunanUseEntityCapturedEvent>().Any(e => e.ActionId == item.Action.ActionId && e.ActorSeat == item.Source.OwnerSeat && e.SkillInstanceId == item.Source.SkillInstanceId && e.GameplayHash == item.GameplayHash && item.Action.PhysicalCards.Any(c => c.CardId == e.CardId)) || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == item.Action.ActionId && e.Action.ActorSeat == item.Action.ActorSeat && e.Action.EffectiveKind == CardKind.Nullification && e.Action.ParentActionId == item.Action.ParentActionId))
                    throw new InvalidOperationException("Deferred Funan counterspell rewards lost their actual accepted use or commitment-time entity source.");
        }
    }
}
