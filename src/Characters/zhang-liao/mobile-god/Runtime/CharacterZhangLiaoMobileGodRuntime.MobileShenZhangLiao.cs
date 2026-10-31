#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoMobileGodRuntime
{
    internal bool HasMobileShenZhangLiao => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RestoreMobileZhitiEquipmentSlot);

    internal static CardConversionSource MobileZhangLiaoSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool MobileDuoruiActive(MobileDuoruiIssuedEvent e) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDuoruiExpiredEvent>().Any(x => x.FrameId == e.FrameId);
    internal IEnumerable<MobileDuoruiIssuedEvent> MobileDuoruiLeases => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDuoruiIssuedEvent>().Where(MobileDuoruiActive);

    internal EquipmentSlot[] MobileDuoruiSlots(int seat) => new[]
    {
        EquipmentSlot.Weapon,
        EquipmentSlot.Armor,
        EquipmentSlot.OffensiveHorse,
        EquipmentSlot.DefensiveHorse
    }.Where(slot => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot) > 0).ToArray();
    internal EquipmentSlot[] MobileZhitiSlots(int seat) => Enum.GetValues<EquipmentSlot>().Where(slot => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot) == 0).ToArray();
    internal static string MobileEquipmentSlotName(EquipmentSlot slot) => slot switch
    {
        EquipmentSlot.Weapon => "武器栏",
        EquipmentSlot.Armor => "防具栏",
        EquipmentSlot.OffensiveHorse => "进攻坐骑栏",
        EquipmentSlot.DefensiveHorse => "防御坐骑栏",
        EquipmentSlot.Treasure => "宝物栏",
        _ => throw new InvalidOperationException("Unknown equipment slot.")};
    internal string[] MobileDuoruiSkills(int seat) => _contentRegistryOperations._contentRegistry.Generals[_coreMatchParticipantsPlayersAccess._players[seat].General.Id].SkillIds.Concat(_coreMatchParticipantsPlayersAccess._players[seat].SecondaryGeneral is { } second ? _contentRegistryOperations._contentRegistry.Generals[second.Id].SkillIds : []).Distinct(StringComparer.Ordinal).Where(id => (_contentRegistryOperations._contentRegistry.GetSkill(id).Tags & (SkillTag.Limited | SkillTag.Awakening | SkillTag.Lord)) == 0 && id is not ("mobile-god:powei" or "mou-mobile:jieyin")).Order(StringComparer.Ordinal).ToArray();
    internal bool MobileZhitiOpponent(int owner, int opponent) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(opponent) && owner != opponent && _coreMatchParticipantsPlayersAccess._players[opponent].IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[opponent]) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner, opponent);
    internal bool IsMobileDuoruiGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (!HasMobileShenZhangLiao)
            return true;
        if (grant.SourceId.StartsWith("mobile-duorui:", StringComparison.Ordinal) && !MobileDuoruiLeases.Any(e => e.Source.OwnerSeat == owner.Seat && e.BorrowGrantId == grant.GrantId && e.BorrowInstanceId == grant.SkillInstanceId && e.SelectedSkillId == grant.SkillId))
            return false;
        return !MobileDuoruiLeases.Any(e => e.TargetSeat == owner.Seat && e.SelectedSkillId == grant.SkillId);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 39950)]
    internal void ExpireMobileDuorui(IGameEvent fact)
    {
        if (!HasMobileShenZhangLiao)
            return;
        var leases = MobileDuoruiLeases.Where(e => fact is TurnEndedEvent turn && turn.ActorSeat == e.TargetSeat && turn.TurnNumber > e.ActualTurn || fact is PlayerDiedEvent death && death.VictimSeat == e.TargetSeat).ToArray();
        foreach (var e in leases)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat];
            owner.SkillGrants.RemoveGrant(e.BorrowGrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuoruiExpiredEvent(e.FrameId, e.TargetSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, fact is PlayerDiedEvent));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        }
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39950, SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill, SkillProgramEffectOp.RestoreMobileZhitiEquipmentSlot)]
    internal bool MobileShenZhangLiaoEligible(ProgramTriggerLegalityContext g)
    {
        var op = _coreProgramLifecycleOperations.GetProgramTrigger2(g.Candidate).Effects[0].Op;
        if (op == SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill)
            return g.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0 } && source == g.OwnerSeat && source != target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play, ActorSeat: var actor } && actor == source && !MobileDuoruiLeases.Any(e => e.Source.OwnerSeat == source) && MobileDuoruiSlots(source).Length > 0 && MobileDuoruiSkills(target).Length > 0;
        if (g.WindowContext is not { } context || MobileZhitiSlots(g.OwnerSeat).Length == 0)
            return false;
        if (context.Window == SkillProgramTriggerWindow.AfterDamageApplied)
            return context is { TargetSeat: var target, SourceSeat: { } source, Amount: > 0 } && target == g.OwnerSeat && MobileZhitiOpponent(g.OwnerSeat, source);
        return MobileZhitiContestCandidateMatches(g.Candidate, context);
    }

    SkillProgramStepOutcome IMobileShenZhangLiaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMobileShenZhangLiao(f, op);
    internal SkillProgramStepOutcome BeginMobileShenZhangLiao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.MobileShenZhangLiao is not null)
            throw new InvalidOperationException("Mobile Shen Zhang Liao requires his exact original unpaid operation.");
        var target = op == SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill ? f.WindowContext!.TargetSeat!.Value : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenZhangLiao = new(MobileZhangLiaoSource(f), f.GameplayHash, op, f.WindowContext!.ParentFrameId, target, op == SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill ? MobileShenZhangLiaoStage.AbolishSlot : MobileShenZhangLiaoStage.RestoreSlot) });
        PublishMobileShenZhangLiao(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MobileShenZhangLiaoChoices(ProgramSkillFrame f)
    {
        if (f.MobileShenZhangLiao is not { } r)
            return[];
        var values = r.Stage == MobileShenZhangLiaoStage.ChooseSkill ? MobileDuoruiSkills(r.TargetSeat) : (r.Stage == MobileShenZhangLiaoStage.AbolishSlot ? MobileDuoruiSlots(f.OwnerSeat) : MobileZhitiSlots(f.OwnerSeat)).Select(s => s.ToString()).ToArray();
        return Array.AsReadOnly(values.Select(value => new PromptChoice(new($"mobile-shen-zhang-liao:{(f.Id)}:{(r.Stage)}:{(value)}"), r.Stage == MobileShenZhangLiaoStage.ChooseSkill ? "获得【" + _contentRegistryOperations._contentRegistry.GetSkill(value).Name + "】并令其失效" : (r.Stage == MobileShenZhangLiaoStage.AbolishSlot ? "废除" : "恢复") + MobileEquipmentSlotName(Enum.Parse<EquipmentSlot>(value)), [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shen-zhang-liao", ["value"] = value })).ToArray());
    }

    internal void PublishMobileShenZhangLiao(ProgramSkillFrame f)
    {
        var choices = MobileShenZhangLiaoChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.MobileShenZhangLiao!.TargetSeat].IsAlive)
        {
            FinishMobileShenZhangLiao(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "【" + skill.Name + "】请选择。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shen-zhang-liao")]
    internal void AnswerMobileShenZhangLiao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShenZhangLiao: { } r } f || !MobileShenZhangLiaoChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(choice.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("Mobile Shen Zhang Liao rejects a stale slot or skill payment.");
        var value = choice.Parameters["value"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MobileShenZhangLiaoStage.ChooseSkill)
        {
            if (MobileDuoruiLeases.Any(e => e.Source.OwnerSeat == f.OwnerSeat) || r.PaidSlot is null)
                throw new InvalidOperationException("Duorui cannot borrow twice or before its real equipment payment.");
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var source = "mobile-duorui:" + f.Id;
            var grant = source + ":" + value;
            var instance = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.FirstOrDefault(g => g.SkillId == value)?.SkillInstanceId ?? source + ":instance:" + value;
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
            owner.SkillGrants.Grant(new(grant, value, instance, source));
            _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, value);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuoruiIssuedEvent(f.Id, r.Source, r.GameplayHash, r.TargetSeat, value, grant, instance, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.PaidSlot.Value));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(f.OwnerSeat, f.SkillId, Array.AsReadOnly(new[] { value })));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileShenZhangLiao = r with { SelectedSkillId = value, Stage = MobileShenZhangLiaoStage.RestoreChildren } });
        }
        else
        {
            var slot = Enum.Parse<EquipmentSlot>(value);
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            if (r.Stage == MobileShenZhangLiaoStage.AbolishSlot)
            {
                _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot, 0);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileShenZhangLiao = r with { PaidSlot = slot, Before = before, After = _coreCardMovementJournalMovementSequenceAccess._movementSequence, Stage = MobileShenZhangLiaoStage.AbolishChildren } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDuoruiSlotPaidEvent(f.Id, r.Source, slot, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            }
            else
            {
                if (!_coreAdvancedLifecycleOperations.RestoreEquipmentSlot(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot))
                    throw new InvalidOperationException("Zhiti requires a genuinely disabled slot.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileShenZhangLiao = r with { PaidSlot = slot, Before = before, After = _coreCardMovementJournalMovementSequenceAccess._movementSequence, Stage = MobileShenZhangLiaoStage.RestoreChildren } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileZhitiSlotRestoredEvent(f.Id, r.Source, slot));
            }
        }

        ResumeMobileShenZhangLiao(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileDuoruiBorrowedSkill, SkillProgramEffectOp.RestoreMobileZhitiEquipmentSlot)]
    internal PromptChoice ChooseMobileShenZhangLiao(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    internal bool DrainMobileShenZhangLiao(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 39950)]
    internal bool ResumeMobileShenZhangLiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShenZhangLiao: { } r } f || f.Id != id)
            return false;
        if (r.Stage is MobileShenZhangLiaoStage.AbolishChildren or MobileShenZhangLiaoStage.RestoreChildren)
        {
            if (DrainMobileShenZhangLiao(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            if (r.Stage == MobileShenZhangLiaoStage.RestoreChildren)
            {
                FinishMobileShenZhangLiao(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenZhangLiao = r with { Stage = MobileShenZhangLiaoStage.ChooseSkill } });
        }

        PublishMobileShenZhangLiao(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39950)]
    internal bool ReturnMobileShenZhangLiaoMovement(ProgramSkillFrame f) => f.MobileShenZhangLiao is not null && ResumeMobileShenZhangLiao(f.Id);
    internal void FinishMobileShenZhangLiao(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenZhangLiao = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39950)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39950)]
    internal bool RetainMobileShenZhangLiao(ProgramSkillFrame f) => f.MobileShenZhangLiao is { PaidSlot: not null };
}
