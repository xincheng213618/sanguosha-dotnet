#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMobileGodRuntime
{
    internal const string ShenGuoJiaHuishi = "mobile:huishi", ShenGuoJiaTianyi = "mobile:tianyi", ShenGuoJiaHuiShi = "mobile:huishi-gift", ShenGuoJiaZuoxing = "mobile:zuoxing";
    SkillProgramStepOutcome IShenGuoJiaHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginShenGuoJia(frame, operation);
    bool IProgramAwakeningOverrideQuery.Granted(int owner, string skill, string? binding) => HasShenGuoJiaAwakeningCondition(owner, skill, binding);
    bool IProgramAwakeningOverrideQuery.Issued(ProgramSkillFrame frame) => HasShenGuoJiaAwakeningReceipt(frame);
    internal CardConversionSource ShenGuoJiaSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 38000, SkillProgramEffectOp.ResolveShenGuoJiaHuishi)]
    internal bool CanActivateShenGuoJiaHuishi(CharacterState owner) => owner.HasHpValue && owner.MaxHp < 10;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 38001, SkillProgramEffectOp.ResolveShenGuoJiaZuoxing)]
    internal bool CanActivateShenGuoJiaZuoxing(CharacterState owner) => ShenGuoJiaBenefactors(owner, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, ShenGuoJiaZuoxing)).Count > 0 && ShenGuoJiaPlans(owner.Seat).Count > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 38000, SkillProgramEffectOp.ResolveShenGuoJiaTianyi)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38000, SkillProgramEffectOp.ResolveShenGuoJiaTianyi)]
    internal bool CanAwakenShenGuoJia(ProgramTriggerLegalityContext gate) => HasShenGuoJiaAwakeningCondition(gate.OwnerSeat, gate.SkillId, gate.Candidate.BindingId) || _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).All(player => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Any(damage => damage.TargetSeat == player.Seat && damage.Amount > 0));
    internal SkillProgramStepOutcome BeginShenGuoJia(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (operation == SkillProgramEffectOp.ResolveShenGuoJiaTianyi)
        {
            if (frame.SkillId != ShenGuoJiaTianyi || frame.TriggerId != "all-living-damaged" || frame.SelectedTargetSeats is not [var recipient] || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
                throw new InvalidOperationException("Tianyi requires the actual awakening's chosen living recipient.");
            var source = $"shen-guo-jia:{(frame.OwnerSeat)}:{(frame.SkillInstanceId)}:{(frame.Id)}";
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[recipient], source, [ShenGuoJiaZuoxing]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaBenefactorEvent(frame.Id, frame.OwnerSeat, recipient, source));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillAwakenedEvent(frame.OwnerSeat, frame.SkillId, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp, Array.AsReadOnly(new[] { ShenGuoJiaZuoxing })));
            return SkillProgramStepOutcome.Continue;
        }

        if (frame.ShenGuoJia is not null || frame.TriggerId is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || operation == SkillProgramEffectOp.ResolveShenGuoJiaHuishi && (frame.SkillId != ShenGuoJiaHuishi || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp >= 10) || operation == SkillProgramEffectOp.ResolveShenGuoJiaHuishiGrant && (frame.SkillId != ShenGuoJiaHuiShi || frame.SelectedTargetSeats.Count != 1) || operation == SkillProgramEffectOp.ResolveShenGuoJiaZuoxing && frame.SkillId != ShenGuoJiaZuoxing)
            throw new InvalidOperationException("Shen Guo Jia requires his unpaid own native activation.");
        var target = frame.SelectedTargetSeats.FirstOrDefault(-1);
        var stage = operation == SkillProgramEffectOp.ResolveShenGuoJiaHuishi ? ShenGuoJiaStage.Judging : operation == SkillProgramEffectOp.ResolveShenGuoJiaHuishiGrant ? ShenGuoJiaStage.AwakeningChoice : ShenGuoJiaStage.UseChoice;
        var receipt = new ShenGuoJiaReceipt(ShenGuoJiaSource(frame), frame.GameplayHash, operation, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, 0, stage)
        {
            Target = target
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaStartedEvent(frame.Id, receipt.Source, frame.GameplayHash, operation, receipt.Turn, receipt.TurnOwner, 0, target));
        if (stage == ShenGuoJiaStage.Judging)
            BeginShenGuoJiaJudgment(frame);
        else if (stage == ShenGuoJiaStage.AwakeningChoice && (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp < _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive) || ShenGuoJiaUnawakened(target).Count == 0))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.DrawAttempt } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, target, 4, new("skill-program.mobile:huishi-gift.draw"));
        }
        else
            PublishShenGuoJiaChoice(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<(int Seat, string Instance)> ShenGuoJiaBenefactors(CharacterState owner, string instance) => Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaBenefactorEvent>().Where(fact => fact.Recipient == owner.Seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.Benefactor) && _coreMatchParticipantsPlayersAccess._players[fact.Benefactor].IsAlive && _coreMatchParticipantsPlayersAccess._players[fact.Benefactor].MaxHp > 1).Where(fact => owner.SkillGrants.Grants.Any(grant => grant.IsEnabled && grant.SkillId == ShenGuoJiaZuoxing && grant.SkillInstanceId == instance && grant.SourceId == "acquired:" + fact.GrantSource)).Select(fact => (fact.Benefactor, fact.GrantSource)).Distinct().ToArray());
    internal IReadOnlyList<IndexedSkillProgramInstance> ShenGuoJiaUnawakened(int seat) => Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Where(instance => _contentRegistryOperations._contentRegistry.GetSkill(instance.SkillId).Tags.HasFlag(SkillTag.Awakening) && !ShenGuoJiaAlreadyAwakened(seat, instance.SkillId) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SkillAwakenedEvent>().Any(fact => fact.PlayerSeat == seat && fact.SkillId == instance.SkillId) && !instance.Program.Triggers.Any(trigger => trigger.UsageScope == SkillUsageScope.Game && trigger.UsageLimit == 1 && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(seat, instance.SkillId, _coreProgramLifecycleOperations.ProgramTriggerUsageId(new(seat, instance.SkillId, trigger.Id, instance.SkillInstanceId, instance.Program.GameplayHash, trigger.Priority)), SkillUsageScope.Game) > 0)).DistinctBy(instance => instance.SkillId).ToArray());
    internal bool ShenGuoJiaAlreadyAwakened(int seat, string skill) => skill switch
    {
        "ol:choujue" => _characterWenYangOlOperations.Value.WenYangHasAwakened(seat, 1),
        "ol:beishui" => _characterWenYangOlOperations.Value.WenYangHasAwakened(seat, 2),
        "ol:dili" => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(seat, skill, "awakening", SkillUsageScope.Game) > 0,
        "mou-mobile:zhiji" => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiangWeiAwakenedEvent>().Any(value => value.OwnerSeat == seat),
        _ => false
    };
    internal IEnumerable<string> ShenGuoJiaAwakeningBindings(string skill) => _contentRegistryOperations._contentRegistry.GetSkill(skill).Program!.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op is SkillProgramEffectOp.ChangeMaximumHp or SkillProgramEffectOp.GrantSkills or SkillProgramEffectOp.ReplaceSkillsOnAwakening or SkillProgramEffectOp.ResolveMouJiangWeiZhiji or SkillProgramEffectOp.ResolveMouCaoShuangFanshi || effect.Op.ToString().Contains("Awaken", StringComparison.Ordinal))).Select(trigger => trigger.Id);
    internal bool HasShenGuoJiaAwakeningCondition(int seat, string skill, string? binding = null) => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(skill, out var definition) && definition.Tags.HasFlag(SkillTag.Awakening) && (binding is null || ShenGuoJiaAwakeningBindings(skill).Contains(binding, StringComparer.Ordinal)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaAwakeningOverrideEvent>().Any(fact => fact.Recipient == seat && fact.SkillId == skill && definition.Program?.GameplayHash == fact.GameplayHash && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skill, fact.InstanceId));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AwakeningConditionOverride, 38000)]
    internal bool ShenGuoJiaAwakeningOverride(ProgramTriggerLegalityContext gate) => HasShenGuoJiaAwakeningCondition(gate.OwnerSeat, gate.SkillId, gate.Candidate.BindingId);
    internal PromptChoice ShenGuoJiaChoice(ProgramSkillFrame frame, string branch, string label, IReadOnlyList<int>? targets = null, string? value = null) => new(new($"shen-guo-jia.{(frame.Id)}.{(frame.ShenGuoJia!.Stage)}.{(branch)}.{(value)}"), label, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "shen-guo-jia", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value ?? "" });
    internal IReadOnlyList<PromptChoice> ShenGuoJiaChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ShenGuoJia!;
        var choices = new List<PromptChoice>();
        switch (receipt.Stage)
        {
            case ShenGuoJiaStage.Repeat:
                if (receipt.MayRepeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp < 10)
                    choices.Add(ShenGuoJiaChoice(frame, "repeat", "增加1点体力上限并继续判定"));
                choices.Add(ShenGuoJiaChoice(frame, "stop", "停止判定"));
                break;
            case ShenGuoJiaStage.Recipient:
                foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
                    choices.Add(ShenGuoJiaChoice(frame, "recipient", $"将全部判定牌交给{(player.Name)}", [player.Seat], player.Seat.ToString(CultureInfo.InvariantCulture)));
                choices.Add(ShenGuoJiaChoice(frame, "decline", "不交给角色，将判定牌置入弃牌堆"));
                break;
            case ShenGuoJiaStage.AwakeningChoice:
                foreach (var instance in ShenGuoJiaUnawakened(receipt.Target))
                    choices.Add(ShenGuoJiaChoice(frame, "awakening", $"令{(_coreMatchParticipantsPlayersAccess._players[receipt.Target].Name)}满足【{(_contentRegistryOperations._contentRegistry.GetSkill(instance.SkillId).Name)}】觉醒条件", [receipt.Target], instance.SkillId));
                break;
            case ShenGuoJiaStage.UseChoice:
                foreach (var benefactor in ShenGuoJiaBenefactors(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillInstanceId).Select(value => value.Seat).Distinct())
                    foreach (var(plan, index)in ShenGuoJiaPlans(frame.OwnerSeat).Select((plan, index) => (plan, index)))
                        choices.Add(ShenGuoJiaChoice(frame, "use", $"令{(_coreMatchParticipantsPlayersAccess._players[benefactor].Name)}减少1点体力上限：{(CardCatalog.Get(plan.Kind).DisplayName)}（{(string.Join('、', plan.Targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)))}）", plan.Targets, $"{(benefactor)}:{(index)}"));
                choices.Add(ShenGuoJiaChoice(frame, "cancel", "取消发动"));
                break;
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShenGuoJiaChoice(ProgramSkillFrame frame)
    {
        var choices = ShenGuoJiaChoices(frame);
        if (choices.Count == 0)
        {
            FinishShenGuoJia(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(frame, frame.OwnerSeat, choices, $"【{(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Name)}】请选择。", frame.OwnerSeat);
    }

    [ProgramAnswer("shen-guo-jia")]
    internal void AnswerShenGuoJia(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenGuoJia: { } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ShenGuoJiaChoices(frame).Where(current => current.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Shen Guo Jia requires the published current owner choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (receipt.Stage == ShenGuoJiaStage.Repeat)
        {
            if (branch == "repeat")
            {
                frame = SetShenGuoJiaChildren(frame, ShenGuoJiaStage.GrowChildren);
                PayShenGuoJiaMaximum(frame, frame.OwnerSeat, 1, "huishi-grow");
                ContinueShenGuoJia(frame.Id);
            }
            else
                ChooseShenGuoJiaRecipient(frame);
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.Recipient)
        {
            var target = branch == "decline" ? -1 : choice.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Target = target } });
            MoveShenGuoJiaJudgments(frame, target);
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.AwakeningChoice)
        {
            var instance = ShenGuoJiaUnawakened(receipt.Target).Single(value => value.SkillId == choice.Parameters["value"]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaAwakeningOverrideEvent(frame.Id, receipt.Source, receipt.Target, instance.SkillId, instance.SkillInstanceId, _contentRegistryOperations._contentRegistry.GetSkill(instance.SkillId).Program!.GameplayHash));
            PayShenGuoJiaFinalCost(frame, -2, "huishi-gift");
            return;
        }

        if (receipt.Stage != ShenGuoJiaStage.UseChoice)
            throw new InvalidOperationException("The chosen instruction is already paid.");
        if (branch == "cancel")
        {
            var activation = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Activation!;
            var key = (frame.OwnerSeat, frame.SkillId, activation.UsageGroup);
            var turnUses = _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault(key);
            var phaseUses = _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault(key);
            if (turnUses < 1 || phaseUses < 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Any(fact => fact.FrameId == frame.Id))
                throw new InvalidOperationException("Only the exact unpaid Zuoxing activation can return its reserved phase allowance.");
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses[key] = turnUses - 1;
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses[key] = phaseUses - 1;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaUnpaidCancelledEvent(frame.Id, receipt.Source, turnUses, phaseUses));
            FinishShenGuoJia(frame);
            return;
        }

        var values = choice.Parameters["value"].Split(':');
        var benefactor = int.Parse(values[0], CultureInfo.InvariantCulture);
        var plan = ShenGuoJiaPlans(frame.OwnerSeat)[int.Parse(values[1], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Plan = plan, Benefactor = benefactor } });
        frame = SetShenGuoJiaChildren(frame, ShenGuoJiaStage.CostChildren);
        PayShenGuoJiaMaximum(frame, benefactor, -1, "zuoxing");
        ContinueShenGuoJia(frame.Id);
    }

    internal ProgramSkillFrame SetShenGuoJiaChildren(ProgramSkillFrame frame, ShenGuoJiaStage stage)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = frame.ShenGuoJia!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        return frame;
    }

    internal void FinishShenGuoJiaIssue(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenGuoJia = frame.ShenGuoJia!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void PayShenGuoJiaMaximum(ProgramSkillFrame frame, int seat, int amount, string purpose)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var before = player.MaxHp;
        var after = _coreWushuangTacticRulesOperations.HasWushuangTactic(seat, 2020) ? 7 : Math.Max(0, checked(before + amount));
        var hp = player.Hp;
        player.MaxHp = after;
        player.Hp = Math.Min(hp, after);
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShenGuoJia = frame.ShenGuoJia!with { MaximumBefore = before, MaximumAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, after - before, after, frame.SkillId) { HpBeforeClamp = hp });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaMaximumPaidEvent(frame.Id, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Count(fact => fact.FrameId == frame.Id), seat, before, after, purpose));
        if (after == 0 && player.IsAlive)
            _coreNativePlayerDeathOperations.BeginPlayerDeath(frame.Id, player, null, null, null, causingProgramSkillFrameId: frame.Id);
    }

    internal void PayShenGuoJiaFinalCost(ProgramSkillFrame frame, int amount, string purpose)
    {
        frame = SetShenGuoJiaChildren(frame, ShenGuoJiaStage.CostChildren);
        PayShenGuoJiaMaximum(frame, frame.OwnerSeat, amount, purpose);
        ContinueShenGuoJia(frame.Id);
    }

    internal bool DrainShenGuoJia(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id);
    internal void ContinueShenGuoJia(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenGuoJia: { } receipt } frame || frame.Id != id)
            return;
        if (receipt.Stage is ShenGuoJiaStage.Judging or ShenGuoJiaStage.DrawAttempt or ShenGuoJiaStage.NativeUse or ShenGuoJiaStage.IssuingUse or ShenGuoJiaStage.ReturnedUse)
            return;
        if (DrainShenGuoJia(frame))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.ShenGuoJia!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
        if (receipt.Stage == ShenGuoJiaStage.JudgmentChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                MoveShenGuoJiaJudgments(frame, -1);
                return;
            }

            if (receipt.MayRepeat && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp < 10)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.Repeat } });
                PublishShenGuoJiaChoice(frame);
            }
            else
                ChooseShenGuoJiaRecipient(frame);
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.GrowChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
                BeginShenGuoJiaJudgment(frame);
            else
                MoveShenGuoJiaJudgments(frame, -1);
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.GainChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.Target) && _coreMatchParticipantsPlayersAccess._players[receipt.Target].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.Target]).Count == _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Max(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count))
            {
                PayShenGuoJiaFinalCost(frame, -1, "huishi-most-hand");
                return;
            }

            FinishShenGuoJia(frame);
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.DrawChildren)
        {
            PayShenGuoJiaFinalCost(frame, -2, "huishi-gift");
            return;
        }

        if (receipt.Stage == ShenGuoJiaStage.CostChildren && receipt.Operation == SkillProgramEffectOp.ResolveShenGuoJiaZuoxing && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            IssueShenGuoJiaVirtualUse(frame);
            return;
        }

        if (receipt.Stage is ShenGuoJiaStage.CostChildren or ShenGuoJiaStage.CleanupChildren)
        {
            FinishShenGuoJia(frame);
            return;
        }

        PublishShenGuoJiaChoice(frame);
    }

    internal void ChooseShenGuoJiaRecipient(ProgramSkillFrame frame)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !frame.ShenGuoJia!.Cards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
        {
            MoveShenGuoJiaJudgments(frame, -1);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = frame.ShenGuoJia with { Stage = ShenGuoJiaStage.Recipient } });
        PublishShenGuoJiaChoice(frame);
    }

    internal void MoveShenGuoJiaJudgments(ProgramSkillFrame frame, int target)
    {
        var ids = frame.ShenGuoJia!.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
        frame = SetShenGuoJiaChildren(frame, target < 0 ? ShenGuoJiaStage.CleanupChildren : ShenGuoJiaStage.GainChildren);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, target < 0 ? CardLocation.DiscardPile : CardLocation.Hand(target), new(target < 0 ? "skill-program.mobile:huishi.cleanup" : "skill-program.mobile:huishi.give"), target < 0 ? null : _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, target, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, _) =>
        {
        }));
        FinishShenGuoJiaIssue(frame.Id);
        ContinueShenGuoJia(frame.Id);
    }

    internal void FinishShenGuoJia(ProgramSkillFrame frame)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaCompletedEvent(frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38000)]
    internal bool ResumeShenGuoJia(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenGuoJia: not null } frame || frame.Id != id)
            return false;
        ContinueShenGuoJia(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38000)]
    internal bool ReturnShenGuoJiaMovement(ProgramSkillFrame frame) => frame.ShenGuoJia is not null && ResumeShenGuoJia(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38000)]
    internal bool ContinuePaidShenGuoJia(ProgramSkillFrame frame) => frame.ShenGuoJia is not null && ValidShenGuoJia(frame);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShenGuoJiaHuishi, SkillProgramEffectOp.ResolveShenGuoJiaHuishiGrant, SkillProgramEffectOp.ResolveShenGuoJiaZuoxing)]
    internal PromptChoice ChooseShenGuoJiaAi(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "recipient" && choice.Targets.Contains(frame.OwnerSeat)) ?? prompt.Choices[0];
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShenGuoJiaHuishiGrant)]
    internal void ReturnShenGuoJiaFourDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var receipt = frame.ShenGuoJia!;
        if (!ValidShenGuoJia(frame) || receipt.Stage != ShenGuoJiaStage.DrawAttempt || frame.NativeDraw is not null || outcome.RecipientSeat != receipt.Target || outcome.RequestedCount != 4 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Hui Shi requires its exact four-card native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        ContinueShenGuoJia(frame.Id);
    }
}
