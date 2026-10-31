#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineVirtualBasicOffersRuntime
{

    internal IEnumerable<(CardKind Kind, int Target)> VirtualBasicOptions(int owner, bool anyRegisteredBasic = false)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != owner || _characterZhouYuOlMouOperations.MouGuquPhysicalUseForbidden(owner, []))
            yield break;
        foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(kind => kind != CardKind.ColdSlash || anyRegisteredBasic && _contentRegistryOperations._contentRegistry.Cards.Values.Any(card => card.LegacyKind == CardKind.ColdSlash)))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, t.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, Suit.None)))
                yield return (kind, target.Seat);
        if (_coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner, CardKind.Peach, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, owner, CardKind.Peach))
            yield return (CardKind.Peach, owner);
        if (!actor.HasAlcoholEffect && (!actor.UsedPlayPhaseAlcoholThisTurn || _characterWeiYanMobileShiOperations.HasMobileShiZhuangshiQuota(actor.Seat) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || (_coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor)) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol)) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner, CardKind.Alcohol, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, owner, CardKind.Alcohol))
            yield return (CardKind.Alcohol, owner);
    }

    internal IEnumerable<(CardKind Kind, int Target)> VirtualBasicOptions(ProgramSkillFrame frame) => frame.ZhaoguMirror is not null ? _coreZhaoguMirrorOperations.ZhaoguVirtualBasicOptions(frame) : frame.SkillId == "guandu:choulue" ? _coreGuanduGeneralNativeCardsOperations.GuanduChoulueFrozenBasicKind(frame)is { } kind ? VirtualBasicOptions(frame.OwnerSeat).Where(o => o.Kind == kind) : [] : VirtualBasicOptions(frame.OwnerSeat, anyRegisteredBasic: true);
    internal SkillProgramStepOutcome OfferVirtualBasicCard(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.VirtualBasicDraft is not null)
            throw new InvalidOperationException("A virtual basic offer cannot start twice.");
        var options = VirtualBasicOptions(active).ToArray();
        if (options.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { VirtualBasicDraft = new(active.InstructionIndex) });
        PromptChoice Choice(string option, string label, IReadOnlyList<int> targets) => new(new($"virtual-basic.{(active.Id)}.{(option)}"), label, [], targets, new Dictionary<string, string> { ["program-action"] = "virtual-basic", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["basic-option"] = option });
        var choices = options.Select(o => Choice($"{(o.Kind)}:{(o.Target)}", $"视为对 {(_coreMatchParticipantsPlayersAccess._players[o.Target].Name)} 使用【{(CardCatalog.Get(o.Kind).DisplayName)}】", [o.Target])).Append(Choice("skip", "不使用基本牌", [])).ToArray();
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, active.OwnerSeat, "请选择视为使用的基本牌，或不使用。", [], options.Select(o => o.Target).Distinct().ToArray(), active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = active.OwnerSeat,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(active.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("virtual-basic")]
    internal void ResolveVirtualBasicChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A basic offer lost its program parent.");
        if (frame.VirtualBasicDraft is not { ChildFrameId: null } draft || draft.InstructionIndex != frame.InstructionIndex || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Cards.Count != 0)
            throw new InvalidOperationException("A basic offer lost its exact unpaid chooser.");
        var option = selected.Parameters.GetValueOrDefault("basic-option");
        if (option == "skip")
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { VirtualBasicDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var chosen = VirtualBasicOptions(frame).SingleOrDefault(o => $"{(o.Kind)}:{(o.Target)}" == option);
        if (option != $"{(chosen.Kind)}:{(chosen.Target)}" || !selected.Targets.SequenceEqual(new[] { chosen.Target }))
            throw new InvalidOperationException("This virtual basic use is no longer legal.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { VirtualBasicDraft = draft with { ChildFrameId = id } });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, chosen.Kind, [chosen.Target], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, chosen.Kind, [chosen.Target], PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(frame.Id, draft.InstructionIndex), FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(actor) });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, chosen.Kind, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [chosen.Target]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, chosen.Kind);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, chosen.Kind);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(chosen.Kind))
        {
            if (!(_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse == true))
                _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, chosen.Kind);
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, chosen.Target, null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: chosen.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[chosen.Target], chosen.Kind));
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, chosen.Kind, actor.Seat, chosen.Target));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        }
        else
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, chosen.Kind == CardKind.Peach ? SimpleCardUseEffect.Recovery : SimpleCardUseEffect.Alcohol));
    }

    internal bool ContinueVirtualBasicEffect(long id, ProgramSimpleCardContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.VirtualBasicReturn is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (actor.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, actor.Seat))
        {
            if (use.CardKind == CardKind.Peach)
            {
                var recoveryAmount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(id, actor.Seat, actor.Seat, 1);
                if (!_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(id, actor.Seat, actor.Seat, recoveryAmount, new(RecoveryAttemptProducer.VirtualBasic)))
                {
                    var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(id, actor.Seat, actor.Seat, recoveryAmount);
                    try
                    {
                        actor.Hp = Math.Min(actor.MaxHp, actor.Hp + recoveryAmount);
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(actor.Seat, actor.Seat, recoveryAmount, actor.Hp));
                    }
                    finally
                    {
                        _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
                    }

                    _coreMatchLogOperations.AddLog("Recovered", $"{(actor.Name)} 使用【桃】回复{(recoveryAmount)}点体力，至 {(actor.Hp)}/{(actor.MaxHp)}。", actor.Seat, actor.Seat);
                }
            }
            else
            {
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
                _coreExtraCardEffectRoundsOperations.ApplyExtraCapableAlcoholEffect(id, actor);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholAppliedEvent(id, actor.Seat, 1));
                _coreMatchLogOperations.AddLog("CardEffect", $"{(actor.Name)} 使用【酒】，下一张直接杀造成的伤害 +1。", actor.Seat, actor.Seat);
            }
        }

        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, frame => frame with { VirtualBasicEffectApplied = true });
        FinishVirtualBasicUse(id);
        return true;
    }

    internal void FinishVirtualBasicUse(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("A virtual basic completion lost its card-use parent.");
        if (use.VirtualBasicReturn is null || use.VirtualBasicEffectApplied != true || use.CardId != 0 || use.PhysicalCardIds?.Count != 0 || use.CardKind is not (CardKind.Peach or CardKind.Alcohol) || use.Step == ResolutionFrameStep.Completed)
            throw new InvalidOperationException("A virtual basic completion lost its paid effect marker.");
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.VirtualBasicCardUse, cardKind: use.CardKind))
            return;
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.VirtualBasicCardUse, cardKind: use.CardKind))
            return;
        if (_coreDesignatedExtraTargetsOperations.TryContinueDesignatedVirtualBasicUse(id))
            return;
        if (_coreExtraCardEffectRoundsOperations.TryBeginExtraCardEffectRound(id))
            return;
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(null, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedCard))
            return;
        _coreNativeCardUseCompletionOperations.PopFinishedCardUse(id);
    }

    [NativeUseReturn(190)]
    internal void CompleteVirtualBasicUseReturn(NativeUseReturnContext context) => ReturnVirtualBasicUse(context.BasicReturn, context.Use.Id);
    internal void ReturnVirtualBasicUse(ProgramVirtualBasicReturn? receipt, long childId)
    {
        if (receipt is null)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(receipt.ParentFrameId);
        if (frame.VirtualBasicDraft is not { } draft || draft.ChildFrameId != childId || draft.InstructionIndex != receipt.InstructionIndex || frame.InstructionIndex != receipt.InstructionIndex || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id)
            throw new InvalidOperationException("The virtual basic child lost its exact paid program return.");
        frame = _coreZhaoguMirrorOperations.CompleteZhaoguVirtualBasicReceipt(frame, childId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { VirtualBasicDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
