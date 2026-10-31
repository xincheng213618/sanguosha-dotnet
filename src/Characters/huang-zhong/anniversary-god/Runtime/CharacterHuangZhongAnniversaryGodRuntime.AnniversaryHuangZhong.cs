#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZhongAnniversaryGodRuntime
{
    internal const string AnniversaryHzLieqiong = "anniversary-god:lieqiong", AnniversaryHzZhanjue = "anniversary-god:zhanjue";
    internal bool HasAnniversaryHuangZhongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryHuangZhongLieqiong);

    internal static CardConversionSource AnniversaryHzSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IEnumerable<AnniversaryHzBodyIssuedEvent> ActiveAnniversaryHzBody(int target, AnniversaryHzBody body) => !HasAnniversaryHuangZhongCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Where(e => e.Target == target && e.Body == body && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzStatusConsumedEvent>().Any(c => c.Id == e.Id) && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzStatusExpiredEvent>().Any(c => c.Id == e.Id));
    internal bool HasAnniversaryHzPendingInvalidation(int actor) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && ActiveAnniversaryHzBody(actor, AnniversaryHzBody.Zhongshu).Any();
    internal AnniversaryHzZhanjueIssuedEvent? ActiveAnniversaryHzZhanjue(int actor, int mode) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == actor && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualPhaseEndedEvent>().Any(e => e.Phase == phase) ? _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzZhanjueIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == actor && e.Phase == phase && e.Mode == mode && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzZhanjueSpentEvent>().Any(c => c.Id == e.Id)) : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39600)]
    internal bool CanTriggerAnniversaryHz(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId == AnniversaryHzLieqiong)
            return g.WindowContext is { SourceSeat: var source, TargetSeat: { } target } && source == g.OwnerSeat && target != g.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[target].IsAlive;
        if (g.SkillId != AnniversaryHzZhanjue)
            return true;
        if (g.Trigger.Window == SkillProgramTriggerWindow.PlayPhaseStarting)
            return g.Owner.HasHpValue;
        return AnniversaryHzRecoveryReservation(g.OwnerSeat, g.WindowContext?.DamageFrameId)is not null;
    }

    internal AnniversaryHzZhanjueSpentEvent? AnniversaryHzRecoveryReservation(int actor, long? damageId) => damageId is null ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzZhanjueSpentEvent>().LastOrDefault(c => c.Origin == damageId && c.Amount > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzZhanjueIssuedEvent>().Any(e => e.Id == c.Id && e.Mode == 2 && e.Source.OwnerSeat == actor) && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzZhanjueRecoveredEvent>().Any(e => e.IssueId == c.Id));
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryHzHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginAnniversaryHz(f, op);
    internal SkillProgramStepOutcome BeginAnniversaryHz(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryHuangZhong is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Huang Zhong requires his original unexecuted instruction.");
        var r = new AnniversaryHzReceipt(AnniversaryHzSource(f), f.GameplayHash, op, f.InstructionIndex, AnniversaryHzStage.Choose);
        if (op == SkillProgramEffectOp.ResolveAnniversaryHuangZhongLieqiong)
        {
            if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: var source, TargetSeat: { } target } || source != f.OwnerSeat || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Lieqiong requires actual damage dealt to a living other participant.");
            r = r with
            {
                Target = target
            };
        }
        else if (op == SkillProgramEffectOp.ResolveAnniversaryHuangZhongZhanjue)
        {
            if (f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat)
                throw new InvalidOperationException("Zhanjue requires its actual Play phase start.");
            r = r with
            {
                Target = f.OwnerSeat,
                Phase = phase
            };
        }
        else
        {
            var reservation = AnniversaryHzRecoveryReservation(f.OwnerSeat, f.WindowContext?.DamageFrameId) ?? throw new InvalidOperationException("Zhanjue recovery lost its exact reserved first actual damage.");
            r = r with
            {
                Target = f.OwnerSeat,
                Stage = AnniversaryHzStage.Children,
                Amount = reservation.Amount,
                IssueId = reservation.Id,
                DamageId = reservation.Origin
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuangZhong = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzZhanjueRecoveredEvent(f.Id, r.IssueId, r.DamageId, r.Amount));
            RecoverAnniversaryHz(f, r.Amount);
            ContinueAnniversaryHz(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryHuangZhong = r });
        ContinueAnniversaryHz(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool AnniversaryHzAlreadyHit(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Target == target && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
    internal PromptChoice AnniversaryHzChoice(ProgramSkillFrame f, string option, string label) => new(new($"anniversary-hz.{(f.Id)}.{(option)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-huang-zhong", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
    internal IReadOnlyList<PromptChoice> AnniversaryHzChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHuangZhong!;
        if (r.Op == SkillProgramEffectOp.ResolveAnniversaryHuangZhongZhanjue)
            return[AnniversaryHzChoice(f, "slash", "摸体力值张牌，下张杀无距离限制且不可响应"), AnniversaryHzChoice(f, "recover", "摸损失体力值张牌，下次伤害后等量回复")];
        return Enum.GetValues<AnniversaryHzBody>().Where(b => b != AnniversaryHzBody.Tianchong || AnniversaryHzAlreadyHit(f.OwnerSeat, r.Target) && _coreMatchParticipantsPlayersAccess._players[r.Target].HasHpValue && _coreMatchParticipantsPlayersAccess._players[r.Target].Hp > 0).Select(b => AnniversaryHzChoice(f, b.ToString(), b switch
        {
            AnniversaryHzBody.Lifeng => "力烽：随机弃置一半手牌",
            AnniversaryHzBody.Diji => "地机：下次受到伤害+1",
            AnniversaryHzBody.Zhongshu => "中枢：下张使用牌无效",
            AnniversaryHzBody.Qihai => "气海：禁用、禁打出红桃牌",
            _ => "天冲：失去所有体力值"
        })).ToArray();
    }

    internal void PublishAnniversaryHz(ProgramSkillFrame f)
    {
        var choices = AnniversaryHzChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择。", [], [], f.AnniversaryHuangZhong!.Target)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-huang-zhong")]
    internal void AnswerAnniversaryHz(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.AnniversaryHuangZhong ?? throw new InvalidOperationException("Huang Zhong lost his original choice.");
        if (r.Stage != AnniversaryHzStage.Choose || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !AnniversaryHzChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Huang Zhong requires the exact published source choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var option = choice.Parameters["option"];
        if (r.Op == SkillProgramEffectOp.ResolveAnniversaryHuangZhongZhanjue)
        {
            var mode = option == "slash" ? 1 : 2;
            var amount = Math.Max(0, mode == 1 ? _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp : _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            r = r with
            {
                Mode = mode,
                Amount = amount,
                Stage = amount == 0 ? AnniversaryHzStage.Children : AnniversaryHzStage.Drawing
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuangZhong = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzZhanjueIssuedEvent(f.Id, r.Source, r.Phase!, mode, amount));
            if (amount > 0)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new("skill.anniversary-huang-zhong.zhanjue.draw"));
                return;
            }
        }
        else
        {
            var body = Enum.Parse<AnniversaryHzBody>(option);
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            r = r with
            {
                Body = body,
                Stage = body == AnniversaryHzBody.Tianchong ? AnniversaryHzStage.TianchongPayment : AnniversaryHzStage.Children,
                Before = before,
                After = before,
                Amount = body == AnniversaryHzBody.Tianchong ? _coreMatchParticipantsPlayersAccess._players[r.Target].Hp : 0
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuangZhong = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzBodyIssuedEvent(f.Id, r.Source, r.Target, body, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【裂穹】，击伤 {(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)} 的{(option)}。", f.OwnerSeat, r.Target);
            if (body == AnniversaryHzBody.Lifeng)
            {
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]);
                var available = hand.Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(r.Target, c, CardLocation.Hand(r.Target), OwnedCardMoveIntent.Discard)).ToList();
                var selected = new List<Card>();
                var count = Math.Min((hand.Count + 1) / 2, available.Count);
                while (selected.Count < count)
                {
                    var c = available[_randomOperations._random.Next(available.Count)];
                    selected.Add(c);
                    available.Remove(c);
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuangZhong = r with { Cards = selected.Select(c => c.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                if (selected.Count > 0)
                    _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, selected.Select(c => c.Id).ToArray(), new("skill.anniversary-huang-zhong.lifeng"));
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AnniversaryHuangZhong = f.AnniversaryHuangZhong!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            }
        }

        ContinueAnniversaryHz(f.Id);
    }

    internal bool DrainAnniversaryHz(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueAnniversaryHz(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryHuangZhong: { } r } f && f.Id == id)
        {
            if (r.Stage == AnniversaryHzStage.Choose)
            {
                PublishAnniversaryHz(f);
                return;
            }

            if (r.Stage == AnniversaryHzStage.Drawing)
                return;
            if (r.Stage == AnniversaryHzStage.TianchongPayment)
            {
                var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(id, f.SkillId, r.Target, r.Amount, (_, _, _) =>
                {
                    var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(live with { AnniversaryHuangZhong = live.AnniversaryHuangZhong!with { HpPaid = true, Stage = AnniversaryHzStage.TianchongChildren } });
                });
                var dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().LastOrDefault(d => d.ParentFrameId == id && d.VictimSeat == r.Target);
                if (dying is not null)
                {
                    var original = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == id);
                    _coreRuntimeOperations.ReplaceRuntimeFrame(id, original with { AnniversaryHuangZhong = original.AnniversaryHuangZhong!with { DyingId = dying.Id } });
                }

                if (outcome == SkillProgramStepOutcome.AwaitChild)
                    return;
                continue;
            }

            if (DrainAnniversaryHz(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.AnniversaryHuangZhong!;
            if (r.Stage == AnniversaryHzStage.TianchongChildren && r.HpPaid && r.DyingId > 0 && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Any(e => e.ResolutionId == r.DyingId && e.VictimSeat == r.Target && !e.Survived))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuangZhong = r with { Stage = AnniversaryHzStage.Children } });
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryHuangZhong = null, PendingMovementContinuation = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return;
        }
    }

    internal void RecoverAnniversaryHz(ProgramSkillFrame f, int amount)
    {
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryHuangZhongZhanjue)]
    internal void ReturnAnniversaryHzDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryHuangZhong!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryHuangZhong = r with { Stage = AnniversaryHzStage.Children, Before = outcome.SequenceBefore, After = outcome.SequenceAfter } });
        ContinueAnniversaryHz(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 39600)]
    internal bool ResumeAnniversaryHz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHuangZhong: not null } f || f.Id != id)
            return false;
        ContinueAnniversaryHz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39600)]
    internal bool ReturnAnniversaryHzMovement(ProgramSkillFrame f) => f.AnniversaryHuangZhong is not null && ResumeAnniversaryHz(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39600)]
    internal bool ContinueIssuedAnniversaryHz(ProgramSkillFrame f) => f.AnniversaryHuangZhong is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHuangZhongLieqiong)]
    internal PromptChoice ChooseAnniversaryHzBody(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "Tianchong") ?? p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHuangZhongZhanjue)]
    internal PromptChoice ChooseAnniversaryHzDraw(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
