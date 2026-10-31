#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramVirtualSlashOffersRuntime
{

    internal int[] GetProgramVirtualSlashOfferTargets(int actorSeat) => _coreMatchParticipantsPlayersAccess._players.Where(target => _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && target.IsAlive && target.Seat != actorSeat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(actorSeat, target.Seat) && !_coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, target.Seat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target)).Select(target => target.Seat).ToArray();
    internal SkillProgramStepOutcome OfferProgramVirtualSlashOrDraw(ProgramSkillFrame frame, int actorSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats.Count != 1 || active.SelectedTargetSeats[0] != actorSeat)
            throw new InvalidOperationException("A virtual Slash offer lost its selected actor.");
        if (!_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (GetProgramVirtualSlashOfferTargets(actorSeat).Length == 0)
        {
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[actorSeat], 1, log: true);
            return SkillProgramStepOutcome.Continue;
        }

        SetProgramVirtualSlashOfferPrompt(active, actorSeat, selectingTarget: false);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void SetProgramVirtualSlashOfferPrompt(ProgramSkillFrame frame, int actorSeat, bool selectingTarget)
    {
        PromptChoice Choice(string option, string label, IReadOnlyList<int> targets) => new(new ChoiceId($"program-virtual-slash.frame-{(frame.Id)}.{(option)}"), label, [], targets, new Dictionary<string, string> { ["program-action"] = "virtual-slash-offer", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["offer-option"] = option });
        var choices = selectingTarget ? GetProgramVirtualSlashOfferTargets(actorSeat).Select(seat => Choice($"target-{(seat)}", $"令 {(_coreMatchParticipantsPlayersAccess._players[actorSeat].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 使用【杀】", [seat])).ToArray() : new[]
        {
            Choice("slash", "视为使用一张【杀】", []),
            Choice("draw", "摸一张牌", [])
        };
        var chooserSeat = selectingTarget ? frame.OwnerSeat : actorSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, chooserSeat, selectingTarget ? "请选择【杀】的目标。" : "请选择使用【杀】或摸一张牌。", [], selectingTarget ? GetProgramVirtualSlashOfferTargets(actorSeat) : [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooserSeat,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("virtual-slash-offer")]
    internal void ResolveProgramVirtualSlashOfferChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A virtual Slash offer lost its frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var actorSeat = frame.SelectedTargetSeats.Single();
        var option = selected.Parameters.GetValueOrDefault("offer-option");
        var selectingTarget = option?.StartsWith("target-", StringComparison.Ordinal) == true;
        if (effect.Op != SkillProgramEffectOp.OfferVirtualSlashOrDraw || selected.Cards.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (selectingTarget ? frame.OwnerSeat : actorSeat))
            throw new InvalidOperationException("The virtual Slash answer changed its frozen actor or instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "参与者或技能实例已失效。");
            return;
        }

        var targets = GetProgramVirtualSlashOfferTargets(actorSeat);
        if (option == "draw" || targets.Length == 0)
        {
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[actorSeat], 1, log: true);
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (option == "slash" && selected.Targets.Count == 0)
        {
            SetProgramVirtualSlashOfferPrompt(frame, actorSeat, selectingTarget: true);
            return;
        }

        if (!selectingTarget || selected.Targets.Count != 1 || !targets.Contains(selected.Targets[0]) || option != $"target-{(selected.Targets[0])}")
            throw new InvalidOperationException("The virtual Slash target is no longer legal.");
        BeginOfferedProgramVirtualSlash(frame, actorSeat, selected.Targets[0]);
    }

    internal void BeginOfferedProgramVirtualSlash(ProgramSkillFrame frame, int actorSeat, int targetSeat)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A virtual Slash cannot overwrite an attack.");
        var source = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actorSeat, actorSeat, null, null, null, CardKind.Slash, [targetSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, actorSeat, 0, CardKind.Slash, [targetSeat], PhysicalCardIds: []) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Slash, actorSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, [targetSeat]));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, actorSeat, targetSeat, card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, CardKind.Slash), programSkillCardUseFrameId: frame.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(resolutionId, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actorSeat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(resolutionId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void GrantProgramTurnCardEffectImmunity(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<CardKind> cardKinds)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (targetSeat != frame.OwnerSeat || cardKinds.Count == 0 || cardKinds.Any(kind => !GrantTurnCardEffectImmunityProgramOperationDescriptor.CardEffectImmunityKinds.Contains(kind)))
            throw new InvalidOperationException("The turn immunity grant has unsupported kinds or subject.");
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), SkillRuleQuery.CardEffectImmunity, SkillRuleOperation.Set, 1, cardKinds);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(granted));
    }
}
