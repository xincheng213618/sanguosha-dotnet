#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePaidHpLossProgramsRuntime
{
    internal SkillProgramStepOutcome BeginHpLossQuantity(ProgramSkillFrame frame, string bind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (active.HpLossQuantity is not null || active.PaidHpLossReceipt is not null || !owner.IsAlive || owner.Hp <= 0 || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != owner.Seat)
            throw new InvalidOperationException("HP quantity requires its unpaid owner activation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { HpLossQuantity = new(active.InstructionIndex, bind, owner.Seat, owner.Hp, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = Enumerable.Range(1, owner.Hp).Select(n => new PromptChoice(new ChoiceId($"hp-loss.frame-{(frame.Id)}.{(n)}"), $"失去{(n)}点体力", [], [], new Dictionary<string, string> { { "program-action", "hp-loss-quantity" }, { "frame-id", frame.Id.ToString(CultureInfo.InvariantCulture) }, { "result-bind", bind }, { "quantity", n.ToString(CultureInfo.InvariantCulture) } })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, owner.Seat, $"【{(skill.Name)}】请选择失去的体力。", [], [], owner.Seat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = owner.Seat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 选择体力", skill.Description),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("hp-loss-quantity")]
    internal void ResolveHpLossQuantity(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The quantity lost its frame.");
        var d = f.HpLossQuantity ?? throw new InvalidOperationException("The quantity lost its draft.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != f.OwnerSeat || choice.Cards.Count != 0 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("result-bind") != d.ResultBind || !int.TryParse(choice.Parameters.GetValueOrDefault("quantity"), NumberStyles.None, CultureInfo.InvariantCulture, out var n) || n < 1 || n > d.Maximum)
            throw new InvalidOperationException("The quantity does not match its owning prompt.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || owner.Hp != d.Maximum || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber != d.TurnNumber || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != d.TurnOwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "技能实例或体力在付款前失效，剩余步骤取消。");
            return;
        }

        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var before = owner.Hp;
        owner.Hp = Math.Max(0, before - n);
        // Freeze actual physical HP apply before any fact, rule callback or child.
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HpLossQuantity = null, PaidHpLossReceipt = new(d.InstructionIndex, d.ResultBind, f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, plan.Activation!.UsageGroup, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, n, before, owner.Hp, before - owner.Hp) });
        _coreHpChangeProgramsOperations.RecordHpChange(f.Id, null, f.OwnerSeat, before, owner.Hp, HpChangeKind.Loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(f.Id, f.SkillId, f.OwnerSeat, before - owner.Hp, owner.Hp));
        if (owner.Hp == 0)
            _coreSkillProgramsOperations.BeginProgramSkillDying(f.Id, owner);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseOwnerHpLoss)]
    internal PromptChoice SelectAiHpLossQuantity(PendingDecision d)
    {
        var wanted = Math.Min(2, Math.Max(1, _coreMatchParticipantsPlayersAccess._players[d.PlayerSeat].Hp - 1));
        return d.Choices.Single(c => c.Parameters.GetValueOrDefault("quantity") == wanted.ToString(CultureInfo.InvariantCulture));
    }

    internal bool HasValidPaidHpLossReceipt(ProgramSkillFrame f, ProgramExecutionPlan plan)
    {
        var p = f.PaidHpLossReceipt;
        return p is not null && PaidHpLossProgram.IsExactChain(plan.Instructions) && plan.Activation is { UsesPerGame: 1, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0 } a && f.TriggerId is null && f.WindowContext is null && f.HpLossQuantity is null && p.InstructionIndex == 1 && p.ResultBind == plan.Instructions[0].ResultBind && p.OwnerSeat == f.OwnerSeat && p.SkillId == f.SkillId && p.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) && p.SkillInstanceId == f.SkillInstanceId && p.GameplayHash == f.GameplayHash && p.UsageGroup == a.UsageGroup && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.TurnOwnerSeat == f.OwnerSeat && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, a.UsageGroup, SkillUsageScope.Game) == 1 && p.Requested > 0 && p.Requested <= p.HpBefore && p.HpAfter == p.HpBefore - p.Requested && p.HpAfter >= 0 && p.ActualLost == p.HpBefore - p.HpAfter && p.ActualLost > 0 && f.InstructionIndex is >= 1 and <= 4 && HasExactPaidHpLossGrant(f, p, p.DistanceGrantSequence, 2, SkillRuleQuery.OutgoingDistance, -p.ActualLost) && HasExactPaidHpLossGrant(f, p, p.SlashGrantSequence, 3, SkillRuleQuery.SlashLimit, p.ActualLost) && (!p.DrawIssued || f.InstructionIndex >= 2) && (!p.DrawReturned || p.DrawIssued) && (p.DistanceGrantSequence is null || p.DrawReturned && f.InstructionIndex >= 3) && (p.SlashGrantSequence is null || p.DistanceGrantSequence is not null && f.InstructionIndex == 4);
    }

    internal bool HasExactPaidHpLossGrant(ProgramSkillFrame f, ProgramPaidHpLossReceipt p, long? sequence, int index, SkillRuleQuery query, int amount)
    {
        if (sequence is null)
            return !_turnCardUseEffectsOperations._turnCardUseEffects.RuleModifiers.Any(m => m.ParentFrameId == f.Id && m.EffectIndex == index);
        var m = _turnCardUseEffectsOperations._turnCardUseEffects.RuleModifiers.SingleOrDefault(m => m.GrantSequence == sequence);
        return m is not null && m.ParentFrameId == f.Id && m.EffectIndex == index && m.TurnNumber == p.TurnNumber && m.TurnSeat == p.TurnOwnerSeat && m.Source == global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f) && m.Query == query && m.Operation == SkillRuleOperation.Add && m.Amount == amount && m.AffectedSeat is null && (m.CardKinds?.Count ?? 0) == 0 && m.PaidHpLossOrigin == CreatePaidHpLossModifierOrigin(f, p);
    }

    internal static ProgramHpLossModifierOrigin CreatePaidHpLossModifierOrigin(ProgramSkillFrame f, ProgramPaidHpLossReceipt p) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_PaidHpLossPrograms.CreatePaidHpLossModifierOrigin(f, p);
    internal void AssertPaidHpLossModifiers()
    {
        foreach (var m in _turnCardUseEffectsOperations._turnCardUseEffects.RuleModifiers.Where(m => m.PaidHpLossOrigin is not null))
        {
            var p = m.PaidHpLossOrigin!;
            var program = _contentRegistryOperations._contentRegistry.GetSkill(p.SkillId).Program;
            var activation = program?.Activations.SingleOrDefault(a => a.Id == p.BindingId);
            if (!PaidHpLossProgram.IsValidModifierOrigin(m) || program?.GameplayHash != p.GameplayHash || activation is not { UsesPerGame: 1, MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0 } || !PaidHpLossProgram.IsExactChain(activation.Effects) || activation.Effects[0].ResultBind != p.ResultBind || activation.UsageGroup != p.UsageGroup || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(p.OwnerSeat, p.SkillId, p.UsageGroup, SkillUsageScope.Game) != 1 || p.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || p.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
                throw new InvalidOperationException("A paid HP loss modifier lost its frozen real producer and turn.");
        }
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 770)]
    internal bool CanContinueIssuedHpLoss(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.PaidHpLossReceipt is null)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        return HasValidPaidHpLossReceipt(f, plan);
    }

    internal SkillProgramStepOutcome DrawProgramPaidHpLoss(ProgramSkillFrame f, string bind)
    {
        var a = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var p = a.PaidHpLossReceipt ?? throw new InvalidOperationException("Actual HP loss receipt missing.");
        var plan = ProgramInstructionResolver.Default.Resolve(a, _contentRegistryOperations._contentRegistry.GetSkill(a.SkillId).Program!);
        if (!HasValidPaidHpLossReceipt(a, plan) || a.InstructionIndex != 2 || p.ResultBind != bind || p.DrawIssued)
            throw new InvalidOperationException("HP loss Draw is not its exact once-paid tail.");
        _coreRuntimeOperations.ReplaceRuntimeTop(a with { PaidHpLossReceipt = p with { DrawIssued = true } });
        if (plan.Instructions[1].PaidHpLossDrawCards != false)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, p.ActualLost, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
        a = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(a with { PaidHpLossReceipt = a.PaidHpLossReceipt!with { DrawReturned = true } });
        return SkillProgramStepOutcome.Continue;
    }

    internal void GrantProgramPaidHpLoss(ProgramSkillFrame f, string bind, bool distance)
    {
        var a = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var p = a.PaidHpLossReceipt ?? throw new InvalidOperationException("Actual HP loss receipt missing.");
        var plan = ProgramInstructionResolver.Default.Resolve(a, _contentRegistryOperations._contentRegistry.GetSkill(a.SkillId).Program!);
        if (!HasValidPaidHpLossReceipt(a, plan) || p.ResultBind != bind || !p.DrawReturned || a.InstructionIndex != (distance ? 3 : 4) || (distance ? p.DistanceGrantSequence is not null : p.DistanceGrantSequence is null || p.SlashGrantSequence is not null))
            throw new InvalidOperationException("HP loss grant is not its exact issued tail.");
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(a);
        var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, a.Id, a.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(a), distance ? SkillRuleQuery.OutgoingDistance : SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, distance ? -p.ActualLost : p.ActualLost, hpLossOrigin: CreatePaidHpLossModifierOrigin(a, p));
        _coreRuntimeOperations.ReplaceRuntimeTop(a with { PaidHpLossReceipt = distance ? p with { DistanceGrantSequence = grant.GrantSequence } : p with { SlashGrantSequence = grant.GrantSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 40)]
    internal void AssertPaidHpLossState(ProgramSkillFrame f, ProgramExecutionPlan plan)
    {
        if (f.PaidHpLossReceipt is not null && !HasValidPaidHpLossReceipt(f, plan))
            throw new InvalidOperationException("Actual HP loss receipt lost its exact owner, source, turn or stage.");
        if (f.HpLossQuantity is { } d)
        {
            if (!PaidHpLossProgram.IsExactChain(plan.Instructions) || plan.Activation?.UsesPerGame != 1 || f.InstructionIndex != 1 || d.InstructionIndex != 1 || f.PaidHpLossReceipt is not null || d.OwnerSeat != f.OwnerSeat || d.Maximum <= 0 || d.ResultBind != plan.Instructions[0].ResultBind || d.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || d.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || d.TurnOwnerSeat != f.OwnerSeat || !ReferenceEquals(f, _resolutionStackOperations._resolutionStack.LastOrDefault()) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || p.Choices.Count != d.Maximum || p.Choices.Where((c, i) => c.Cards.Count != 0 || c.Targets.Count != 0 || c.Parameters.GetValueOrDefault("program-action") != "hp-loss-quantity" || c.Parameters.GetValueOrDefault("quantity") != (i + 1).ToString(CultureInfo.InvariantCulture) || c.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || c.Parameters.GetValueOrDefault("result-bind") != d.ResultBind).Any())
                throw new InvalidOperationException("The HP quantity lost its exact finite owning prompt.");
        }
    }
}
