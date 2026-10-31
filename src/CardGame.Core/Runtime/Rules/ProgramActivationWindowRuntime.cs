namespace CardGame.Core;

/// <summary>Finds an activation's exact native parent and returns skipped candidates to that owner.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramActivationWindowRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramActivationWindowFacts facts, IProgramActivationWindowReturns native) : IProgramActivationWindows
{
    public bool TryResolveTurnEndingOrder(PromptChoice selected) => native.TryResolveTurnEndingOrder(selected);
    public bool TryResolveGroup(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action) =>
        native.TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);

    public (ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context) PendingCandidate()
    {
        if (facts.TryGetMobileZhitiContestTrigger(out var zhitiCandidate, out var zhitiContext))
            return (zhitiCandidate, zhitiContext);
        if (facts.TryGetDrivenProgramTriggerCandidate(out var drivenCandidate, out var drivenContext))
            return (drivenCandidate, drivenContext);
        if (facts.TryGetPindianRevealTrigger(out var revealCandidate,out var revealContext)) return (revealCandidate,revealContext);
        if (frames.Top is FirstSlashDuelLegacyWindowFrame legacyBenefit)
            return (legacyBenefit.Candidates[legacyBenefit.CandidateIndex], legacyBenefit.Contexts[legacyBenefit.CandidateIndex]);
        if (frames.Top is ForeignActualTurnStartWindowFrame foreign)
            return (foreign.Candidates[foreign.CandidateIndex], foreign.Contexts[foreign.CandidateIndex]);
        if (frames.Top is ActualUseTargetWindowFrame actualTargets)
            return (actualTargets.Candidates[actualTargets.CandidateIndex], actualTargets.Contexts[actualTargets.CandidateIndex]);
        if (frames.Top is CardEffectBeforeApplyFrame effect)
        {
            var candidate = effect.Candidates[effect.CandidateIndex];
            return (RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(candidate),candidate.FrozenContext!);
        }
        if (frames.Top is DeferredTurnEndFrame { AfterTurnEnded: not null } afterTurnEnded)
        {
            var candidate = facts.AfterTurnEndedCandidate(afterTurnEnded);
            if (afterTurnEnded.AfterTurnEnded.CurrentChild is not null)
                throw new InvalidOperationException("A pending after-turn-ended prompt cannot belong to a live child.");
            return (candidate, facts.CreateAfterTurnEndedContext(afterTurnEnded, candidate));
        }
        if (frames.Top is TurnEndingBoundaryFrame turnEnding)
        {
            var candidate = turnEnding.Items[turnEnding.ItemIndex].Candidate ??
                throw new InvalidOperationException("The turn-ending program item lost its candidate.");
            return (candidate, facts.CreateTurnEndingProgramContext(turnEnding, candidate,
                turnEnding.Items[turnEnding.ItemIndex].Facts));
        }
        if (frames.Top is ProgramDeathTriggerWindowFrame deathWindow)
        {
            var candidate = deathWindow.Candidates[deathWindow.CandidateIndex];
            return (candidate, facts.CreateOwnerDiedProgramContext(deathWindow, candidate));
        }
        if (frames.Top is ProgramKillTriggerWindowFrame killWindow)
            return (killWindow.Candidates[killWindow.CandidateIndex],
                killWindow.Contexts[killWindow.CandidateIndex]);
        if (frames.Top is BeforeDamageProgramWindowFrame beforeDamage)
        {
            var item = beforeDamage.Candidates[beforeDamage.CandidateIndex];
            return (item.Candidate, facts.CreateBeforeDamageProgramContext(beforeDamage, item));
        }
        if (facts.ActiveDamageTrigger is { } damage &&
            damage.Candidates[damage.CandidateIndex].ToProgramCandidate() is { } damageCandidate)
            return (damageCandidate, facts.CreateAfterDamageProgramContext(damage, damageCandidate));
        throw new InvalidOperationException("The program prompt has no parent trigger candidate.");
    }

    public void CompleteSkipped(ProgramTriggerCandidate candidate)
    {
        if (native.TrySkipMobileZhitiContestTrigger(candidate)) return;
        if (native.TrySkipDrivenProgramCandidate(candidate)) return;
        if (frames.Top is PindianRevealWindowFrame reveal && reveal.Candidates[reveal.CandidateIndex] == candidate)
        { native.CompletePindianRevealBinding(candidate,facts.PindianRevealContext(reveal,candidate)); return; }
        if (frames.Top is FirstSlashDuelLegacyWindowFrame legacyBenefit &&
            legacyBenefit.Candidates[legacyBenefit.CandidateIndex] == candidate)
        {
            native.CompleteFirstLegacyBenefitCandidate(candidate, legacyBenefit.Contexts[legacyBenefit.CandidateIndex]); return;
        }
        if (frames.Top is ForeignActualTurnStartWindowFrame foreign &&
            foreign.Candidates[foreign.CandidateIndex] == candidate)
        {
            native.AdvanceForeignActualTurnStartCandidate(foreign, false, false);
            frames.AdvanceTop<ForeignActualTurnStartWindowFrame>(); return;
        }
        if (frames.Top is ActualUseTargetWindowFrame actualTargets &&
            actualTargets.Candidates[actualTargets.CandidateIndex] == candidate)
        {
            native.AdvanceActualUseTargetCandidate(actualTargets, false, false);
            frames.AdvanceTop<ActualUseTargetWindowFrame>();
            return;
        }
        if (frames.Top is CardEffectBeforeApplyFrame effect)
        {
            if (RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(effect.Candidates[effect.CandidateIndex]) != candidate) throw new InvalidOperationException("Skipped effect candidate changed.");
            native.CompleteCardEffectCandidate(effect.Candidates[effect.CandidateIndex].FrozenContext!); return;
        }
        if (frames.Top is DeferredTurnEndFrame { AfterTurnEnded: not null } afterTurnEnded)
        {
            native.AdvanceAfterTurnEndedCandidate(afterTurnEnded, candidate, activated: false, completed: false);
            native.ContinueDeferredTurnEnd();
            return;
        }
        if (frames.Top is ProgramKillTriggerWindowFrame killWindow &&
            killWindow.Candidates[killWindow.CandidateIndex] == candidate)
        {
            native.AdvanceKillDiedProgramCandidate(killWindow, candidate, activated: false, completed: false);
            frames.AdvanceTop<ProgramKillTriggerWindowFrame>();
            return;
        }
        if (frames.Top is TurnEndingBoundaryFrame turnEnding &&
            turnEnding.Items[turnEnding.ItemIndex].Candidate == candidate)
        {
            native.AdvanceTurnEndingBoundaryCandidate(
                turnEnding, candidate, activated: false, completed: false);
            frames.AdvanceTop<TurnEndingBoundaryFrame>();
            return;
        }
        if (frames.Top is ProgramDeathTriggerWindowFrame deathWindow &&
            deathWindow.Candidates[deathWindow.CandidateIndex] == candidate)
        {
            native.AdvanceOwnerDiedProgramCandidate(deathWindow, candidate, activated: false, completed: false);
            frames.AdvanceTop<ProgramDeathTriggerWindowFrame>();
            return;
        }
        if (frames.Top is BeforeDamageProgramWindowFrame beforeDamage &&
            beforeDamage.Candidates[beforeDamage.CandidateIndex].Candidate == candidate)
        {
            native.AdvanceBeforeDamageProgramCandidate(beforeDamage, activated: false, completed: false);
            frames.AdvanceTop<BeforeDamageProgramWindowFrame>();
            return;
        }
        if (facts.ActiveDamageTrigger is { } damage &&
            damage.Candidates[damage.CandidateIndex].ToProgramCandidate() == candidate)
        {
            events.AdvanceFact(new ProgramBindingResolvedEvent(
                damage.Id, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId,
                candidate.OwnerSeat, damage.TriggerWindow,
                Activated: false, Completed: false));
            native.AdvanceDamageTriggerCandidate(damage);
            return;
        }
        throw new InvalidOperationException("The skipped program candidate lost its parent.");
    }
}
