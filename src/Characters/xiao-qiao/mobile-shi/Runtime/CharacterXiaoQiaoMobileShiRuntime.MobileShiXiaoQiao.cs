#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMobileShiRuntime
{
    internal const string ShiXqHeyun = "mobile:shi-heyun", ShiXqYinhui = "mobile:shi-yinhui", ShiXqDrawReason = "skill.mobile-shi-xiao-qiao.heyun.draw";
    internal static CardConversionSource ShiXqSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static string ShiXqGrantSource(int owner, long frame) => $"acquired:mobile:shi-yinhui:{(owner)}:{(frame)}";
    internal string[] ShiXqVisibleOwnedSkills(int viewer, int seat)
    {
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var visible = new List<GeneralSkillDefinition>();
        if (player.IdentityConcealed && viewer != seat)
            return[];
        if (player.GeneralSelected && (player.GeneralRevealed || viewer == seat))
            visible.AddRange(_coreNationalSkillsOperations.OwnedRuntimeSkills(player, player.General, includeAcquired: true));
        if (player.SecondaryGeneral is { } second && player.SecondaryGeneralSelected && (player.SecondaryGeneralRevealed || viewer == seat))
            visible.AddRange(_coreNationalSkillsOperations.OwnedRuntimeSkills(player, second, includeAcquired: false));
        var owned = _coreNationalSkillsOperations.GetSkillBindingShard(player).ActiveGrants.Select(g => g.SkillId).ToHashSet(StringComparer.Ordinal);
        return visible.Where(s => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.WenheActivity is null || _coreWenheActivitiesOperations.IsWenhePlayerSkillVisible(player, s, viewer, false)).Select(s => s.ContentId).OfType<string>().Where(owned.Contains).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    }

    internal string[] ShiXqLosableSkills(int owner)
    {
        var grants = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[owner]).ActiveGrants;
        return ShiXqVisibleOwnedSkills(owner, owner).Where(id => grants.Any(g => g.SkillId == id && !g.IsModeRule) && !grants.Any(g => g.SkillId == id && g.IsModeRule)).ToArray();
    }

    internal int[] ShiXqHeyunTargets(int owner)
    {
        if (ShiXqLosableSkills(owner).Length == 0)
            return[];
        var own = ShiXqVisibleOwnedSkills(owner, owner).ToHashSet(StringComparer.Ordinal);
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && ShiXqVisibleOwnedSkills(owner, p.Seat).Any(own.Contains)).Select(p => p.Seat).ToArray();
    }

    internal (int Seat, string Skill)[] ShiXqCopyOptions(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).SelectMany(p => ShiXqVisibleOwnedSkills(owner, p.Seat).Select(id => (p.Seat, Skill: id))).ToArray();
    internal ShiXqCopiedEvent[] ShiXqGrantLedger(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqCopiedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == ShiXqYinhui && e.GrantSourceId == ShiXqGrantSource(owner, e.FrameId) && e.GrantId == e.GrantSourceId + ":" + e.SkillId).ToArray();
    internal ShiXqCopiedEvent[] ShiXqLiveCopies(int owner) => ShiXqGrantLedger(owner).Where(e => _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.Any(g => g.GrantId == e.GrantId && g.SourceId == e.GrantSourceId && g.SkillId == e.SkillId && g.SkillInstanceId == e.SkillInstanceId)).ToArray();
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 50400, SkillProgramEffectOp.ResolveMobileShiXiaoQiaoHeyun)]
    internal bool CanActivateShiXqHeyun(CharacterState owner) => ShiXqHeyunTargets(owner.Seat).Length != 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 50400)]
    internal bool CanOfferShiXqYinhui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => c.SkillId != ShiXqYinhui || context.Window == SkillProgramTriggerWindow.RoundStarting && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && ShiXqCopyOptions(c.OwnerSeat).Length != 0;
    SkillProgramStepOutcome IShiXqHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginShiXq(f, operation);
    internal SkillProgramStepOutcome BeginShiXq(ProgramSkillFrame input, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var copying = operation == SkillProgramEffectOp.ResolveMobileShiXiaoQiaoYinhui;
        if (f.ShiXiaoQiao is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.SkillId != (copying ? ShiXqYinhui : ShiXqHeyun) || copying && (f.TriggerId != "copy-current-skill" || f.WindowContext?.Window != SkillProgramTriggerWindow.RoundStarting) || !copying && (f.TriggerId is not null || f.ActivationId != "lose-for-draw" || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play))
            throw new InvalidOperationException("Shi Xiao Qiao requires its original round-start or twice-per-Play skill-change instruction.");
        var r = new ShiXqReceipt(ShiXqSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.WindowContext?.ParentFrameId ?? f.ParentFrameId ?? 0, operation, copying ? ShiXqStage.RevokeChildren : ShiXqStage.Target)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Paid = copying
        };
        if (copying)
            r = r with
            {
                RevokeIds = ShiXqGrantLedger(f.OwnerSeat).Where(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiXqGrantClearedEvent>().Any(c => c.Owner == f.OwnerSeat && c.GrantId == e.GrantId)).Select(e => e.GrantId).Distinct(StringComparer.Ordinal).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.Round, r.ParentId, operation));
        if (!copying)
        {
            PublishShiXq(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var removed = _coreProgramSkillChangesOperations.RevokeOwnedProgramSkillSources(f, r.RevokeIds);
        foreach (var id in r.RevokeIds)
        {
            var old = ShiXqGrantLedger(f.OwnerSeat).Single(e => e.GrantId == id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqGrantClearedEvent(f.Id, f.OwnerSeat, id, old.SkillId, removed.Any(g => g.GrantId == id)));
        }

        PublishShiXqSkillMutation(f.Id);
        ContinueShiXq(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiXqChoices(ProgramSkillFrame f)
    {
        var r = f.ShiXiaoQiao!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string key, string label, int? target = null, string? skill = null)
        {
            var parameters = new Dictionary<string, string>
            {
                ["program-action"] = "mobile-shi-xiao-qiao",
                ["branch"] = branch,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
            };
            if (skill is not null)
                parameters["skill-id"] = skill;
            choices.Add(new(new($"shi-xiao-qiao.{(f.Id)}.{(branch)}.{(key)}"), label, [], target is { } seat ? [seat] : [], parameters));
        }

        if (r.Stage == ShiXqStage.Target)
            foreach (var seat in ShiXqHeyunTargets(f.OwnerSeat))
                Add("target", seat.ToString(CultureInfo.InvariantCulture), "令" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "摸两张牌", seat);
        if (r.Stage == ShiXqStage.LoseSkill && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && ShiXqHeyunTargets(f.OwnerSeat).Contains(r.Target))
            foreach (var id in ShiXqLosableSkills(f.OwnerSeat))
                Add("lose", id, "失去【" + _contentRegistryOperations._contentRegistry.GetSkill(id).Name + "】，令" + _coreMatchParticipantsPlayersAccess._players[r.Target].Name + "摸两张牌", skill: id);
        if (r.Stage == ShiXqStage.CopyTarget)
            foreach (var seat in ShiXqCopyOptions(f.OwnerSeat).Select(o => o.Seat).Distinct())
                Add("donor", seat.ToString(CultureInfo.InvariantCulture), "获得" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "当前拥有的一个技能", seat);
        if (r.Stage == ShiXqStage.CopySkill)
            foreach (var option in ShiXqCopyOptions(f.OwnerSeat).Where(o => o.Seat == r.Target))
                Add("copy", option.Skill, "获得【" + _contentRegistryOperations._contentRegistry.GetSkill(option.Skill).Name + "】", r.Target, option.Skill);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiXq(ProgramSkillFrame f)
    {
        var choices = ShiXqChoices(f);
        if (choices.Count == 0)
        {
            FinishShiXq(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ShiXiaoQiao!.Stage switch
        {
            ShiXqStage.Target => "合韵：选择一名与你有相同技能的角色，可以选择自己。",
            ShiXqStage.LoseSkill => "合韵：选择自己失去的一个技能；随后原目标摸两张牌。",
            ShiXqStage.CopyTarget => "音洄：已经清除此前因此获得的来源，选择一名其他角色。",
            _ => "音洄：选择其当前拥有的一个技能。"
        }, [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-xiao-qiao")]
    internal void AnswerShiXq(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiaoQiao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiXqChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Shi Xiao Qiao lost its exact current common-skill target, lost skill or public current donor skill.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage is ShiXqStage.Target or ShiXqStage.CopyTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { Target = selected.Targets.Single(), Stage = r.Stage == ShiXqStage.Target ? ShiXqStage.LoseSkill : ShiXqStage.CopySkill } });
            PublishShiXq(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var id = selected.Parameters["skill-id"];
        if (r.Stage == ShiXqStage.LoseSkill)
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { SelectedSkill = id, Paid = true, Stage = ShiXqStage.LossChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqHeyunPaidEvent(f.Id, r.Source, r.Target, id));
            _coreProgramSkillChangesOperations.ReplaceProgramSkills(f, [id], [], ProgramSkillSourceRemoval.NonModePreserveProjections, acquireFirst: false);
            PublishShiXqSkillMutation(f.Id);
            ContinueShiXq(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var sourceId = ShiXqGrantSource(f.OwnerSeat, f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { SelectedSkill = id, Paid = true, Stage = ShiXqStage.GrantChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var grant = _coreProgramSkillChangesOperations.GrantOwnedProgramSkillSource(f, sourceId, id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiXiaoQiao = f.ShiXiaoQiao!with { GrantId = grant.GrantId, GrantSource = grant.SourceId, GrantedInstance = grant.SkillInstanceId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqCopiedEvent(f.Id, r.Source, r.Hash, r.Round, r.Target, id, grant.GrantId, grant.SourceId, grant.SkillInstanceId));
        PublishShiXqSkillMutation(f.Id);
        ContinueShiXq(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void PublishShiXqSkillMutation(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiXiaoQiao = f.ShiXiaoQiao!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiXq(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiXq(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiaoQiao: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || r.Stage == ShiXqStage.Drawing)
            return;
        if (r.Stage is ShiXqStage.LossChildren or ShiXqStage.RevokeChildren or ShiXqStage.GrantChildren or ShiXqStage.DrawChildren)
        {
            if (DrainShiXq(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShiXiaoQiao!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == ShiXqStage.RevokeChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishShiXq(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { Stage = ShiXqStage.CopyTarget } });
                PublishShiXq(f);
                return;
            }

            if (r.Stage == ShiXqStage.LossChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                {
                    FinishShiXq(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { Stage = ShiXqStage.Drawing } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, 2, new(ShiXqDrawReason));
                return;
            }

            FinishShiXq(f);
            return;
        }

        PublishShiXq(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiXiaoQiaoHeyun)]
    internal void ReturnShiXqDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.ShiXiaoQiao!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = r with { Stage = ShiXqStage.DrawChildren, Draw = outcome, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter) }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqDrawPaidEvent(f.Id, r.Target, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.Replacement is not null, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShiXq(f.Id);
    }

    internal void FinishShiXq(ProgramSkillFrame f)
    {
        if (f.ShiXiaoQiao!.Stage == ShiXqStage.Complete)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiaoQiao = f.ShiXiaoQiao with { Stage = ShiXqStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXqCompletedEvent(f.Id, f.ShiXiaoQiao.Operation));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 50400)]
    internal bool ResumeShiXq(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiaoQiao: not null } f || f.Id != id)
            return false;
        ContinueShiXq(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 50400)]
    internal bool ReturnShiXqMovement(ProgramSkillFrame f)
    {
        if (f.ShiXiaoQiao is null)
            return false;
        ContinueShiXq(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiXiaoQiaoHeyun, SkillProgramEffectOp.ResolveMobileShiXiaoQiaoYinhui)]
    internal PromptChoice ChooseShiXq(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.ShiXiaoQiao!.Stage == ShiXqStage.Target)
            return p.Choices.FirstOrDefault(c => c.Targets.SequenceEqual([f.OwnerSeat])) ?? p.Choices[0];
        if (f.ShiXiaoQiao.Stage == ShiXqStage.LoseSkill)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("skill-id") != ShiXqHeyun && c.Parameters.GetValueOrDefault("skill-id") != ShiXqYinhui) ?? p.Choices[0];
        return p.Choices[0];
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiXiaoQiaoPublicStates(CharacterState owner, string id)
    {
        if (id is not (ShiXqHeyun or ShiXqYinhui))
            return[];
        var copies = ShiXqLiveCopies(owner.Seat);
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == id).SelectMany(i => id == ShiXqHeyun ? new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "heyun-uses", ProgramPublicRuleStateKind.ActivationLimit, 2, _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, ShiXqHeyun, "lose-for-draw")), SkillUsageScope.Phase) } : new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "yinhui-grants", ProgramPublicRuleStateKind.GameActivationProgress, copies.Length) }.Concat(copies.Select(c => new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "yinhui-skill:" + c.SkillId, ProgramPublicRuleStateKind.GameActivationProgress, 1)))).ToArray();
    }
}
