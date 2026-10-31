#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureVirtualDuelLegacy, 3)]
    internal void CaptureMoDcLegacyDuel(ProgramSkillFrame f, int actor, int target)
    {
        if (!HasMoDiaoChan || ActiveMoDcPromises(actor).Length == 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase)
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (f.MoDiaoChanLegacyDuel is not null || _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(f.Id) != f.InstructionIndex || f.CardAttack is not { EffectiveCardKind: CardKind.Duel, Active: true, CardId: null, PhysicalCardIds.Count: 0 })
            throw new InvalidOperationException("Huanhuo direct Duel requires its real freshly issued native producer.");
        var entry = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var r = new MoDiaoChanLegacyDuelReceipt(entry, f.Id, f.InstructionIndex, MoDcSource(f), f.GameplayHash, actor, target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MoDiaoChanLegacyDuel = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanLegacyDuelIssuedEvent(r));
        foreach (var p in ActiveMoDcPromises(actor))
        {
            var ordinal = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim.PromiseId == p.PromiseId) + 1;
            var q = new MoDiaoChanUseClaim(p.PromiseId, f.Id, -entry, actor, actor, CardKind.Duel, phase.Token, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, ordinal, p.Source with { BindingId = "affected-use-completed" }, p.GameplayHash);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { MoDiaoChanClaims = (f.MoDiaoChanClaims ?? []).Append(q).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanUseClaimedEvent(q));
        }
    }

    internal bool MoDcLegacyProducerMatches(ProgramSkillFrame parent, MoDiaoChanLegacyDuelReceipt r) => r.ProducerFrameId == parent.Id && r.ProducerSource == MoDcSource(parent) && r.ProducerGameplayHash == parent.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program?.GameplayHash == r.ProducerGameplayHash && _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(parent.Id) == r.InstructionIndex && parent.CardAttack is { EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanLegacyDuelIssuedEvent>().Count(e => e.Receipt == (r with { Returned = false, Settled = false, WindowId = 0, DamageApplied = false, FinalTarget = 0 })) == 1;
    // Runs before Wenlan's own direct-Duel completion. Return routes through the
    // same native phase again after signing Settled, so other real producers keep their order.
    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 29080)]
    internal bool ReturnMoDcLegacyDuel(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoDiaoChanLegacyDuel: { Settled: false } r } parent || parent.Id != parentId)
            return false;
        if (!MoDcLegacyProducerMatches(parent, r) || completion.ResolutionId != parent.Id || completion.DelayedTurn || completion.RequestedSlashChild || completion.DelayedTurnSeat is not null || parent.CardAttack is not { Active: false } || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null)
            throw new InvalidOperationException("Huanhuo lost its completely returned native direct Duel.");
        if (!r.Returned)
        {
            r = r with
            {
                Returned = true,
                DamageApplied = completion.DamageWasApplied,
                FinalTarget = completion.FinalTargetSeat
            };
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent = parent with { MoDiaoChanLegacyDuel = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanLegacyDuelReturnedEvent(r.EntryId, parent.Id, r.DamageApplied, r.FinalTarget));
        }

        return BeginNextMoDcLegacyClaim(parent);
    }

    internal bool BeginNextMoDcLegacyClaim(ProgramSkillFrame parent)
    {
        var r = parent.MoDiaoChanLegacyDuel!;
        var claim = parent.MoDiaoChanClaims?.FirstOrDefault(q => q.ActionId == -r.EntryId && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanUseSettledEvent>().Any(e => e.PromiseId == q.PromiseId && e.ActionId == q.ActionId));
        if (claim is null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MoDiaoChanLegacyDuel = r with { Settled = true, WindowId = 0 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanLegacyDuelResumedEvent(r.EntryId, parent.Id));
            return false;
        }

        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        r = r with
        {
            WindowId = id
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MoDiaoChanLegacyDuel = r });
        var candidate = new ProgramTriggerCandidate(claim.Source.OwnerSeat, claim.Source.SkillId, claim.Source.BindingId, claim.Source.SkillInstanceId, claim.GameplayHash, 0);
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.CardUseCompleted, id, candidate.OwnerSeat, SourceSeat: r.ActorSeat, TargetSeat: r.TargetSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]));
        _coreRuntimeOperations.PushRuntimeFrame(new MoDiaoChanDirectDuelWindowFrame(id, parent.Id, r, claim, candidate, context));
        _coreProgramLifecycleOperations.BeginProgramBinding(candidate, context);
        return true;
    }

    internal MoDiaoChanUseClaim? MoDcDirectClaim(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not MoDiaoChanDirectDuelWindowFrame w || w.Candidate != c || w.Context != context || context.CardUse is not null || context.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramSkillFrame p || p.MoDiaoChanLegacyDuel != w.Duel || !w.Duel.Returned || w.Duel.Settled || w.Duel.WindowId != w.Id || !MoDcLegacyProducerMatches(p, w.Duel) || w.Claim.ActionId != -w.Duel.EntryId || w.Claim.ParentFrameId != p.Id || p.MoDiaoChanClaims?.Contains(w.Claim) != true || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanLegacyDuelReturnedEvent>().Count(e => e.EntryId == w.Duel.EntryId && e.ProducerFrameId == p.Id) != 1)
            return null;
        return w.Claim;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 1)]
    internal bool TryReturnMoDcLegacyBinding(ProgramSkillFrame child, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not MoDiaoChanDirectDuelWindowFrame w || child.WindowContext?.ParentFrameId != w.Id)
            return false;
        if (MoDcDirectClaim(w.Candidate, w.Context) != w.Claim || child.SkillId != w.Candidate.SkillId || child.SkillInstanceId != w.Candidate.SkillInstanceId || child.OwnerSeat != w.Candidate.OwnerSeat || child.GameplayHash != w.Candidate.GameplayHash || completed && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanUseSettledEvent>().Any(e => e.PromiseId == w.Claim.PromiseId && e.ActionId == w.Claim.ActionId))
            throw new InvalidOperationException("Huanhuo native direct Duel lost its exact completed child.");
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.MoDiaoChanDirectDuelWindow);
        var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(w.ParentFrameId);
        if (BeginNextMoDcLegacyClaim(parent))
            return true;
        var r = _coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id).MoDiaoChanLegacyDuel!;
        if (!_coreProgramNativeReturnsOperations.NativeReturns.ReturnAttack(NativeAttackReturnPhase.BeforeParentResume, new(parent.Id, false, parent.Id, null, DamageWasApplied: r.DamageApplied, FinalTargetSeat: r.FinalTarget)))
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }
}
