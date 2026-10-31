#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPopulationAndArrowBarrageOperationsRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterInitialPayments, 20, SkillProgramEffectOp.GrantFactionPopulationMarker)]
    internal bool AllowsGrantFactionPopulationMarkerTrigger(ProgramTriggerLegalityContext gate)
    {
        var owner = gate.Owner;
        var features = gate.Features;
        var candidate = gate.Candidate;
        var context = gate.Context;
        return !(features.HasOperation(SkillProgramEffectOp.GrantFactionPopulationMarker) && HasInitialPopulationMarkerGrant(owner.Seat, candidate.SkillId, context));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterInitialPayments, 10, SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget)]
    internal bool AllowsRemoveSelectedCurrentArrowBarrageTargetTrigger(ProgramTriggerLegalityContext gate)
    {
        var owner = gate.Owner;
        var features = gate.Features;
        var context = gate.Context;
        return !(features.HasOperation(SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget) && GetCurrentArrowBarrageTargets(owner.Seat, context).Count == 0);
    }

    internal bool HasInitialPopulationMarkerGrant(int ownerSeat, string skillId, ProgramSkillWindowContext context) => _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)?.PopulationMarkerGrants?.Contains(new(ownerSeat, skillId)) == true;
    internal void GrantProgramFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, FrozenFactionPopulation: { } population } parent || parent.Id != context.ParentFrameId || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId) || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId, Role.Lord))
            throw new InvalidOperationException("Population marker grant lost its qualified game-start owner.");
        if (HasInitialPopulationMarkerGrant(active.OwnerSeat, active.SkillId, context))
            return;
        var amount = checked(effect.Amount * population.GetValueOrDefault(effect.ProviderFactionId!));
        if (amount < 0)
            throw new InvalidOperationException("Population marker amount is negative.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { PopulationMarkerGrants = Array.AsReadOnly((parent.PopulationMarkerGrants ?? []).Append(new(active.OwnerSeat, active.SkillId)).ToArray()) });
        if (amount > 0)
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(active, active.OwnerSeat, effect.Marker!.Value, amount);
    }

    internal IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } cardContext } || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != context.ParentFrameId || window.ParentFrameId != cardContext.ParentCardUseFrameId || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == window.ParentFrameId)is not { Action: { } action } use || window.Action.ActionId != action.ActionId || action.ActionId != cardContext.CardActionId || action.Type != CardActionType.Use || action.EffectiveKind != CardKind.ArrowBarrage || action.ActorSeat != ownerSeat || use.SourceSeat != ownerSeat || use.ArrowBarrageExclusion is not null)
            return[];
        return use.TargetSeats.Where(seat => action.EffectiveDesignatedTargetSeats.Contains(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Distinct().ToArray();
    }

    internal void RemoveProgramSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var targetSeat] || !GetCurrentArrowBarrageTargets(active.OwnerSeat, active.WindowContext).Contains(targetSeat))
            throw new InvalidOperationException("The ArrowBarrage exclusion no longer names one actual remaining target.");
        var cardContext = active.WindowContext!.CardUse!;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == cardContext.ParentCardUseFrameId);
        var targets = Array.AsReadOnly(use.TargetSeats.Where(seat => seat != targetSeat).ToArray());
        var action = _coreProgramCardUseRolesOperations.CloneRoleAction(use.Action!, use.SourceSeat, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, ArrowBarrageExclusion = new(active.Id, active.OwnerSeat, active.SkillId, active.SkillInstanceId, action.ActionId, targetSeat) }, action);
        // Rebase only this changed actual use's remaining candidate contexts. Historical
        // target-confirmed facts and the old selector's query remain untouched.
        foreach (var window in _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Where(window => window.ParentFrameId == use.Id).ToArray())
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Candidates = Array.AsReadOnly(window.Candidates.Select(candidate => candidate.FrozenContext?.CardUse is { } frozen && frozen.ParentCardUseFrameId == use.Id ? candidate with { FrozenContext = candidate.FrozenContext with { CardUse = frozen with { ActorSeat = action.ActorSeat, DesignatedTargetSeats = targets } } } : candidate).ToArray()) });
        for (var i = 0; i < _resolutionStackOperations._resolutionStack.Count; i++)
            if (_resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame program && program.WindowContext?.CardUse is { } current && current.ParentCardUseFrameId == use.Id)
                _coreRuntimeOperations.ReplaceRuntimeFrame(program.Id, program with { WindowContext = program.WindowContext with { CardUse = current with { ActorSeat = action.ActorSeat, DesignatedTargetSeats = targets } } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramArrowBarrageTargetRemovedEvent(use.Id, action.ActionId, active.OwnerSeat, active.SkillId, targetSeat));
    }

    void IPopulationAndArrowBarrageProgramHost.GrantFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect) => GrantProgramFactionPopulationMarker(frame, effect);
    void IPopulationAndArrowBarrageProgramHost.RemoveSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame) => RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
