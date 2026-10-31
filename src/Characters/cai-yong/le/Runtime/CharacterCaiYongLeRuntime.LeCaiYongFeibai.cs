#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiYongLeRuntime
{
    internal const string FeibaiSkill = "le:feibai";
    internal const string FeibaiBinding = "after-use";
    internal const string FeibaiObtainReason = "skill-program.le-cai-yong.feibai.obtain";
    internal const string FeibaiDrawReason = "skill-program.le-cai-yong.feibai.draw";
    internal bool HasLeCaiYongFeibaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveLeCaiYongFeibai);

    internal static CardConversionSource FeibaiSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cai_yong_le_LeCaiYongFeibai.FeibaiSource(f);
    internal static int FeibaiNameLength(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cai_yong_le_LeCaiYongFeibai.FeibaiNameLength(kind);
    internal bool FeibaiDisabled(int owner, int turn) => _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiDisabledEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == turn);
    internal long? FeibaiSetupEpoch => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber == 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.NotStarted && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Where(window => window.Window == SkillProgramTriggerWindow.GameStarting && window.Continuation == ProgramLifecycleContinuation.CompleteGameStarting).ToArray()is [var startup] ? startup.Id : null;
    internal bool FeibaiActualUseScopeIsOpen => _coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || FeibaiSetupEpoch is not null;

    internal void CaptureFeibaiDeclaredName(long origin, CardActionContext? action, CardKind kind, int? instruction)
    {
        if (!HasLeCaiYongFeibaiCapability || !FeibaiActualUseScopeIsOpen)
            return;
        var fact = new FeibaiDeclaredNameEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, origin, action?.ActionId, instruction, kind)
        {
            SetupEpochId = FeibaiSetupEpoch
        };
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiDeclaredNameEvent>().Any(e => e.ActualTurn == fact.ActualTurn && e.TurnOwnerSeat == fact.TurnOwnerSeat && e.OriginFrameId == origin && e.ActionId == action?.ActionId && e.ProgramInstructionIndex == instruction))
            throw new InvalidOperationException("Feibai cannot capture a declaration name twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    }

    internal FeibaiDeclaredNameEvent FeibaiRecordedName(CurrentTurnCardUseKindsRecordedEvent fact) => _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiDeclaredNameEvent>().Where(e => e.ActualTurn == fact.TurnNumber && e.TurnOwnerSeat == fact.TurnOwnerSeat && e.OriginFrameId == fact.OriginFrameId && e.ActionId == fact.CardActionId && e.ProgramInstructionIndex == fact.ProgramInstructionIndex && e.SetupEpochId == FeibaiSetupEpoch).ToArray()is [var name] ? name : throw new InvalidOperationException("Feibai needs the exact dependency-captured native declaration name.");
    internal CardKind FeibaiRecordedKind(CurrentTurnCardUseKindsRecordedEvent fact) => FeibaiRecordedName(fact).EffectiveKind;
    internal CardKind FeibaiSourceBoundEffectiveUseKind(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int prefixEnd, CurrentTurnCardUseKindsRecordedEvent current)
    {
        var original = FeibaiRecordedKind(fact);
        // A defensive response shares the incoming Use's origin frame. Its own
        // name must never inherit that Slash's later Lihuo conversion.
        if (fact.RoundActionType != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(original))
            return original;
        var changes = history.Select((payload, index) => (payload, index)).Where(item => item.payload is ProgramCurrentSlashFireChangedEvent change && change.CardUseFrameId == fact.OriginFrameId && (item.index < prefixEnd || fact == current)).ToArray();
        if (changes.Length == 0)
            return original;
        var declarations = history.Select((payload, index) => (payload, index)).Where(item => Equals(item.payload, fact)).ToArray();
        if (changes is not [var entry] || entry.payload is not ProgramCurrentSlashFireChangedEvent changed || declarations is not [var declaration] || entry.index <= declaration.index || fact.CardActionId != changed.ActionId || changed.OriginalKind != original || changed.FinalKind != CardKind.FireSlash || history.OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == fact.OriginFrameId && e.SourceSeat == fact.ActorSeat && e.CardKind == original) != 1)
            throw new InvalidOperationException("Feibai's changed name requires the exact original declaration and one native Slash conversion.");
        var starts = history.Select((payload, index) => (payload, index)).Where(item => item.payload is ProgramBindingStartedEvent started && started.FrameId == changed.ProgramFrameId).ToArray();
        if (starts is not [var start] || start.index <= declaration.index || start.index >= entry.index || start.payload is not ProgramBindingStartedEvent binding || binding.SkillId != changed.Source.SkillId || binding.BindingId != changed.Source.BindingId || binding.SkillInstanceId != changed.Source.SkillInstanceId || binding.OwnerSeat != changed.Source.OwnerSeat || binding.Window != SkillProgramTriggerWindow.CardUseCommitted || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(binding.SkillId)?.Program is not { } program || program.Triggers.SingleOrDefault(trigger => trigger.Id == binding.BindingId)is not { } trigger || trigger.Window != binding.Window || trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || trigger.CardKinds.Count != 0 && !ProgramCardKindMatch.Contains(trigger.CardKinds, original) || !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
            throw new InvalidOperationException("Feibai's changed name lost its source-bound native committed binding.");
        var actor = fact.ActorSeat;
        foreach (var replacement in history.Take(entry.index).OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == fact.OriginFrameId))
        {
            if (replacement.PreviousActorSeat != actor || replacement.OwnerSeat != actor || replacement.ProviderSeat != fact.RoundProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(replacement.ActorSeat))
                throw new InvalidOperationException("Feibai's native kind change lost its actual actor chain.");
            actor = replacement.ActorSeat;
        }

        if (actor != changed.Source.OwnerSeat)
            throw new InvalidOperationException("Feibai's native kind change belongs to another user.");
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == changed.ProgramFrameId)is ProgramSkillFrame producer)
        {
            var plan = ProgramInstructionResolver.Default.Resolve(producer, program);
            if (producer.SkillId != binding.SkillId || producer.TriggerId != binding.BindingId || producer.SkillInstanceId != binding.SkillInstanceId || producer.OwnerSeat != binding.OwnerSeat || producer.GameplayHash != program.GameplayHash || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } || context.ParentCardUseFrameId != fact.OriginFrameId || context.CardActionId != changed.ActionId || producer.InstructionIndex < 1 || producer.InstructionIndex > plan.Instructions.Count || !plan.Instructions.Take(producer.InstructionIndex).Any(effect => effect.Op == SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
                throw new InvalidOperationException("Feibai's live native conversion lost its exact paused producer.");
        }
        else if (history.Skip(entry.index + 1).OfType<ProgramBindingResolvedEvent>().Count(e => e.FrameId == binding.FrameId && e.SkillId == binding.SkillId && e.BindingId == binding.BindingId && e.SkillInstanceId == binding.SkillInstanceId && e.OwnerSeat == binding.OwnerSeat && e.Window == binding.Window && e.Activated) != 1)
            throw new InvalidOperationException("Feibai's historical native conversion lost its original producer return.");
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.OriginFrameId)is { } use)
        {
            if (use.CurrentSlashFirePolicy is not { } policy || policy.ProgramFrameId != changed.ProgramFrameId || policy.Source != changed.Source || policy.OriginalKind != original || policy.OriginalAction.ActionId != changed.ActionId || policy.ExtraTargetSeat != changed.ExtraTargetSeat)
                throw new InvalidOperationException("Feibai's native conversion lost its owning card-use policy.");
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        }

        return changed.FinalKind;
    }

    internal int FeibaiDeclarationSequence(CurrentTurnCardUseKindsRecordedEvent fact)
    {
        if (fact.WholeActualTurnUseSequence is> 0)
            return fact.WholeActualTurnUseSequence.Value;
        // Setup has real declarations but intentionally has no gameplay ordinal.
        // Its exact native GameStarting mother bounds this one setup-only prefix.
        var name = FeibaiRecordedName(fact);
        if (fact.TurnNumber != 0 || FeibaiSetupEpoch is not { } epoch || name.SetupEpochId != epoch)
            throw new InvalidOperationException("Feibai cannot invent a whole-turn ordinal outside an actual turn or its original setup mother.");
        var names = _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiDeclaredNameEvent>().Where(e => e.ActualTurn == 0 && e.TurnOwnerSeat == fact.TurnOwnerSeat && e.SetupEpochId == epoch).ToArray();
        return Array.IndexOf(names, name) + 1;
    }

    internal CurrentTurnCardUseKindsRecordedEvent? FeibaiDeclarationForAction(CardActionContext action, CurrentTurnCardUseKindsRecordedEvent[] facts)
    {
        var matching = facts.Where(e => e.CardActionId == action.ActionId).ToArray();
        if (matching is [var native])
            return native;
        if (matching.Length != 0)
            throw new InvalidOperationException("Feibai found duplicate actual declaration identities.");
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Action?.ActionId == action.ActionId);
        if (action.Type != CardActionType.Use || use is null || !(_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || _coreActualHandGainProgramsOperations.IsNormalizedLegacyCompletedCategoryUse(use, action.ActorSeat, action.EffectiveKind, action.TargetSeats)))
            return null;
        return facts.Where(e => e.CardActionId is null && e.OriginFrameId == use.Id && !e.IsNullificationUse).ToArray()is [var legacy] ? legacy : null;
    }

    internal bool IsFeibaiTrueUse(CardActionContext action)
    {
        if (action.Type == CardActionType.Use)
            return true;
        if (action.EffectiveKind == CardKind.Nullification)
            return _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.NullificationResponse);
        if (!_coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.Dodge))
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } kind } attack } && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.TargetSeat == action.ActorSeat;
    }

    internal FeibaiUsePrefixCapturedEvent? FeibaiPrefix(CardActionContext action)
    {
        if (!FeibaiActualUseScopeIsOpen || !IsFeibaiTrueUse(action))
            return null;
        var existing = _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiUsePrefixCapturedEvent>().SingleOrDefault(e => e.ActionId == action.ActionId);
        if (existing is not null)
            return existing.ActorSeat == action.ActorSeat && existing.EffectiveKind == action.EffectiveKind && existing.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && existing.SetupEpochId == FeibaiSetupEpoch ? existing : throw new InvalidOperationException("Feibai cannot change an already frozen actual-use prefix.");
        var facts = _coreWholeActualTurnUsesOperations.WholeActualTurnUseHistory();
        if (FeibaiDeclarationForAction(action, facts)is not { } current || FeibaiDeclarationSequence(current) < 1 || current.RoundActionType != action.Type || current.RoundProviderSeat != action.ProviderSeat)
            throw new InvalidOperationException("Feibai needs its exact native declaration and effective-name dependency.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declarationPositions = history.Select((payload, index) => (payload, index)).Where(item => Equals(item.payload, current)).Select(item => item.index).ToArray();
        if (declarationPositions is not [var declarationPosition] || FeibaiSourceBoundEffectiveUseKind(current, history, declarationPosition + 1, current) != action.EffectiveKind)
            throw new InvalidOperationException("Feibai needs its exact native declaration and effective-name dependency.");
        var prefixEnd = declarationPosition + 1;
        var replacements = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent or ActualTurnCursorReturnedEvent).OfType<ProgramCardUseActorReplacedEvent>().GroupBy(e => e.CardUseFrameId).ToDictionary(g => g.Key, g => g.ToArray());
        CurrentTurnCardUseKindsRecordedEvent? previous = null;
        var seen = new HashSet<(long? , long? , int? )>();
        var sequence = FeibaiDeclarationSequence(current);
        for (var i = 0; i < sequence; i++)
        {
            if (i >= facts.Length || FeibaiDeclarationSequence(facts[i]) != i + 1 || facts[i].RoundActionType is not (CardActionType.Use or CardActionType.Response) || facts[i].RoundProviderSeat is not { } provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(provider))
                throw new InvalidOperationException("Feibai's declaration prefix cannot skip an actual native use or its name.");
            var fact = facts[i];
            _ = FeibaiSourceBoundEffectiveUseKind(fact, history, prefixEnd, current);
            if (!seen.Add(fact.CardActionId is { } id ? (id, null, null) : (null, fact.OriginFrameId, fact.ProgramInstructionIndex)))
                throw new InvalidOperationException("Feibai cannot count one declaration twice.");
            var actor = fact.ActorSeat;
            if (fact.RoundActionType == CardActionType.Use && replacements.TryGetValue(fact.OriginFrameId, out var changes))
                foreach (var change in changes)
                {
                    if (change.PreviousActorSeat != actor || change.OwnerSeat != actor || change.ProviderSeat != provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat))
                        throw new InvalidOperationException("Feibai lost the native actor-replacement chain.");
                    actor = change.ActorSeat;
                }

            if (i == sequence - 1)
            {
                if (fact != current || actor != action.ActorSeat)
                    throw new InvalidOperationException("Feibai's completed actor differs from its final native user.");
                break;
            }

            if (actor == action.ActorSeat)
                previous = fact;
        }

        var length = FeibaiNameLength(action.EffectiveKind);
        var previousKind = previous is null ? (CardKind? )null : FeibaiSourceBoundEffectiveUseKind(previous, history, prefixEnd, current);
        var priorLength = previousKind is { } prior ? FeibaiNameLength(prior) : 0;
        var prefix = new FeibaiUsePrefixCapturedEvent(action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, sequence, action.EffectiveKind, previous?.CardActionId, previousKind, length, priorLength, checked(length + priorLength))
        {
            SetupEpochId = FeibaiSetupEpoch
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(prefix);
        return prefix;
    }

    internal void CaptureLeCaiYongFeibaiQualification(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasLeCaiYongFeibaiCapability || window != SkillProgramTriggerWindow.CardUseCompleted || !IsFeibaiTrueUse(action) || FeibaiPrefix(action)is not { } prefix || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || FeibaiDisabled(action.ActorSeat, prefix.ActualTurn))
            return;
        foreach (var b in _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetInstanceTriggers(window).Where(b => b.SkillId == FeibaiSkill && b.Trigger.Id == FeibaiBinding && LeCaiYongComposition.IsFeibaiTrigger(b.Trigger)))
        {
            var q = new FeibaiQualification(prefix, new(b.SkillId, b.Trigger.Id, action.ActorSeat, b.SkillInstanceId), b.Program.GameplayHash);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiQualifiedEvent>().Any(e => e.Qualification == q))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiQualifiedEvent(q));
        }
    }

    internal FeibaiQualification? FeibaiQualificationFor(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId != FeibaiSkill || candidate.BindingId != FeibaiBinding || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } use ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiQualifiedEvent>().Select(e => e.Qualification).SingleOrDefault(q => q.Prefix.ActionId == use.CardActionId && q.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) && q.GameplayHash == candidate.GameplayHash);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29071, SkillProgramEffectOp.ResolveLeCaiYongFeibai)]
    internal bool CanOfferLeCaiYongFeibai(ProgramTriggerLegalityContext gate) => gate.Owner.IsAlive && FeibaiQualificationFor(gate.Candidate, gate.Context)is { } q && q.Prefix.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && !FeibaiDisabled(gate.Owner.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber) && !_coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiStartedEvent>().Any(e => e.Source == q.Source && e.ActionId == q.Prefix.ActionId);
    internal bool FeibaiWindowMatches(ProgramCardTriggerWindowFrame window, FeibaiQualification q) => global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) == SkillProgramTriggerWindow.CardUseCompleted && window.Action.ActionId == q.Prefix.ActionId && window.Action.ActorSeat == q.Source.OwnerSeat && window.Action.EffectiveKind == q.Prefix.EffectiveKind && IsFeibaiTrueUse(window.Action) && (window.Action.Type != CardActionType.Response || window.CompletedResponseReturn is { IsCommitted: false } && _coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window));
    internal SkillProgramStepOutcome BeginLeCaiYongFeibai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LeCaiYongFeibai is not null || f.SkillId != FeibaiSkill || f.TriggerId != FeibaiBinding || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || FeibaiQualificationFor(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context)is not { } q || FeibaiDisabled(f.OwnerSeat, q.Prefix.ActualTurn) || _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || !FeibaiWindowMatches(window, q) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window) || _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiStartedEvent>().Any(e => e.Source == q.Source && e.ActionId == q.Prefix.ActionId))
            throw new InvalidOperationException("Feibai requires its exact completed true Use and issued source-bound declaration prefix.");
        var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => FeibaiNameLength(card.Kind) == q.Prefix.X).Select(card => card.Id).ToList();
        var randomBefore = _randomOperations._random.State;
        _randomOperations._random.Shuffle(pool);
        var r = new FeibaiReceipt(q, window.Id, window.ParentFrameId, FeibaiStage.Selecting, randomBefore, _randomOperations._random.State)
        {
            Sample = pool.Take(2).ToArray(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaiYongFeibai = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiStartedEvent(f.Id, q.Prefix.ActionId, q.Source, q.GameplayHash, window.Id, window.ParentFrameId, randomBefore, _randomOperations._random.State, r.Sample.Count));
        for (var i = 0; i < r.Sample.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiSampleEntityEvent(f.Id, i, r.Sample[i]));
        if (r.Sample.Count == 0)
        {
            BeginFeibaiFallback(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"飞白：获得一张字数为{(q.Prefix.X)}的牌。", r.Sample, [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = FeibaiSampleChoices(f)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> FeibaiSampleChoices(ProgramSkillFrame f) => f.LeCaiYongFeibai!.Sample.Select(id => new PromptChoice(new($"le-cai-yong.feibai.{(f.Id)}.{(id)}"), $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】。", [id], [], new Dictionary<string, string> { ["program-action"] = "le-cai-yong-feibai-sample", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
    [ProgramAnswer("le-cai-yong-feibai-sample")]
    internal void AnswerFeibaiSample(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (f.LeCaiYongFeibai is not { Stage: FeibaiStage.Selecting } r || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], FeibaiSampleChoices(f).Where(item => item.Id == choice.Id).ToArray()) || choice.Cards is not [var id] || choice.Targets.Count != 0 || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DrawPile)
            throw new InvalidOperationException("Feibai must choose one of its real private deck entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            BeginFeibaiFallback(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LeCaiYongFeibai = r with { Stage = FeibaiStage.ObtainChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(FeibaiObtainReason), movement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { LeCaiYongFeibai = current.LeCaiYongFeibai!with { ObtainedCardId = id, After = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiObtainedEvent(f.Id, id, movement.Sequence));
        });
        ContinueLeCaiYongFeibai(f.Id);
    }

    internal void BeginFeibaiFallback(ProgramSkillFrame f)
    {
        var r = f.LeCaiYongFeibai ?? throw new InvalidOperationException("Feibai fallback has no original sample.");
        if (r.ObtainedCardId is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiFallbackDrawIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Feibai cannot pay fallback after an actual obtain or issue it twice.");
        // Commit before invoking native Draw2: replacement and gain observers can re-enter uses.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiDisabledEvent(f.OwnerSeat, r.Qualification.Prefix.ActualTurn, f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaiYongFeibai = r with { Stage = FeibaiStage.Drawing }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiFallbackDrawIssuedEvent(f.Id, f.OwnerSeat, r.Qualification.Prefix.ActualTurn));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(FeibaiDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveLeCaiYongFeibai)]
    internal void ReturnLeCaiYongFeibaiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.LeCaiYongFeibai is not { Stage: FeibaiStage.Drawing, ObtainedCardId: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiFallbackDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Feibai cannot substitute or replay its native fallback draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LeCaiYongFeibai = r with { Stage = FeibaiStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiFallbackDrawReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        MarkFeibaiActualDrawEntities(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), outcome);
        ContinueLeCaiYongFeibai(f.Id);
    }

    internal bool IsExactFeibaiFallbackDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => f.LeCaiYongFeibai is { Stage: FeibaiStage.DrawChildren, ObtainedCardId: null, DrawOutcome: { } stored } r && f.NativeDraw is null && stored.RecipientSeat == outcome.RecipientSeat && stored.RequestedCount == 2 && stored.AttemptFrameId == outcome.AttemptFrameId && stored.SequenceBefore == outcome.SequenceBefore && stored.SequenceAfter == outcome.SequenceAfter && stored.ActualDrawIds.SequenceEqual(outcome.ActualDrawIds) && stored.ReplacementGainedIds.SequenceEqual(outcome.ReplacementGainedIds) && outcome.RecipientSeat == f.OwnerSeat && outcome.RequestedCount == 2 && FeibaiRootMatches(f) && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiDisabledEvent>().Count(e => e == new FeibaiDisabledEvent(f.OwnerSeat, r.Qualification.Prefix.ActualTurn, f.Id)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiFallbackDrawIssuedEvent>().Count(e => e == new FeibaiFallbackDrawIssuedEvent(f.Id, f.OwnerSeat, r.Qualification.Prefix.ActualTurn)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiFallbackDrawReturnedEvent>().Count(e => e == new FeibaiFallbackDrawReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter)) == 1 && outcome.ActualDrawIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FeibaiDrawReason) == 1);
    internal void ContinueLeCaiYongFeibai(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.LeCaiYongFeibai!;
        if (r.Stage is FeibaiStage.Selecting or FeibaiStage.Drawing)
            return;
        if (AwaitFeibaiChildren(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.LeCaiYongFeibai!;
        if (r.Stage == FeibaiStage.ObtainChildren && r.ObtainedCardId is null)
        {
            BeginFeibaiFallback(f);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeibaiCompletedEvent(f.Id, r.Qualification.Prefix.ActionId, r.ObtainedCardId, r.Stage == FeibaiStage.DrawChildren));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool AwaitFeibaiChildren(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29071)]
    internal bool ResumeLeCaiYongFeibai(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.LeCaiYongFeibai is null)
            return false;
        ContinueLeCaiYongFeibai(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29071)]
    internal bool ReturnLeCaiYongFeibaiMovement(ProgramSkillFrame f)
    {
        if (f.LeCaiYongFeibai is null || f.PendingMovementContinuation is null)
            return false;
        ContinueLeCaiYongFeibai(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 300)]
    internal bool IsFeibaiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.LeCaiYongFeibai is { Stage: FeibaiStage.ObtainChildren or FeibaiStage.DrawChildren } && effect?.Op == SkillProgramEffectOp.ResolveLeCaiYongFeibai && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29071)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29071)]
    internal bool PermitPaidLeCaiYongFeibai(ProgramSkillFrame f)
    {
        if (f.LeCaiYongFeibai is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveLeCaiYongFeibai)]
    internal PromptChoice SelectAiFeibaiSample(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderByDescending(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Cards.Single()).First();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 110)]
    internal CardSnapshot[] FeibaiPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.LeCaiYongFeibai is { Stage: FeibaiStage.Selecting }).SelectMany(f => f.LeCaiYongFeibai!.Sample.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
    internal bool FeibaiRootMatches(ProgramSkillFrame f)
    {
        if (f.LeCaiYongFeibai is not { } r || f.SkillId != FeibaiSkill || f.TriggerId != FeibaiBinding || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash || !LeCaiYongComposition.IsFeibaiProgram(program) || r.Qualification.Source != FeibaiSource(f) || r.Qualification.GameplayHash != f.GameplayHash || context.ParentFrameId != r.WindowId || _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == r.WindowId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != r.NativeParentId || !FeibaiWindowMatches(window, r.Qualification) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == window.Id && r.Sample.Count <= 2 && r.Sample.Distinct().Count() == r.Sample.Count && r.Sample.All(id => id > 0 && FeibaiNameLength(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind) == r.Qualification.Prefix.X) && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiUsePrefixCapturedEvent>().Count(e => e == r.Qualification.Prefix) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiQualifiedEvent>().Count(e => e.Qualification == r.Qualification) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiStartedEvent>().Count(e => e == new FeibaiStartedEvent(f.Id, r.Qualification.Prefix.ActionId, r.Qualification.Source, f.GameplayHash, window.Id, window.ParentFrameId, r.RandomBefore, r.RandomAfter, r.Sample.Count)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<FeibaiSampleEntityEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(r.Sample);
    }

    internal bool FeibaiOperationMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (f.LeCaiYongFeibai is { Stage: FeibaiStage.ObtainChildren, ObtainedCardId: { } id } r)
            return m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FeibaiObtainReason;
        if (f.LeCaiYongFeibai is not { Stage: FeibaiStage.DrawChildren, DrawOutcome: { } o } || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FeibaiDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool FeibaiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!FeibaiRootMatches(f))
            return false;
        if (f.LeCaiYongFeibai is { Stage: FeibaiStage.Drawing } && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => FeibaiOperationMovement(f, m));
    }

    internal bool FeibaiObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame f && (_characterCaiYongLeOperations.FeibaiFirstChild(f, _resolutionStackOperations2._resolutionStack()[child]) || f.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(f, _resolutionStackOperations2._resolutionStack()[child])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? FeibaiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterCaiYongLeOperations.FeibaiFirstChild(f, child), (index) => _characterCaiYongLeOperations.FeibaiObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1250)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1360)]
    internal bool HasFeibaiDamageObserver(long id) => FeibaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1390)]
    internal bool HasFeibaiBeforeDamageObserver(long id) => FeibaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 780)]
    internal bool IsFeibaiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && FeibaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 550)]
    internal bool IsFeibaiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.LeCaiYongFeibai is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && FeibaiFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && FeibaiObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29071)]
    internal bool AdvanceFeibaiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FeibaiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);

    SkillProgramStepOutcome global::CardGame.Core.ILeCaiYongFeibaiHost.Resolve(ProgramSkillFrame f) => BeginLeCaiYongFeibai(f);
}
