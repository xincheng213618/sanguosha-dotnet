#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrepPoisonOutcomesRuntime
{
    // Same-name poison is one preparation obligation, even when two legitimate
    // grant sources already existed before this capability was introduced.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1248, SkillProgramEffectOp.ResolvePrepPoisonOutcomeByRound)]
    internal bool IsFirstPrepPoisonCandidate(ProgramTriggerLegalityContext c)
    {
        if (!ResolvePrepPoisonOutcomeByRoundDescriptor.IsTrigger(c.Trigger))
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.Context.ParentFrameId);
        var candidates = parent?.Candidates ?? _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(c.Owner, c.Trigger.Window);
        var first = candidates.FirstOrDefault(candidate => candidate.SkillId == c.SkillId && ResolvePrepPoisonOutcomeByRoundDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)));
        return first is not null && first.SkillInstanceId == c.Candidate.SkillInstanceId && first.BindingId == c.Candidate.BindingId;
    }

    internal bool HasPrepPoisonParent(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        return index > 0 && f.TriggerId is not null && f.InstructionIndex == 3 && ResolvePrepPoisonOutcomeByRoundDescriptor.IsTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)) && f.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context && context.OwnerSeat == f.OwnerSeat && context.SourceSeat == f.OwnerSeat && context.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Id == context.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.Window == context.Window && parent.Continuation == ProgramLifecycleContinuation.NormalTurnStart && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    internal SkillProgramStepOutcome ResolvePrepPoisonOutcomeByRound(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PrepPoisonOutcome is not null || !HasPrepPoisonParent(f))
            throw new InvalidOperationException("Preparation poison requires its exact own preparation and terminal judgment instruction.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        var terminal = trigger.Effects[2];
        var outcomes = f.JudgmentOutcomeBindings?.Where(o => o.Name == terminal.SourceBind).ToArray();
        if (outcomes is not [var outcome] || outcome.InstructionIndex != 1 || outcome.TargetSeat != f.OwnerSeat || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, outcome))
            throw new InvalidOperationException("Preparation poison lost its unique finalized judgment value.");
        var cured = outcome.Suit == Suit.Diamond;
        // Undefined HP has no numeric loss value. Diamond cure needs no HP at all.
        if (!cured && !owner.HasHpValue)
            return SkillProgramStepOutcome.Continue;
        var revoked = cured ? _coreDamageTargetSkillRoundDrawOperations.OwnedAndRestorableRoundSkillGrants(owner, f.SkillId).Where(g => !g.IsModeRule).Select(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime.RoundSkillIdentity).Distinct().ToArray() : [];
        var receipt = new PrepPoisonOutcomeReceipt(new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.WindowContext!.ParentFrameId, outcome, Math.Max(0, _coreActualRoundProgressRoundNumberAccess._roundNumber), cured, cured ? 0 : Math.Max(0, _coreActualRoundProgressRoundNumberAccess._roundNumber), cured ? 0 : owner.Hp)
        {
            Stage = cured ? PrepPoisonOutcomeStage.SkillLossChildren : PrepPoisonOutcomeStage.HpLossChildren,
            RevokedGrants = revoked
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrepPoisonOutcome = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepPoisonOutcomeIssuedEvent(f.Id, receipt.Source, receipt.GameplayHash, receipt.PrepWindowId, outcome.JudgmentFrameId, outcome.Name, receipt.RoundNumber, receipt.Cured, receipt.RequestedHpLoss));
        if (cured)
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            for (var i = 0; i < revoked.Length; i++)
            {
                var identity = revoked[i];
                var live = owner.SkillGrants.Grants.SingleOrDefault(g => global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetSkillRoundDrawRuntime.SameRoundSkillIdentity(g, identity));
                if (live is not null && !owner.SkillGrants.RemoveGrant(live.GrantId))
                    throw new InvalidOperationException("Preparation poison cure changed its exact removed grant.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepPoisonGrantRevokedEvent(f.Id, i, f.OwnerSeat, identity));
            }
        }
        else if (receipt.RequestedHpLoss > 0)
        {
            var result = ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, receipt.RequestedHpLoss, (before, after, lost) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { PrepPoisonOutcome = active.PrepPoisonOutcome!with { HpLossApplied = true, HpAfter = after, ActualHpLoss = lost } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepPoisonHpLossPaidEvent(f.Id, f.OwnerSeat, receipt.RequestedHpLoss, lost, before, after));
            });
            if (result == SkillProgramStepOutcome.AwaitChild)
                return result;
        }

        ResumePrepPoisonOutcome(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, Action<int, int, int>? afterPayment = null)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException("amount");
        if (amount == 0)
            return SkillProgramStepOutcome.Continue;
        if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForProgramHpLoss(frameId, skillId, targetSeat, amount))
            return SkillProgramStepOutcome.AwaitChild;
        long? completedCardUseResolutionId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == frameId)?.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } context } && _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } action } use && action.ActionId == context.CardActionId ? use.Id : null;
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var before = target.Hp;
        var lost = amount;
        target.Hp = checked(target.Hp - amount);
        _coreHpChangeProgramsOperations.RecordHpChange(frameId, null, targetSeat, before, target.Hp, HpChangeKind.Loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(frameId, skillId, targetSeat, lost, target.Hp) { CompletedCardUseResolutionId = completedCardUseResolutionId });
        afterPayment?.Invoke(before, target.Hp, lost);
        if (target.Hp > 0)
            return SkillProgramStepOutcome.Continue;
        _coreSkillProgramsOperations.BeginProgramSkillDying(frameId, target);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1248)]
    internal bool ResumePrepPoisonOutcome(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PrepPoisonOutcome: { } r } f || f.Id != id)
            return false;
        RequirePrepPoisonOutcome(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepPoisonOutcomeCompletedEvent(id, r.Cured, r.ActualHpLoss));
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
        return true;
    }

    internal void RequirePrepPoisonOutcome(ProgramSkillFrame f)
    {
        if (f.PrepPoisonOutcome is not { } r || !HasPrepPoisonParent(f) || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.PrepWindowId != f.WindowContext!.ParentFrameId || r.Outcome.InstructionIndex != 1 || r.Outcome.TargetSeat != f.OwnerSeat || r.Outcome.Name != _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects[2].SourceBind || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, r.Outcome) || r.Cured != (r.Outcome.Suit == Suit.Diamond) || r.RoundNumber < 0 || r.RequestedHpLoss != (r.Cured ? 0 : r.RoundNumber) || r.Stage != (r.Cured ? PrepPoisonOutcomeStage.SkillLossChildren : PrepPoisonOutcomeStage.HpLossChildren) || r.RevokedGrants.Distinct().Count() != r.RevokedGrants.Count || r.RevokedGrants.Any(g => g.SkillId != f.SkillId) || _coreProgramEventHistoryOperations.ProgramEventHistory<PrepPoisonOutcomeIssuedEvent>().Count(e => e == new PrepPoisonOutcomeIssuedEvent(f.Id, r.Source, r.GameplayHash, r.PrepWindowId, r.Outcome.JudgmentFrameId, r.Outcome.Name, r.RoundNumber, r.Cured, r.RequestedHpLoss)) != 1)
            throw new InvalidOperationException("Preparation poison lost its frozen exact final judgment, source or payment.");
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<PrepPoisonGrantRevokedEvent>().Where(e => e.ProgramFrameId == f.Id).SequenceEqual(r.RevokedGrants.Select((g, i) => new PrepPoisonGrantRevokedEvent(f.Id, i, f.OwnerSeat, g))) || r.Cured && (r.HpLossApplied || r.ActualHpLoss != 0 || r.HpAfter != 0 || r.HpBefore != 0) || !r.Cured && r.RevokedGrants.Count != 0)
            throw new InvalidOperationException("Preparation poison cure lost its exact once-revoked grant identities.");
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<PrepPoisonHpLossPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (!r.HpLossApplied)
        {
            if (paid.Length != 0 || r.ActualHpLoss != 0 || r.HpAfter != 0)
                throw new InvalidOperationException("Unpaid poison cannot have an HP loss receipt.");
            return;
        }

        if (r.Cured || r.RequestedHpLoss <= 0 || r.ActualHpLoss != r.RequestedHpLoss || (long)r.HpAfter != (long)r.HpBefore - r.RequestedHpLoss || paid is not [var loss] || loss != new PrepPoisonHpLossPaidEvent(f.Id, f.OwnerSeat, r.RequestedHpLoss, r.ActualHpLoss, r.HpBefore, r.HpAfter) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == r.ActualHpLoss && e.RemainingHp == r.HpAfter) != 1)
            throw new InvalidOperationException("Preparation poison lost its single genuine native HP-loss payment.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 1248)]
    internal void AssertPrepPoisonOutcome(ProgramSkillFrame f)
    {
        if (f.PrepPoisonOutcome is not null)
            RequirePrepPoisonOutcome(f);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 1248)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 1248)]
    internal bool ContinuePaidPrepPoison(ProgramSkillFrame f)
    {
        if (f.PrepPoisonOutcome is null)
            return false;
        RequirePrepPoisonOutcome(f);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IPrepPoisonOutcomeHost.ResolvePrepPoisonOutcomeByRound(ProgramSkillFrame frame) => ResolvePrepPoisonOutcomeByRound(frame);
}
