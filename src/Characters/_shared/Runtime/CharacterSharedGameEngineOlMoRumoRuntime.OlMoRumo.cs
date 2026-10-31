#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSharedGameEngineOlMoRumoRuntime
{
    internal const string OlMoRumoSkill = "ol:mo-rumo";
    internal const string OlMoRumoBinding = "no-damage-round-ending";
    internal bool HasOlMoRumoCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveOlMoRumoRoundPenalty);

    internal static string OlMoRumoInstance(long entryId) => RuntimeSkills.RuntimeRules__shared_OlMoRumo.OlMoRumoInstance(entryId);
    internal void EnterOlMoRumo(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!HasOlMoRumoCapability || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !(_characterSunQuanOlMagicOperations.IsPaidQiangangRumoEntry(frame) || _characterLuBuOlMagicOperations.IsPaidMoKuangmoEntry(frame) || _characterDiaoChanOlMagicOperations.IsPaidMoDcQingshiEntry(frame) || _programPipelineHooksOperations._programPipelineHooks.RumoEntry(frame)))
            throw new InvalidOperationException("OL Rumo requires the exact paid Qiangang entry, not a generic operation or a live skill label.");
        if (_characterSharedGameEngineOlMoRumoProofsOperations.OlMoRumoEntry(frame.OwnerSeat)is not null)
            return;
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        if (!(_characterSunQuanOlMagicOperations.IsPaidQiangangRumoOrigin(frame.Id, source, frame.GameplayHash) || _characterLuBuOlMagicOperations.IsPaidMoKuangmoOrigin(frame.Id, source, frame.GameplayHash) || _characterDiaoChanOlMagicOperations.IsPaidMoDcQingshiOrigin(frame.Id, source, frame.GameplayHash) || _programPipelineHooksOperations._programPipelineHooks.RumoOrigin(frame.Id, source, frame.GameplayHash)))
            throw new InvalidOperationException("OL Rumo lost the original irreversible entry payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlMoRumoEnteredEvent(frame.Id, frame.OwnerSeat, source, frame.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat));
    }

    internal bool IsOlMoRumoEntered(int ownerSeat) => _characterSharedGameEngineOlMoRumoProofsOperations.OlMoRumoEntry(ownerSeat)is not null;
    internal PlayerMarkerSnapshot? OlMoRumoMarker(int ownerSeat) => IsOlMoRumoEntered(ownerSeat) ? new(PlayerMarkerKind.OlMoRumo, "入魔", 1) : null;
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendOlMoRumoMarker(int ownerSeat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary)
    {
        var result = OlMoRumoMarker(ownerSeat)is { } marker ? Array.AsReadOnly((ordinary ?? []).Where(item => item.Kind != PlayerMarkerKind.OlMoRumo).Append(marker).OrderBy(item => item.Kind).ToArray()) : ordinary;
        return _characterLuBuOlMagicOperations.AppendMoLuBuKuangMarker(ownerSeat, result);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 1)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeOlMoRumoRoundCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        // This is an issued derived state, never a fourth printed skill and
        // never a fresh grant inferred from the current general or its label.
        var remaining = ordinary.Where(candidate => candidate.SkillId != OlMoRumoSkill).ToArray();
        if (!HasOlMoRumoCapability || window != SkillProgramTriggerWindow.RoundEnding || !owner.IsAlive || _characterSharedGameEngineOlMoRumoProofsOperations.OlMoRumoEntry(owner.Seat)is not { } entry || _contentRegistryOperations._contentRegistry.GetSkill(OlMoRumoSkill).Program is not { } program)
            return remaining.Length == ordinary.Count ? ordinary : Array.AsReadOnly(remaining);
        var candidate = new ProgramTriggerCandidate(owner.Seat, OlMoRumoSkill, OlMoRumoBinding, OlMoRumoInstance(entry.EntryId), program.GameplayHash, 0);
        return Array.AsReadOnly(remaining.Append(candidate).ToArray());
    }

    [ProgramIssuedTrigger(140)]
    internal bool IsIssuedOlMoRumoCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => HasOlMoRumoCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _characterSharedGameEngineOlMoRumoProofsOperations.OlMoRumoCandidateSource(candidate) && _coreRoundEndingOperations.Value.IsExactRoundEndingCandidate(candidate, context) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlMoRumoRoundStartedEvent>().Any(fact => fact.OwnerSeat == candidate.OwnerSeat && fact.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    SkillProgramStepOutcome IOlMoRumoHost.Resolve(ProgramSkillFrame frame) => ResolveOlMoRumoRoundPenalty(frame);
    internal SkillProgramStepOutcome ResolveOlMoRumoRoundPenalty(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.OlMoRumo is not null)
        {
            ContinueOlMoRumo(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.InstructionIndex != 1 || frame.WindowContext is not { } context || !IsIssuedOlMoRumoCandidate(CharacterSharedGameEngineOlMoRumoProofsRuntime.OlMoRumoCandidate(frame), context) || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame boundary || _characterSharedGameEngineOlMoRumoProofsOperations.OlMoRumoEntry(frame.OwnerSeat)is not { } entry)
            throw new InvalidOperationException("OL Rumo lost its exact entered owner and old-round boundary.");
        // Read the entire current round at this effect's real start. Entry does
        // not erase earlier damage, and earlier round-end children can deal it.
        var hadDamage = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is RoundStartedEvent).OfType<DamageAppliedEvent>().Any(item => item.Amount > 0 && !item.SourceLess && item.SourceSeat == frame.OwnerSeat);
        var receipt = new OlMoRumoReceipt(new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, entry.EntryId, boundary.Id, boundary.RoundNumber, boundary.ActualTurnNumber, boundary.OwnerSeat, frame.InstructionIndex, hadDamage)
        {
            Stage = hadDamage ? OlMoRumoStage.LossChildren : OlMoRumoStage.LossIssuing
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlMoRumo = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlMoRumoRoundStartedEvent(frame.Id, entry.EntryId, boundary.Id, frame.OwnerSeat, receipt.RoundNumber, receipt.ActualTurn, receipt.TurnOwner, hadDamage));
        ContinueOlMoRumo(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainOlMoRumo(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id);
    internal void ContinueOlMoRumo(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlMoRumo: { } receipt } frame || frame.Id != frameId || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (receipt.Stage == OlMoRumoStage.LossIssuing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                CompleteOlMoRumo(frame);
                return;
            }

            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, frame.OwnerSeat, 1);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frameId)
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            if (frame.OlMoRumo!.Stage == OlMoRumoStage.LossIssuing)
                throw new InvalidOperationException("OL Rumo's native HP deduction lost its once-only return.");
        }

        if (DrainOlMoRumo(frame))
            return;
        CompleteOlMoRumo(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId));
    }

    internal void CompleteOlMoRumo(ProgramSkillFrame frame)
    {
        var receipt = frame.OlMoRumo!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlMoRumoRoundCompletedEvent(frame.Id, receipt.EntryId, receipt.BoundaryId, frame.OwnerSeat, receipt.RoundNumber, receipt.HadRoundDamage, receipt.ActualLoss, receipt.AppearancePrevented));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OlMoRumo = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28940)]
    internal bool ResumeOlMoRumo(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlMoRumo: not null } frame || frame.Id != frameId)
            return false;
        ContinueOlMoRumo(frameId);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28940)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28940)]
    internal bool ContinueIssuedOlMoRumo(ProgramSkillFrame frame)
    {
        if (frame.OlMoRumo is not null)
        {
            return true;
        }

        return frame.InstructionIndex is 0 or 1 && frame.WindowContext is { } context && IsIssuedOlMoRumoCandidate(CharacterSharedGameEngineOlMoRumoProofsRuntime.OlMoRumoCandidate(frame), context);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 28940)]
    internal void ObserveOlMoRumoLoss(IGameEvent payload)
    {
        if (payload is ProgramSkillHpLostEvent fact && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OlMoRumo: { Stage: OlMoRumoStage.LossIssuing, HadRoundDamage: false, LossReturned: false } receipt } frame && fact.FrameId == frame.Id && fact.SkillId == OlMoRumoSkill && fact.TargetSeat == frame.OwnerSeat && fact.Amount is >= 0 and <= 1 && fact.RemainingHp >= 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OlMoRumo = receipt with { Stage = OlMoRumoStage.LossChildren, LossReturned = true, ActualLoss = fact.Amount, HpAfter = fact.RemainingHp } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlMoRumoHpLossAppliedEvent(frame.Id, frame.OwnerSeat, 1, fact.Amount, fact.RemainingHp, false));
        }
        else if (payload is IdentityHpDeductionPreventedEvent prevented && _resolutionStackOperations._resolutionStack.LastOrDefault()is IdentityAppearanceFrame appearance && appearance.Id == prevented.AppearanceFrameId && appearance.Receipt is { Entry: IdentityAppearanceEntry.ProgramHpLoss, ParentFrameId: { } parent } native && _resolutionStackOperations._resolutionStack.SingleOrDefault(item => item.Id == parent)is ProgramSkillFrame { OlMoRumo: { Stage: OlMoRumoStage.LossIssuing, HadRoundDamage: false, LossReturned: false } receipt2 } frame2 && native.OwnerSeat == frame2.OwnerSeat && native.OriginalInstructionIndex == frame2.InstructionIndex && prevented.RequestedDeduction == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame2.Id, frame2 with { OlMoRumo = receipt2 with { Stage = OlMoRumoStage.LossChildren, LossReturned = true, AppearancePrevented = true, HpAfter = _coreMatchParticipantsPlayersAccess._players[frame2.OwnerSeat].Hp } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlMoRumoHpLossAppliedEvent(frame2.Id, frame2.OwnerSeat, 1, 0, _coreMatchParticipantsPlayersAccess._players[frame2.OwnerSeat].Hp, true));
        }
    }
}
