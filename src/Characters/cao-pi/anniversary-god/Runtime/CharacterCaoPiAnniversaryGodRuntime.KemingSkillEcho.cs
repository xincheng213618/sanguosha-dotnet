#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiAnniversaryGodRuntime
{
    internal bool HasKemingInvocationObservers => HasShenCaoPiCapability && (_resolutionStackOperations._resolutionStack.Any(f => f.KemingCapture is { Closed: false }) || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoArmedEvent>().Any(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoReservedEvent>().Any(r => r.IssueFrameId == e.FrameId)));

    internal void CaptureKemingInvocation(long invocationId, long parentFrameId, int actorSeat, string skillKey, string occurrenceKey, string skillInstanceId)
    {
        if (!HasKemingInvocationObservers || parentFrameId <= 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || phase.ActorSeat != actorSeat || phase.Kind is not (ActualDiscardRecoveryPhaseKind.Preparation or ActualDiscardRecoveryPhaseKind.Play or ActualDiscardRecoveryPhaseKind.Ending) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != parentFrameId)
            return;
        var issues = _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoArmedEvent>().Where(e => e.TargetSeat == actorSeat && invocationId > e.InvocationCutoff && !_coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoReservedEvent>().Any(r => r.IssueFrameId == e.FrameId)).OrderBy(e => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, e.Source.OwnerSeat)).ThenBy(e => e.FrameId).ToArray();
        if (issues.Length == 0)
            return;
        var capture = new KemingSourceCapture(invocationId, parentFrameId, actorSeat, skillKey, occurrenceKey, skillInstanceId, phase)
        {
            Entitlements = issues.Select(e => new KemingEchoEntitlement(e.FrameId, e.Source, e.GameplayHash)).ToArray()
        };
        if (parent is ProgramCardTriggerWindowFrame cardWindow && cardWindow.Action.Type == CardActionType.Use && cardWindow.Action.ActorSeat == actorSeat)
            capture = capture with
            {
                Names = [new(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(cardWindow.Action.EffectiveKind), false, cardWindow.Action.ActionId, 0)]
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { KemingCapture = capture });
        foreach (var issue in issues)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoReservedEvent(issue.FrameId, invocationId, parentFrameId, actorSeat, skillKey, occurrenceKey));
        foreach (var name in capture.Names)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingSourceCardUsedEvent(invocationId, parentFrameId, name.ProducerId));
    }

    internal void ObserveKemingProducedFacts(IGameEvent payload)
    {
        if (payload is NativeDrawInvocationRecordedEvent draw)
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == draw.ParentFrameId);
            if (index >= 0 && _resolutionStackOperations._resolutionStack[index] is { KemingCapture: { Closed: false } capture } parent && capture.ActorSeat == draw.OwnerSeat)
            {
                var names = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.To == CardLocation.Hand(draw.OwnerSeat) && m.From == CardLocation.DrawPile && m.Reason == draw.Reason).OrderBy(m => m.Sequence).Select(m => new KemingProducedName(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(m.CardKind), true, draw.InvocationId, m.Sequence));
                _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { KemingCapture = capture with { Names = capture.Names.Concat(names).Distinct().Take(5).ToArray() } });
            }
        }

        CardActionContext? action = payload switch
        {
            CardUseDeclaredEvent declared => _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)?.Action,
            CardActionAcceptedEvent accepted when accepted.Action.Type == CardActionType.Response && _coreResponseUseCompletionOperations.IsCompletedResponseUse(accepted.Action, accepted.Action.EffectiveKind == CardKind.Dodge ? ProgramCardContinuation.Dodge : ProgramCardContinuation.NullificationResponse) => accepted.Action,
            _ => null
        };
        if (action is null)
            return;
        foreach (var parent in _resolutionStackOperations._resolutionStack.Where(f => f.KemingCapture is { Closed: false } c && c.ActorSeat == action.ActorSeat).ToArray())
        {
            var capture = parent.KemingCapture!;
            if (capture.Names.Any(n => !n.IsDraw && n.ProducerId == action.ActionId) || parent is ProgramCardTriggerWindowFrame)
                continue;
            if (parent is ProgramSkillFrame)
            {
                var sourceIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
                var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f is CardUseFrame use && use.Action?.ActionId == action.ActionId);
                if (action.Type != CardActionType.Use || useIndex <= sourceIndex || _resolutionStackOperations._resolutionStack.Skip(sourceIndex + 1).Take(useIndex - sourceIndex - 1).Any(f => f is ProgramSkillFrame or SkillInvocationFrame))
                    continue;
            }
            else if (parent is CardUseFrame use)
            {
                var native = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeEquipmentInvocationIssuedEvent>().LastOrDefault(e => e.ParentFrameId == parent.Id && e.OccurrenceKey == capture.OccurrenceKey);
                if (native is null || action.Type == CardActionType.Use && native.Kind != NativeEquipmentInvocationKind.Qinglong || action.Type == CardActionType.Response && (native.Kind is not (NativeEquipmentInvocationKind.Bagua or NativeEquipmentInvocationKind.FactionBagua) || action.EffectiveKind != CardKind.Dodge || action.ParentActionId != use.Action?.ActionId))
                    continue;
            }
            else
                continue;
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { KemingCapture = capture with { Names = capture.Names.Append(new(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind), false, action.ActionId, 0)).Take(5).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingSourceCardUsedEvent(capture.InvocationId, parent.Id, action.ActionId));
        }
    }

    internal bool HasKemingCompletedCardEffect(CardActionContext action, SkillProgramTriggerWindow window) => HasShenCaoPiCapability && window is SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted && _resolutionStackOperations._resolutionStack.Any(f => f.KemingCapture is { Closed: false } c && c.Names.Any(n => !n.IsDraw && n.ProducerId == action.ActionId));
    internal bool TryCompleteKemingProgram(ProgramSkillFrame f, bool completed) => f.KemingCapture is { Closed: false } capture && BeginKemingEcho(f, capture, new(KemingEchoReturnKind.ProgramCompletion, completed));
    internal bool TryCompleteKemingConverted(ProgramCardTriggerWindowFrame supplied)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramCardTriggerWindowFrame;
        if (f is null || f.Id != supplied.Id)
            return false;
        if (f.KemingCapture is { Closed: false } own)
            return BeginKemingEcho(f, own, new(KemingEchoReturnKind.CardWindow));
        var source = _resolutionStackOperations._resolutionStack.LastOrDefault(parent => parent.KemingCapture is { Closed: false } capture && capture.Names.Any(n => !n.IsDraw && n.ProducerId == f.Action.ActionId));
        return source?.KemingCapture is { } native && source is not ProgramSkillFrame && BeginKemingEcho(f, native, new(KemingEchoReturnKind.CardWindow));
    }

    internal bool TryCompleteKemingCixiong(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending, bool activated, bool targetDiscarded, int? discardedCardId, int sourceDrawCount)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.Attack.ResolutionId)is not { KemingCapture: { Closed: false } capture } source || capture.SkillKey != CardCatalog.Get(CardKind.CixiongDoubleSwords).DisplayName)
            return false;
        return BeginKemingEcho(source, capture, new(KemingEchoReturnKind.CixiongCompletion, Activated: activated, TargetDiscarded: targetDiscarded, DiscardedCardId: discardedCardId, SourceDrawCount: sourceDrawCount));
    }

    internal bool BeginKemingEcho(ResolutionFrame parent, KemingSourceCapture capture, KemingEchoReturn returned)
    {
        var source = _resolutionStackOperations._resolutionStack.Single(f => f.Id == capture.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(source.Id, source with { KemingCapture = capture with { Closed = true } });
        if (capture.Names.Count == 0 || !capture.Entitlements.Any(e => _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id)
            throw new InvalidOperationException("Keming's effect completion must retain the actual live source host.");
        if (returned.Kind == KemingEchoReturnKind.CixiongCompletion)
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Keming cannot overwrite another accepted source decision.");
        var frame = new KemingSkillEchoFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, capture, returned);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoStartedEvent(frame.Id, parent.Id, capture.InvocationId, capture.Names.Count));
        _coreRuntimeOperations.AdvanceRuntimeFrame(frame.Id);
        return true;
    }

    internal IReadOnlyList<PromptChoice> KemingEchoChoices(KemingSkillEchoFrame f) => Array.AsReadOnly(new[] { true, false }.Select(accept => new PromptChoice(new($"keming-echo:{(f.Id)}:{(f.Cursor)}:{(accept)}"), accept ? $"摸至多{(f.Capture.Names.Count)}张同名牌" : "不发动刻名", [], [], new Dictionary<string, string> { ["program-action"] = "keming-echo", ["accept"] = accept ? "yes" : "no" })).ToArray());
    internal void PublishKemingEcho(KemingSkillEchoFrame f)
    {
        var owner = f.Capture.Entitlements[f.Cursor].Source.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(ShenCaoPiComposition.Keming);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, owner, "刻名：是否摸等量同名牌？", [], [], owner)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = KemingEchoChoices(f),
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[owner].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool TryChooseKemingEchoAi(PendingDecision decision, out PromptChoice selected)
    {
        selected = null !;
        if (!ReferenceEquals(decision, _corePendingDecisionPolicyPipelineOperations._pendingDecision) || !HasKemingEchoPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame { Stage: KemingSkillEchoStage.Choosing } frame)
            return false;
        AssertKemingEcho(frame);
        // This is an already-issued native effect return, not a new trigger
        // activation. Select its published benefit and retain AnswerKemingEcho's
        // exact source, owner, parent and once-only native-draw validation.
        selected = decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "keming-echo" && choice.Parameters.GetValueOrDefault("accept") == "yes");
        return true;
    }

    [ProgramAnswer("keming-echo")]
    internal void AnswerKemingEcho(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame { Stage: KemingSkillEchoStage.Choosing } f || !KemingEchoChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(choice.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Keming rejects a stale original source-effect return.");
        AssertKemingEcho(f);
        var owner = f.Capture.Entitlements[f.Cursor].Source.OwnerSeat;
        var accept = choice.Parameters["accept"] == "yes";
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoChoiceEvent(f.Id, f.Cursor, owner, accept));
        if (!accept)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Cursor = f.Cursor + 1 });
        else
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Stage = KemingSkillEchoStage.Drawing, NativeDraw = new(f.Cursor, f.Capture.Names.Count, null, SkillProgramCardSetVisibility.Private, new("skill-program.anniversary:keming.same-name-draw")) { ReturnKind = NativeDrawAttemptReturnKind.KemingSkillEcho, Recipients = [owner] } });
        _coreRuntimeOperations.AdvanceRuntimeFrame(f.Id);
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 39700, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(KemingSkillEchoFrame), 39700)]
    internal void ContinueKemingSkillEcho(KemingSkillEchoFrame supplied)
    {
        var id = supplied.Id;
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is KemingSkillEchoFrame f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            AssertKemingEcho(f);
            if (f.Stage == KemingSkillEchoStage.Drawing)
            {
                var paid = f.NativeDraw ?? throw new InvalidOperationException("Keming lost its frozen same-name draw request.");
                if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                    throw new InvalidOperationException("Keming must await its exact native draw request.");
                if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(f))
                    return;
                f = (KemingSkillEchoFrame)_resolutionStackOperations._resolutionStack[^1];
                paid = f.NativeDraw!;
                var owner = paid.Recipients.Single();
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                var ids = new List<int>();
                var draft = _coreOutsidePhaseDrawDiscardOperations.CaptureNativeDrawInvocation(owner, paid.RequestedCount, paid.Reason, f.Capture.Entitlements[f.Cursor].Source);
                if (paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
                    foreach (var name in f.Capture.Names)
                    {
                        if (!_corePhysicalDrawFacadesOperations.EnsureDrawPile())
                            break;
                        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(c => global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(c.Kind) == name.Name);
                        if (card is null)
                            continue;
                        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(owner), paid.Reason);
                        ids.Add(card.Id);
                    }

                _coreOutsidePhaseDrawDiscardOperations.CompleteNativeDrawInvocation(draft, ids);
                var outcome = new NativeDrawOutcome(owner, paid.RequestedCount, paid.ActiveAttemptId, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.Replacement)
                {
                    ActualDrawIds = ids
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { Stage = KemingSkillEchoStage.Children, NativeDraw = null, Outcome = outcome });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoDrawPaidEvent(f.Id, f.Cursor, owner, paid.RequestedCount, ids.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.Replacement is not null));
                continue;
            }

            if (f.Stage == KemingSkillEchoStage.Children)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.KemingEcho) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.KemingEcho) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.KemingEcho) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                    return;
                f = (KemingSkillEchoFrame)_resolutionStackOperations._resolutionStack[^1];
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { Stage = KemingSkillEchoStage.Choosing, Cursor = f.Cursor + 1, Outcome = null });
                continue;
            }

            if (f.Cursor < f.Capture.Entitlements.Count && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.Capture.Entitlements[f.Cursor].Source.OwnerSeat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { Cursor = f.Cursor + 1 });
                    continue;
                }

                PublishKemingEcho(f);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KemingEchoFinishedEvent(f.Id, f.ParentFrameId, f.Capture.InvocationId));
            _coreRuntimeOperations.PopResolutionFrame(f.Id, ResolutionFrameKind.KemingSkillEcho);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != f.ParentFrameId)
                throw new InvalidOperationException("Keming lost its typed completed-source parent.");
            if (f.Return.Kind == KemingEchoReturnKind.ProgramCompletion)
                _coreSkillProgramsOperations.FinishProgramSkill((ProgramSkillFrame)parent, f.Return.SourceCompleted);
            else if (f.Return.Kind == KemingEchoReturnKind.CardWindow)
                _coreRuntimeOperations.AdvanceRuntimeFrame(parent.Id);
            else
            {
                global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending = _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords ?? throw new InvalidOperationException("Keming lost its completed Cixiong source.");
                _coreNativeWeaponResponsesOperations.CompleteCixiongDoubleSwords(pending, f.Return.Activated, f.Return.TargetDiscarded, f.Return.DiscardedCardId, f.Return.SourceDrawCount);
            }

            return;
        }
    }

    internal bool IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b) => f.Stage == KemingSkillEchoStage.Children && f.Outcome is { } o && b.ParentFrameId == f.Id && b.AwaitingProgramFrameId is null && b.Movements.All(m => m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter) && b.Movements.Any(m => m.To == CardLocation.Hand(o.RecipientSeat));
    internal void ReturnKemingEchoMovement(CardsMovedTriggerWindowFrame window)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame f || window.ResumeKemingEchoFrameId != f.Id || !IsKemingEchoMovement(f, window.Batch))
            throw new InvalidOperationException("Keming's draw observer lost its exact native producer return.");
        _coreRuntimeOperations.AdvanceRuntimeFrame(f.Id);
    }

    internal static bool SameKemingCapture(KemingSourceCapture a, KemingSourceCapture b) => a.InvocationId == b.InvocationId && a.ParentFrameId == b.ParentFrameId && a.ActorSeat == b.ActorSeat && a.SkillKey == b.SkillKey && a.OccurrenceKey == b.OccurrenceKey && a.SkillInstanceId == b.SkillInstanceId && a.Phase == b.Phase && a.Entitlements.SequenceEqual(b.Entitlements) && a.Names.SequenceEqual(b.Names);
    internal void AssertKemingEcho(KemingSkillEchoFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != f.ParentFrameId || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == f.Capture.ParentFrameId)?.KemingCapture is not { Closed: true } source || !SameKemingCapture(source, f.Capture) || f.Capture.Names is not System.Collections.IList { IsReadOnly: true } || f.Capture.Entitlements is not System.Collections.IList { IsReadOnly: true } || f.Capture.Names.Count is < 1 or > 5 || f.Cursor < 0 || f.Cursor > f.Capture.Entitlements.Count || !Enum.IsDefined(f.Stage) || !Enum.IsDefined(f.Return.Kind) || f.Capture.Entitlements.Any(e => _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoReservedEvent>().Count(r => r.IssueFrameId == e.IssueFrameId && r.InvocationId == f.Capture.InvocationId && r.ParentFrameId == f.Capture.ParentFrameId && r.ActorSeat == f.Capture.ActorSeat && r.SkillKey == f.Capture.SkillKey && r.OccurrenceKey == f.Capture.OccurrenceKey) != 1) || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoStartedEvent>().Count(e => e.FrameId == f.Id && e.ParentFrameId == f.ParentFrameId && e.InvocationId == f.Capture.InvocationId && e.Count == f.Capture.Names.Count) != 1)
            throw new InvalidOperationException("Keming's private completed-effect receipt lost its exact invocation, source, phase or entitlement.");
        AssertKemingSourceReceipt(f.Capture);
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        if (!KemingSkillEchoFrameRidesOn(f, parent) || f.Stage == KemingSkillEchoStage.Finished || f.Stage is KemingSkillEchoStage.Drawing or KemingSkillEchoStage.Children && f.Cursor >= f.Capture.Entitlements.Count)
            throw new InvalidOperationException("Keming's completed source changed its typed native return or payment stage.");
        if (f.Stage == KemingSkillEchoStage.Drawing)
        {
            var owner = f.Capture.Entitlements[f.Cursor].Source.OwnerSeat;
            if (f.Outcome is not null || f.NativeDraw is not { } draw || draw.ReturnKind != NativeDrawAttemptReturnKind.KemingSkillEcho || draw.InstructionIndex != f.Cursor || draw.RequestedCount != f.Capture.Names.Count || !draw.Recipients.SequenceEqual([owner]) || draw.RecipientIndex != 0 || draw.Reason.Value != "skill-program.anniversary:keming.same-name-draw" || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoChoiceEvent>().Count(e => e.FrameId == f.Id && e.Cursor == f.Cursor && e.OwnerSeat == owner && e.Accepted) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoDrawPaidEvent>().Any(e => e.FrameId == f.Id && e.Cursor == f.Cursor))
                throw new InvalidOperationException("Keming's accepted matching-name draw changed its exact unpaid native request.");
        }
        else if (f.NativeDraw is not null)
            throw new InvalidOperationException("Keming retained an unrelated native draw producer.");
        if (f.Stage == KemingSkillEchoStage.Children)
        {
            if (f.Outcome is not { } o || o.RecipientSeat != f.Capture.Entitlements[f.Cursor].Source.OwnerSeat || o.RequestedCount != f.Capture.Names.Count || o.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || o.ActualDrawIds.Distinct().Count() != o.ActualDrawIds.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoDrawPaidEvent>().Count(e => e.FrameId == f.Id && e.Cursor == f.Cursor && e.OwnerSeat == o.RecipientSeat && e.Requested == o.RequestedCount && e.Actual == o.ActualDrawIds.Count && e.Before == o.SequenceBefore && e.After == o.SequenceAfter && e.Replaced == (o.Replacement is not null)) != 1 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == "skill-program.anniversary:keming.same-name-draw").OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(o.ActualDrawIds))
                throw new InvalidOperationException("Keming's paid return changed its actual matching deck entities or original native movement range.");
        }
        else if (f.Outcome is not null)
            throw new InvalidOperationException("Keming retained a draw outcome outside its exact children stage.");
    }

    internal void AssertKemingSourceReceipt(KemingSourceCapture c)
    {
        if (!_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(c.Phase) || c.Phase.ActorSeat != c.ActorSeat || c.Phase.Kind is not (ActualDiscardRecoveryPhaseKind.Preparation or ActualDiscardRecoveryPhaseKind.Play or ActualDiscardRecoveryPhaseKind.Ending) || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e.FrameId == c.InvocationId && e.ParentFrameId == c.ParentFrameId && e.ActorSeat == c.ActorSeat && e.SkillKey == c.SkillKey && e.OccurrenceKey == c.OccurrenceKey && e.SkillInstanceId == c.SkillInstanceId && e.TurnNumber == c.Phase.TurnNumber && e.TurnOwnerSeat == c.Phase.ActualTurnOwnerSeat) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationCompletedEvent>().Count(e => e.FrameId == c.InvocationId && e.ParentFrameId == c.ParentFrameId && e.OccurrenceKey == c.OccurrenceKey) != 1 || c.Entitlements.Distinct().Count() != c.Entitlements.Count || c.Names.Distinct().Count() != c.Names.Count || c.Entitlements.Any(ent => _coreProgramEventHistoryOperations.ProgramEventHistory<KemingEchoArmedEvent>().Count(e => e.FrameId == ent.IssueFrameId && e.Source == ent.Source && e.GameplayHash == ent.GameplayHash && e.TargetSeat == c.ActorSeat && e.InvocationCutoff < c.InvocationId) != 1))
            throw new InvalidOperationException("Keming's receipt is not the recorded actual next completed skill invocation.");
        foreach (var name in c.Names)
        {
            if (name.IsDraw)
            {
                var draws = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.InvocationId == name.ProducerId && e.ParentFrameId == c.ParentFrameId && e.OwnerSeat == c.ActorSeat && name.Sequence > e.SequenceBefore && name.Sequence <= e.SequenceAfter).ToArray();
                if (draws.Length != 1 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == name.Sequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(c.ActorSeat) && m.Reason == draws[0].Reason && global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(m.CardKind) == name.Name))
                    throw new InvalidOperationException("Keming's private drawn name must match its original native draw invoice and exact entity movement.");
            }
            else if (name.Sequence != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<KemingSourceCardUsedEvent>().Count(e => e.InvocationId == c.InvocationId && e.SourceFrameId == c.ParentFrameId && e.ActionId == name.ProducerId) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == name.ProducerId && e.Action.ActorSeat == c.ActorSeat && global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(e.Action.EffectiveKind) == name.Name && (e.Action.Type == CardActionType.Use || _coreResponseUseCompletionOperations.IsCompletedResponseUse(e.Action, e.Action.EffectiveKind == CardKind.Dodge ? ProgramCardContinuation.Dodge : ProgramCardContinuation.NullificationResponse))))
                throw new InvalidOperationException("Keming's public used name must match the exact accepted native action observed by this source invocation.");
        }
    }

    internal void AssertKemingEchoFrames()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<KemingSkillEchoFrame>())
            AssertKemingEcho(f);
    }

    internal bool HasKemingEchoPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is KemingSkillEchoFrame { Stage: KemingSkillEchoStage.Choosing } f && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } p && f.Cursor < f.Capture.Entitlements.Count && p.PlayerSeat == f.Capture.Entitlements[f.Cursor].Source.OwnerSeat && p.Choices.Select(c => c.Id).SequenceEqual(KemingEchoChoices(f).Select(c => c.Id));
    internal bool KemingSkillEchoFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => child switch
    {
        KemingSkillEchoFrame f => f.ParentFrameId == parent.Id && KemingEchoTypedParentMatches(f, parent),
        CardsMovedTriggerWindowFrame moved when parent is KemingSkillEchoFrame f => moved.ResumeKemingEchoFrameId == f.Id && IsKemingEchoMovement(f, moved.Batch),
        HpChangedTriggerWindowFrame hp when parent is KemingSkillEchoFrame f => hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.KemingEcho && hp.Change.ParentFrameId == f.Id,
        ProgramLifecycleTriggerWindowFrame state when parent is KemingSkillEchoFrame f => state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.KemingEcho,
        RecoveryReplacementFrame recovery when parent is KemingSkillEchoFrame f => f.Stage == KemingSkillEchoStage.Children && recovery.ParentFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.KemingEcho && recovery.Return.ResumeFrameId == f.Id,
        _ => global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent)};
    internal bool KemingEchoTypedParentMatches(KemingSkillEchoFrame f, ResolutionFrame parent) => f.Return.Kind switch
    {
        KemingEchoReturnKind.ProgramCompletion => parent is ProgramSkillFrame p && f.Capture.ParentFrameId == p.Id && f.Capture.SkillKey == p.SkillId && f.Capture.SkillInstanceId == p.SkillInstanceId && f.Capture.OccurrenceKey == "program:" + global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p) && !f.Return.Activated && !f.Return.TargetDiscarded && f.Return.DiscardedCardId is null && f.Return.SourceDrawCount == 0,
        KemingEchoReturnKind.CardWindow => parent is ProgramCardTriggerWindowFrame w && f.Capture.ActorSeat == w.Action.ActorSeat && f.Capture.Names.Any(n => !n.IsDraw && n.ProducerId == w.Action.ActionId) && !f.Return.Activated && !f.Return.TargetDiscarded && f.Return.DiscardedCardId is null && f.Return.SourceDrawCount == 0,
        KemingEchoReturnKind.CixiongCompletion => parent is CardUseFrame use && f.Capture.ParentFrameId == use.Id && f.Capture.SkillKey == CardCatalog.Get(CardKind.CixiongDoubleSwords).DisplayName && f.Return.Activated && !f.Return.TargetDiscarded && f.Return.DiscardedCardId is null && f.Return.SourceDrawCount == 1 && _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is { Stage: CixiongDoubleSwordsStage.TargetChoice } pending && pending.Attack.ResolutionId == use.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeEquipmentInvocationIssuedEvent>().Any(e => e.ParentFrameId == use.Id && e.Kind == NativeEquipmentInvocationKind.Cixiong && e.OccurrenceKey == f.Capture.OccurrenceKey),
        _ => false
    };
    internal bool HasKemingCixiongContinuation(global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle pending)
    {
        if (!HasShenCaoPiCapability || pending.Stage != CixiongDoubleSwordsStage.TargetChoice || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == pending.Attack.ResolutionId);
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame { Step: ResolutionFrameStep.Declared, KemingCapture: { Closed: true } source } root || root.CardAttack is not { Active: true, CixiongDoubleSwordsResolved: false } attack || root.SourceSeat != pending.Attack.SourceSeat || attack.TargetSeat != pending.Attack.TargetSeat || _resolutionStackOperations._resolutionStack[index + 1] is not KemingSkillEchoFrame { Return.Kind: KemingEchoReturnKind.CixiongCompletion } echo || !SameKemingCapture(source, echo.Capture) || !KemingSkillEchoFrameRidesOn(echo, root) || echo.Capture.Names is not [{ IsDraw: true } paid] || !_coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Any(e => e.InvocationId == paid.ProducerId && e.ParentFrameId == root.Id && e.OwnerSeat == attack.SourceSeat && e.Requested == 1 && e.ActualCount == 1 && e.Reason == CardMoveReasons.CixiongDraw && paid.Sequence > e.SequenceBefore && paid.Sequence <= e.SequenceAfter) || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeEquipmentInvocationReturnedEvent>().Count(e => e.InvocationId == source.InvocationId && e.ParentFrameId == root.Id && e.AttackId == root.Id && e.Kind == NativeEquipmentInvocationKind.Cixiong && e.OccurrenceKey == source.OccurrenceKey && !e.Canceled) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<CixiongDoubleSwordsResolvedEvent>().Any(e => e.ResolutionId == root.Id) || !_coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _characterCaoPiAnniversaryGodOperations.KemingCixiongObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(child, dying)))
            return false;
        AssertKemingEcho(echo);
        return true;
    }

    internal bool KemingCixiongObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is KemingSkillEchoFrame || child is KemingSkillEchoFrame)
            return KemingSkillEchoFrameRidesOn(child, parent);
        return _coreNativeEquipmentInvocationsOperations.NativeEquipmentObserverEdge(index);
    }
}
