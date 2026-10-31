namespace CardGame.Core;

internal readonly record struct DamageCursorProjection(
    ResolutionFrame? Top, bool HasPaidObserverSubtree, long? OwnedDyingId = null);

// This module projects the original contiguous frame chain and advances the
// owning damage window. Payment and recovery state remain on those frames.
[ProgramRuntimeModule]
internal sealed class DamageAdjacencyRuntime(
    IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IDamageAdjacencyFacts facts, IDamageAdjacencyProtocols protocols,
    IDamageWindowOperations native)
{
    internal ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false)
    {
        var index = facts.FrameCount - 1;
        while (index >= 1 && (protocols.RequestedDeckBasicFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) || protocols.DrawFundedDistinctBasicFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) || protocols.ChainedStateBasicFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) || DamageFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.ProvenanceClaimTurnedOverEdge(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               RuntimeSkills.RuntimeRules_Core_CharacterTurnedOverPrograms.CharacterTurnedOverFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.HulaoActivityFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.ShenwuTrickFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.HellSoulFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.WenheChaosFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               RuntimeSkills.RuntimeRules_Core_NativeDrawAttempts.NativeDrawAttemptRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.RecoveryReplacementFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.PileEquipmentFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.RandomEquipmentFrameRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1)) ||
               protocols.RecoveryReplacementDamageObserverRidesOn(index) ||
               protocols.CharacterTurnedOverDamageObserverRidesOn(index) ||
               includeNestedObservers && RuntimeSkills.RuntimeRules_Core_CommonRules.DamageObserverRidesOn(facts.FrameAt(index), facts.FrameAt(index - 1))))
            index--;
        return index >= 0 ? facts.FrameAt(index) : null;
    }

    internal ProgramSkillFrame? TypedDamageProgramObserver(long windowId)
    {
        var index = facts.FrameCount - 1;
        while (index > 0)
        {
            var child = facts.FrameAt(index);
            var parent = facts.FrameAt(index - 1);
            if (!TypedDamagePostEventEdge(parent, child)) break;
            index--;
        }
        if (index < 1 || facts.FrameAt(index) is not ProgramSkillFrame root ||
            facts.FrameAt(index - 1) is not DamageTriggerWindowFrame window || window.Id != windowId ||
            !DamageProgramMatchesWindow(root, window))
            return null;
        return root;
    }

    internal bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window)
    {
        var windowIndex = facts.FrameIndex(window.Id);
        return windowIndex > 0 && facts.FrameAt(windowIndex - 1) is DamageFrame damage &&
            damage.Id == window.ParentFrameId && damage.SourceSeat == window.SourceSeat && damage.TargetSeat == window.TargetSeat &&
            window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count &&
            TypedDamageObserverCandidateMatches(root, window.Candidates[window.CandidateIndex].ToProgramCandidate()) &&
            root.WindowContext is { } context && context.Window == window.TriggerWindow &&
            context.ParentFrameId == window.Id && context.DamageFrameId == damage.Id &&
            (context.SourceSeat is null || context.SourceSeat == damage.SourceSeat) &&
            context.TargetSeat == damage.TargetSeat && context.Amount == damage.Amount && context.Amount > 0;
    }

    internal bool TypedDamageObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) =>
        RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate) &&
        frame.WindowContext is { } context && context.OwnerSeat == frame.OwnerSeat &&
        facts.Program(frame.SkillId) is { } program && program.GameplayHash == frame.GameplayHash &&
        program.Triggers.Any(trigger => trigger.Id == frame.TriggerId && trigger.Window == context.Window);

    internal bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (child is HpChangedTriggerWindowFrame poisonHealth && protocols.IsWenhePoisonHealthChild(parent, poisonHealth)) return true;
        if (child is DyingFrame poisonDying && protocols.IsWenhePoisonDyingChild(parent, poisonDying)) return true;
        if (child is ProgramSkillFrame observer && observer.WindowContext is { } context && context.ParentFrameId == parent.Id)
        {
            if (parent is CardsMovedTriggerWindowFrame moved && moved.Contexts is { } contexts &&
                moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && contexts.Count == moved.Candidates.Count)
                return context == contexts[moved.CandidateIndex] && context.MovementBatch == moved.Batch &&
                    context.Window is SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained or SkillProgramTriggerWindow.DiscardPileReceived &&
                    TypedDamageObserverCandidateMatches(observer, moved.Candidates[moved.CandidateIndex]);
            if (parent is HpChangedTriggerWindowFrame hp && hp.CandidateIndex >= 0 && hp.CandidateIndex < hp.Candidates.Count &&
                hp.Contexts.Count == hp.Candidates.Count)
                return context == hp.Contexts[hp.CandidateIndex] && context.HpChange == hp.Change &&
                    context.Window is SkillProgramTriggerWindow.AfterHpLost or SkillProgramTriggerWindow.AfterHpRecovered or SkillProgramTriggerWindow.AfterHealthChanged &&
                    TypedDamageObserverCandidateMatches(observer, hp.Candidates[hp.CandidateIndex]);
            return false;
        }
        if (parent is not ProgramSkillFrame producer) return false;
        if (child is CardsMovedTriggerWindowFrame movement)
            return RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement) ||
                protocols.RandomEquipmentFrameRidesOn(movement, producer);
        return child is HpChangedTriggerWindowFrame changed && RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramHpReturnMatches(producer, changed);
    }

    internal bool HasMalformedDamagePostEventEdge(long windowId)
    {
        var windowIndex = facts.FrameIndex(windowId);
        if (windowIndex < 0) return true;
        for (var index = windowIndex + 1; index < facts.FrameCount; index++)
        {
            var parent = facts.FrameAt(index - 1);
            var child = facts.FrameAt(index);
            if (parent is ProgramSkillFrame producer)
            {
                if (child is CardsMovedTriggerWindowFrame movement)
                {
                    // A paid faction request may belong to a Program frame;
                    // its dedicated return replaces, rather than supplements,
                    // the ordinary instruction return. Other alternatives have
                    // different owning frame types and cannot ride this edge.
                    var paidFactionReturn = movement.ResumeFactionRequestCostFrameId == producer.Id &&
                        movement.ResumeProgramFrameId is null && producer.PaidFactionRequestCostRecovery is { } receipt &&
                        protocols.HasPaidFactionRequestCostRecovery(receipt);
                    if (!RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement, paidFactionReturn) &&
                        !protocols.RandomEquipmentFrameRidesOn(movement, producer))
                        return true;
                }
                if (child is HpChangedTriggerWindowFrame hp)
                {
                    var paidFactionReturn = hp.Continuation == PostEventContinuation.FactionRequestCost &&
                        producer.PaidFactionRequestCostRecovery is { } receipt && protocols.HasPaidFactionRequestCostRecovery(receipt);
                    if (!RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramHpReturnMatches(producer, hp, paidFactionReturn)) return true;
                }
            }
            if (child is not ProgramSkillFrame program) continue;
            // Receipt-specific compatibility must never re-admit a malformed
            // candidate edge rejected by the common typed projection.
            if (parent is DamageTriggerWindowFrame damage && !DamageProgramMatchesWindow(program, damage) ||
                parent is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame && !TypedDamagePostEventEdge(parent, child))
                return true;
        }
        return false;
    }

    internal bool DamageFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => ride switch
    {
        // Movement and HP-change windows legitimately ride on the in-flight
        // program frame their continuation suspended at; a verified ride does
        // not displace the underlying damage cursor.
        CardsMovedTriggerWindowFrame { ResumePaidCardUseFrameId: not null } movement =>
            beneath is CardUseFrame paidUse && protocols.IsPaidCardUseMovementReturn(paidUse, movement),
        CardsMovedTriggerWindowFrame movement =>
            beneath is RecoveryReplacementFrame
                ? movement.ResumeRecoveryReplacementFrameId == beneath.Id && movement.Batch.ParentFrameId == beneath.Id
                : movement.Batch.AwaitingProgramFrameId == beneath.Id ||
                  movement.Batch.AwaitingProgramFrameId is null && movement.Batch.ParentFrameId == beneath.Id,
        HpChangedTriggerWindowFrame changed when changed.Continuation == PostEventContinuation.WenyiDamageHealth =>
            beneath is DamageFrame damage && protocols.IsWenyiDamageHealthReturn(damage, changed),
        HpChangedTriggerWindowFrame changed when changed.Continuation == PostEventContinuation.HappyQiaoDamageHealth =>
            beneath is DamageFrame damage && protocols.IsHappyQiaoDamageHealthReturn(damage, changed),
        HpChangedTriggerWindowFrame changed when changed.Continuation == PostEventContinuation.AmountSupport =>
            protocols.IsAmountSupportHealthChild(beneath, changed),
        HpChangedTriggerWindowFrame changed => beneath is RecoveryReplacementFrame
            ? changed.Continuation == PostEventContinuation.RecoveryReplacement &&
              changed.ResumeFrameId == beneath.Id && changed.Change.ParentFrameId == beneath.Id
            : changed.ResumeFrameId == beneath.Id,
        _ => false
    };

    internal void AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame window)
    {
        if (facts.ActiveDamageTrigger is not { } current || current.Id != window.Id)
        {
            throw new InvalidOperationException("The completed damage trigger window is not current.");
        }
        window = current;

        var nextIndex = window.CandidateIndex + 1;
        // A nested program damage death can determine the winner before the final
        // trigger candidate. Close this window now so the host never stops at
        // Winner != None with live frames, Processing cards or a new prompt.
        if (facts.Winner != Winner.None) nextIndex = window.Candidates.Count;
        window = native.SetWindowCursor(window.Id, nextIndex);
        events.AdvanceFact(new DamageTriggerWindowAdvancedEvent(
            window.Id,
            window.CandidateIndex,
            window.CandidateIndex >= window.Candidates.Count));
        if (window.CandidateIndex >= window.Candidates.Count)
        {
            CompleteDamageTriggerWindow(window);
        }
    }

    internal void CompleteDamageTriggerWindow(DamageTriggerWindowFrame window)
    {
        if (facts.ActiveDamageTrigger is not { } current || current.Id != window.Id)
        {
            throw new InvalidOperationException("The damage trigger window is not ready to complete.");
        }
        window = current;

        var attack = native.CaptureAttack(window);
        native.SetWindowStep(window.Id, ResolutionFrameStep.Completed);
        var target = attack.BindTarget();
        frames.Pop(window.Id, ResolutionFrameKind.DamageTriggerWindow);
        if (window.TriggerWindow == SkillProgramTriggerWindow.DamageAppliedBeforeDying)
        {
            if (target.Hp <= 0 && target.IsAlive)
            {
                target.BeginDying(window.ParentFrameId);
                return;
            }
            if (target.TryBeginAfterDamage(window.ParentFrameId))
            {
                return;
            }
        }
        if (target.Hp <= 0 && target.IsAlive &&
            !target.HasResolvedDying(window.ParentFrameId))
        {
            target.BeginDying(window.ParentFrameId);
            return;
        }

        events.AdvanceFact(new AfterDamageEvent(
            window.ParentFrameId,
            target.SourceSeat,
            target.TargetSeat,
            target.DamageAmount,
            target.Hp,
            target.Nature) { SourceLess = target.IsSourceLess });

        frames.Pop(window.ParentFrameId, ResolutionFrameKind.Damage);
        target.CompleteAttack();
    }
}
