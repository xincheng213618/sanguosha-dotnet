#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    internal const string HappyQiaoHealthAction = "happy-qiao-health";
    internal void BindHappyQiaoHealth(CharacterState player) => player.BindHappyQiaoHealth(p => CurrentHappyQiaoPool(p.Seat), p => CurrentHappyQiaoPool(p.Seat, healthWrite: true));
    internal bool HasHappyQiaoHealth(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].HappyQiaoHealth is not null;
    internal bool HappyQiaoAnyWounded(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].HappyQiaoHealth?.AnyWounded ?? _coreMatchParticipantsPlayersAccess._players[seat].Hp < _coreMatchParticipantsPlayersAccess._players[seat].MaxHp;
    internal HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = null, bool healthWrite = false)
    {
        if (!HasHappyQiaoHealth(seat))
            return null;
        var top = parentFrameId is { } id ? _resolutionStackOperations._resolutionStack.FindLastIndex(f => f.Id == id) : _resolutionStackOperations._resolutionStack.Count - 1;
        if (top < 0)
            return null;
        for (var i = top; i >= 0; i--)
        {
            var frame = _resolutionStackOperations._resolutionStack[i];
            if (frame is RecoveryFrame { HappyQiaoPool: { } recoveryPool } recovery && recovery.TargetSeat == seat)
                return recoveryPool;
            if (!healthWrite && frame is HpChangedTriggerWindowFrame { Change.HappyQiaoPool: { } changedPool } hp && hp.Change.TargetSeat == seat)
                return changedPool;
            if (frame is DyingFrame { HappyQiaoPool: { } dyingPool } dying && dying.VictimSeat == seat)
                return dyingPool;
            if (frame.HappyQiaoHealth is { Pool: { } selected, Released: false } r && r.TargetSeat == seat && r.ParentFrameId == frame.Id && r.ParentKind == frame.Kind && r.ReturnKind != HappyQiaoHealthReturnKind.Query && (!healthWrite || !r.Applied) && (frame is not ProgramSkillFrame program || program.InstructionIndex == r.InstructionIndex))
                return selected;
            if (healthWrite)
                continue;
            if (frame is ProgramSkillFrame instructionFrame && frame.HappyQiaoQueries?.SingleOrDefault(q => q.ParentFrameId == frame.Id && q.ParentKind == frame.Kind && q.TargetSeat == seat && q.InstructionIndex == instructionFrame.InstructionIndex && q.QueryKey == $"query:{(frame.Id)}:{(instructionFrame.InstructionIndex)}:instruction-hp:{(seat)}")is { Pool: { } instructionPool })
                return instructionPool;
            if (frame.HappyQiaoActiveQueryKey is { } key && frame.HappyQiaoQueries?.SingleOrDefault(q => q.QueryKey == key && q.TargetSeat == seat && (frame is not ProgramSkillFrame queryProgram || q.InstructionIndex == queryProgram.InstructionIndex))is { Pool: { } queryPool })
                return queryPool;
        }

        return null;
    }

    internal bool TryBeginHappyQiaoHealthChoice(long parentFrameId, int targetSeat, HpChangeKind kind, int amount, HappyQiaoHealthReturnKind returnKind, string occurrenceKey, int? sourceSeat = null, long? attackResolutionId = null)
    {
        if (!HasHappyQiaoHealth(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != parentFrameId || amount < 0 || string.IsNullOrEmpty(occurrenceKey))
            throw new InvalidOperationException("A Qiao pool choice requires its actual unmodified native producer.");
        var instruction = parent is ProgramSkillFrame f ? f.InstructionIndex : 0;
        if (parent.HappyQiaoHealth is { } prior && prior.OccurrenceKey == occurrenceKey)
        {
            if (prior.TargetSeat != targetSeat || prior.Kind != kind || prior.Amount != amount || prior.SourceSeat != sourceSeat || prior.ReturnKind != returnKind || prior.AttackResolutionId != attackResolutionId || prior.InstructionIndex != instruction || prior.ParentFrameId != parent.Id || prior.ParentKind != parent.Kind || prior.Pool is null || prior.Applied)
                throw new InvalidOperationException("A Qiao pool selection cannot be substituted or consumed twice.");
            RequireHappyQiaoSelectedReceipt(parent, prior);
            return false;
        }

        if (parent.HappyQiaoHealth is { Applied: false, Released: false, ReturnKind: not HappyQiaoHealthReturnKind.Query })
            throw new InvalidOperationException("The original frame still owns an unpaid Qiao health selection.");
        var r = new HappyQiaoHealthReceipt(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, parent.Kind, targetSeat, sourceSeat, kind, amount, returnKind, occurrenceKey, instruction, _coreMatchParticipantsPlayersAccess._players[targetSeat].HappyQiaoHealth!, attackResolutionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { HappyQiaoHealth = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthRequestedEvent(r.Id, parent.Id, targetSeat, kind, amount, returnKind, occurrenceKey));
        var child = new HappyQiaoHealthChoiceFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, r.Id, targetSeat);
        _coreRuntimeOperations.PushRuntimeFrame(child);
        _coreRuntimeOperations.AdvanceRuntimeFrame(child.Id);
        return true;
    }

    internal bool TryBeginHappyQiaoDamageChoice(GameEngine.IDamageAttempt attack)
    {
        if (!HasHappyQiaoHealth(attack.TargetSeat) || attack.DamageWasApplied || attack.DamageAmount <= 0)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() ?? throw new InvalidOperationException("Qiao damage has no native host.");
        return TryBeginHappyQiaoHealthChoice(parent.Id, attack.TargetSeat, HpChangeKind.Damage, attack.DamageAmount, HappyQiaoHealthReturnKind.Damage, $"damage:{(attack.ResolutionId)}:{(attack.TargetSeat)}:{(_characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(attack.ResolutionId))}", attack.IsSourceLess ? null : attack.SourceSeat, attack.ResolutionId);
    }

    internal bool TryBeginHappyQiaoRecoveryRequestChoice(RecoveryReplacementFrame frame)
    {
        if (frame.Attempt.HappyQiaoRecoverToOrigin is not null)
            return TryBindHappyQiaoRecoverToRequest(frame);
        if (frame.Attempt.HappyQiaoBefore is null || frame.HappyQiaoHealth is not null)
            return false;
        return TryBeginHappyQiaoHealthChoice(frame.Id, frame.Attempt.TargetSeat, HpChangeKind.Recovery, frame.Attempt.Amount, HappyQiaoHealthReturnKind.RecoveryRequest, $"recovery-request:{(frame.Id)}:{(frame.Attempt.TargetSeat)}", frame.Attempt.SourceSeat);
    }

    internal int HappyQiaoRecoveryHpBefore(RecoveryReplacementFrame frame) => HappyQiaoRecoveryProof.HpBefore(frame);
    internal bool TryBeginHappyQiaoRecoveryChoice(RecoveryReplacementFrame frame, PreparedRecoveryApplication application)
    {
        if (!HasHappyQiaoHealth(application.TargetSeat))
        {
            ReleaseHappyQiaoHealthSelection(frame.Id);
            return false;
        }

        if (frame.HappyQiaoHealth is { Pool: not null, Released: false } r && r.TargetSeat == application.TargetSeat)
        {
            RequireHappyQiaoSelectedReceipt(frame, r);
            return false;
        }

        if (frame.HappyQiaoHealth is { Released: false })
            ReleaseHappyQiaoHealthSelection(frame.Id);
        return TryBeginHappyQiaoHealthChoice(frame.Id, application.TargetSeat, HpChangeKind.Recovery, application.RequestedAmount, HappyQiaoHealthReturnKind.Recovery, $"recovery:{(frame.Id)}:{(application.TargetSeat)}", application.SourceSeat);
    }

    internal bool TryBeginHappyQiaoProgramHealthChoice(ProgramSkillFrame frame, int targetSeat, HpChangeKind kind, int amount) => TryBeginHappyQiaoHealthChoice(frame.Id, targetSeat, kind, amount, HappyQiaoHealthReturnKind.Program, $"program:{(frame.Id)}:{(frame.InstructionIndex)}:{(targetSeat)}:{(kind)}", frame.OwnerSeat);
    internal bool TryBeginHappyQiaoQuery(ResolutionFrame supplied, int subjectSeat, string queryKey)
    {
        if (!HasHappyQiaoHealth(subjectSeat))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { } frame || frame.Id != supplied.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || string.IsNullOrWhiteSpace(queryKey) || frame is not ProgramSkillFrame program)
            throw new InvalidOperationException("A real HP query needs its exact registered Program parent; native query returns require their own typed entry.");
        var key = $"query:{(frame.Id)}:{(program.InstructionIndex)}:{(queryKey)}:{(subjectSeat)}";
        var prior = frame.HappyQiaoQueries?.SingleOrDefault(q => q.QueryKey == key);
        if (prior is not null)
        {
            RequireHappyQiaoQuery(frame, prior, selected: true);
            if (frame.HappyQiaoActiveQueryKey != key)
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyQiaoActiveQueryKey = key });
            return false;
        }

        var receipt = new HappyQiaoQueryReceipt(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, frame.Id, frame.Kind, subjectSeat, key, program.InstructionIndex, _coreMatchParticipantsPlayersAccess._players[subjectSeat].HappyQiaoHealth!);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyQiaoQueries = (frame.HappyQiaoQueries ?? []).Append(receipt).ToArray(), HappyQiaoActiveQueryKey = key });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthRequestedEvent(receipt.Id, frame.Id, subjectSeat, HpChangeKind.Setting, 0, HappyQiaoHealthReturnKind.Query, key));
        var child = new HappyQiaoHealthChoiceFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, frame.Id, receipt.Id, subjectSeat, IsQuery: true);
        _coreRuntimeOperations.PushRuntimeFrame(child);
        _coreRuntimeOperations.AdvanceRuntimeFrame(child.Id);
        return true;
    }

    internal int HappyQiaoQueryValue(ResolutionFrame supplied, int subjectSeat, string queryKey)
    {
        var frame = _resolutionStackOperations._resolutionStack.Single(f => f.Id == supplied.Id);
        if (!HasHappyQiaoHealth(subjectSeat))
            return _coreMatchParticipantsPlayersAccess._players[subjectSeat].Hp;
        var instruction = frame is ProgramSkillFrame program ? program.InstructionIndex : 0;
        var key = $"query:{(frame.Id)}:{(instruction)}:{(queryKey)}:{(subjectSeat)}";
        var receipt = frame.HappyQiaoQueries?.SingleOrDefault(q => q.QueryKey == key) ?? throw new InvalidOperationException("The actual HP query has not selected a sister.");
        RequireHappyQiaoQuery(frame, receipt, selected: true);
        return receipt.Before.Hp(receipt.Pool!.Value);
    }

    internal IReadOnlyList<PromptChoice> HappyQiaoHealthChoices(HappyQiaoHealthChoiceFrame f)
    {
        var parent = _resolutionStackOperations._resolutionStack.Single(p => p.Id == f.ParentFrameId);
        var before = f.IsQuery ? parent.HappyQiaoQueries!.Single(q => q.Id == f.RequestId).Before : parent.HappyQiaoHealth!.Before;
        return Array.AsReadOnly(Enum.GetValues<HappyQiaoPool>().Select(pool => new PromptChoice(new($"happy-qiao-health.{(f.Id)}.{(pool)}"), $"{((pool == HappyQiaoPool.DaQiao ? "大乔" : "小乔"))}：{(before.Hp(pool))}/{(before.MaximumHp)}", [], [f.OwnerSeat], new Dictionary<string, string> { ["program-action"] = HappyQiaoHealthAction, ["pool"] = pool.ToString() })).ToArray());
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 42400, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(HappyQiaoHealthChoiceFrame), 42400)]
    internal void ContinueHappyQiaoHealthChoice(HappyQiaoHealthChoiceFrame f)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        RequireHappyQiaoHealthChoice(f);
        var choices = HappyQiaoHealthChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman)
        {
            var parent = _resolutionStackOperations._resolutionStack[^2];
            var r = parent.HappyQiaoHealth;
            var before = f.IsQuery ? parent.HappyQiaoQueries!.Single(q => q.Id == f.RequestId).Before : r!.Before;
            var pool = !f.IsQuery && r!.Kind == HpChangeKind.Recovery ? (before.DaQiaoHp < before.MaximumHp ? HappyQiaoPool.DaQiao : HappyQiaoPool.XiaoQiao) : before.DaQiaoHp >= before.XiaoQiaoHp ? HappyQiaoPool.DaQiao : HappyQiaoPool.XiaoQiao;
            AnswerHappyQiaoHealth(choices.Single(c => c.Parameters["pool"] == pool.ToString()));
            return;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "双姝：选择此次结算的大乔或小乔", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.AwaitingHumanResponse;
    }

    [ProgramAnswer(HappyQiaoHealthAction)]
    internal void AnswerHappyQiaoHealth(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not HappyQiaoHealthChoiceFrame f || !HappyQiaoHealthChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Qiao health rejects a stale or substituted pool choice.");
        RequireHappyQiaoHealthChoice(f);
        var parent = _resolutionStackOperations._resolutionStack[^2];
        var pool = Enum.Parse<HappyQiaoPool>(choice.Parameters["pool"]);
        if (f.IsQuery)
        {
            var query = parent.HappyQiaoQueries!.Single(q => q.Id == f.RequestId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HappyQiaoQueries = parent.HappyQiaoQueries!.Select(q => q.Id == query.Id ? q with { Pool = pool } : q).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthPoolSelectedEvent(query.Id, parent.Id, f.OwnerSeat, pool, query.Before.Hp(pool), query.Before.MaximumHp));
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.PopResolutionFrame(f.Id, ResolutionFrameKind.HappyQiaoHealthChoice);
            if (parent is not ProgramSkillFrame)
                throw new InvalidOperationException("Qiao query has no registered native typed return.");
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id)with { ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        var r = parent.HappyQiaoHealth!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HappyQiaoHealth = r with { Pool = pool } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthPoolSelectedEvent(r.Id, parent.Id, f.OwnerSeat, pool, r.Before.Hp(pool), r.Before.MaximumHp));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PopResolutionFrame(f.Id, ResolutionFrameKind.HappyQiaoHealthChoice);
        if (r.ReturnKind == HappyQiaoHealthReturnKind.Damage)
        {
            var attack = _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(r.AttackResolutionId!.Value) ?? throw new InvalidOperationException("Qiao damage lost its native attack.");
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
            return;
        }

        if (r.ReturnKind == HappyQiaoHealthReturnKind.Recovery)
        {
            var recovery = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack[^1];
            _coreRecoveryReplacementOperations.ApplyRecoveryReplacement(recovery, recovery.PendingApplication!.CandidateIndex);
            return;
        }

        if (r.ReturnKind == HappyQiaoHealthReturnKind.RecoveryRequest)
        {
            _coreRuntimeOperations.AdvanceRuntimeFrame(parent.Id);
            return;
        }

        if (parent is ProgramSkillFrame program)
        {
            if (r.ReturnKind is HappyQiaoHealthReturnKind.Program or HappyQiaoHealthReturnKind.RecoverTo)
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(parent.Id)with { ReexecuteParticipantInstruction = true });
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
            return;
        }

        throw new InvalidOperationException("Qiao health query lost its registered typed return.");
    }

    internal void RequireHappyQiaoSelectedReceipt(ResolutionFrame parent, HappyQiaoHealthReceipt r)
    {
        if (r.ParentFrameId != parent.Id || r.ParentKind != parent.Kind || r.Pool is not { } pool || !HasHappyQiaoHealth(r.TargetSeat) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyQiaoHealthRequestedEvent>().Count(e => e == new HappyQiaoHealthRequestedEvent(r.Id, parent.Id, r.TargetSeat, r.Kind, r.Amount, r.ReturnKind, r.OccurrenceKey)) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyQiaoHealthPoolSelectedEvent>().Count(e => e == new HappyQiaoHealthPoolSelectedEvent(r.Id, parent.Id, r.TargetSeat, pool, r.Before.Hp(pool), r.Before.MaximumHp)) != 1 || !r.Applied && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].HappyQiaoHealth != r.Before)
            throw new InvalidOperationException("Qiao health lost its original selected pool, source boundary or unpaid state.");
    }

    internal void RequireHappyQiaoHealthChoice(HappyQiaoHealthChoiceFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (f.IsQuery)
        {
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not { } queryParent || queryParent.Id != f.ParentFrameId || queryParent.HappyQiaoQueries?.SingleOrDefault(q => q.Id == f.RequestId && q.TargetSeat == f.OwnerSeat)is not { Pool: null } query)
                throw new InvalidOperationException("Qiao query choice lost its exact original query receipt.");
            RequireHappyQiaoQuery(queryParent, query, selected: false);
            return;
        }

        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not { HappyQiaoHealth: { Pool: null } r } parent || parent.Id != f.ParentFrameId || parent.Kind != r.ParentKind || r.Id != f.RequestId || r.TargetSeat != f.OwnerSeat || r.Applied || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HappyQiaoHealth != r.Before || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyQiaoHealthRequestedEvent>().Count(e => e == new HappyQiaoHealthRequestedEvent(r.Id, parent.Id, r.TargetSeat, r.Kind, r.Amount, r.ReturnKind, r.OccurrenceKey)) != 1)
            throw new InvalidOperationException("Qiao health choice lost its exact unmodified owning frame.");
    }

    internal bool HappyQiaoHealthChoiceRidesOn(ResolutionFrame child, ResolutionFrame parent)
    {
        if (child is not HappyQiaoHealthChoiceFrame f || f.ParentFrameId != parent.Id || (f.IsQuery ? parent.HappyQiaoQueries?.Count(q => q.Id == f.RequestId) != 1 : parent.HappyQiaoHealth?.Id != f.RequestId))
            return false;
        RequireHappyQiaoHealthChoice(f);
        return true;
    }

    internal void RequireHappyQiaoQuery(ResolutionFrame parent, HappyQiaoQueryReceipt query, bool selected)
    {
        if (query.ParentFrameId != parent.Id || query.ParentKind != parent.Kind || parent is not ProgramSkillFrame program || query.InstructionIndex != program.InstructionIndex || query.TargetSeat < 0 || !HasHappyQiaoHealth(query.TargetSeat) || string.IsNullOrWhiteSpace(query.QueryKey) || selected != query.Pool.HasValue || !selected && _coreMatchParticipantsPlayersAccess._players[query.TargetSeat].HappyQiaoHealth != query.Before || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyQiaoHealthRequestedEvent>().Count(e => e == new HappyQiaoHealthRequestedEvent(query.Id, parent.Id, query.TargetSeat, HpChangeKind.Setting, 0, HappyQiaoHealthReturnKind.Query, query.QueryKey)) != 1 || selected && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyQiaoHealthPoolSelectedEvent>().Count(e => e == new HappyQiaoHealthPoolSelectedEvent(query.Id, parent.Id, query.TargetSeat, query.Pool!.Value, query.Before.Hp(query.Pool.Value), query.Before.MaximumHp)) != 1)
            throw new InvalidOperationException("Qiao query lost its exact key, original instruction, frozen values or selected sister.");
    }
    internal bool HasHappyQiaoHealthChoiceInSubtree(long id) => _resolutionStackOperations._resolutionStack.LastOrDefault()is HappyQiaoHealthChoiceFrame f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id) <= _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.ParentFrameId) && _resolutionStackOperations._resolutionStack.Any(p => p.Id == id) && HappyQiaoHealthChoiceRidesOn(f, _resolutionStackOperations._resolutionStack[^2]);

    internal void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, HpChangeKind kind, int before, int after)
    {
        if (!HasHappyQiaoHealth(targetSeat))
            return;
        var parent = _resolutionStackOperations._resolutionStack.Single(f => f.Id == parentFrameId);
        if (parent.HappyQiaoHealth is not { Pool: { } pool, Applied: false } r || r.TargetSeat != targetSeat || r.Kind != kind || r.Before.Hp(pool) != before)
            throw new InvalidOperationException("Qiao HP payment lost its original pool request.");
        RequireHappyQiaoSelectedReceipt(parent, r with { Applied = true });
        if (_coreMatchParticipantsPlayersAccess._players[targetSeat].HappyQiaoHealth!.Hp(pool) != after)
            throw new InvalidOperationException("Qiao payment changed another pool.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HappyQiaoHealth = r with { Applied = true, HpAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthAppliedEvent(r.Id, parent.Id, targetSeat, pool, kind, before, after));
    }

    internal void ReleaseHappyQiaoHealthSelection(long parentFrameId)
    {
        var parent = _resolutionStackOperations._resolutionStack.Single(f => f.Id == parentFrameId);
        if (parent.HappyQiaoHealth is not { Applied: false, Released: false } r)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HappyQiaoHealth = r with { Released = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoHealthSelectionReleasedEvent(r.Id, parent.Id));
    }

    internal bool TryCompleteHappyQiaoWholeDeath(long deathFrameId, CharacterState victim)
    {
        if (victim.HappyQiaoHealth is not { } before)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeathFrame death || death.Id != deathFrameId || death.VictimSeat != victim.Seat || !victim.IsAlive)
            throw new InvalidOperationException("Whole Qiao death must be committed by the real victim's native death frame.");
        victim.CompleteHappyQiaoWholeDeath();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoWholeDeathEvent(deathFrameId, victim.Seat, before));
        return true;
    }

    internal bool TryApplyHappyQiaoMaximumHp(long parentFrameId, int targetSeat, int newMaximum, string sourceId)
    {
        if (!HasHappyQiaoHealth(targetSeat))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var before = owner.HappyQiaoHealth!;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parentFrameId || newMaximum < 0)
            throw new InvalidOperationException("Synchronized Qiao maximum HP requires its original native producer.");
        owner.MaxHp = newMaximum;
        var after = owner.HappyQiaoHealth!;
        if (before.MaximumHp == after.MaximumHp)
            return true;
        PublishHappyQiaoMaximumChange(parentFrameId, targetSeat, before, after, sourceId);
        return true;
    }

    internal void PublishHappyQiaoMaximumChange(long parentFrameId, int targetSeat, HappyQiaoDualHealth before, HappyQiaoDualHealth after, string sourceId)
    {
        if (after != before.WithMaximum(after.MaximumHp))
            throw new InvalidOperationException("A synchronized maximum change must preserve each sister's exact clamp.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoMaximumChangedEvent(parentFrameId, targetSeat, before, after, sourceId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(targetSeat, after.MaximumHp - before.MaximumHp, after.MaximumHp, sourceId));
        if (before.MaximumHp == after.MaximumHp)
            return;
        foreach (var pool in Enum.GetValues<HappyQiaoPool>())
        {
            var change = new HpChangeContext(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentFrameId, null, targetSeat, HpChangeKind.MaximumHp, Math.Abs(after.MaximumHp - before.MaximumHp), before.Hp(pool), after.Hp(pool))
            {
                HappyQiaoPool = pool
            };
            _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Add(change);
            _coreQimeiProgramsOperations.CaptureQimeiHpQualification(change);
            _characterSunJianMouOlOperations.CaptureMouSunJianOlHp(change);
            _programPipelineHooksOperations._programPipelineHooks.CaptureHealthChange(change);
        }
    }
}
