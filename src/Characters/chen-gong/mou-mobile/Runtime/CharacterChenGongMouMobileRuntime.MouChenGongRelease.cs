#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenGongMouMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29601, SkillProgramEffectOp.ResolveMouChenGongMingceRelease)]
    internal bool CanOfferMouChenGongRelease(ProgramTriggerLegalityContext c) => c.SkillId == MouChenGongComposition.Mingce && c.Owner.IsAlive && c.Context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.Context.SourceSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && c.Owner.Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe) > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat);
    internal bool MouChenGongReleaseParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouChenGongComposition.Mingce || f.TriggerId != "release-plan" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } parent || parent.OwnerSeat != f.OwnerSeat || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id;
    }

    internal bool MouChenGongReleaseRoot(ProgramSkillFrame f) => f.MouChenGongRelease is { } r && MouChenGongReleaseParent(f) && r.Source == MouChenGongSource(f) && r.GameplayHash == f.GameplayHash && r.WindowId == f.WindowContext!.ParentFrameId && r.Phase.ActorSeat == f.OwnerSeat && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenGongReleaseStartedEvent>().Count(e => e == new MouChenGongReleaseStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.WindowId)) == 1;
    internal SkillProgramStepOutcome BeginMouChenGongRelease(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MouChenGongReleaseParent(f) || f.MouChenGongRelease is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe) <= 0)
            throw new InvalidOperationException("Mingce release belongs to its original actual own Play-start boundary with available Ce.");
        var r = new MouChenGongReleaseReceipt(MouChenGongSource(f), f.GameplayHash, phase, f.WindowContext!.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongRelease = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongReleaseStartedEvent(f.Id, r.Source, r.GameplayHash, phase, r.WindowId));
        PublishMouChenGong(f, f.OwnerSeat, MouChenGongReleaseChoices(f), "明策：选择一名其他角色，造成等同当前策数的伤害，然后移除所有策。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouChenGongReleaseChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => new PromptChoice(new($"mou-chen-gong.release.{(f.Id)}.{(p.Seat)}"), $"对{(p.Name)}造成策数点伤害", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mou-chen-gong-release", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    [ProgramAnswer("mou-chen-gong-release")]
    internal void AnswerMouChenGongRelease(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouChenGongRelease!;
        if (!MouChenGongReleaseRoot(f) || r.Stage != MouChenGongReleaseStage.TargetChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouChenGongReleaseChoices(f).Where(c => c.Id == selected.Id).ToArray()) || selected.Targets is not [var target] || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe)is not > 0)
            throw new InvalidOperationException("Mingce release requires its one living other and a fresh positive accepted Ce snapshot.");
        var x = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongRelease = r with { Stage = MouChenGongReleaseStage.Damage, TargetSeat = target, FrozenX = x } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongReleaseAcceptedEvent(f.Id, target, x));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongReleaseDamageIssuedEvent(f.Id, f.OwnerSeat, target, x));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, x);
    }

    internal void ContinueMouChenGongRelease(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouChenGongRelease: { } r } f || f.Id != id)
            return;
        if (r.Stage == MouChenGongReleaseStage.TargetChoice || f.AttackAttempt is not null)
            return;
        if (DrainMouChenGongChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouChenGongRelease!;
        if (r.Stage == MouChenGongReleaseStage.Damage && !r.Cleared)
        {
            var count = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongRelease = r with { Stage = MouChenGongReleaseStage.ClearChildren, Cleared = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            if (count > 0)
                ChangeMouChenGongCe(f, -count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongCeClearedEvent(f.Id, f.OwnerSeat, r.FrozenX!.Value, count));
            ContinueMouChenGongRelease(id);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongReleaseCompletedEvent(f.Id, r.TargetSeat, r.FrozenX, r.Cleared));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouChenGongRelease = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal void ChangeMouChenGongCe(ProgramSkillFrame f, int delta)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var marker = PlayerMarkerKind.MouChenGongCe;
        var before = owner.Markers.GetValueOrDefault(marker);
        var sources = owner.MarkerSourceCounts.Where(p => p.Key.Marker == marker).ToArray();
        if (delta > 0)
        {
            var key = (marker, f.OwnerSeat);
            owner.MarkerSourceCounts[key] = owner.MarkerSourceCounts.GetValueOrDefault(key) + delta;
            owner.Markers[marker] = before + delta;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, marker, delta, before + delta, f.OwnerSeat, "mou-mobile:mingce.plan"));
        }
        else
        {
            var current = before;
            foreach (var source in sources.OrderBy(p => p.Key.SkillOwnerSeat))
            {
                owner.MarkerSourceCounts.Remove(source.Key);
                current -= source.Value;
                if (current == 0)
                    owner.Markers.Remove(marker);
                else
                    owner.Markers[marker] = current;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, marker, -source.Value, current, source.Key.SkillOwnerSeat, "mou-mobile:mingce.plan"));
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenGongCeChangedEvent(f.Id, f.OwnerSeat, before, before + delta));
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouChenGongMingceRelease)]
    internal PromptChoice AiMouChenGongRelease(PendingDecision pending, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var amount = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MouChenGongCe);
        return pending.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets.Single(), new(0, 0, 0, 0, 0, 0, false, false) { TargetDamage = amount, TargetHealthEffects = [new(PublicHealthEffectKind.Damage, amount)] })).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
