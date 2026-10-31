#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKaiOlRuntime
{
    internal SkillProgramStepOutcome TunanProgramUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var user] || user == active.OwnerSeat)
            throw new InvalidOperationException("Tunan lost its selected counterpart.");
        var binding = active.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind!);
        var card = ReadTunanRevealedCard(binding);
        if (!_coreMatchParticipantsPlayersAccess._players[user].IsAlive || card is null)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), card is null ? "图南亮出的牌已离开处理区，剩余结算取消。" : "图南的用牌角色已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var canUse = card.Kind == CardKind.Slash || card.Kind == CardKind.Peach && !_coreCardUseModulesOperations.IsCardUseForbidden(user, CardKind.Peach, CardActionType.Use) || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind);
        var slashVictims = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != user && _coreNativeRuleQueriesOperations.GetSeatDistance(user, player.Seat) <= _coreNativeRuleQueriesOperations.GetAttackRange(user)).Select(player => player.Seat).ToArray();
        if (!canUse && slashVictims.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "图南没有可行的使用方式，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var choices = new List<PromptChoice>();
        if (canUse)
            choices.Add(new PromptChoice(new ChoiceId($"tunan-branch.frame-{(frame.Id)}.use"), $"使用【{(card.DisplayName)}】（无距离限制）。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "tunan-branch", ["branch"] = "use", ["source-bind"] = effect.SourceBind! }));
        if (slashVictims.Length > 0)
            choices.Add(new PromptChoice(new ChoiceId($"tunan-branch.frame-{(frame.Id)}.slash"), $"将【{(card.DisplayName)}】当【杀】使用（正常距离）。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "tunan-branch", ["branch"] = "slash", ["source-bind"] = effect.SourceBind! }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, user, $"【{(presentation.Name)}】请选择使用亮出之牌的方式。", [card.Id], [], frame.WindowContext?.SourceSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = user,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{(presentation.Name)} · 选择使用方式", "将亮出的牌按所选方式使用。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[user].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("tunan-branch")]
    internal void ResolveTunanBranchChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Tunan branch choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.TunanUseRevealedCard } effect || selected.Parameters.GetValueOrDefault("source-bind") != effect.SourceBind)
            throw new InvalidOperationException("The Tunan branch choice does not match the suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var user])
            throw new InvalidOperationException("Tunan lost its selected counterpart.");
        var card = ReadTunanRevealedCard(active.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind!)) ?? throw new InvalidOperationException("The Tunan revealed card left the processing zone.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters.GetValueOrDefault("branch");
        if (branch == "use" && card.Kind == CardKind.Peach)
        {
            ExecuteTunanPeachUse(frame, card, user);
            return;
        }

        if (branch == "use" && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
        {
            ExecuteTunanEquipmentUse(frame, card, user);
            return;
        }

        // Both slash shapes pick their victim in a follow-up prompt; the real
        // use ignores distance, the converted Slash keeps the normal distance.
        var unlimited = branch == "use";
        var victims = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != user && (unlimited || _coreNativeRuleQueriesOperations.GetSeatDistance(user, player.Seat) <= _coreNativeRuleQueriesOperations.GetAttackRange(user))).Select(player => player.Seat).ToArray();
        if (victims.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "图南的杀没有合法目标，剩余结算取消。");
            return;
        }

        var presentation = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = victims.Select(victim => new PromptChoice(new ChoiceId($"tunan-target.frame-{(frame.Id)}.seat-{(victim)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 使用【杀】。", [], [victim], new Dictionary<string, string> { ["program-action"] = "tunan-target", ["branch"] = branch ?? string.Empty, ["source-bind"] = effect.SourceBind!, ["victim"] = victim.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, user, $"【{(presentation.Name)}】请选择【杀】的目标。", [], victims, frame.WindowContext?.SourceSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = user,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{(presentation.Name)} · 选择目标", unlimited ? "此【杀】无距离限制。" : "此【杀】按正常距离规则。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[user].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("tunan-target")]
    internal void ResolveTunanTargetChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Tunan target choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.TunanUseRevealedCard } effect || selected.Parameters.GetValueOrDefault("source-bind") != effect.SourceBind)
            throw new InvalidOperationException("The Tunan target choice does not match the suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var user] || !int.TryParse(selected.Parameters.GetValueOrDefault("victim"), System.Globalization.CultureInfo.InvariantCulture, out var victim))
            throw new InvalidOperationException("Tunan lost its selected counterpart or victim.");
        var card = ReadTunanRevealedCard(active.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind!)) ?? throw new InvalidOperationException("The Tunan revealed card left the processing zone.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var asSlash = selected.Parameters.GetValueOrDefault("branch") == "slash";
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, user, [victim], playedCardKind: asSlash ? CardKind.Slash : null, conversionSource: asSlash ? new CardConversionSource(active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), user, active.SkillInstanceId) : null);
        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Last(use => use.Id == resolutionId).Action ?? throw new InvalidOperationException("The Tunan slash lost its action context.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, user, victim, card: asSlash ? null : card, damageAmount: 1, playedCardKind: CardKind.Slash, physicalCards: asSlash ? null : [card], programSkillCardUseFrameId: frame.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreMatchLogOperations.AddLog("CardUsed", $"{(_coreMatchParticipantsPlayersAccess._players[user].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 使用【杀】。", user, victim);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(asSlash ? 0 : card.Id, CardKind.Slash, user, victim));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, [victim], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void ExecuteTunanPeachUse(ProgramSkillFrame frame, Card card, int user)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_coreCardUseModulesOperations.IsCardUseForbidden(user, CardKind.Peach, CardActionType.Use))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "图南的实际用牌角色已不能使用桃，剩余结算取消。");
            return;
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, user, [user]);
        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(card.Id, SimpleCardUseEffect.Recovery, 1, []));
    }

    internal void ExecuteTunanEquipmentUse(ProgramSkillFrame frame, Card card, int user)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = new ProgramMovementContinuation(frame.OwnerSeat, 0, null) });
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, user, []);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(_coreMatchParticipantsPlayersAccess._players[user], card, resolutionId))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(_coreMatchParticipantsPlayersAccess._players[user], card, resolutionId);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame)
        {
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        }
    }

    internal Card? ReadTunanRevealedCard(ProgramSkillCardSetBinding? binding)
    {
        if (binding is null || binding.CardIds.Count != 1)
            return null;
        var cardId = binding.CardIds[0];
        return _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(card => card.Id == cardId);
    }
}
