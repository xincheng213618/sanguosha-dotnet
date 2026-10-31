#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceFactionPreventionsRuntime
{
    internal static string SourceFactionPreventionUsage(string stateId, string faction) => RuntimeSkills.RuntimeRules_Core_SourceFactionPreventions.SourceFactionPreventionUsage(stateId, faction);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 60)]
    internal bool CanRunSourceFactionPrevention(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var effect = trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.PreventDamageAndConsumeSourceFaction);
        if (effect is null)
            return true;
        return context.Window == SkillProgramTriggerWindow.BeforeDamageApplied && context.TargetSeat == candidate.OwnerSeat && context.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && source != candidate.OwnerSeat && _corePublicFactionCountsOperations.GetPublicEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[source])is { } faction && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(candidate.OwnerSeat, candidate.SkillId, SourceFactionPreventionUsage(effect.StateId!, faction), SkillUsageScope.Game) == 0;
    }

    internal SkillProgramStepOutcome PreventDamageAndConsumeSourceFaction(ProgramSkillFrame frame, string stateId)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.SourceFactionPrevention is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target } context || target != frame.OwnerSeat || source == target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != context.ParentFrameId || parent.TargetSeat != target || parent.SourceSeat != source || parent.Prevented || parent.Amount <= 0 || context.Amount != parent.Amount || _coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(parent) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex].Candidate))
            throw new InvalidOperationException("Source-faction prevention requires its exact live foreign damage candidate.");
        if (_corePublicFactionCountsOperations.GetPublicEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[source])is not { } faction)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "伤害来源没有可冻结的有效势力。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var usage = SourceFactionPreventionUsage(stateId, faction);
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, usage, SkillUsageScope.Game, 1))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "该真实来源势力的整局防伤额度已消耗。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SourceFactionPrevention = new(frame.InstructionIndex - 1, parent.Id, parent.ParentFrameId, source, target, parent.Amount, stateId, faction, usage, before) });
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(frame.OwnerSeat, frame.SkillId, usage, SkillUsageScope.Game, 1));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSourceFactionPreventionIssuedEvent(frame.Id, parent.Id, source, target, parent.Amount, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, stateId, faction, usage));
        if (!_coreMatchParticipantsPlayersAccess._players[source].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "伤害仍由已阵亡角色造成；防伤和势力额度已发行，得牌尾部因接收者阵亡取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsValidSourceFactionPrevention(ProgramSkillFrame root, BeforeDamageProgramWindowFrame parent)
    {
        if (root.SourceFactionPrevention is not { } receipt || root.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || context.ParentFrameId != parent.Id || context.SourceSeat != receipt.SourceSeat || context.TargetSeat != root.OwnerSeat || context.Amount != parent.Amount || receipt.BeforeDamageFrameId != parent.Id || receipt.DamageOwnerFrameId != parent.ParentFrameId || receipt.SourceSeat != parent.SourceSeat || receipt.TargetSeat != root.OwnerSeat || parent.TargetSeat != root.OwnerSeat || receipt.SourceSeat == root.OwnerSeat || receipt.PreventedAmount != parent.Amount || !parent.Prevented || parent.Amount <= 0 || receipt.MovementSequenceBefore < 0 || string.IsNullOrWhiteSpace(receipt.SourceFactionId) || receipt.InstructionIndex != 0 || root.InstructionIndex is < 1 or > 2 || receipt.UsageId != SourceFactionPreventionUsage(receipt.StateId, receipt.SourceFactionId) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex].Candidate) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(root.OwnerSeat, root.SkillId, receipt.UsageId, SkillUsageScope.Game) != 1)
            return false;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program;
        if (program is null || program.GameplayHash != root.GameplayHash)
            return false;
        var effects = ProgramInstructionResolver.Default.Resolve(root, program).Instructions;
        if (effects.Count != 2 || effects[0].Op != SkillProgramEffectOp.PreventDamageAndConsumeSourceFaction || effects[0].StateId != receipt.StateId || !IsSourceFactionYieldTransfer(effects[1]))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSourceFactionPreventionIssuedEvent>().Any(e => e.FrameId == root.Id && e.BeforeDamageFrameId == parent.Id && e.SourceSeat == receipt.SourceSeat && e.TargetSeat == receipt.TargetSeat && e.PreventedAmount == receipt.PreventedAmount && e.StateId == receipt.StateId && e.SourceFactionId == receipt.SourceFactionId && e.UsageId == receipt.UsageId && e.GameplayHash == root.GameplayHash && e.Source == new CardConversionSource(root.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(root), root.OwnerSeat, root.SkillInstanceId));
    }

    internal static bool IsSourceFactionYieldTransfer(SkillProgramEffect effect) => RuntimeSkills.RuntimeRules_Core_SourceFactionPreventions.IsSourceFactionYieldTransfer(effect);
    internal ProgramSkillFrame? SourceFactionYieldObserverRoot(long beforeDamageId)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame parent || parent.Id != beforeDamageId || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !IsValidSourceFactionPrevention(root, parent) || root.SourceFactionPrevention is not { } receipt || root.InstructionIndex != 2 || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat)
                continue;
            var transfer = ProgramInstructionResolver.Default.Resolve(root, _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program!).Instructions[1];
            var binding = root.CardSetBindings.SingleOrDefault(b => b.Name == transfer.ResultBind);
            if (binding is not { CardIds.Count: 1, SourceLocations.Count: 1 } || binding.SourceLocations[0] != CardLocation.Hand(receipt.SourceSeat))
                continue;
            var cardId = binding.CardIds[0];
            var reason = $"skill-program.{(root.SkillId)}.{(SkillProgramEffectOp.SelectAndMoveOwnedCard)}";
            var paid = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.MovementSequenceBefore && m.CardId == cardId && m.Reason.Value == reason).ToArray();
            if (!paid.Any(m => m.From.OwnerSeat == root.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && m.To == CardLocation.Processing) || !paid.Any(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(receipt.SourceSeat)))
                continue;
            var first = _resolutionStackOperations._resolutionStack[index + 1];
            if (first is HpChangedTriggerWindowFrame hp)
            {
                if (hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.ParentFrameId != root.Id)
                    continue;
            }
            else if (first is RecoveryReplacementFrame recovery)
            {
                if (!_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != root.OwnerSeat || recovery.Attempt.TargetSeat != root.OwnerSeat || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != reason || !paid.Any(m => m.From == CardLocation.Equipment(root.OwnerSeat) && m.To == CardLocation.Processing) || !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Any(c => c.Id == cardId && c.Kind == CardKind.SilverLion))
                    continue;
            }
            else if (first is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0 || moved.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.Sequence <= receipt.MovementSequenceBefore || m.CardId != cardId || m.Reason.Value != reason))
                    continue;
            }
            else
                continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying))
                    break;
            }

            if (aligned)
                return root;
        }

        return null;
    }
    internal bool HasSourceFactionYieldDying(long beforeDamageId) => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { ResumesProgramSkill: true } dying && SourceFactionYieldObserverRoot(beforeDamageId)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 430)]
    internal bool IsSourceFactionYieldProgramDying() => _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(f => HasSourceFactionYieldDying(f.Id));

    SkillProgramStepOutcome ISourceFactionPreventionProgramHost.PreventDamageAndConsumeSourceFaction(ProgramSkillFrame f, string stateId) => PreventDamageAndConsumeSourceFaction(f, stateId);
}
