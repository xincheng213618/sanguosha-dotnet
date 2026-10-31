#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHandComparisonRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 80)]
    internal bool SelfCardDirectedTargetProhibited(ProgramDirectedTargetQuery query) => query.ActorSeat == query.TargetSeat && HasSelfCardTargetProhibition(query.ActorSeat);
    internal IEnumerable<IGameEvent> HandComparisonHistory() => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents);
    internal int? HandComparisonPhaseLimit(CharacterState owner, SkillProgram program, SkillProgramActivation activation, string? instanceId = null)
    {
        var op = ProgramInstructionResolver.Default.Features(activation).ForOperation(SkillProgramEffectOp.CompareSelectedHandWithHpHand).SingleOrDefault(e => e.Op == SkillProgramEffectOp.CompareSelectedHandWithHpHand);
        if (op is null)
            return _characterBuZhiOlOperations.BuZhiPhaseLimit(owner, activation);
        var instance = instanceId ?? _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id);
        return HandComparisonHistory().OfType<ProgramHandComparisonResolvedEvent>().Any(e => e.OwnerSeat == owner.Seat && e.SkillId == program.Id && e.SkillInstanceId == instance && e.StateId == op.StateId && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.RankMatched) ? 2 : 1;
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot>? GetHandComparisonPublicRuleStates(CharacterState owner, string skillId)
    {
        List<ProgramPublicRuleStateSnapshot>? result = null;
        var instances = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances;
        for (var index = 0; index < instances.Count; index++)
        {
            var instance = instances[index];
            if (instance.SkillId != skillId)
                continue;
            var hasPublicState = false;
            for (var i = 0; i < instance.Program.Activations.Count; i++)
            {
                var activation = instance.Program.Activations[i];
                var features = ProgramInstructionResolver.Default.Features(activation);
                if (features.HasOperation(SkillProgramEffectOp.CompareSelectedHandWithHpHand) || _characterBuZhiOlOperations.IsBuZhiActivation(activation))
                    (result ??= []).Add(new(instance.SkillInstanceId, activation.Id, ProgramPublicRuleStateKind.ActivationLimit, HandComparisonPhaseLimit(owner, instance.Program, activation, instance.SkillInstanceId)!.Value, _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, skillId, activation.UsageGroup)), SkillUsageScope.Phase));
                hasPublicState |= features.HasOperation(SkillProgramEffectOp.CompareSelectedHandWithHpHand) || features.HasOperation(SkillProgramEffectOp.AdjustPersistentHandLimit) || features.HasOperation(SkillProgramEffectOp.ProhibitSelfCardTargetsForTurn);
            }

            for (var i = 0; !hasPublicState && i < instance.Program.Triggers.Count; i++)
            {
                var features = ProgramInstructionResolver.Default.Features(instance.Program.Triggers[i]);
                hasPublicState = features.HasOperation(SkillProgramEffectOp.CompareSelectedHandWithHpHand) || features.HasOperation(SkillProgramEffectOp.AdjustPersistentHandLimit) || features.HasOperation(SkillProgramEffectOp.ProhibitSelfCardTargetsForTurn);
            }

            if (!hasPublicState)
                continue;
            AddHandComparisonEffectPublicRuleStates(owner, skillId, instance, result ??= []);
        }

        return result is { Count: > 0 } ? result.ToArray() : null;
    }

    internal void AddHandComparisonEffectPublicRuleStates(CharacterState owner, string skillId, IndexedSkillProgramInstance instance, List<ProgramPublicRuleStateSnapshot> result)
    {
        var effects = instance.Program.Activations.SelectMany(a => a.Effects).Concat(instance.Program.Triggers.SelectMany(t => t.Effects)).ToArray();
        foreach (var stateId in effects.Where(e => e.Op is SkillProgramEffectOp.CompareSelectedHandWithHpHand or SkillProgramEffectOp.AdjustPersistentHandLimit).Select(e => e.StateId!).Distinct())
            result.Add(new(instance.SkillInstanceId, stateId, ProgramPublicRuleStateKind.PersistentHandLimit, HandComparisonHistory().OfType<ProgramPersistentHandLimitChangedEvent>().Where(e => e.OwnerSeat == owner.Seat && e.SkillId == skillId && e.SkillInstanceId == instance.SkillInstanceId && e.StateId == stateId).Sum(e => e.Amount)));
        if (effects.Any(e => e.Op == SkillProgramEffectOp.ProhibitSelfCardTargetsForTurn))
            result.Add(new(instance.SkillInstanceId, "self-target", ProgramPublicRuleStateKind.SelfTargetProhibition, HandComparisonHistory().OfType<ProgramSelfCardTargetsProhibitedEvent>().Any(e => e.OwnerSeat == owner.Seat && e.SkillId == skillId && e.SkillInstanceId == instance.SkillInstanceId && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) ? 1 : 0, null, SkillUsageScope.Turn));
    }

    internal IReadOnlyList<int> GetSelfProhibitionPolicyTargets(CharacterState actor, LegalAction action)
    {
        if (!HasSelfCardTargetProhibition(actor.Seat))
            return _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats).ToArray();
        if (action.Kind == LegalActionKind.BorrowedSword)
            return action.TargetSeats.Where((_, index) => index % 2 == 0).ToArray();
        return action.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.ExcludeOwnerEffects == true ? action.TargetSeats : _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats).ToArray();
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 150)]
    internal bool CanActivateHandComparison(CharacterState owner, SkillProgramActivation activation) => !ProgramInstructionResolver.Default.Features(activation).HasOperation(SkillProgramEffectOp.CompareSelectedHandWithHpHand) || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat && p.Hp > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0);
    internal IEnumerable<RuleQueryContribution> PersistentHandLimitContributions(CharacterState owner)
    {
        foreach (var g in HandComparisonHistory().OfType<ProgramPersistentHandLimitChangedEvent>().Where(e => (e.TargetSeat < 0 ? e.OwnerSeat : e.TargetSeat) == owner.Seat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[e.OwnerSeat], e.SkillId, e.SkillInstanceId)).GroupBy(e => (e.SkillId, e.SkillInstanceId, e.StateId)))
            yield return new FiniteRuleQueryContribution("persistent:" + g.Key, SkillRuleOperation.Add, g.Sum(e => e.Amount), 0);
    }

    internal bool HasSelfCardTargetProhibition(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSelfCardTargetsProhibitedEvent>().Any(e => e.OwnerSeat == seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], e.SkillId, e.SkillInstanceId));
    internal bool IsImplicitSelfCardUse(CardKind kind) => kind is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.Lightning || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind);
    internal bool IsSelfTargetForbiddenAction(CharacterState actor, CardKind kind, IReadOnlyList<int> targets)
    {
        if (!HasSelfCardTargetProhibition(actor.Seat))
            return false;
        var designated = kind == CardKind.BorrowedSword ? targets.Where((_, index) => index % 2 == 0) : targets;
        return designated.Contains(actor.Seat) || targets.Count == 0 && IsImplicitSelfCardUse(kind);
    }

    SkillProgramStepOutcome global::CardGame.Core.IHandComparisonProgramHost.CompareSelectedHandWithHpHand(ProgramSkillFrame f, SkillProgramEffect e) => CompareSelectedHandWithHpHand(f, e);
    void global::CardGame.Core.IHandComparisonProgramHost.AdjustPersistentHandLimit(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => AdjustPersistentHandLimit(f, targetSeat, amount, stateId);
    void global::CardGame.Core.IHandComparisonProgramHost.ProhibitSelfCardTargetsForTurn(ProgramSkillFrame f)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSelfCardTargetsProhibitedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
    }

    internal void AdjustPersistentHandLimit(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPersistentHandLimitChangedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, stateId, amount, targetSeat));
    internal SkillProgramStepOutcome CompareSelectedHandWithHpHand(ProgramSkillFrame f, SkillProgramEffect effect)
    {
        if (f.TriggerId is not null || f.SelectedCardIds is not [var source] || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(source) != CardLocation.Hand(f.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[target].Hp <= 0 || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0)
            throw new InvalidOperationException("A hand comparison requires one own hand card and another living positive-HP nonempty hand.");
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HandComparisonDraft = new(target, source, cards, Math.Min(_coreMatchParticipantsPlayersAccess._players[target].Hp, cards.Length), [], "reveal") });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandComparisonSourceRevealedEvent(f.Id, f.OwnerSeat, source));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "hand-comparison-source"))
            return SkillProgramStepOutcome.AwaitChild;
        PublishHandComparison(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishHandComparison(ProgramSkillFrame f)
    {
        var d = f.HandComparisonDraft!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string token, string label, IReadOnlyList<int> cards, IReadOnlyList<int> seats) => choices.Add(new(new ChoiceId($"hand-comparison.{(f.Id)}.{(branch)}.{(token)}"), label, cards, seats, new Dictionary<string, string> { ["program-action"] = "hand-comparison", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["branch"] = branch, ["token"] = token }));
        if (d.Stage == "reveal")
            foreach (var id in d.CandidateIds.Except(d.RevealedIds))
                Add("reveal", Array.IndexOf(d.CandidateIds.ToArray(), id).ToString(System.Globalization.CultureInfo.InvariantCulture), "展示对方第" + (Array.IndexOf(d.CandidateIds.ToArray(), id) + 1) + "张手牌", [], [d.TargetSeat]);
        else if (d.Stage == "complete")
            Add("complete", "complete", "展示完成，继续", [], []);
        else if (d.Stage == "benefit")
        {
            Add("draw", "draw", "摸一张牌", [], []);
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(f.OwnerSeat, p.Seat)))
                Add("target", p.Seat.ToString(), "弃置" + p.Name + "的一张牌", [], [p.Seat]);
        }
        else
        {
            var target = _coreMatchParticipantsPlayersAccess._players[int.Parse(d.Stage.Split(':')[1], System.Globalization.CultureInfo.InvariantCulture)];
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(target))
                Add("discard", ("hand-" + _corePhysicalOwnedCardQueriesOperations.GetHand(target).ToList().IndexOf(card)), "弃置第" + (_corePhysicalOwnedCardQueriesOperations.GetHand(target).ToList().IndexOf(card) + 1) + "张手牌", [], [target.Seat]);
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, CardLocation.Equipment(target.Seat), OwnedCardMoveIntent.Discard)))
                Add("discard", ("equipment-" + card.Id), "弃置【" + card.DisplayName + " " + global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit) + card.RankText + "】", [card.Id], [target.Seat]);
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, d.Stage == "reveal" ? "选择展示与其体力值等量的手牌。" : "选择同色奖励。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = d.TargetSeat,
            Choices = choices.ToArray(),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    [ProgramAnswer("hand-comparison")]
    internal void ResolveHandComparison(PromptChoice selected)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var d = f.HandComparisonDraft!;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        AssertHandComparison(f, effect);
        if (selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Hand comparison lost its frame.");
        var branch = selected.Parameters["branch"];
        var token = selected.Parameters["token"];
        if (branch == "reveal")
        {
            var slot = int.Parse(token, System.Globalization.CultureInfo.InvariantCulture);
            if (slot < 0 || slot >= d.CandidateIds.Count)
                throw new InvalidOperationException("Invalid hand slot.");
            var id = d.CandidateIds[slot];
            if (d.Stage != "reveal" || !d.CandidateIds.Contains(id) || d.RevealedIds.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(d.TargetSeat))
                throw new InvalidOperationException("Invalid concealed hand reveal.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            d = d with
            {
                RevealedIds = d.RevealedIds.Append(id).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HandComparisonDraft = d });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandComparisonCardRevealedEvent(f.Id, d.TargetSeat, id));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "hand-comparison-target"))
            {
                return;
            }

            ContinueHandComparisonAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (branch == "complete")
        {
            if (d.Stage != "complete")
                throw new InvalidOperationException("Invalid completion choice.");
            FinishHandComparison(f);
            return;
        }

        if (branch == "target")
        {
            var target = int.Parse(token, System.Globalization.CultureInfo.InvariantCulture);
            if (d.Stage != "benefit" || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(f.OwnerSeat, target))
                throw new InvalidOperationException("Invalid reward discard target.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HandComparisonDraft = d with { Stage = "discard:" + target } });
            PublishHandComparison(f);
            return;
        }

        if (branch == "draw")
        {
            if (d.Stage != "benefit")
                throw new InvalidOperationException("Invalid reward draw.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HandComparisonDraft = null });
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new("skill-program.hand-comparison.reward-draw"));
            _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
            return;
        }

        if (branch == "discard")
        {
            if (!d.Stage.StartsWith("discard:", StringComparison.Ordinal))
                throw new InvalidOperationException("Invalid reward discard stage.");
            var target = int.Parse(d.Stage.Split(':')[1], System.Globalization.CultureInfo.InvariantCulture);
            var id = token.StartsWith("hand-", StringComparison.Ordinal) ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target])[int.Parse(token[5..], System.Globalization.CultureInfo.InvariantCulture)].Id : int.Parse(token[10..], System.Globalization.CultureInfo.InvariantCulture);
            var location = _cardZonesOperations._cardZones.GetLocation(id);
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || location.OwnerSeat != target || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Discard reward lost its real cost.");
            var card = _cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == id);
            if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, location, OwnedCardMoveIntent.Discard))
            {
                FinishHandComparison(f);
                return;
            }

            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HandComparisonDraft = null });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, location, new("skill-program.hand-comparison.reward-discard"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
            _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
            return;
        }

        throw new InvalidOperationException("Unknown comparison branch.");
    }

    internal void FinishHandComparison(ProgramSkillFrame f)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HandComparisonDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AssertHandComparison(ProgramSkillFrame f, SkillProgramEffect? op)
    {
        if (f.HandComparisonDraft is not { } d)
            return;
        if (op?.Op != SkillProgramEffectOp.CompareSelectedHandWithHpHand || f.TriggerId is not null || f.SelectedCardIds is not [var source] || source != d.SourceCardId || f.SelectedTargetSeats is not [var target] || target != d.TargetSeat || target == f.OwnerSeat || d.Required <= 0 || d.Required > d.CandidateIds.Count || d.CandidateIds.Distinct().Count() != d.CandidateIds.Count || d.RevealedIds.Distinct().Count() != d.RevealedIds.Count || d.RevealedIds.Count > d.Required || d.RevealedIds.Any(id => !d.CandidateIds.Contains(id)) || d.CandidateIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(target)) || _cardZonesOperations._cardZones.GetLocation(source) != CardLocation.Hand(f.OwnerSeat) || d.Stage != "reveal" && d.Stage != "complete" && d.Stage != "benefit" && !d.Stage.StartsWith("discard:", StringComparison.Ordinal))
            throw new InvalidOperationException("Invalid serialized hand comparison.");
        if (ReferenceEquals(f, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: false } p || p.PlayerSeat != f.OwnerSeat))
            throw new InvalidOperationException("Comparison lost its public chooser.");
    }

    [PublicHandRevealReturn("hand-comparison-target")]
    internal void ContinueHandComparisonAfterPublicReveal(ProgramSkillFrame f)
    {
        var d = f.HandComparisonDraft ?? throw new InvalidOperationException("Hand comparison lost its public reveal draft.");
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (d.RevealedIds.Count < d.Required)
        {
            PublishHandComparison(f);
            return;
        }

        var own = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == d.SourceCardId);
        var revealed = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat]).Where(c => d.RevealedIds.Contains(c.Id)).ToArray();
        var color = revealed.Any(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat], c)) == global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], own)));
        var rank = revealed.Any(c => (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat], c) ?? c.Rank) == (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], own) ?? own.Rank));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandComparisonResolvedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, effect.StateId!, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, d.SourceCardId, d.TargetSeat, d.RevealedIds, color, rank));
        if (!color && !rank)
            AdjustPersistentHandLimit(f, f.OwnerSeat, -1, effect.StateId!);
        if (color)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HandComparisonDraft = d with { Stage = "benefit" } });
            PublishHandComparison(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HandComparisonDraft = d with { Stage = "complete" } });
        PublishHandComparison(f);
        return;
    }
}
