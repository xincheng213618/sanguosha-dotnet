#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineKuangfuRuntime
{
    internal const string KuangfuEquipmentReason = "skill-program.kuangfu.equipment-cost";
    internal const string KuangfuHandReason = "skill-program.kuangfu.no-damage-hand-cost";
    internal const string KuangfuDrawReason = "skill-program.kuangfu.own-equipment-draw";
    internal static CardConversionSource KuangfuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_Kuangfu.KuangfuSource(f);
    internal bool KuangfuGameEnded() => _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed;
    internal bool KuangfuUnpaidSourceCurrent(ProgramSkillFrame f) => !KuangfuGameEnded() && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f);
    internal bool CanIssueKuangfuSlash(int actor, int target, int? excludedEquipmentId = null) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && actor != target && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !KuangfuGameEnded() && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor, target, Suit.None) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], ignoresCount: false, CardKind.Slash, effectiveRank: null, excludedEquipmentId: excludedEquipmentId);
    internal IEnumerable<(int Owner, Card Card)> KuangfuEquipment(int actor) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Select(c => (Owner: p.Seat, Card: c))).Where(x => !x.Card.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, x.Card, CardLocation.Equipment(x.Owner), OwnedCardMoveIntent.Discard) && _coreMatchParticipantsPlayersAccess._players.Any(t => CanIssueKuangfuSlash(actor, t.Seat, x.Owner == actor ? x.Card.Id : null)));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 80, SkillProgramEffectOp.DiscardEquipmentThenSlashAndOwnershipOutcome)]
    internal bool CanStartKuangfu(CharacterState owner) => KuangfuEquipment(owner.Seat).Any() && _coreMatchParticipantsPlayersAccess._players.Any(p => CanIssueKuangfuSlash(owner.Seat, p.Seat));
    internal SkillProgramStepOutcome BeginKuangfu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Kuangfu is not null || f.InstructionIndex != 1 || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("An equipment Slash needs its own clean zero-input activation.");
        if (!KuangfuUnpaidSourceCurrent(f) || !CanStartKuangfu(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
            return SkillProgramStepOutcome.Continue;
        var source = KuangfuSource(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = new() { InstructionIndex = f.InstructionIndex, Source = source, GameplayHash = f.GameplayHash, ActualTurn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, Stage = KuangfuStage.EquipmentChoice } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuStartedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
        PublishKuangfu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> KuangfuChoices(ProgramSkillFrame f)
    {
        var r = f.Kuangfu!;
        var choices = new List<PromptChoice>();
        void Add(string token, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => choices.Add(new(new($"kuangfu.{(f.Id)}.{(token)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "kuangfu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["kuangfu-action"] = token }));
        if (r.Stage == KuangfuStage.EquipmentChoice)
            foreach (var x in KuangfuEquipment(f.OwnerSeat).OrderBy(x => x.Owner).ThenBy(x => x.Card.Id))
                Add("equipment-" + x.Card.Id, $"弃置 {(_coreMatchParticipantsPlayersAccess._players[x.Owner].Name)} 装备区的【{(x.Card.DisplayName)}】", [x.Card.Id], [x.Owner]);
        else if (r.Stage == KuangfuStage.SlashTargetChoice)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => CanIssueKuangfuSlash(f.OwnerSeat, p.Seat)))
                Add("slash-" + target.Seat, "对 " + target.Name + " 使用无距离限制的杀", [], [target.Seat]);
        else if (r.Stage == KuangfuStage.HandDiscardChoice)
            foreach (var c in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.SelectedHandIds.Contains(c.Id)).OrderBy(c => c.Id))
                Add("hand-" + c.Id, "弃置手牌【" + c.DisplayName + "】", [c.Id], []);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishKuangfu(ProgramSkillFrame f)
    {
        if (KuangfuGameEnded())
            return;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.Kuangfu!.Stage == KuangfuStage.EquipmentChoice && !KuangfuUnpaidSourceCurrent(f))
        {
            FinishKuangfu(f);
            return;
        }

        var choices = KuangfuChoices(f);
        if (choices.Count == 0)
        {
            FinishKuangfu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var r = f.Kuangfu!;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == KuangfuStage.HandDiscardChoice ? $"狂斧：选择 {(r.DiscardRequired)} 张手牌弃置。" : skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == KuangfuStage.HandDiscardChoice,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("kuangfu")]
    internal void ResolveKuangfuChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !_coreKuangfuReceiptsOperations.ValidKuangfu(f) || f.Kuangfu is not { Stage: KuangfuStage.EquipmentChoice or KuangfuStage.SlashTargetChoice or KuangfuStage.HandDiscardChoice } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || p.IsPrivate != (r.Stage == KuangfuStage.HandDiscardChoice) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], KuangfuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Equipment Slash choice lost its owning frame and exact published material.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (KuangfuGameEnded())
            return;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishKuangfu(f);
            return;
        }

        if (r.Stage == KuangfuStage.EquipmentChoice)
        {
            // Check an actual use before any irreversible cost, not merely a virtual card offer.
            if (!KuangfuUnpaidSourceCurrent(f) || !_coreMatchParticipantsPlayersAccess._players.Any(t => CanIssueKuangfuSlash(f.OwnerSeat, t.Seat)))
            {
                FinishKuangfu(f);
                return;
            }

            var entity = KuangfuEquipment(f.OwnerSeat).Single(x => x.Card.Id == choice.Cards.Single() && choice.Targets.SequenceEqual([x.Owner]));
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = r with { Stage = KuangfuStage.EquipmentChildren, EquipmentOwnerSeat = entity.Owner, EquipmentCardId = entity.Card.Id, EquipmentKind = entity.Card.Kind, PaymentBefore = before, PaymentAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [entity.Card.Id], new(KuangfuEquipmentReason), (_, records) =>
            {
                foreach (var movement in records)
                    _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id);
            });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = f.Kuangfu!with { PaymentAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuEquipmentPaidEvent(f.Id, entity.Owner, entity.Card.Id, entity.Card.Kind, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == KuangfuStage.SlashTargetChoice)
        {
            IssueKuangfuSlash(f, choice.Targets.Single());
            return;
        }

        var ids = r.SelectedHandIds.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = r with { SelectedHandIds = ids } });
        if (ids.Length < r.DiscardRequired)
        {
            PublishKuangfu(f);
            return;
        }

        var costBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = f.Kuangfu!with { Stage = KuangfuStage.OutcomeChildren, SelectedHandIds = [], DiscardedHandIds = ids, OutcomeBefore = costBefore, OutcomeAfter = costBefore }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(KuangfuHandReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = f.Kuangfu!with { OutcomeAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuHandDiscardPaidEvent(f.Id, ids, costBefore, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainKuangfu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 230)]
    internal bool ResumeKuangfu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.Kuangfu is not { } r)
            return false;
        if (!_coreKuangfuReceiptsOperations.ValidKuangfu(f))
            throw new InvalidOperationException("Equipment Slash lost its original paid instruction.");
        if (KuangfuGameEnded())
            return true;
        if (r.Stage is KuangfuStage.EquipmentChoice or KuangfuStage.SlashTargetChoice or KuangfuStage.HandDiscardChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishKuangfu(f);
            return true;
        }

        if (r.Stage == KuangfuStage.SlashIssued)
            throw new InvalidOperationException("An issued equipment Slash needs its exact typed completed-use return.");
        if (DrainKuangfu(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Kuangfu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage == KuangfuStage.OutcomeChildren)
        {
            FinishKuangfu(f);
            return true;
        }

        // A paid equipment's children may change all legal targets or quota. Preserve the paid receipt and close, never repay or leave a dead prompt.
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = r with { Stage = KuangfuStage.SlashTargetChoice } });
        PublishKuangfu(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 250)]
    internal bool ReturnKuangfuMovement(ProgramSkillFrame f)
    {
        if (f.Kuangfu is not { Stage: KuangfuStage.EquipmentChildren or KuangfuStage.OutcomeChildren })
            return false;
        if (f.PendingMovementContinuation is null || !_coreKuangfuReceiptsOperations.ValidKuangfu(f))
            throw new InvalidOperationException("An equipment Slash movement returned to a different invoice.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal void IssueKuangfuSlash(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.Kuangfu!;
        if (r.Stage != KuangfuStage.SlashTargetChoice || !_coreKuangfuReceiptsOperations.ValidKuangfu(f))
            throw new InvalidOperationException("A paid equipment Slash cannot issue twice.");
        if (!CanIssueKuangfuSlash(f.OwnerSeat, targetSeat))
        {
            PublishKuangfu(f);
            return;
        }

        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("An equipment Slash cannot overwrite its payment child.");
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        var returned = new KuangfuSlashReturn(f.Id, f.InstructionIndex, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwnerSeat, r.EquipmentOwnerSeat!.Value, r.EquipmentCardId!.Value, useId, action.ActionId, targetSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SelectedTargetSeats = [targetSeat], Kuangfu = r with { Stage = KuangfuStage.SlashIssued, SlashReturn = returned } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, [targetSeat], PhysicalCardIds: []) { Action = action, KuangfuSlashReturn = returned, FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(actor) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuSlashIssuedEvent(returned));
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, [targetSeat]));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        // This Play activation grants distance only. The native ordinary Slash ledger is debited exactly once.
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.UnlimitedUse != true)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(useId, actor.Seat);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targetSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 30)]
    internal bool ReturnRegisteredCompleteKuangfuSlash(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.KuangfuSlashReturn is null)
            return false;
        CompleteKuangfuSlash(completion);
        return true;
    }

    internal void CompleteKuangfuSlash(GameEngine.AttackCompletionReceipt completion)
    {
        var returned = completion.KuangfuSlashReturn ?? throw new InvalidOperationException("Missing equipment Slash return.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != returned.ProgramFrameId || completion.ProgramFrameId != f.Id || completion.ResolutionId != returned.CardUseFrameId || !_coreKuangfuReceiptsOperations.ValidKuangfu(f) || f.Kuangfu?.SlashReturn != returned || f.Kuangfu.Stage != KuangfuStage.SlashIssued || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Id == returned.CardUseFrameId) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == returned.CardUseFrameId) != 1)
            throw new InvalidOperationException("The equipment Slash lost its exact retired Use and original paid parent.");
        var r = f.Kuangfu;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuSlashResolvedEvent(f.Id, returned.CardUseFrameId, returned.CardActionId, r.CausedDamage));
        if (KuangfuGameEnded() || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            if (!KuangfuGameEnded())
                FinishKuangfu(f);
            return;
        }

        if (r.EquipmentOwnerSeat != f.OwnerSeat && !r.CausedDamage)
        {
            var required = Math.Min(2, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
            if (required == 0)
            {
                FinishKuangfu(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = r with { Stage = KuangfuStage.HandDiscardChoice, DiscardRequired = required } });
            PublishKuangfu(f);
            return;
        }

        if (r.EquipmentOwnerSeat == f.OwnerSeat && r.CausedDamage)
        {
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = r with { Stage = KuangfuStage.OutcomeChildren, OutcomeBefore = before, OutcomeAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new(KuangfuDrawReason)).Count;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuangfu = f.Kuangfu!with { ActualDraw = drawn, OutcomeAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuDrawIssuedEvent(f.Id, 2, drawn, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        FinishKuangfu(f);
    }

    internal void FinishKuangfu(ProgramSkillFrame f)
    {
        var r = f.Kuangfu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangfuFinishedEvent(f.Id, r.EquipmentCardId is not null, r.SlashReturn is not null));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { Kuangfu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardEquipmentThenSlashAndOwnershipOutcome)]
    internal PromptChoice SelectAiKuangfu(PendingDecision p, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var own = view.Players.Single(x => x.Seat == f.OwnerSeat);
        if (f.Kuangfu!.Stage == KuangfuStage.HandDiscardChoice)
        {
            var hand = own.Hand.ToDictionary(c => c.Id);
            return p.Choices.OrderBy(c => CardCatalog.Get(hand[c.Cards.Single()].Kind).HandKeepValue).ThenBy(c => c.Cards.Single()).First();
        }

        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat];
        if (f.Kuangfu.Stage == KuangfuStage.SlashTargetChoice)
            return p.Choices.OrderByDescending(c => brain.ScoreProgramTarget(view, c.Targets.Single(), new(1, 0, 0, 0, 0, 0, false, false))).ThenBy(c => c.Targets.Single()).First();
        // Equipment identity is public. No opponent hand is inspected to guess a response.
        return p.Choices.OrderBy(c => c.Targets.Single() == f.OwnerSeat ? 1 : 0).ThenBy(c => c.Cards.Single()).First();
    }

    SkillProgramStepOutcome IKuangfuProgramHost.DiscardEquipmentThenSlashAndOwnershipOutcome(ProgramSkillFrame f) => BeginKuangfu(f);
}
