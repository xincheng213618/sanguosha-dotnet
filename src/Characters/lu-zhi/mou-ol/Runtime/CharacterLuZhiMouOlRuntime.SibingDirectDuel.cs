#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouOlRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 32900)]
    internal void CaptureMouLzDirect(ProgramSkillFrame supplied, int actor, int target)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuZhiSibingPress))
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(f.Id) != f.InstructionIndex || f.CardAttack is not { Active: true, EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } a || a.CardUserSeat != actor || a.TargetSeat != target || f.Continuations.Duel is not { Active: true })
            throw new InvalidOperationException("Sibing direct-Duel capture requires the original native producer instruction.");
        var d = new MouLuZhiOlDirectDuel(f.Id, f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, actor, target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouLuZhiOlDirectDuel = d });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlDirectDeclaredEvent(d));
    }

    internal bool ValidMouLzDirect(ProgramSkillFrame p, MouLuZhiOlDirectDuel d) => p.Id == d.FrameId && p.InstructionIndex == d.Instruction && p.GameplayHash == d.Hash && d.Producer == new CardConversionSource(p.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(p), p.OwnerSeat, p.SkillInstanceId) && _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(p.Id) == d.Instruction && p.CardAttack is { EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } a && a.CardUserSeat == d.Actor && (p.TriggerId is null ? p.SelectedTargetSeats.SequenceEqual([d.Actor, d.Target]) : p.OwnerSeat == d.Actor && p.SelectedTargetSeats.SequenceEqual([d.Target])) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlDirectDeclaredEvent>().Count(e => e.Duel == d with { PressClosed = false, Returned = false, CounterClosed = false, DamageApplied = false, FinalTarget = 0 }) == 1;
    internal bool TryBeginMouLzDirectDesignation(long producerId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(producerId);
        if (f.MouLuZhiOlDirectDuel is not { PressClosed: false } d)
            return false;
        if (!ValidMouLzDirect(f, d))
            throw new InvalidOperationException("Sibing lost the native Duel designation identity.");
        return OpenMouLzDirectWindow(f, d, completed: false);
    }

    internal bool OpenMouLzDirectWindow(ProgramSkillFrame p, MouLuZhiOlDirectDuel d, bool completed)
    {
        var owner = completed ? d.Target : d.Actor;
        var binding = completed ? "undamaged-retaliation" : "red-press";
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ProgramInstances.FirstOrDefault(i => i.SkillId == MouLuZhiOlComposition.Sibing);
        var available = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && instance is not null && (completed ? !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlUseDamageEvent>().Any(e => e.OriginalId == d.FrameId && e.Instruction == d.Instruction && e.Target == owner && e.Amount > 0) && MouLzCards(owner, false, false).Count > 0 && MouLzSlashTargets(owner).Length > 0 : MouLzPressAvailable(owner));
        if (!available)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(p.Id, p with { MouLuZhiOlDirectDuel = completed ? d with { CounterClosed = true } : d with { PressClosed = true } });
            return false;
        }

        var t = instance!.Program.Triggers.Single(x => x.Id == binding);
        var c = new ProgramTriggerCandidate(owner, instance.SkillId, binding, instance.SkillInstanceId, instance.Program.GameplayHash, t.Priority);
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var w = new ProgramSkillWindowContext(t.Window, id, owner, SourceSeat: d.Actor, TargetSeat: d.Target);
        var frame = new MouLuZhiOlDirectWindowFrame(id, p.Id, completed, c, w);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlDirectWindowEvent(id, p.Id, d.Instruction, completed, new(c.SkillId, c.BindingId, owner, c.SkillInstanceId), c.GameplayHash));
        AdvanceMouLzDirectWindow(frame);
        return true;
    }

    [NativeFrameRoute(typeof(MouLuZhiOlDirectWindowFrame), 32900)]
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 32900)]
    internal void AdvanceMouLzDirectWindow(MouLuZhiOlDirectWindowFrame w)
    {
        if (!ExactMouLzDirectWindow(w) || !IsMouLzDirectCandidate(w.Candidate, w.Context))
            throw new InvalidOperationException("Sibing direct native window lost its live accepted source.");
        _coreProgramLifecycleOperations.BeginProgramBinding(w.Candidate, w.Context);
    }

    [ProgramIssuedTrigger(50)]
    internal bool IsMouLzDirectCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        return context.CardUse is null && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is MouLuZhiOlDirectWindowFrame w && w.Candidate == c && w.Context == context && ExactMouLzDirectWindow(w) && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive;
    }

    internal bool ExactMouLzDirectWindow(MouLuZhiOlDirectWindowFrame w)
    {
        var c = w.Candidate;
        var context = w.Context;
        if (context.CardUse is not null || context.ParentFrameId != w.Id || context.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramSkillFrame { MouLuZhiOlDirectDuel: { } d } p || !ValidMouLzDirect(p, d) || c.SkillId != MouLuZhiOlComposition.Sibing || c.OwnerSeat != (w.Completed ? d.Target : d.Actor) || c.BindingId != (w.Completed ? "undamaged-retaliation" : "red-press") || c.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash || context.Window != (w.Completed ? SkillProgramTriggerWindow.CardUseCompleted : SkillProgramTriggerWindow.CardUseTargetsFinalized) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlDirectWindowEvent>().Count(e => e.WindowId == w.Id && e.ProducerId == p.Id && e.Instruction == d.Instruction && e.Completed == w.Completed) != 1)
            return false;
        return w.Completed ? d.Returned && !d.CounterClosed && p.CardAttack is { Active: false } : !d.PressClosed && !d.Returned && p.CardAttack is { Active: true };
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 32900)]
    internal bool ReturnMouLzDirect(GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuZhiOlDirectDuel: { Returned: false } d } p || c.ResolutionId != p.Id || c.ProgramFrameId != p.Id)
            return false;
        if (!ValidMouLzDirect(p, d) || p.CardAttack is not { Active: false } || c.DelayedTurn || c.RequestedSlashChild)
            throw new InvalidOperationException("Sibing direct Duel must return its genuine resolved producer attack.");
        d = d with
        {
            Returned = true,
            DamageApplied = c.DamageWasApplied,
            FinalTarget = c.FinalTargetSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(p.Id, p = p with { MouLuZhiOlDirectDuel = d });
        return OpenMouLzDirectWindow(p, d, true);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 32900)]
    internal bool ReturnMouLzDirectBinding(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not MouLuZhiOlDirectWindowFrame w || f.WindowContext?.ParentFrameId != w.Id)
            return false;
        if (f.SkillId != w.Candidate.SkillId || f.TriggerId != w.Candidate.BindingId || f.OwnerSeat != w.Candidate.OwnerSeat || f.SkillInstanceId != w.Candidate.SkillInstanceId || f.GameplayHash != w.Candidate.GameplayHash || !ExactMouLzDirectWindow(w))
            throw new InvalidOperationException("Sibing direct window cannot resume another native producer.");
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.MouLuZhiOlDirectWindow);
        var p = _coreProgramLifecycleOperations.GetActiveProgramFrame(w.ParentFrameId);
        var d = p.MouLuZhiOlDirectDuel!;
        _coreRuntimeOperations.ReplaceRuntimeTop(p with { MouLuZhiOlDirectDuel = w.Completed ? d with { CounterClosed = true } : d with { PressClosed = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlDirectResumedEvent(w.Id, p.Id, w.Completed));
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return true;
        if (w.Completed)
            _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(new(p.Id, false, p.Id, null, DamageWasApplied: d.DamageApplied, FinalTargetSeat: d.FinalTarget));
        else
            _coreNativeDuelOperations.BeginDuelResponse(_coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("Sibing lost its still-unresolved direct Duel."));
        return true;
    }
}
