#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5XuncanRuntime
{
    internal const string ShanheS5YunshenSkill = "shanhe:s5-yunshen-3226";
    internal const string ShanheS5ShangshenSkill = "shanhe:s5-shangshen-3227";
    internal const string ShanheS5FenchaiSkill = "shanhe:s5-fenchai-3228";
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5771)]
    internal void RecordShanheS5FirstElementalDamage(DamageAppliedEvent damage)
    {
        if (damage.Amount <= 0 || damage.Nature == DamageNature.Normal || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheS5Xuncan) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5FirstElementalDamageEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber))
            return;
        var native = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault();
        if (native is null || native.SourceSeat != damage.SourceSeat || native.TargetSeat != damage.TargetSeat || native.Nature != damage.Nature)
            throw new InvalidOperationException("The first elemental damage must belong to its real applied damage segment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5FirstElementalDamageEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, native.Id, damage.TargetSeat, damage.Nature));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5771, SkillProgramEffectOp.ResolveShanheS5Xuncan)]
    internal bool CanOfferShanheS5Xuncan(ProgramTriggerLegalityContext c) => c.Trigger.Effects.Any(e => e.StateId == "leijie") || c.Context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: { } damageId } && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5FirstElementalDamageEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.DamageFrameId == damageId);
    internal ShanheS5SkillTargetEvent RequireShanheS5SkillTarget(ProgramSkillFrame f)
    {
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5SkillTargetEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (facts is not [var target] || target.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || target.GameplayHash != f.GameplayHash || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target.TargetSeat) || (f.SkillId == ShanheS5YunshenSkill ? f.SelectedTargetSeats is not [var selected] || selected != target.TargetSeat : f.SkillId != ShanheS5ShangshenSkill || f.WindowContext?.TargetSeat != target.TargetSeat))
            throw new InvalidOperationException("Xuncan lost his original accepted skill target and source instance.");
        return target;
    }

    internal void CaptureShanheS5SkillTarget(ProgramSkillFrame f, int targetSeat)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5SkillTargetEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Xuncan cannot accept the same skill target twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5SkillTargetEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, targetSeat));
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        // Only these printed skills declare a target. Acquiring an unrelated skill
        // never retroactively binds Fenchai to an unrelated character.
        if (owner.Gender == target.Gender || !(owner.General.Skills.Any(s => s.ContentId == f.SkillId) || owner.SecondaryGeneral?.Skills.Any(s => s.ContentId == f.SkillId) == true) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5FenchaiBoundEvent>().Any(e => e.OwnerSeat == f.OwnerSeat && e.FenchaiSkillId == ShanheS5FenchaiSkill))
            return;
        var policy = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.ShanheFenchaiJudgment).FirstOrDefault();
        if (policy.Source is null || policy.Source.SkillId != ShanheS5FenchaiSkill)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5FenchaiBoundEvent(f.Id, f.OwnerSeat, policy.Source.SkillId, policy.Source.SkillInstanceId, targetSeat));
    }

    internal Suit ShanheS5FenchaiJudgmentSuit(JudgmentFrame judgment, Suit currentSuit)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[judgment.TargetSeat];
        if (!_coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.ShanheFenchaiJudgment))
            return currentSuit;
        var bound = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5FenchaiBoundEvent>().SingleOrDefault(e => e.OwnerSeat == owner.Seat && e.FenchaiSkillId == ShanheS5FenchaiSkill);
        if (bound is null)
            return currentSuit;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(bound.TargetSeat) || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5SkillTargetEvent>().Any(e => e.FrameId == bound.ProducerFrameId && e.Source.OwnerSeat == owner.Seat && e.TargetSeat == bound.TargetSeat))
            throw new InvalidOperationException("Fenchai lost its first real opposite-gender skill target.");
        return _coreMatchParticipantsPlayersAccess._players[bound.TargetSeat].IsAlive ? Suit.Heart : Suit.Spade;
    }

    internal SkillProgramStepOutcome ResolveShanheS5Xuncan(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        switch (effect.StateId)
        {
            case "yunshen-bind":
                if (f.InstructionIndex != 1 || f.TriggerId is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    throw new InvalidOperationException("Yunshen requires one real other living Play target.");
                CaptureShanheS5SkillTarget(f, target);
                return SkillProgramStepOutcome.Continue;
            case "yunshen-slash":
                var accepted = RequireShanheS5SkillTarget(f);
                if (f.InstructionIndex != 3 || f.ShanheS5Yunshen is not null)
                    throw new InvalidOperationException("Yunshen cannot repeat its directional Cold Slash.");
                var directions = ShanheS5YunshenDirections(f.OwnerSeat, accepted.TargetSeat);
                if (directions.Length == 0)
                    return SkillProgramStepOutcome.Continue;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Yunshen = new(accepted.Source, f.GameplayHash, accepted.TargetSeat) });
                PublishShanheS5YunshenChoice(f, directions);
                return SkillProgramStepOutcome.AwaitChoice;
            case "shangshen-bind":
                if (f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: { } damage, TargetSeat: { } victim, Amount: > 0 } || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5FirstElementalDamageEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.DamageFrameId == damage && e.TargetSeat == victim))
                    throw new InvalidOperationException("Shangshen requires the first actual elemental segment this turn.");
                CaptureShanheS5SkillTarget(f, victim);
                return SkillProgramStepOutcome.Continue;
            case "shangshen-lightning":
                _ = RequireShanheS5SkillTarget(f);
                var lightning = RequireShanheS5Outcome(f, effect, f.OwnerSeat);
                var hit = lightning.Suit == Suit.Spade && lightning.Rank is >= 2 and <= 9;
                RecordShanheS5JudgmentEffect(f, lightning, effect.StateId, f.OwnerSeat, hit, hit ? 3 : 0);
                return hit && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true) : SkillProgramStepOutcome.Continue;
            case "shangshen-refill":
                var recipient = RequireShanheS5SkillTarget(f).TargetSeat;
                if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
                    return SkillProgramStepOutcome.Continue;
                var count = Math.Max(0, 4 - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).Count);
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5RefillRequestedEvent>().Any(e => e.FrameId == f.Id))
                    throw new InvalidOperationException("Shangshen cannot pay its refill twice.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5RefillRequestedEvent(f.Id, recipient, count));
                return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, count, new("shanhe.s5.shangshen.refill"));
            case "leijie":
                if (f.SelectedTargetSeats is not [var judged])
                    throw new InvalidOperationException("Leijie lost its selected judgment target.");
                var result = RequireShanheS5Outcome(f, effect, judged);
                var struck = result.Suit == Suit.Spade && result.Rank is >= 2 and <= 9;
                RecordShanheS5JudgmentEffect(f, result, effect.StateId, judged, struck, 2);
                if (!_coreMatchParticipantsPlayersAccess._players[judged].IsAlive)
                    return SkillProgramStepOutcome.Continue;
                return struck ? _coreSkillProgramsOperations.BeginProgramSkillDamage(f, judged, 2, nature: DamageNature.Thunder) : _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, judged, 2, new("shanhe.s5.leijie.draw"));
            default:
                throw new InvalidOperationException("Unknown S5 Xuncan stage.");
        }
    }

    internal ProgramJudgmentOutcomeBinding RequireShanheS5Outcome(ProgramSkillFrame f, SkillProgramEffect effect, int target)
    {
        var outcomes = f.JudgmentOutcomeBindings?.Where(o => o.Name == effect.SourceBind).ToArray();
        if (outcomes is not [var outcome] || outcome.TargetSeat != target || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, outcome))
            throw new InvalidOperationException("The S5 judgment lost its exact finalized native outcome.");
        return outcome;
    }

    internal void RecordShanheS5JudgmentEffect(ProgramSkillFrame f, ProgramJudgmentOutcomeBinding outcome, string stage, int target, bool hit, int requested)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5JudgmentEffectEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("An S5 judgment result cannot be applied twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5JudgmentEffectEvent(f.Id, outcome.JudgmentFrameId, stage, target, hit, requested));
    }

    internal int[] ShanheS5YunshenDirections(int owner, int target) => new[]
    {
        owner,
        target
    }.Where(actor => _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[actor == owner ? target : owner], CardKind.ColdSlash, physicalSuit: Suit.None, physicalCardIds: []) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[actor == owner ? target : owner], CardKind.ColdSlash, Suit.None)).ToArray();
    internal void PublishShanheS5YunshenChoice(ProgramSkillFrame f, int[] actors)
    {
        var r = f.ShanheS5Yunshen!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = actors.Select(actor => new PromptChoice(new ChoiceId($"shanhe-yunshen.frame-{(f.Id)}.actor-{(actor)}"), $"{(_coreMatchParticipantsPlayersAccess._players[actor].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[actor == f.OwnerSeat ? r.OriginalTargetSeat : f.OwnerSeat].Name)} 使用冰【杀】", [], [], new Dictionary<string, string> { ["program-action"] = "shanhe-s5-yunshen", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["actor-seat"] = actor.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择熨身的冰【杀】方向。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-s5-yunshen")]
    internal void AnswerShanheS5Yunshen(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheS5Yunshen: { UseId: null, ActorSeat: null } r } f || f.InstructionIndex != 3 || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || !int.TryParse(choice.Parameters.GetValueOrDefault("actor-seat"), out var actor) || !ShanheS5YunshenDirections(f.OwnerSeat, r.OriginalTargetSeat).Contains(actor) || choice.Cards.Count != 0 || choice.Targets.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Yunshen lost its exact native direction choice.");
        _ = RequireShanheS5SkillTarget(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        BeginShanheS5YunshenSlash(f, actor, actor == f.OwnerSeat ? r.OriginalTargetSeat : f.OwnerSeat);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheS5Xuncan)]
    internal PromptChoice SelectAiShanheS5Yunshen(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("actor-seat") == f.OwnerSeat.ToString(System.Globalization.CultureInfo.InvariantCulture)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    internal void BeginShanheS5YunshenSlash(ProgramSkillFrame f, int actorSeat, int targetSeat)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Yunshen cannot overwrite a native attack.");
        var source = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actorSeat, actorSeat, null, null, null, CardKind.ColdSlash, [targetSeat], [], [f.ShanheS5Yunshen!.Source], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Yunshen = f.ShanheS5Yunshen!with { ActorSeat = actorSeat, SlashTargetSeat = targetSeat, UseId = useId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5YunshenIssuedEvent(f.Id, useId, action.ActionId, actorSeat, targetSeat));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actorSeat, 0, CardKind.ColdSlash, [targetSeat], PhysicalCardIds: []) { Action = action, FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(source) });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.ColdSlash, actorSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, [targetSeat]));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actorSeat, CardKind.ColdSlash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actorSeat, CardKind.ColdSlash);
        // Only the actual current Play actor pays its normal quota. The opposite
        // direction is an outside-turn use by the selected role, never Xuncan's debit.
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && actorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.UnlimitedUse != true)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(useId, actorSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, CardKind.ColdSlash);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actorSeat, targetSeat, card: null, damageAmount: 1 + source.AlcoholDamageBonus, playedCardKind: CardKind.ColdSlash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(source, target, CardKind.ColdSlash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, source);
        source.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _corePhaseNamePredictionOperations.ObservePhaseNamePredictionUse(action);
        if (_corePhaseStartControlPoliciesOperations.HasPhaseStartCardUseParticipant(source.Seat))
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.ColdSlash, actorSeat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsShanheS5YunshenUse(long useId) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5YunshenIssuedEvent>().Any(e => e.UseId == useId);

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 5771)]
    internal bool ReturnShanheS5YunshenSlash(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheS5Yunshen: { } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (r.Returned || completion.ProgramFrameId != f.Id || !completion.RequestedSlashChild || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId && e.CardKind == CardKind.ColdSlash) != 1)
            throw new InvalidOperationException("Yunshen lost its exact completed native Cold Slash return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Yunshen = r with { Returned = true } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheS5Xuncan)]
    internal void ReturnShanheS5XuncanDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var request = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5RefillRequestedEvent>().SingleOrDefault(e => e.FrameId == f.Id);
        var judgment = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5JudgmentEffectEvent>().SingleOrDefault(e => e.FrameId == f.Id);
        var target = request?.TargetSeat ?? judgment?.TargetSeat;
        var count = request?.Requested ?? judgment?.Requested;
        if (target != draw.RecipientSeat || count != draw.RequestedCount || f.NativeDraw is not null || request is null && judgment is not { Stage: "leijie", LightningHit: false } || draw.SequenceAfter != _coreCardMovementJournalMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5XuncanDrawPaidEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("The S5 refill lost its exact once-only native Draw return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5XuncanDrawPaidEvent(f.Id, draw.RecipientSeat, draw.RequestedCount, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5771)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5771)]
    internal bool ContinueAcceptedShanheS5Xuncan(ProgramSkillFrame f) => f.SkillId is ShanheS5YunshenSkill or ShanheS5ShangshenSkill && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5SkillTargetEvent>().Any(e => e.FrameId == f.Id) || f.SkillId == "shanhe:s5-leijie-3231" && (_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.ParentFrameId == f.Id && j.Reason == "shanhe.s5.leijie") || f.JudgmentOutcomeBindings?.Any(o => o.Reason == "shanhe.s5.leijie" && _coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, o)) == true) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5JudgmentEffectEvent>().Any(e => e.FrameId == f.Id);
    SkillProgramStepOutcome IShanheS5XuncanHost.ResolveShanheS5Xuncan(ProgramSkillFrame f, SkillProgramEffect e) => ResolveShanheS5Xuncan(f, e);
}
