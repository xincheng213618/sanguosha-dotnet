#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnDefaultStatsRuntime
{
    internal bool TracksTurnDefaultStats => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AllocateCurrentTurnDefaultStats);

    internal static IReadOnlyList<int> NewTurnDefaultSlots() => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TurnDefaultStats.NewTurnDefaultSlots();
    internal static IReadOnlyDictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment> EmptyTurnDefaultAssignments() => new System.Collections.ObjectModel.ReadOnlyDictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment>(new Dictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment>());
    internal static TurnDefaultStatKind FirstTurnDefaultStat(SkillProgramTriggerWindow window) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TurnDefaultStats.FirstTurnDefaultStat(window);
    internal static string TurnDefaultStatName(TurnDefaultStatKind stat) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TurnDefaultStats.TurnDefaultStatName(stat);
    internal static global::CardGame.Core.GameEngine.TurnDefaultStatState EmptyTurnDefaultState(CardConversionSource source, string hash) => new(source, hash, 0, false, NewTurnDefaultSlots(), 0, EmptyTurnDefaultAssignments(), NewTurnDefaultSlots());
    internal void RecordTurnDefaultStatFact(IGameEvent fact)
    {
        _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatLedgerStarted = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 350)]
    internal void ObserveTurnDefaultStats(IGameEvent payload)
    {
        if (!TracksTurnDefaultStats || payload is not TurnStartedEvent started || started.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || started.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(started.ActorSeat))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[started.ActorSeat];
        var sources = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(instance => instance.Program.Triggers.Where(t => t.Window == SkillProgramTriggerWindow.DrawPhaseStarting && t.Effects is [{ Op: SkillProgramEffectOp.AllocateCurrentTurnDefaultStats }]).Select(t => (Key: new global::CardGame.Core.GameEngine.TurnDefaultStatKey(owner.Seat, instance.SkillId, t.Effects[0].StateId!), Source: new CardConversionSource(instance.SkillId, t.Id, owner.Seat, instance.SkillInstanceId), Hash: instance.Program.GameplayHash))).GroupBy(x => x.Key).ToDictionary(g => g.Key, g => g.OrderBy(x => x.Source.SkillInstanceId, StringComparer.Ordinal).First());
        foreach (var key in _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.Keys.Where(k => k.OwnerSeat == owner.Seat).Union(sources.Keys).OrderBy(k => k.SkillId, StringComparer.Ordinal).ThenBy(k => k.StateId, StringComparer.Ordinal).ToArray())
        {
            var hasBinding = sources.TryGetValue(key, out var binding);
            var qualified = owner.IsAlive && hasBinding;
            var old = _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.GetValueOrDefault(key) ?? EmptyTurnDefaultState(binding.Source, binding.Hash);
            if (old.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
                throw new InvalidOperationException("An actual turn cannot initialize its default-stat pool twice.");
            var source = qualified ? binding.Source : old.Source;
            var hash = qualified ? binding.Hash : old.GameplayHash;
            var slots = old.NextSlots;
            _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates[key] = new(source, hash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, qualified, slots, 0, EmptyTurnDefaultAssignments(), NewTurnDefaultSlots());
            // Even an unqualified or face-down owner consumes the pending pool.
            // A later acquisition during this turn starts from the base values.
            RecordTurnDefaultStatFact(new TurnDefaultStatPoolInitializedEvent(source, hash, key.StateId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner.Seat, qualified, false, slots[0], slots[1], slots[2], slots[3]));
        }
    }

    internal int? GetCurrentTurnDefaultStat(int seat, SkillRuleQuery query)
    {
        if (!TracksTurnDefaultStats || seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber <= 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase is TurnPhase.Finished or TurnPhase.NotStarted)
            return null;
        var stat = query switch
        {
            SkillRuleQuery.DrawCount => TurnDefaultStatKind.DrawCount,
            SkillRuleQuery.AttackRange => TurnDefaultStatKind.AttackRange,
            SkillRuleQuery.SlashLimit => TurnDefaultStatKind.SlashLimit,
            SkillRuleQuery.HandLimit => TurnDefaultStatKind.HandLimit,
            _ => (TurnDefaultStatKind? )null
        };
        if (stat is null)
            return null;
        // Concurrent generic sources use the last actual assignment, rather than
        // dictionary iteration or the current lifetime of a grant instance.
        return _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.Where(p => p.Key.OwnerSeat == seat && p.Value.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.Value.HasPool).Select(p => p.Value.Assigned.GetValueOrDefault(stat.Value)).OfType<global::CardGame.Core.GameEngine.TurnDefaultStatAssignment>().MaxBy(a => a.FrameId)?.Value;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 960)]
    internal bool CanOfferTurnDefaultStatProgram(int ownerSeat, string skillId, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var effect] || !TurnDefaultStatsComposition.IsOperation(effect.Op))
            return true;
        if (context.OwnerSeat != ownerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        if (effect.Op == SkillProgramEffectOp.IncreaseNextTurnDefaultStatMinimum)
            return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == ownerSeat && ownerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.Amount > 0 && context.DamageFrameId is { } damageId && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnDefaultStatMinimumIncreasedEvent>().Any(e => e.Source.OwnerSeat == ownerSeat && e.Source.SkillId == skillId && e.StateId == effect.StateId && e.DamageFrameId == damageId);
        if (ownerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.SourceSeat != ownerSeat || context.TargetSeat != ownerSeat || context.Window is not (SkillProgramTriggerWindow.DrawPhaseStarting or SkillProgramTriggerWindow.PlayPhaseStarting or SkillProgramTriggerWindow.DiscardPhaseStarting))
            return false;
        var key = new global::CardGame.Core.GameEngine.TurnDefaultStatKey(ownerSeat, skillId, effect.StateId!);
        return !_coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.TryGetValue(key, out var state) || state.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !state.HasPool || !state.Assigned.ContainsKey(FirstTurnDefaultStat(context.Window));
    }

    internal SkillProgramStepOutcome ExecuteTurnDefaultStatOperation(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        if (f.InstructionIndex != 1 || f.TurnDefaultStatAllocation is not null || trigger.Effects is not [var configured] || configured != effect || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferTurnDefaultStatProgram(f.OwnerSeat, f.SkillId, trigger, context))
            throw new InvalidOperationException("The default-stat operation lost its exact mandatory candidate or source.");
        return effect.Op == SkillProgramEffectOp.AllocateCurrentTurnDefaultStats ? BeginTurnDefaultStatAllocation(f, effect.StateId!) : IncreaseNextTurnDefaultStatMinimum(f, effect.StateId!);
    }

    internal SkillProgramStepOutcome BeginTurnDefaultStatAllocation(ProgramSkillFrame f, string stateId)
    {
        if (!_coreTurnDefaultStatsValidationOperations.ExactTurnDefaultStatPhaseParent(f))
            throw new InvalidOperationException("Default-stat allocation lost its actual owner phase parent.");
        var key = new global::CardGame.Core.GameEngine.TurnDefaultStatKey(f.OwnerSeat, f.SkillId, stateId);
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var state = _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.GetValueOrDefault(key);
        if (state is null || state.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !state.HasPool)
        {
            var next = state?.NextSlots ?? NewTurnDefaultSlots();
            state = new(source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, true, NewTurnDefaultSlots(), 0, EmptyTurnDefaultAssignments(), next);
            _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates[key] = state;
            RecordTurnDefaultStatFact(new TurnDefaultStatPoolInitializedEvent(source, f.GameplayHash, stateId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, true, true, 1, 2, 3, 4));
        }

        var stat = FirstTurnDefaultStat(f.WindowContext!.Window);
        var origin = new TurnDefaultStatAllocationStartedEvent(f.Id, source, f.GameplayHash, stateId, f.WindowContext.ParentFrameId, f.WindowContext.Window, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, stat, state.UsedSlotMask, state.Slots[0], state.Slots[1], state.Slots[2], state.Slots[3]);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TurnDefaultStatAllocation = new() { Origin = origin, CurrentStat = stat, UsedSlotMask = state.UsedSlotMask, Slots = state.Slots } });
        RecordTurnDefaultStatFact(origin);
        PublishTurnDefaultStatChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome IncreaseNextTurnDefaultStatMinimum(ProgramSkillFrame f, string stateId)
    {
        if (!_coreTurnDefaultStatsValidationOperations.ExactTurnDefaultStatDamageParent(f, out var damage, out var window, out var attack))
            throw new InvalidOperationException("A next-turn minimum increment requires one real positive applied-damage event outside the owner's turn.");
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var key = new global::CardGame.Core.GameEngine.TurnDefaultStatKey(f.OwnerSeat, f.SkillId, stateId);
        var state = _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates.GetValueOrDefault(key) ?? EmptyTurnDefaultState(source, f.GameplayHash);
        var values = state.NextSlots.ToArray();
        var minimum = values.Min();
        var slot = Array.IndexOf(values, minimum);
        values[slot] = checked(minimum + 1);
        _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates[key] = state with
        {
            NextSlots = Array.AsReadOnly(values)
        };
        RecordTurnDefaultStatFact(new TurnDefaultStatMinimumIncreasedEvent(f.Id, source, f.GameplayHash, stateId, window.Id, damage.Id, attack.ResolutionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, damage.Amount, damage.Nature, slot, minimum, values[slot]));
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<PromptChoice> TurnDefaultStatChoices(ProgramSkillFrame f)
    {
        var r = f.TurnDefaultStatAllocation!;
        return Array.AsReadOnly(Enumerable.Range(0, 4).Where(slot => (r.UsedSlotMask & (1 << slot)) == 0).Select(slot => PromptChoiceProjection.Freeze(new PromptChoice(new($"turn-default-stat.{(f.Id)}.{(r.CurrentStat)}.{(slot)}"), $"{(TurnDefaultStatName(r.CurrentStat))}：{(r.Slots[slot])}", [], [], new Dictionary<string, string> { ["program-action"] = "turn-default-stat", ["stat-kind"] = r.CurrentStat.ToString(), ["slot-index"] = slot.ToString(CultureInfo.InvariantCulture), ["value"] = r.Slots[slot].ToString(CultureInfo.InvariantCulture) }))).ToArray());
    }

    internal void PublishTurnDefaultStatChoice(ProgramSkillFrame f)
    {
        var choices = TurnDefaultStatChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("A mandatory default-stat allocation has no remaining slot.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"请选择本回合的{(TurnDefaultStatName(f.TurnDefaultStatAllocation!.CurrentStat))}。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("turn-default-stat")]
    internal void ResolveTurnDefaultStatChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Default-stat choice lost its owner.");
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        _coreTurnDefaultStatsValidationOperations.AssertTurnDefaultStatAllocation(f, plan);
        if (f.TurnDefaultStatAllocation is not { } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } decision || !_coreTurnDefaultStatsValidationOperations.IsTurnDefaultStatChoice(f, decision) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], TurnDefaultStatChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("A default-stat choice changed its frozen slot or phase.");
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var key = new global::CardGame.Core.GameEngine.TurnDefaultStatKey(f.OwnerSeat, f.SkillId, r.Origin.StateId);
        var state = _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates[key];
        var assignments = state.Assigned.ToDictionary(p => p.Key, p => p.Value);
        assignments.Add(r.CurrentStat, new(r.Slots[slot], f.Id, slot));
        var mask = r.UsedSlotMask | (1 << slot);
        _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess._turnDefaultStatStates[key] = state with
        {
            UsedSlotMask = mask,
            Assigned = new System.Collections.ObjectModel.ReadOnlyDictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment>(assignments)
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        RecordTurnDefaultStatFact(new TurnDefaultStatAssignedEvent(f.Id, r.Origin.Source, r.Origin.StateId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, r.CurrentStat, slot, r.Slots[slot], mask));
        if (r.CurrentStat == TurnDefaultStatKind.DrawCount && _resolutionStackOperations._resolutionStack[^2] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting, FrozenBaseDrawCount: not null } draw && draw.Id == r.Origin.ParentFrameId && draw.OwnerSeat == f.OwnerSeat)
            _coreRuntimeOperations.ReplaceRuntimeFrame(draw.Id, draw with { FrozenBaseDrawCount = _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) });
        if (r.CurrentStat == TurnDefaultStatKind.AttackRange)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TurnDefaultStatAllocation = r with { CurrentStat = TurnDefaultStatKind.SlashLimit, UsedSlotMask = mask } });
            PublishTurnDefaultStatChoice(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TurnDefaultStatAllocation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    SkillProgramStepOutcome global::CardGame.Core.ITurnDefaultStatsProgramHost.ExecuteTurnDefaultStatOperation(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteTurnDefaultStatOperation(f, e);
}
