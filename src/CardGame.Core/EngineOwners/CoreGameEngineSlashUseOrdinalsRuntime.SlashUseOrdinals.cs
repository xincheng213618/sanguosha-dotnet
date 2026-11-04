#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashUseOrdinalsRuntime
{
    internal bool MatchesSlashUseOrdinalModifier(SkillProgramDamageModifier modifier, GameEngine.IDamageAttempt? attack, int actorSeat, CardKind effectiveKind)
    {
        var elemental = modifier.Condition == SkillProgramDamageModifierCondition.OwnerActualTurnElementalSlashUseOrdinalWithinLimit;
        var wholeGame = modifier.Condition == SkillProgramDamageModifierCondition.OwnerGameSlashUseOrdinalMultipleOfThree;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind) || elemental && effectiveKind == CardKind.Slash || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat))
            return false;
        var dependencies = _contentRegistryOperations._contentRegistry.ProgramDependencies;
        if (wholeGame ? !dependencies.HasGameSlashUseDamageModifiers : elemental ? !dependencies.HasActualTurnElementalSlashUseDamageModifiers : !dependencies.HasRoundSlashUseDamageModifiers)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var end = history.Length;
        CurrentTurnCardUseKindsRecordedEvent? current = null;
        if (attack is not null)
        {
            if (attack is not NativeOwners.CardAttackHandle || attack.IsSourceLess || attack.IsChainPropagation || attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage || attack.CardUserSeat != actorSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == attack.ResolutionId)is not CardUseFrame use)
                return false;
            var matches = history.Select((fact, index) => (fact, index)).Where(item => item.fact is CurrentTurnCardUseKindsRecordedEvent fact && fact.OriginFrameId == use.Id && !fact.IsNullificationUse && fact.RoundActionType == CardActionType.Use && fact.ProgramInstructionIndex is null).ToArray();
            if (matches is not [var match] || match.fact is not CurrentTurnCardUseKindsRecordedEvent issued || issued.CardActionId != use.Action?.ActionId || issued.RoundProviderSeat != (use.Action?.ProviderSeat ?? issued.ActorSeat) || issued.CardCategoryMask != 1 << (int)SkillProgramCardCategory.Basic)
                throw new InvalidOperationException("Slash damage requires its exact effective native-use declaration.");
            current = issued;
            end = match.index + 1;
            if (SlashUseKindAtPrefix(issued, history, end, current) != effectiveKind || effectiveKind != use.CardKind)
                throw new InvalidOperationException("Slash damage lost its exact native effective-kind evolution.");
        }
        else if (wholeGame ? _coreActualTurnProgressTurnNumberAccess._turnNumber < 1 : elemental ? !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn : _coreActualRoundProgressRoundNumberAccess._roundNumber < 1)
            return false;
        var boundary = -1;
        for (var index = end - 1; !wholeGame && index >= 0; index--)
        {
            if (elemental ? history[index] is TurnStartedEvent : history[index] is RoundStartedEvent)
            {
                boundary = index;
                break;
            }
        }

        if (!wholeGame && boundary < 0)
            return false;
        if (elemental)
        {
            var turn = (TurnStartedEvent)history[boundary];
            if (turn.TurnNumber < 1 || current is not null && (current.TurnNumber != turn.TurnNumber || current.TurnOwnerSeat != turn.ActorSeat) || history.Skip(boundary + 1).Take(end - boundary - 1).OfType<ActualTurnCursorReturnedEvent>().Any())
                return false;
        }
        else if (!wholeGame && ((RoundStartedEvent)history[boundary]).RoundNumber < 1)
            return false;
        var count = CountSlashUsePrefix(history, boundary + 1, end, actorSeat, elemental, current);
        // A preview represents the next real declaration, never consumes a use.
        var ordinal = current is null ? checked(count + 1) : count;
        return ordinal > 0 && (wholeGame ? ordinal % 3 == 0 : ordinal <= modifier.MaximumUseOrdinal);
    }

    internal int CountSlashUsePrefix(IReadOnlyList<IGameEvent> history, int start, int end, int expectedActorSeat, bool elemental, CurrentTurnCardUseKindsRecordedEvent? current)
    {
        // Reuse the ledger's exact native actor-replacement contract. Defending
        // Responses share an origin with the attack and must not follow it.
        var lookup = new SlashUsePrefixLookup(history);
        var seen = new HashSet<(long? ActionId, long? LegacyUseId, int? InstructionIndex)>();
        var count = 0;
        for (var index = start; index < end; index++)
        {
            if (history[index] is not CurrentTurnCardUseKindsRecordedEvent fact)
                continue;
            (long? ActionId, long? LegacyUseId, int? InstructionIndex) key = fact.CardActionId is not null ? (fact.CardActionId, null, null) : (null, fact.OriginFrameId, fact.ProgramInstructionIndex);
            if (!seen.Add(key) || fact.EffectiveCardKind is null || fact.RoundActionType is not (CardActionType.Use or CardActionType.Response) || fact.RoundProviderSeat is not { } provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(provider))
                throw new InvalidOperationException("A Slash-use prefix requires unique effective declarations and providers.");
            if (fact.RoundActionType != CardActionType.Use)
                continue;
            var kind = SlashUseKindAtPrefix(fact, lookup, end, current, fact.EffectiveCardKind.Value);
            var actor = fact.ActorSeat;
            foreach (var entry in lookup.ActorChanges(fact.OriginFrameId))
                if (entry.Index >= start && (entry.Index < end || current is not null && entry.Fact.CardUseFrameId == current.OriginFrameId))
                {
                    var change = entry.Fact;
                    if (change.PreviousActorSeat != actor || change.OwnerSeat != actor || change.ProviderSeat != provider || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.ActorSeat))
                        throw new InvalidOperationException("A Slash-use prefix lost its native actor-replacement chain.");
                    actor = change.ActorSeat;
                }

            if (fact == current && actor != expectedActorSeat)
                return 0;
            if (actor == expectedActorSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (!elemental || kind != CardKind.Slash))
                count = checked(count + 1);
        }

        return count;
    }

    internal CardKind SlashUseKindAtPrefix(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent? current) => SlashUseKindAtPrefix(fact, history, end, current, fact.EffectiveCardKind ?? throw new InvalidOperationException("A Slash-use ordinal requires its frozen declaration kind."));
    internal CardKind SlashUseKindAtPrefix(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent? current, CardKind original) =>
        SlashUseKindAtPrefix(fact, new SlashUsePrefixLookup(history), end, current, original);

    private CardKind SlashUseKindAtPrefix(CurrentTurnCardUseKindsRecordedEvent fact, SlashUsePrefixLookup lookup, int end, CurrentTurnCardUseKindsRecordedEvent? current, CardKind original)
    {
        if (fact.EffectiveCardKind is { } recorded && recorded != original)
            throw new InvalidOperationException("A proven declaration kind differs from its native declaration receipt.");
        var changes = lookup.FireChanges(fact.OriginFrameId);
        var changeCount = 0;
        (int Index, ProgramCurrentSlashFireChangedEvent Fact) entry = default;
        foreach (var candidate in changes)
            if (candidate.Index < end || fact == current)
            {
                entry = candidate;
                changeCount++;
            }
        if (changeCount == 0)
            return original;
        var declarationIndexes = lookup.DeclarationPositions(fact);
        var change = entry.Fact;
        if (changeCount != 1 || declarationIndexes.Count != 1 || entry.Index <= declarationIndexes[0] || fact.RoundActionType != CardActionType.Use || fact.CardActionId != change.ActionId || !CoreNativeCardClassificationsRuntime.IsSlashCard(original) || change.OriginalKind != original || change.FinalKind != CardKind.FireSlash || lookup.DeclaredCount(fact.OriginFrameId, fact.ActorSeat, original) != 1)
            throw new InvalidOperationException("An effective Slash kind requires one exact native committed conversion.");
        var starts = lookup.BindingStarts(change.ProgramFrameId);
        if (starts.Count != 1)
            throw new InvalidOperationException("A converted Slash lost its exact committed binding issuance.");
        var start = starts[0];
        var binding = start.Fact;
        if (start.Index >= entry.Index || binding.SkillId != change.Source.SkillId || binding.BindingId != change.Source.BindingId || binding.SkillInstanceId != change.Source.SkillInstanceId || binding.OwnerSeat != change.Source.OwnerSeat || binding.Window != SkillProgramTriggerWindow.CardUseCommitted || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(binding.SkillId)?.Program is not { } program || ProgramInstructionResolver.Default.FindTrigger(program, binding.BindingId)is not { } trigger || trigger.Window != binding.Window || trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || trigger.CardKinds.Count != 0 && !ProgramCardKindMatch.Contains(trigger.CardKinds, original) || !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
            throw new InvalidOperationException("A converted Slash lost its exact committed binding issuance.");
        var actor = fact.ActorSeat;
        foreach (var actorEntry in lookup.ActorChanges(fact.OriginFrameId))
        {
            if (actorEntry.Index >= entry.Index) break;
            var replacement = actorEntry.Fact;
            if (replacement.PreviousActorSeat != actor || replacement.OwnerSeat != actor || replacement.ProviderSeat != fact.RoundProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(replacement.ActorSeat))
                throw new InvalidOperationException("A converted Slash lost its earlier native actor chain.");
            actor = replacement.ActorSeat;
        }

        if (actor != change.Source.OwnerSeat)
            throw new InvalidOperationException("A committed Slash conversion must belong to its actual card user.");
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == change.ProgramFrameId)is ProgramSkillFrame producer)
        {
            var plan = ProgramInstructionResolver.Default.Resolve(producer, program);
            if (producer.SkillId != binding.SkillId || producer.TriggerId != binding.BindingId || producer.SkillInstanceId != binding.SkillInstanceId || producer.OwnerSeat != binding.OwnerSeat || producer.GameplayHash != program.GameplayHash || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } useContext } || useContext.ParentCardUseFrameId != fact.OriginFrameId || useContext.CardActionId != change.ActionId || producer.InstructionIndex < 1 || producer.InstructionIndex > plan.Instructions.Count || !plan.Instructions.Take(producer.InstructionIndex).Any(effect => effect.Op == SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget))
                throw new InvalidOperationException("A live Slash conversion lost its exact paused committed producer.");
        }
        else if (lookup.BindingReturns(binding.FrameId).Count(returned => returned.Index > entry.Index && returned.Fact.SkillId == binding.SkillId && returned.Fact.BindingId == binding.BindingId && returned.Fact.SkillInstanceId == binding.SkillInstanceId && returned.Fact.OwnerSeat == binding.OwnerSeat && returned.Fact.Window == binding.Window && returned.Fact.Activated) != 1)
            throw new InvalidOperationException("A historical Slash conversion lost its exact native producer return.");
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.OriginFrameId)is { } use)
        {
            if (use.CurrentSlashFirePolicy is not { } policy || policy.ProgramFrameId != change.ProgramFrameId || policy.Source != change.Source || policy.OriginalKind != original || policy.OriginalAction.ActionId != change.ActionId || policy.ExtraTargetSeat != change.ExtraTargetSeat)
                throw new InvalidOperationException("A converted Slash lost its owning native kind-change receipt.");
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        }

        return change.FinalKind;
    }

    // One interpretation query owns these structural indexes. They retain full
    // journal positions; validation and each caller's cutoff stay above.
    private sealed class SlashUsePrefixLookup(IReadOnlyList<IGameEvent> history)
    {
        private Dictionary<long, List<(int Index, ProgramCardUseActorReplacedEvent Fact)>>? _actors;
        private Dictionary<long, List<(int Index, ProgramCurrentSlashFireChangedEvent Fact)>>? _fires;
        private Dictionary<CurrentTurnCardUseKindsRecordedEvent, List<int>>? _declarations;
        private Dictionary<(long UseId, int Actor, CardKind Kind), int>? _declaredCounts;
        private Dictionary<long, List<(int Index, ProgramBindingStartedEvent Fact)>>? _starts;
        private Dictionary<long, List<(int Index, ProgramBindingResolvedEvent Fact)>>? _returns;

        internal IReadOnlyList<(int Index, ProgramCardUseActorReplacedEvent Fact)> ActorChanges(long useId)
        {
            if (_actors is null)
            {
                _actors = [];
                for (var index = 0; index < history.Count; index++)
                    if (history[index] is ProgramCardUseActorReplacedEvent actor)
                    {
                        if (!_actors.TryGetValue(actor.CardUseFrameId, out var changes))
                            _actors.Add(actor.CardUseFrameId, changes = []);
                        changes.Add((index, actor));
                    }
            }
            return _actors.TryGetValue(useId, out var result) ? result : [];
        }

        internal IReadOnlyList<(int Index, ProgramCurrentSlashFireChangedEvent Fact)> FireChanges(long useId)
        {
            if (_fires is null)
            {
                _fires = [];
                for (var index = 0; index < history.Count; index++)
                    if (history[index] is ProgramCurrentSlashFireChangedEvent fire)
                    {
                        if (!_fires.TryGetValue(fire.CardUseFrameId, out var changes))
                            _fires.Add(fire.CardUseFrameId, changes = []);
                        changes.Add((index, fire));
                    }
            }
            return _fires.TryGetValue(useId, out var result) ? result : [];
        }

        internal IReadOnlyList<int> DeclarationPositions(CurrentTurnCardUseKindsRecordedEvent fact)
        {
            IndexProofFacts();
            return _declarations!.TryGetValue(fact, out var result) ? result : [];
        }

        internal int DeclaredCount(long useId, int actor, CardKind kind)
        {
            IndexProofFacts();
            return _declaredCounts!.GetValueOrDefault((useId, actor, kind));
        }

        internal IReadOnlyList<(int Index, ProgramBindingStartedEvent Fact)> BindingStarts(long frameId)
        {
            IndexProofFacts();
            return _starts!.TryGetValue(frameId, out var result) ? result : [];
        }

        internal IReadOnlyList<(int Index, ProgramBindingResolvedEvent Fact)> BindingReturns(long frameId)
        {
            IndexProofFacts();
            return _returns!.TryGetValue(frameId, out var result) ? result : [];
        }

        private void IndexProofFacts()
        {
            if (_declarations is not null) return;
            _declarations = [];
            _declaredCounts = [];
            _starts = [];
            _returns = [];
            for (var index = 0; index < history.Count; index++)
                switch (history[index])
                {
                    case CurrentTurnCardUseKindsRecordedEvent declaration:
                        if (!_declarations.TryGetValue(declaration, out var positions))
                            _declarations.Add(declaration, positions = []);
                        positions.Add(index);
                        break;
                    case CardUseDeclaredEvent declared:
                        var key = (declared.ResolutionId, declared.SourceSeat, declared.CardKind);
                        _declaredCounts[key] = checked(_declaredCounts.GetValueOrDefault(key) + 1);
                        break;
                    case ProgramBindingStartedEvent started:
                        if (!_starts.TryGetValue(started.FrameId, out var starts))
                            _starts.Add(started.FrameId, starts = []);
                        starts.Add((index, started));
                        break;
                    case ProgramBindingResolvedEvent returned:
                        if (!_returns.TryGetValue(returned.FrameId, out var returns))
                            _returns.Add(returned.FrameId, returns = []);
                        returns.Add((index, returned));
                        break;
                }
        }
    }
}
