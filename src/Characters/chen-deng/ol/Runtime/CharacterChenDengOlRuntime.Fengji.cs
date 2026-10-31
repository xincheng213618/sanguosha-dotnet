#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenDengOlRuntime
{
    internal bool TracksFengjiRoundLedger => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.FengjiRoundChoice) == true;

    internal IReadOnlyList<ProgramFengjiOptionChosenEvent> CurrentRoundFengjiOptions(int ownerSeat)
    {
        if (_coreActualRoundProgressRoundNumberAccess._roundNumber == 0)
            return[];
        return[.._coreProgramEventHistoryOperations.ProgramEventHistory<ProgramFengjiOptionChosenEvent>().Where(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.OwnerSeat == ownerSeat)];
    }

    internal FengjiOptionKind? PendingFengjiOption(int ownerSeat)
    {
        var chosen = CurrentRoundFengjiOptions(ownerSeat);
        if (chosen.Any(e => e.Option == FengjiOptionKind.Draw))
            return chosen.Any(e => e.Option == FengjiOptionKind.Slash) ? null : FengjiOptionKind.Slash;
        return FengjiOptionKind.Draw;
    }

    internal bool IsFengjiRoundChoicePending(int ownerSeat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualRoundProgressRoundNumberAccess._roundNumber == 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || _coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra)
            return false;
        var latestRound = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundStartedEvent>().LastOrDefault();
        if (latestRound is not { } round || round.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || round.ActorSeat != _coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat)
            return false;
        return !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramFengjiOptionChosenEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.OwnerSeat == ownerSeat);
    }

    internal SkillProgramStepOutcome FengjiProgramRoundChoice(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualRoundProgressRoundNumberAccess._roundNumber == 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (PendingFengjiOption(active.OwnerSeat)is not { } option)
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return SkillProgramStepOutcome.Continue;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var label = option == FengjiOptionKind.Draw ? "摸牌阶段摸牌数" : "出牌阶段使用【杀】的次数上限";
        PromptChoice Answer(bool accept) => new(new ChoiceId($"fengji-option.frame-{(frame.Id)}.{((option == FengjiOptionKind.Draw ? "draw" : "slash"))}.{((accept ? "yes" : "no"))}"), accept ? $"是：你的{(label)}本轮 -1，并令一名其他角色该项 +2。" : $"否：你的{(label)}本轮 +1。", [], [], new Dictionary<string, string> { ["program-action"] = "fengji-option", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = option == FengjiOptionKind.Draw ? "draw" : "slash", ["accept"] = accept ? "true" : "false" });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择是否令你的{(label)}本轮 -1。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择", "选择“是”后需指定一名其他角色令其对应项数值 +2；选择“否”则你的对应项数值 +1。"),
            Choices = new[]
            {
                Answer(true),
                Answer(false)
            }.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("fengji-option")]
    internal void ResolveFengjiOptionChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Fengji choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.FengjiRoundChoice } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("program-action") != "fengji-option")
            throw new InvalidOperationException("The Fengji choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Fengji chooser changed while suspended.");
        var accept = selected.Parameters.GetValueOrDefault("accept");
        if (accept is not ("true" or "false"))
            throw new InvalidOperationException("The Fengji choice answer is malformed.");
        var option = selected.Parameters.GetValueOrDefault("option") == "draw" ? FengjiOptionKind.Draw : FengjiOptionKind.Slash;
        if (option != PendingFengjiOption(active.OwnerSeat))
            throw new InvalidOperationException("The Fengji option does not match the pending prompt.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (accept == "false")
        {
            CommitFengjiOption(active, option, accepted: false, recipientSeat: active.OwnerSeat);
            ResumeFengjiRoundChoice(active);
            return;
        }

        PresentFengjiRecipientPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), option);
    }

    internal void PresentFengjiRecipientPrompt(ProgramSkillFrame frame, FengjiOptionKind option)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat).Select(player => player.Seat).ToArray();
        if (recipients.Length == 0 || !owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "丰积没有可指定的其他角色，剩余结算取消。");
            return;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var label = option == FengjiOptionKind.Draw ? "摸牌阶段摸牌数" : "出牌阶段使用【杀】的次数上限";
        var choices = recipients.Select(seat => new PromptChoice(new ChoiceId($"fengji-recipient.frame-{(frame.Id)}.{((option == FengjiOptionKind.Draw ? "draw" : "slash"))}.seat-{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的{(label)}本轮 +2。", [], [seat], new Dictionary<string, string> { ["program-action"] = "fengji-recipient", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = option == FengjiOptionKind.Draw ? "draw" : "slash", ["recipient"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择获得“{(label)} +2”的角色。", [], recipients, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择角色", "该角色本轮的对应项数值 +2。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("fengji-recipient")]
    internal void ResolveFengjiRecipientChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Fengji recipient lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.FengjiRoundChoice } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("program-action") != "fengji-recipient" || !int.TryParse(selected.Parameters.GetValueOrDefault("recipient"), System.Globalization.CultureInfo.InvariantCulture, out var recipient))
            throw new InvalidOperationException("The Fengji recipient does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Fengji chooser changed while suspended.");
        var option = selected.Parameters.GetValueOrDefault("option") == "draw" ? FengjiOptionKind.Draw : FengjiOptionKind.Slash;
        if (option != PendingFengjiOption(active.OwnerSeat))
            throw new InvalidOperationException("The Fengji recipient does not match the pending prompt.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (recipient == active.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "丰积的目标已失效，剩余结算取消。");
            return;
        }

        CommitFengjiOption(active, option, accepted: true, recipientSeat: recipient);
        ResumeFengjiRoundChoice(active);
    }

    internal void CommitFengjiOption(ProgramSkillFrame active, FengjiOptionKind option, bool accepted, int recipientSeat)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramFengjiOptionChosenEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, option, accepted, recipientSeat));
        var label = option == FengjiOptionKind.Draw ? "摸牌阶段摸牌数" : "出牌阶段使用【杀】的次数上限";
        _coreMatchLogOperations.AddLog("SkillEffect", accepted ? $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 丰积：{(label)}本轮 -1，{(_coreMatchParticipantsPlayersAccess._players[recipientSeat].Name)} 的该数值本轮 +2。" : $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 丰积：{(label)}本轮 +1。", active.OwnerSeat, accepted ? recipientSeat : active.OwnerSeat);
    }

    internal void ResumeFengjiRoundChoice(ProgramSkillFrame active)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id);
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { } effect)
            return;
        FengjiProgramRoundChoice(frame, effect);
    }
}
