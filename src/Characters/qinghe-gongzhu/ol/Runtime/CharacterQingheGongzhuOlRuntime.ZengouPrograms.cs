#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQingheGongzhuOlRuntime
{
    internal SkillProgramStepOutcome ZengouProgramNullifyDodge(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || frame.WindowContext is not { CardUse: { } cardUse, TargetSeat: { } dodgerSeat } || dodgerSeat == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[dodgerSeat].IsAlive || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(active.OwnerSeat, dodgerSeat))
            return SkillProgramStepOutcome.Continue;
        var attack = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == cardUse.ParentCardUseFrameId);
        if (attack is null || attack.TargetSeats.Count != 1 || (attack.Enhancements & CurrentCardEnhancement.Uncancelable) != 0)
            return SkillProgramStepOutcome.Continue;
        var dodge = LatestSettledDodgeEntity();
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = new List<PromptChoice>();
        foreach (var card in NonBasicOwnedCards(active.OwnerSeat))
        {
            if (dodge is null)
                break;
            choices.Add(new PromptChoice(new ChoiceId($"zengou-cost.frame-{(frame.Id)}.card-{(card.Id)}"), $"弃置【{(card.DisplayName)}】。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "zengou-cost", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["cost"] = "discard", ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        if (owner.Hp > 0)
            choices.Add(new PromptChoice(new ChoiceId($"zengou-cost.frame-{(frame.Id)}.lose-hp"), "失去1点体力。", [], [], new Dictionary<string, string> { ["program-action"] = "zengou-cost", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["cost"] = "loseHp" }));
        if (choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择支付方式，令此【闪】无效并获得之。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = dodgerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 令【闪】无效", $"{(_coreMatchParticipantsPlayersAccess._players[dodgerSeat].Name)} 使用的【闪】结算完成，你可以支付代价令其无效并获得之。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("zengou-cost")]
    internal void ResolveZengouCostChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zengou cost choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZengouNullifyDodge } || selected.Parameters.GetValueOrDefault("program-action") != "zengou-cost" || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Zengou cost choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Zengou cost chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (active.WindowContext is not { CardUse: { } cardUse, TargetSeat: { } dodgerSeat } || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[dodgerSeat].IsAlive || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(active.OwnerSeat, dodgerSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "谮构的目标已失效，剩余结算取消。");
            return;
        }

        var attack = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == cardUse.ParentCardUseFrameId);
        var dodge = LatestSettledDodgeEntity();
        if (attack is null || attack.TargetSeats.Count != 1 || (attack.Enhancements & CurrentCardEnhancement.Uncancelable) != 0 || dodge is null)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "谮构的【闪】结算已变化，剩余结算取消。");
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var cost = selected.Parameters.GetValueOrDefault("cost");
        int? paidCardId = null;
        if (cost == "discard")
        {
            if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var parsedCardId) || NonBasicOwnedCards(active.OwnerSeat).All(card => card.Id != parsedCardId))
                throw new InvalidOperationException("The Zengou cost card is no longer available.");
            paidCardId = parsedCardId;
            var paidCard = NonBasicOwnedCards(active.OwnerSeat).Single(card => card.Id == parsedCardId);
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(active.OwnerSeat, active.OwnerSeat, [paidCard], _cardZonesOperations._cardZones.GetLocation(paidCard.Id), new($"skill-program.{(active.SkillId)}.zengou-cost"));
        }
        else if (cost == "loseHp")
        {
            var before = owner.Hp;
            owner.Hp = Math.Max(0, before - 1);
            _coreHpChangeProgramsOperations.RecordHpChange(active.Id, null, active.OwnerSeat, before, owner.Hp, HpChangeKind.Loss);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(active.Id, active.SkillId, active.OwnerSeat, before - owner.Hp, owner.Hp));
        }
        else
        {
            throw new InvalidOperationException($"The Zengou cost '{(cost)}' is unsupported.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(attack.Id, attack with { Enhancements = attack.Enhancements | CurrentCardEnhancement.Uncancelable, EnhancementOwnerSeat = active.OwnerSeat });
        _corePhysicalMovementFacadesOperations.MoveCard(dodge, CardLocation.DiscardPile, CardLocation.Hand(active.OwnerSeat), new($"skill-program.{(active.SkillId)}.zengou-gain"));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】：" + (cost == "discard" ? $"弃置一张非基本牌" : "失去1点体力") + $"，令 {(_coreMatchParticipantsPlayersAccess._players[dodgerSeat].Name)} 使用的【闪】无效，并获得之。", active.OwnerSeat, dodgerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramZengouNullifiedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, dodgerSeat, cost, paidCardId, dodge.Id));
        if (owner.Hp == 0)
        {
            _coreSkillProgramsOperations.BeginProgramSkillDying(active.Id, owner);
            return;
        }

        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
