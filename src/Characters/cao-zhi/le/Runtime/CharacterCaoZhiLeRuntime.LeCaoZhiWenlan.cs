#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiLeRuntime
{
    internal const string WenlanSkill = "le:wenlan";
    internal const string WenlanUseBinding = "after-pair-use";
    internal const string WenlanPlayedBinding = "after-pair-played";
    internal const string WenlanObtainReason = "skill-program.le-cao-zhi.wenlan.obtain";
    internal IReadOnlyList<int> WenlanHandLikeIds(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[owner])).Select(card => card.Id).ToArray();
    internal bool IsWenlanHandLike(int owner, int cardId) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOwnedPlayableLocation(_coreMatchParticipantsPlayersAccess._players[owner], _cardZonesOperations._cardZones.GetLocation(cardId));
    internal bool HasLeCaoZhiWenlanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveLeCaoZhiWenlan);

    internal static WenlanSource WenlanFrameSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cao_zhi_le_LeCaoZhiWenlan.WenlanFrameSource(f);
    internal WenlanSource? CurrentWenlanSource(int owner, SkillProgramTriggerWindow window)
    {
        if (!HasLeCaoZhiWenlanCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return null;
        var binding = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).GetInstanceTriggers(window).Where(b => b.SkillId == WenlanSkill && LeCaoZhiComposition.IsWenlanProgram(b.Program)).OrderBy(b => b.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        return binding is null ? null : new(new(binding.SkillId, binding.Trigger.Id, owner, binding.SkillInstanceId), binding.Program.GameplayHash);
    }

    internal WenlanActionCapture? CaptureWenlanMaterials(int owner, IReadOnlyList<int> ids, SkillProgramTriggerWindow window)
    {
        if (CurrentWenlanSource(owner, window)is not { } source)
            return null;
        return new(source, ids.Count, ids is [var id] && id > 0 ? CaptureLeCaoZhiFuEntity(id) : null);
    }

    internal WenlanActionAcceptedEvent[] WenlanEntries(int owner)
    {
        var retired = _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionReassignedEvent>().Where(e => e.RetiredEntryId is not null).Select(e => e.RetiredEntryId!.Value).ToHashSet();
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionAcceptedEvent>().Where(e => e.OwnerSeat == owner && !retired.Contains(e.EntryId)).ToArray();
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionReassignedEvent>().Any(e => e.PreviousActorSeat == owner || e.ActorSeat == owner))
            return entries.OrderBy(e => e.Ordinal).ToArray();
        // Issued pairs never move or roll back. Only still-unclaimed entries
        // form the remaining tail when a real native actor replacement occurs.
        var frozen = new Dictionary<long, int>();
        foreach (var q in _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Where(e => e.Qualification.Source.Source.OwnerSeat == owner).Select(e => e.Qualification))
        {
            frozen.Add(q.FirstEntryId, q.SecondOrdinal - 1);
            frozen.Add(q.SecondEntryId, q.SecondOrdinal);
        }

        var next = frozen.Count == 0 ? 0 : frozen.Values.Max();
        return entries.Where(e => frozen.ContainsKey(e.EntryId)).Select(e => e with { Ordinal = frozen[e.EntryId] }).Concat(entries.Where(e => !frozen.ContainsKey(e.EntryId)).OrderBy(e => e.EntryId).Select(e => e with { Ordinal = ++next })).OrderBy(e => e.Ordinal).ToArray();
    }

    internal int NextWenlanOrdinal(int owner) => WenlanEntries(owner).Select(e => e.Ordinal).DefaultIfEmpty(0).Max() + 1;
    internal bool IsWenlanEntryClaimed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.FirstEntryId == id || e.Qualification.SecondEntryId == id);
    internal long WenlanBoundActionId(WenlanActionAcceptedEvent entry) => entry.ActionId != 0 ? entry.ActionId : _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanLegacyActionBoundEvent>().SingleOrDefault(e => e.EntryId == entry.EntryId)?.ActionId ?? 0;
    internal void RecordWenlanAccepted(CardActionContext action, long nativeId, WenlanActionCapture? capture)
    {
        var owner = action.Type == CardActionType.Response ? action.ProviderSeat : action.ActorSeat;
        if (capture?.Source is not { } source || source.Source.OwnerSeat != owner || capture.MaterialCount != action.PhysicalCards.Count)
            return;
        if (WenlanEntries(owner).Any(e => WenlanBoundActionId(e) == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanActionAcceptedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner, NextWenlanOrdinal(owner), action.ActionId, nativeId, action.Type, action.ProviderSeat, action.EffectiveKind, capture));
    }

    internal void PrepareLeCaoZhiWenlanResponse(CharacterState provider, CardKind kind, IReadOnlyList<Card> cards)
    {
        var ids = cards.Select(c => c.Id).ToArray();
        if (CaptureWenlanMaterials(provider.Seat, ids, SkillProgramTriggerWindow.CardResponseCompleted)is not { } capture)
            return;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() ?? throw new InvalidOperationException("Wenlan response needs its actual payment owner.");
        if (parent.LeCaoZhiWenlanResponse is not null)
            throw new InvalidOperationException("Wenlan cannot prepare another unpaid response invoice.");
        var p = new WenlanResponseCapture(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, provider.Seat, kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, capture)
        {
            Materials = cards.Select(c => new CardActionCost(c.Id, c.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(provider, c))).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LeCaoZhiWenlanResponse = p });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanResponsePreparedEvent(p.CaptureId, p.NativeParentId, provider.Seat, kind, p.Before, capture));
        for (var i = 0; i < p.Materials.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanResponseMaterialEvent(p.CaptureId, i, p.Materials[i]));
    }

    internal WenlanActionCapture? BindLeCaoZhiWenlanResponse(CardActionContext action, out long parentId)
    {
        parentId = 0;
        var matches = _resolutionStackOperations._resolutionStack.Where(f => f.LeCaoZhiWenlanResponse is { } p && p.ProviderSeat == action.ProviderSeat && p.EffectiveKind == action.EffectiveKind && p.Materials.Select(c => c.CardId).SequenceEqual(action.PhysicalCards.Select(c => c.CardId))).ToArray();
        if (matches.Length == 0)
            return CaptureWenlanMaterials(action.ProviderSeat, action.PhysicalCards.Select(c => c.CardId).ToArray(), SkillProgramTriggerWindow.CardResponseCompleted);
        if (matches is not [var parent] || parent.LeCaoZhiWenlanResponse is not { } payment || parent.Id != payment.NativeParentId || payment.Materials.Count != action.PhysicalCards.Count || !payment.Materials.Zip(action.PhysicalCards).All(pair => pair.First.CardId == pair.Second.CardId && pair.First.CardKind == pair.Second.CardKind && pair.First.From == pair.Second.From) || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanResponsePreparedEvent>().Count(e => e == new WenlanResponsePreparedEvent(payment.CaptureId, parent.Id, payment.ProviderSeat, payment.EffectiveKind, payment.Before, payment.Capture)) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanResponseMaterialEvent>().Where(e => e.CaptureId == payment.CaptureId).OrderBy(e => e.Index).Select(e => e.Material).SequenceEqual(payment.Materials) || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanResponseBoundEvent>().Any(e => e.CaptureId == payment.CaptureId) || payment.Materials.Any(c => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > payment.Before && m.CardId == c.CardId && m.CardKind == c.CardKind && m.From == c.From && m.Reason is var reason && (reason == CardMoveReasons.Respond || reason == CardMoveReasons.Nullification))))
            throw new InvalidOperationException("Wenlan requires its exact original, once-paid response materials.");
        parentId = parent.Id;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LeCaoZhiWenlanResponse = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanResponseBoundEvent(payment.CaptureId, action.ActionId));
        return payment.Capture;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29081)]
    internal void ObserveLeCaoZhiWenlanAcceptance(IGameEvent payload)
    {
        if (!HasLeCaoZhiWenlanCapability)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use)
        {
            IReadOnlyList<int> ids = use.Action is { } declaredAction ? declaredAction.PhysicalCards.Select(c => c.CardId).ToArray() : use.PhysicalCardIds ?? [];
            // Capture material names even before a later native actor replacement
            // grants the actual user a Wenlan source. The nullable source alone
            // decides admission; no earlier unowned action is counted.
            var capture = new WenlanActionCapture(CurrentWenlanSource(use.SourceSeat, SkillProgramTriggerWindow.CardUseCompleted), ids.Count, ids is [var id] && id > 0 ? CaptureLeCaoZhiFuEntity(id) : null);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { LeCaoZhiWenlanUse = capture });
            // The declaration is the native accepted Use boundary. Its payment
            // may open child Uses before targets are finally accepted later.
            if (use.Action is { } declaredUse)
                RecordWenlanAccepted(declaredUse, use.Id, capture);
            // Exact legacy zero-material declarations acquire their Action only
            // at native completion. Preserve this earlier accepted order now.
            if (capture.Source is not null && use.Action is null && use.CardId == 0 && ids.Count == 0 && _coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) && !WenlanEntries(use.SourceSeat).Any(e => e.NativeFrameId == use.Id && e.ActionId == 0))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanActionAcceptedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, use.SourceSeat, NextWenlanOrdinal(use.SourceSeat), 0, use.Id, CardActionType.Use, use.SourceSeat, use.CardKind, capture));
            return;
        }

        if (payload is ProgramCardUseActorReplacedEvent replaced)
        {
            ReassignWenlanNativeActor(replaced);
            return;
        }

        if (payload is not CardActionAcceptedEvent accepted)
            return;
        var action = accepted.Action;
        if (action.Type == CardActionType.Response)
        {
            var capture = BindLeCaoZhiWenlanResponse(action, out var parent);
            RecordWenlanAccepted(action, parent, capture);
            return;
        }

        if (LifecycleCardUseForWenlan(action.ActionId)is not { } native)
            return;
        var legacy = WenlanEntries(action.ActorSeat).SingleOrDefault(e => e.NativeFrameId == native.Id && e.ActionId == 0);
        if (legacy is not null)
        {
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanLegacyActionBoundEvent>().Any(e => e.EntryId == legacy.EntryId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanLegacyActionBoundEvent(legacy.EntryId, action.ActionId));
            return;
        }

        RecordWenlanAccepted(action, native.Id, native.LeCaoZhiWenlanUse);
    }

    internal void ReassignWenlanNativeActor(ProgramCardUseActorReplacedEvent changed)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(changed.CardUseFrameId)is not { Action: { Type: CardActionType.Use } action, LeCaoZhiWenlanUse: { } original } use)
            return;
        if (use.SourceSeat != changed.ActorSeat || action.ActorSeat != changed.ActorSeat || action.ProviderSeat != changed.ProviderSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == changed.FrameId)is not ProgramSkillFrame producer || producer.SkillId != changed.SkillId || producer.OwnerSeat != changed.PreviousActorSeat || changed.OwnerSeat != changed.PreviousActorSeat || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } || context.ParentCardUseFrameId != use.Id || context.CardActionId != action.ActionId || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(producer)?.Op != SkillProgramEffectOp.ReplaceCurrentCardUseActor || !producer.SelectedTargetSeats.SequenceEqual([changed.ActorSeat]))
            throw new InvalidOperationException("Wenlan actor reassignment requires the exact native role-change instruction and paid action.");
        var capture = original with
        {
            Source = CurrentWenlanSource(changed.ActorSeat, SkillProgramTriggerWindow.CardUseCompleted)
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { LeCaoZhiWenlanUse = capture });
        var retired = _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionReassignedEvent>().Where(e => e.RetiredEntryId is not null).Select(e => e.RetiredEntryId!.Value).ToHashSet();
        var all = _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionAcceptedEvent>().Where(e => WenlanBoundActionId(e) == action.ActionId).ToArray();
        // A later mutation cannot undo a reward already issued while this Use
        // was an earlier member of another completed pair.
        if (all.Any(e => IsWenlanEntryClaimed(e.EntryId)))
            return;
        var prior = all.SingleOrDefault(e => !retired.Contains(e.EntryId));
        if (prior is not null && prior.OwnerSeat != changed.PreviousActorSeat)
            throw new InvalidOperationException("Wenlan cannot skip the native actor replacement chain.");
        var newId = capture.Source is null ? (long? )null : ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanActionReassignedEvent(use.Id, action.ActionId, changed.PreviousActorSeat, changed.ActorSeat, prior?.EntryId, newId));
        if (newId is { } entryId)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanActionAcceptedEvent(entryId, changed.ActorSeat, NextWenlanOrdinal(changed.ActorSeat), action.ActionId, use.Id, action.Type, action.ProviderSeat, action.EffectiveKind, capture));
    }

    internal CardUseFrame? LifecycleCardUseForWenlan(long actionId) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == actionId);
    internal bool IsWenlanTrueUse(CardActionContext action) => action.Type == CardActionType.Use || _coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Nullification) || _coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Dodge) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } kind } attack } && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.TargetSeat == action.ActorSeat;
    internal void CaptureLeCaoZhiWenlanQualification(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasLeCaoZhiWenlanCapability || window is not (SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.CardResponseCompleted))
            return;
        if (window == SkillProgramTriggerWindow.CardUseCompleted ? !IsWenlanTrueUse(action) : IsWenlanTrueUse(action))
            return;
        var owner = window == SkillProgramTriggerWindow.CardResponseCompleted ? action.ProviderSeat : action.ActorSeat;
        var entries = WenlanEntries(owner);
        var current = entries.SingleOrDefault(e => WenlanBoundActionId(e) == action.ActionId);
        if (current is null || current.Ordinal % 2 != 0 || entries.SingleOrDefault(e => e.Ordinal == current.Ordinal - 1)is not { } first || CurrentWenlanSource(owner, window)is not { } source || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.SecondEntryId == current.EntryId))
            return;
        var q = new WenlanPairQualification(source, first.EntryId, current.EntryId, action.ActionId, window)
        {
            SecondOrdinal = current.Ordinal
        };
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanPairQualifiedEvent>().Any(e => e.Qualification == q))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanPairQualifiedEvent(q));
    }

    internal WenlanPairQualification? WenlanQualificationFor(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId != WenlanSkill ? null : context.CardUse is not { } use ? WenlanDirectQualificationFor(c, context) : _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanPairQualifiedEvent>().Select(e => e.Qualification).SingleOrDefault(q => q.ActionId == use.CardActionId && q.Source == new WenlanSource(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash) && q.Window == context.Window);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29081, SkillProgramEffectOp.ResolveLeCaoZhiWenlan)]
    internal bool CanOfferLeCaoZhiWenlan(ProgramTriggerLegalityContext gate) => gate.Owner.IsAlive && WenlanQualificationFor(gate.Candidate, gate.Context)is { } q && !_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.SecondEntryId == q.SecondEntryId);
    internal static CardKind[] WenlanNames(WenlanActionAcceptedEvent entry) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cao_zhi_le_LeCaoZhiWenlan.WenlanNames(entry);
    internal WenlanActionAcceptedEvent WenlanEntry(long id)
    {
        var raw = _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanActionAcceptedEvent>().Single(e => e.EntryId == id);
        return WenlanEntries(raw.OwnerSeat).Single(e => e.EntryId == id);
    }

    internal bool WenlanWindowMatches(ProgramCardTriggerWindowFrame window, WenlanPairQualification q) => q.DirectDuelEntryId is null && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) == q.Window && window.Action.ActionId == q.ActionId && (q.Window == SkillProgramTriggerWindow.CardUseCompleted ? window.Action.ActorSeat == q.Source.Source.OwnerSeat && IsWenlanTrueUse(window.Action) && (window.Action.Type != CardActionType.Response || window.CompletedResponseReturn is { IsCommitted: false } && _coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window)) : window.ResponseCompletion is { CostsDrained: true } response && response.CompletionActorSeat == q.Source.Source.OwnerSeat && _coreResponseCompletionOperations.ValidResponseCompletionWindow(window) && !IsWenlanTrueUse(window.Action));
    internal SkillProgramStepOutcome BeginLeCaoZhiWenlan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LeCaoZhiWenlan is not null || f.SkillId != WenlanSkill || f.TriggerId is not (WenlanUseBinding or WenlanPlayedBinding) || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || WenlanQualificationFor(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context)is not { } q || _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Any(e => e.Qualification.SecondEntryId == q.SecondEntryId))
            throw new InvalidOperationException("Wenlan requires its exact accepted pair and the second action's completed native window.");
        long windowId;
        long nativeParentId;
        WenlanNativeParentKind nativeKind;
        if (TryGetWenlanDirectWindow(f, out var direct))
        {
            windowId = direct.Id;
            nativeParentId = direct.ParentFrameId;
            nativeKind = WenlanNativeParentKind.DirectDuel;
        }
        else if (_resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == context.ParentFrameId)is ProgramCardTriggerWindowFrame window && WenlanWindowMatches(window, q) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window))
        {
            windowId = window.Id;
            nativeParentId = window.ParentFrameId;
            nativeKind = WenlanNativeParentKind.CardAction;
        }
        else
            throw new InvalidOperationException("Wenlan cannot borrow another native action or direct-Duel parent.");
        var first = WenlanEntry(q.FirstEntryId);
        var second = WenlanEntry(q.SecondEntryId);
        var one = WenlanNames(first);
        var two = WenlanNames(second);
        var common = one.Intersect(two).Any();
        var r = new WenlanReceipt(q, windowId, nativeParentId, common ? WenlanStage.Obtaining : WenlanStage.SelectingHand, common)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            NativeParentKind = nativeKind
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanStartedEvent(f.Id, q, windowId, nativeParentId, common));
        if (common)
            ObtainWenlanNamedCards(f, one.Concat(two).Distinct().Order().ToArray());
        else
        {
            var hand = WenlanHandLikeIds(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r with { HandIds = hand } });
            for (var i = 0; i < hand.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanHandCandidateEvent(f.Id, i, hand[i]));
            if (hand.Count == 0)
                FinishWenlan(f);
            else
                PublishWenlanHandChoices(f);
        }

        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> WenlanHandChoices(ProgramSkillFrame f)
    {
        var r = f.LeCaoZhiWenlan!;
        var choices = r.HandIds.Select(id => new PromptChoice(new($"le-cao-zhi.wenlan.{(f.Id)}.toggle.{(id)}"), $"{((r.SelectedIds.Contains(id) ? "取消选择" : "选择"))}【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [id], [], new Dictionary<string, string> { ["program-action"] = "le-cao-zhi-wenlan-hand", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["verb"] = "toggle" })).ToList();
        choices.Add(new(new($"le-cao-zhi.wenlan.{(f.Id)}.confirm"), $"确定（已选{(r.SelectedIds.Count)}张）", [], [], new Dictionary<string, string> { ["program-action"] = "le-cao-zhi-wenlan-hand", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["verb"] = "confirm" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishWenlanHandChoices(ProgramSkillFrame f)
    {
        var r = f.LeCaoZhiWenlan!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "文澜：选择任意张手牌，普通牌标记为赋，赋牌替换其额外牌名；可以不选。", r.HandIds, [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = WenlanHandChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    [ProgramAnswer("le-cao-zhi-wenlan-hand")]
    internal void AnswerWenlanHand(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (f.LeCaoZhiWenlan is not { Stage: WenlanStage.SelectingHand } r || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WenlanHandChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Wenlan only accepts its currently published private Hand choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishWenlan(f);
            return;
        }

        if (choice.Parameters.GetValueOrDefault("verb") == "toggle")
        {
            var id = choice.Cards.Single();
            if (!IsWenlanHandLike(f.OwnerSeat, id))
                throw new InvalidOperationException("Wenlan selected Hand entity moved.");
            var selected = r.SelectedIds.Contains(id) ? r.SelectedIds.Where(c => c != id).ToArray() : r.SelectedIds.Append(id).Order().ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r with { SelectedIds = selected } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanHandToggledEvent(f.Id, id, selected.Contains(id)));
            PublishWenlanHandChoices(f);
            return;
        }

        if (choice.Parameters.GetValueOrDefault("verb") != "confirm" || choice.Cards.Count != 0 || r.SelectedIds.Any(id => !IsWenlanHandLike(f.OwnerSeat, id)))
            throw new InvalidOperationException("Wenlan confirmation must retain all actual selected Hand entities.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r with { Stage = WenlanStage.MarkingHand } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanHandConfirmedEvent(f.Id, r.SelectedIds.Count));
        MarkWenlanFuEntities(f, r.SelectedIds, FuMarkOrigin.WenlanHandRewrite);
        FinishWenlan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ObtainWenlanNamedCards(ProgramSkillFrame f, IReadOnlyList<CardKind> names)
    {
        var plan = names.Select(kind => new WenlanNamedObtain(kind, _coreNamedCardAcquisitionsOperations.DeckNamedCard(kind)?.Id)).ToArray();
        var ids = plan.Where(p => p.CardId is not null).Select(p => p.CardId!.Value).ToArray();
        var r = f.LeCaoZhiWenlan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r with { ObtainPlan = plan, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < plan.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanNamedObtainPlannedEvent(f.Id, i, plan[i].Kind, plan[i].CardId));
        if (ids.Length > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(WenlanObtainReason), move =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var receipt = current.LeCaoZhiWenlan!;
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { LeCaoZhiWenlan = receipt with { ObtainedIds = receipt.ObtainedIds.Append(move.CardId).ToArray(), After = move.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanEntityObtainedEvent(f.Id, move.CardId, move.CardKind, move.Sequence));
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.LeCaoZhiWenlan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaoZhiWenlan = r with { Stage = WenlanStage.ObtainChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanObtainReturnedEvent(f.Id, r.ObtainedIds.Count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        MarkWenlanFuEntities(f, r.ObtainedIds, FuMarkOrigin.WenlanObtain);
        ContinueLeCaoZhiWenlan(f.Id);
    }

    internal bool IsExactWenlanFuMarking(ProgramSkillFrame f, IReadOnlyList<int> ids, FuMarkOrigin origin)
    {
        if (!WenlanRootMatches(f) || ids.Distinct().Count() != ids.Count || ids.Any(id => id <= 0))
            return false;
        var r = f.LeCaoZhiWenlan!;
        if (origin == FuMarkOrigin.WenlanHandRewrite)
            return r.Stage == WenlanStage.MarkingHand && ids.SequenceEqual(r.SelectedIds) && ids.All(id => r.HandIds.Contains(id) && IsWenlanHandLike(f.OwnerSeat, id)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanHandConfirmedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var confirmed] && confirmed.Count == ids.Count;
        return origin == FuMarkOrigin.WenlanObtain && r.Stage == WenlanStage.ObtainChildren && ids.SequenceEqual(r.ObtainedIds) && ids.All(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanObtainReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var returned] && returned.Count == ids.Count && returned.Before == r.Before && returned.After == r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanEntityObtainedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Sequence).Select(e => e.CardId).SequenceEqual(ids) && ids.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WenlanObtainReason && r.ObtainPlan.Any(p => p.CardId == id && p.Kind == m.CardKind)) == 1);
    }

    internal void FinishWenlan(ProgramSkillFrame f)
    {
        var r = f.LeCaoZhiWenlan!;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WenlanCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Wenlan pair cannot complete twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenlanCompletedEvent(f.Id, f.OwnerSeat, r.Qualification.FirstEntryId, r.Qualification.SecondEntryId, r.CommonName, r.CommonName ? r.ObtainedIds.Count : r.SelectedIds.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LeCaoZhiWenlan = null, PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "文澜已发行的标记或实体获得及其子结算已结清。");
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueLeCaoZhiWenlan(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.LeCaoZhiWenlan is { Stage: WenlanStage.SelectingHand })
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishWenlanHandChoices(f);
            return;
        }

        if (f.PendingMovementContinuation is not null && _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return;
        FinishWenlan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29081)]
    internal bool ResumeLeCaoZhiWenlan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.LeCaoZhiWenlan is null)
            return false;
        ContinueLeCaoZhiWenlan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29081)]
    internal bool ReturnLeCaoZhiWenlanMovement(ProgramSkillFrame f)
    {
        if (f.LeCaoZhiWenlan is null || f.PendingMovementContinuation is null)
            return false;
        ContinueLeCaoZhiWenlan(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29081)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29081)]
    internal bool PermitPaidLeCaoZhiWenlan(ProgramSkillFrame f)
    {
        if (f.LeCaoZhiWenlan is not { Stage: WenlanStage.Obtaining or WenlanStage.ObtainChildren or WenlanStage.MarkingHand })
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveLeCaoZhiWenlan)]
    internal PromptChoice SelectAiWenlanHand(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(c => c.Cards is [var id] && !f.LeCaoZhiWenlan!.SelectedIds.Contains(id) && CaptureLeCaoZhiFuEntity(id)is null) ?? decision.Choices.Single(c => c.Parameters.GetValueOrDefault("verb") == "confirm");
    internal bool WenlanRootMatches(ProgramSkillFrame f)
    {
        if (f.LeCaoZhiWenlan is not { } r || f.SkillId != WenlanSkill || f.TriggerId is not (WenlanUseBinding or WenlanPlayedBinding) || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash || !LeCaoZhiComposition.IsWenlanProgram(program) || r.Qualification.SecondOrdinal < 2 || r.Qualification.SecondOrdinal % 2 != 0 || r.Qualification.Source != WenlanFrameSource(f) || context.ParentFrameId != r.WindowId || !WenlanNativeParentMatches(f, r, context))
            return false;
        var first = WenlanEntry(r.Qualification.FirstEntryId);
        var second = WenlanEntry(r.Qualification.SecondEntryId);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == r.WindowId && first.OwnerSeat == f.OwnerSeat && second.OwnerSeat == f.OwnerSeat && second.Ordinal % 2 == 0 && first.Ordinal + 1 == second.Ordinal && (r.NativeParentKind == WenlanNativeParentKind.DirectDuel ? r.Qualification.DirectDuelEntryId == second.EntryId : WenlanBoundActionId(second) == r.Qualification.ActionId) && r.CommonName == WenlanNames(first).Intersect(WenlanNames(second)).Any() && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanPairQualifiedEvent>().Count(e => e.Qualification == r.Qualification) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Count(e => e == new WenlanStartedEvent(f.Id, r.Qualification, r.WindowId, r.NativeParentId, r.CommonName)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WenlanStartedEvent>().Count(e => e.Qualification.SecondEntryId == second.EntryId) == 1;
    }

    internal bool WenlanOperationMovement(ProgramSkillFrame f, CardMovementRecord m) => f.LeCaoZhiWenlan is { Stage: WenlanStage.ObtainChildren } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && r.ObtainedIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WenlanObtainReason && r.ObtainPlan.Any(p => p.CardId == m.CardId && p.Kind == m.CardKind);
    internal bool WenlanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!WenlanRootMatches(f))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WenlanOperationMovement(f, m));
    }

    internal bool WenlanObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame f && (_characterCaoZhiLeOperations.WenlanFirstChild(f, _resolutionStackOperations2._resolutionStack()[child]) || f.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(f, _resolutionStackOperations2._resolutionStack()[child])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? WenlanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterCaoZhiLeOperations.WenlanFirstChild(f, child), (index) => _characterCaoZhiLeOperations.WenlanObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 790)]
    internal bool IsWenlanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && WenlanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 560)]
    internal bool IsWenlanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.LeCaoZhiWenlan is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && WenlanFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && WenlanObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29081)]
    internal bool AdvanceWenlanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WenlanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);

    SkillProgramStepOutcome global::CardGame.Core.ILeCaoZhiWenlanHost.Resolve(ProgramSkillFrame f) => BeginLeCaoZhiWenlan(f);
}
