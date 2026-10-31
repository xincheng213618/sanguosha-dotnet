#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal const string HuanShuJunChoice = "huan-shu-jun";
    internal const string HuanpeiDrawReason = "skill-program.huan-shu-jun.huanpei.draw";
    internal const string HuanpeiDiscardReason = "skill-program.huan-shu-jun.huanpei.discard";
    internal const string MiaoxinDrawReason = "skill-program.huan-shu-jun.miaoxin.draw";
    internal bool HasHuanShuJunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWenyiHealthOneAid) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh);

    internal static CardConversionSource HuanShuJunSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_huan_shu_jun_ol_HuanShuJun.HuanShuJunSource(f);
    SkillProgramStepOutcome IHuanShuJunHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHuanShuJun(f, op);
    internal SkillProgramStepOutcome BeginHuanShuJun(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HuanShuJun is not null)
        {
            ContinueHuanShuJun(f.Id);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var c = f.WindowContext;
        var target = op == SkillProgramEffectOp.ResolveWenyiHealthOneAid ? c?.HpChange?.TargetSeat ?? throw new InvalidOperationException("Wenyi lost its real HP-one window.") : f.OwnerSeat;
        var stage = op == SkillProgramEffectOp.ResolveWenyiHealthOneAid ? HuanShuJunStage.WenyiChoice : op == SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh ? HuanShuJunStage.Drawing : HuanShuJunStage.AdjustHand;
        var r = new HuanShuJunReceipt(HuanShuJunSource(f), f.GameplayHash, op, c?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage)
        {
            TargetSeat = target,
            FrozenHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count,
            FrozenHp = owner.Hp,
            Round = _coreActualRoundProgressRoundNumberAccess._roundNumber
        };
        if (op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse)
        {
            if (!HuanpeiSources(owner).Contains(r.Source) || !CanAdjustHuanpeiHand(owner))
                throw new InvalidOperationException("Huanpei lost its live source, unspent round or adjustable Hand.");
            r = r with
            {
                Request = new(HuanpeiIntent.Play, null, null, null, f.OwnerSeat, 0, null, null, new PromptId(0), _coreCommandSessionQueriesOperations.Revision)
            };
        }

        if (op == SkillProgramEffectOp.ResolveWenyiHealthOneAid && (!WenyiHpRoot(f) || WenyiRemaining(f.OwnerSeat) == 0))
            throw new InvalidOperationException("Wenyi requires its exact issued real HP change and an available charge.");
        if (op == SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh)
        {
            var refresh = WenyiRemaining(f.OwnerSeat) == 0;
            r = r with
            {
                Paid = true,
                RequestedCount = refresh ? 1 : 2,
                RefreshWenyi = refresh
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanShuJunStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner, target, r.FrozenHandCount, r.FrozenHp));
        if (op == SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiaoxinBenefitIssuedEvent(f.Id, f.OwnerSeat, r.WindowId, c!.OccurrenceIndex, r.RequestedCount, r.RefreshWenyi));
        ContinueHuanShuJun(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HuanShuJunChoices(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            [HuanShuJunChoice] = branch,
            ["program-action"] = HuanShuJunChoice,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, string suffix = "") => new(new($"huan-shu-jun.{(f.Id)}.{(r.Stage)}.{(branch)}.{(suffix)}"), label, cards ?? [], [], Args(branch));
        if (r.Stage == HuanShuJunStage.AdjustHand)
        {
            var required = Math.Max(0, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count - Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
            foreach (var card in _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                choices.Add(Choice("cost-toggle", (r.Selected.Contains(card.Id) ? "取消选择" : "选择") + card.DisplayName, [card.Id], card.Id.ToString(CultureInfo.InvariantCulture)));
            if (r.Selected.Count == required)
                choices.Add(Choice("cost-confirm", required == 0 ? "调整手牌并使用基本牌" : $"弃置选中的{(required)}张手牌"));
            choices.Add(Choice("cancel", "取消"));
        }
        else if (r.Stage == HuanShuJunStage.BasicChoice)
        {
            foreach (var a in HuanpeiPaidPlayActions(f))
            {
                var p = Args("basic-use");
                p["kind"] = a.PlayedCardKind!.Value.ToString();
                choices.Add(new(new($"huan-shu-jun.{(f.Id)}.basic.{(a.PlayedCardKind)}.{(string.Join('-', a.TargetSeats))}"), a.Description, [], a.TargetSeats, p));
            }
        }
        else if (r.Stage == HuanShuJunStage.WenyiChoice)
        {
            if (WenyiTargetCanReceivePeach(f))
                choices.Add(Choice("wenyi-peach", "对其使用一张桃"));
            if (r.TargetSeat != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && !card.IsGeneralWeapon))
                    choices.Add(Choice("wenyi-equipment", "交给其并令其使用" + card.DisplayName, [card.Id], card.Id.ToString(CultureInfo.InvariantCulture)));
            choices.Add(Choice("cancel", "取消温宜"));
        }
        else if (r.Stage == HuanShuJunStage.WenyiPeachRequest)
        {
            choices.AddRange(_coreWenyiPeachSuppliesOperations.WenyiPeachSupplyChoices(f));
            choices.Add(Choice("cancel", "取消温宜"));
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishHuanShuJun(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        if (!r.Paid && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f)))
        {
            FinishHuanShuJun(f);
            return;
        }

        var choices = HuanShuJunChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            FinishHuanShuJun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == HuanShuJunStage.AdjustHand ? "环佩：将手牌数调整至当前体力值。" : r.Stage == HuanShuJunStage.BasicChoice ? "环佩：选择一张基本牌及其合法目标。" : "温宜：使用桃，或交给目标装备牌令其使用。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), r.TargetSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("huan-shu-jun")]
    internal void AnswerHuanShuJun(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuanShuJun: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HuanShuJunChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Huan Shu Jun requires her exact current private decision.");
        if (r.Stage == HuanShuJunStage.WenyiPeachRequest && !choice.Parameters.ContainsKey(HuanShuJunChoice))
        {
            if (!_coreWenyiPeachSuppliesOperations.TryResolveWenyiPeachSupply(choice))
                throw new InvalidOperationException("Wenyi lost its exact supplied Peach.");
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters[HuanShuJunChoice];
        if (branch == "cancel")
        {
            FinishHuanShuJun(f);
            return;
        }

        if (branch == "cost-toggle")
        {
            var id = choice.Cards.Single();
            var next = r.Selected.Contains(id) ? r.Selected.Where(x => x != id).ToArray() : r.Selected.Append(id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { Selected = next } });
            PublishHuanShuJun(f);
            return;
        }

        if (branch == "cost-confirm")
        {
            PayHuanpeiAdjustment(f);
            return;
        }

        if (branch == "basic-use")
        {
            ChooseHuanpeiOutput(f, Enum.Parse<CardKind>(choice.Parameters["kind"]), choice.Targets);
            return;
        }

        if (branch == "wenyi-peach")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { Stage = HuanShuJunStage.WenyiPeachRequest } });
            PublishHuanShuJun(f);
            return;
        }

        if (branch == "wenyi-equipment")
        {
            GiveWenyiEquipment(f, choice.Cards.Single());
            return;
        }

        throw new InvalidOperationException("Unknown Huan Shu Jun branch.");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh)]
    internal void ReturnHuanShuJunDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.HuanShuJun ?? throw new InvalidOperationException("Huan Shu Jun's draw lost its own receipt.");
        if (r.Stage != HuanShuJunStage.Drawing || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.RequestedCount || !r.Paid)
            throw new InvalidOperationException("Huan Shu Jun draw returned to another paid request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { DrawOutcome = outcome, Stage = HuanShuJunStage.DrawChildren, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanShuJunDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        if (r.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanpeiAdjustmentCompletedEvent(f.Id, r.Before, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        ContinueHuanShuJun(f.Id);
    }

    internal bool DrainHuanShuJun(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueHuanShuJun(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HuanShuJun: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (f.NativeDraw is not null)
                return;
            if (r.Stage is HuanShuJunStage.AdjustmentChildren or HuanShuJunStage.DrawChildren or HuanShuJunStage.EquipmentGiftChildren or HuanShuJunStage.NativeChildren or HuanShuJunStage.FaceChildren or HuanShuJunStage.ChainChildren)
            {
                if (DrainHuanShuJun(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.HuanShuJun!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishHuanShuJun(f);
                return;
            }

            if (r.Stage == HuanShuJunStage.Drawing)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishHuanShuJun(f);
                    return;
                }

                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.RequestedCount, new(r.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse ? HuanpeiDrawReason : MiaoxinDrawReason));
                return;
            }

            if (r.Op == SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh && r.Stage == HuanShuJunStage.DrawChildren)
            {
                if (r.RefreshWenyi && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiaoxinWenyiRefreshedEvent(f.Id, f.OwnerSeat));
                FinishHuanShuJun(f);
                return;
            }

            if (r.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse && r.Stage is HuanShuJunStage.AdjustmentChildren or HuanShuJunStage.DrawChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishHuanShuJun(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { Stage = HuanShuJunStage.BasicChoice } });
                if (r.Request?.Intent != HuanpeiIntent.Play)
                {
                    ContinueHuanpeiOriginalRequest(f);
                    return;
                }

                PublishHuanShuJun(f);
                return;
            }

            if (r.Stage == HuanShuJunStage.EquipmentGiftChildren)
            {
                IssueWenyiEquipmentUse(f);
                return;
            }

            if (r.Stage == HuanShuJunStage.NativeChildren)
            {
                RestoreWenyiRecipient(f, true);
                return;
            }

            if (r.Stage == HuanShuJunStage.FaceChildren)
            {
                RestoreWenyiRecipient(f, false);
                return;
            }

            if (r.Stage == HuanShuJunStage.ChainChildren)
            {
                FinishHuanShuJun(f);
                return;
            }

            if (r.Stage is HuanShuJunStage.NativeIssuing or HuanShuJunStage.NativeUse)
                return;
            PublishHuanShuJun(f);
            return;
        }
    }

    internal void FinishHuanShuJun(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuanShuJunCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        if (r.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse && r.Request is { Intent: HuanpeiIntent.WenyiPeach, ParentFrameId: { } parentId } && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HuanShuJun: { Paid: true } } parent && parent.Id == parentId)
            FinishUnissuedWenyiPeach(parent);
        else if (r.Op == SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse && r.Request is { Intent: not HuanpeiIntent.Play } && r.OriginalDecision is not null)
            RepublishHuanpeiOriginal(r);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28100)]
    internal bool ResumeHuanShuJun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuanShuJun: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueHuanShuJun(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28100)]
    internal bool ReturnHuanShuJunMovement(ProgramSkillFrame f)
    {
        if (f.HuanShuJun is null)
            return false;
        ContinueHuanShuJun(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveHuanpeiHandAdjustmentAndBasicUse, SkillProgramEffectOp.ResolveWenyiHealthOneAid, SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh)]
    internal PromptChoice ChooseHuanShuJunAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        if (r.Stage == HuanShuJunStage.AdjustHand)
            return prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault(HuanShuJunChoice) == "cost-confirm") ?? prompt.Choices.First(c => c.Parameters.GetValueOrDefault(HuanShuJunChoice) == "cost-toggle" && !r.Selected.Contains(c.Cards.Single()));
        return prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault(HuanShuJunChoice) != "cancel") ?? prompt.Choices[0];
    }
}
