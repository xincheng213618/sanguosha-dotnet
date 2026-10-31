#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreRecoveryReplacementRuntime
{
    internal bool HasRecoveryReplacementCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery);

    internal bool TryQueueHengyeRecovery(CharacterState player) => _coreMouLuMengOperations.Value.UsesFormalMouLuMeng && player.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(player, "mou:hengye") && _characterLuMengMouOperations.GetHengyeGrowth(player) >= 3 && player.Hp < player.MaxHp && TryQueueRecoveryReplacement(0, player.Seat, player.Seat, 1, new(RecoveryAttemptProducer.Hengye));
    internal bool TryPauseRecoveryPaidCardUse(long useId, RecoveryPaidCardUseContinuation continuation)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId);
        if (use is null)
            return false;
        continuation = _coreNativePublicPileEquipmentChildrenOperations.Value.FreezeNativePublicPileEquipmentChildren(use, continuation);
        var observesPayment = _coreProgramCardUsePaymentsOperations.ObservesPaidCardUseChildren(use, continuation);
        var paymentOwner = _coreNativePublicPileEquipmentChildrenOperations.Value.HasNativePublicPileEquipmentTail(use, continuation) ? use with
        {
            RecoveryPaidContinuation = continuation
        }

        : use;
        if (use.PendingRecoveryAttempts is not { Count: > 0 } && !((observesPayment || continuation.Kind == RecoveryPaidCardUseKind.DodgeCompletion) && _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Any(change => change.ParentFrameId == useId)) && !(observesPayment && _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(batch => _coreCardUsePaymentsOperations.IsCardUsePaymentBatch(paymentOwner, batch))))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != useId || use.RecoveryPaidContinuation is not null)
            throw new InvalidOperationException("Paid card use recovery lost its exact fresh parent boundary.");
        continuation = continuation with
        {
            TargetSeats = continuation.TargetSeats is { } targets ? Array.AsReadOnly(targets.ToArray()) : null,
            Simple = continuation.Simple is { } simple ? simple with
            {
                RecoveryPolicySources = simple.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
            }

            : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { RecoveryPaidContinuation = continuation, Step = ResolutionFrameStep.ResolvingEffect });
        if (TryBeginQueuedRecoveryReplacement(useId, PostEventContinuation.RecoveryPaidCardUse))
            return true;
        if (observesPayment || continuation.Kind == RecoveryPaidCardUseKind.DodgeCompletion)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(useId, PostEventContinuation.RecoveryPaidCardUse))
                return true;
            if (observesPayment && _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(useId))
                return true;
            // No observer qualified. Keep the already paid response on its ordinary tail.
            var current = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("The paid response lost its card-use parent.");
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { RecoveryPaidContinuation = null, Step = use.Step });
            return false;
        }

        return false;
    }

    internal void ContinueRecoveryPaidCardUse(long useId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Paid recovery lost its card-use producer.");
        var continuation = use.RecoveryPaidContinuation ?? throw new InvalidOperationException("Paid recovery lost its exact continuation.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != useId)
            throw new InvalidOperationException("Paid card use cannot advance before its recovery children return.");
        var observesPayment = _coreProgramCardUsePaymentsOperations.ObservesPaidCardUseChildren(use, continuation);
        if ((observesPayment || continuation.Kind == RecoveryPaidCardUseKind.DodgeCompletion) && _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(useId, PostEventContinuation.RecoveryPaidCardUse))
            return;
        if (use.PendingRecoveryAttempts is { Count: > 0 })
            throw new InvalidOperationException("Paid card use cannot advance before its recovery children return.");
        if (observesPayment && _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(useId))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { RecoveryPaidContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } completedAttack && completedAttack.ResolutionId == useId)
                _coreNativeAttackCompletionOperations.CompleteAttack(completedAttack);
            else
                _coreNativeCardUseCompletionOperations.FinishCardUse(useId, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(useId, continuation.CardId ?? use.CardId), continuation.PlayedCardKind ?? use.CardKind);
            return;
        }

        if (continuation.Kind is RecoveryPaidCardUseKind.Trick or RecoveryPaidCardUseKind.Simple or RecoveryPaidCardUseKind.EquipmentUse)
        {
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(useId, continuation.CardId!.Value, requireProcessing: continuation.Kind != RecoveryPaidCardUseKind.Simple);
            if (!_coreMatchParticipantsPlayersAccess._players[continuation.SourceSeat].IsAlive)
            {
                _coreNativeCardUseCompletionOperations.FinishCardUse(useId, card, continuation.PlayedCardKind ?? use.CardKind);
                return;
            }

            if (continuation.Kind == RecoveryPaidCardUseKind.Trick)
                _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, continuation.SourceSeat, continuation.TargetSeats!, continuation.ActionKind!.Value, continuation.TargetCardId, continuation.RequiredCardKind, continuation.PlayedCardKind);
            else if (continuation.Kind == RecoveryPaidCardUseKind.Simple)
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, continuation.Simple!);
            else
                CompletePaidEquipmentEntry(_coreMatchParticipantsPlayersAccess._players[continuation.SourceSeat], card, useId, continuation.EquipmentSlot!.Value, continuation.ReplacedCardId);
            return;
        }

        NativeOwners.CardAttackHandle attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
        if (attack is null || attack.ResolutionId != useId)
            throw new InvalidOperationException("A paid recovery continuation lost its actual active attack.");
        switch (continuation.Kind)
        {
            case RecoveryPaidCardUseKind.CommittedSlash:
                var action = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.Action;
                if (action is not null && _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId) && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
                    return;
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
                break;
            case RecoveryPaidCardUseKind.SlashTarget:
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
                break;
            case RecoveryPaidCardUseKind.AttackDamage:
                if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                break;
            case RecoveryPaidCardUseKind.DodgeCompletion:
                _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
                break;
            default:
                throw new InvalidOperationException("Paid card use recovery has an unsupported continuation.");
        }
    }
    internal void CompletePaidEquipmentEntry(CharacterState source, Card equipment, long useId, EquipmentSlot slot, int? replacedCardId)
    {
        if (EquipmentCatalog.Get(equipment.Kind).Slot != slot || _cardZonesOperations._cardZones.GetLocation(equipment.Id) != CardLocation.Processing)
            throw new InvalidOperationException("A paid equipment entry lost its frozen slot or processing entity.");
        // The replacement payment is complete. A child may occupy/abolish the slot;
        // that cancels this incoming entry rather than charging a second replacement.
        if (!source.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetEquipment(source).Count(c => EquipmentCatalog.Get(c.Kind).Slot == slot) >= source.EquipmentSlotCapacity(slot))
        {
            _corePhysicalMovementFacadesOperations.MoveCard(equipment, CardLocation.Processing, CardLocation.DiscardPile, new("equipment.paid-entry-cancelled"));
            _coreNativeCardUseCompletionOperations.FinishCardUse(useId, equipment, equipment.Kind);
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(equipment, CardLocation.Processing, CardLocation.Equipment(source.Seat), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(useId, source.Seat, slot, equipment.Id, equipment.Kind, replacedCardId));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(source.Name)} 装备【{(equipment.DisplayName)}】。", source.Seat);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(equipment.Id, SimpleCardUseEffect.Equipment));
    }

    internal IReadOnlyList<RecoveryReplacementCandidate> RecoveryReplacementCandidates(int seat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!HasRecoveryReplacementCapability || !target.IsAlive || !target.HasHpValue || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != seat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || !_characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(seat) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(target);
        // Before a pool is selected this is a candidate union, not a resolved
        // comparison. The actual selected-pool comparison is checked below.
        var thresholds = target.HappyQiaoHealth is { } dual ? new[]
        {
            dual.DaQiaoHp,
            dual.XiaoQiaoHp
        }

        : new[]
        {
            target.Hp
        };
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue && p.Seat != seat && thresholds.Any(h => p.HappyQiaoHealth is { } other ? other.DaQiaoHp <= h || other.XiaoQiaoHp <= h : p.Hp <= h)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(seat, p.Seat)).SelectMany(owner => _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery).Where(item => item.Policy.FactionId == faction && item.Source.SkillId != "boundary:jiuyuan-mobile").Select(item => new RecoveryReplacementCandidate(owner.Seat, item.Source.SkillId, item.Source.SkillInstanceId, item.Policy.Id, item.Policy.Value, item.Policy.ProviderDrawCount))).GroupBy(c => (c.OwnerSeat, c.SkillId, c.PolicyId)).Select(g => g.First()).ToArray());
    }

    internal bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion, HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = null)
    {
        if (amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].HasHpValue || !_characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(targetSeat))
            return false;
        var qiao = _coreMatchParticipantsPlayersAccess._players[targetSeat].HappyQiaoHealth;
        if (qiao is null && !HasRecoveryReplacementCapability && !_characterCaoSongOlOperations.HasRecoveryAmountSupport(targetSeat) && !_coreRecoveryAmountBonusesOperations.Value.HasOwnRecoveryAmountSupport(targetSeat))
            return false;
        var parent = parentFrameId > 0 ? _resolutionStackOperations._resolutionStack.Single(f => f.Id == parentFrameId) : null;
        var candidates = _characterSunQuanBoundaryMobileOperations.BoundarySunQuanMobileRecoveryCandidates(parentFrameId, sourceSeat, targetSeat, completion, RecoveryReplacementCandidates(targetSeat));
        if (qiao is null && candidates.Count == 0 && !_characterCaoSongOlOperations.HasRecoveryAmountSupport(targetSeat) && !_coreRecoveryAmountBonusesOperations.Value.HasOwnRecoveryAmountSupport(targetSeat) && parent?.PendingRecoveryAttempts is not { Count: > 0 })
            return false;
        var request = new RecoveryAttempt(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, sourceSeat, targetSeat, amount, qiao is null ? _coreMatchParticipantsPlayersAccess._players[targetSeat].Hp : null, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, candidates, completion with { Policies = completion.Policies is { } policies ? Array.AsReadOnly(policies.ToArray()) : null })
        {
            HappyQiaoBefore = qiao,
            HappyQiaoRecoverToOrigin = happyQiaoRecoverToOrigin
        };
        if (parent is null)
        {
            _coreRuntimeOperations.PushRuntimeFrame(new RecoveryReplacementFrame(request.Id, 0, request, new(PostEventContinuation.RecoveryProducer)));
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { PendingRecoveryAttempts = Array.AsReadOnly((parent.PendingRecoveryAttempts ?? []).Append(request).ToArray()) });
        }

        _coreProgramRecoveryReceiptsOperations.RecordProgramRecoveryRequest(parent, request);
        return true;
    }

    internal bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, PostEventContinuation continuation, int? cardId = null, CardKind? cardKind = null)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || parentFrameId is null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != parentFrameId || parent.PendingRecoveryAttempts is not { Count: > 0 } requests)
            return false;
        var request = requests[0];
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { PendingRecoveryAttempts = requests.Count == 1 ? null : Array.AsReadOnly(requests.Skip(1).ToArray()) });
        if (continuation == PostEventContinuation.DrawFundedDistinctBasic)
            _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasicChild(parent.Id, request.Id);
        _coreRuntimeOperations.PushRuntimeFrame(new RecoveryReplacementFrame(request.Id, parent.Id, request, new(continuation, parent.Id, cardId, cardKind)));
        _coreRuntimeOperations.AdvanceRuntimeFrame(request.Id);
        return true;
    }

    internal bool IsRecoveryReplacementCandidateCurrent(RecoveryReplacementFrame frame, RecoveryReplacementCandidate c)
    {
        if (c.SkillId == "boundary:jiuyuan-mobile")
            return _characterSunQuanBoundaryMobileOperations.IsBoundarySunQuanMobileRecoveryCandidateCurrent(frame, c);
        var attempt = frame.Attempt;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue || !_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].HasHpValue || attempt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || attempt.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || attempt.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp > _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoRecoveryHpBefore(frame))
            return false;
        return _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery).Any(item => item.Source.SkillId == c.SkillId && item.Source.SkillInstanceId == c.SkillInstanceId && item.Policy.Id == c.PolicyId && item.Policy.FactionId == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat]));
    }

    internal IReadOnlyList<PromptChoice> RecoveryReplacementChoices(RecoveryReplacementFrame frame)
    {
        var choices = frame.Attempt.Candidates.Select((c, i) => (c, i)).Where(item => IsRecoveryReplacementCandidateCurrent(frame, item.c)).Select(item => new PromptChoice(new($"recovery.{(frame.Id)}.redirect.{(item.i)}"), $"发动【{(_contentRegistryOperations._contentRegistry.GetSkill(item.c.SkillId).Name)}】：令 {(_coreMatchParticipantsPlayersAccess._players[item.c.OwnerSeat].Name)} 回复{(item.c.RecoveryAmount)}点体力，然后你摸{(item.c.ProviderDrawCount)}张牌", [], [item.c.OwnerSeat], new Dictionary<string, string> { ["recovery-action"] = "redirect", ["candidate-index"] = item.i.ToString(System.Globalization.CultureInfo.InvariantCulture) })).Append(new(new($"recovery.{(frame.Id)}.keep"), $"按原效果回复{(frame.Attempt.Amount)}点体力", [], [], new Dictionary<string, string> { ["recovery-action"] = "keep" })).ToArray();
        return Array.AsReadOnly(choices);
    }

    [NativeFrameRoute(typeof(RecoveryReplacementFrame), 330)]
    internal void ContinueRecoveryReplacement()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RecoveryReplacementFrame frame)
            return;
        if (frame.Stage == RecoveryReplacementStage.Choosing)
        {
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoRecoveryRequestChoice(frame))
                return;
            if (frame.PendingApplication is { } pending)
            {
                ApplyRecoveryReplacement(frame, pending.CandidateIndex);
                return;
            }

            var choices = RecoveryReplacementChoices(frame);
            if (choices.Count == 1 || !_coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                ApplyRecoveryReplacement(frame, null);
                return;
            }

            var first = frame.Attempt.Candidates.First(c => IsRecoveryReplacementCandidateCurrent(frame, c));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(first.SkillId);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.RecoveryReplacement, frame.Attempt.TargetSeat, $"【{(skill.Name)}】：是否改为令其他角色回复体力，并由你摸牌？", [], [], first.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices,
                SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        if (frame.Stage == RecoveryReplacementStage.RecoveryApplied)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.RecoveryReplacement))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = RecoveryReplacementStage.RewardApplied });
            var sequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            if (frame.CandidateIndex is { } index && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat].IsAlive)
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat], frame.Attempt.Candidates[index].ProviderDrawCount, true, new CardMoveReason("skill-program.recovery-replacement.reward"));
            frame = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
            var receipt = new RecoveryRewardMovementReceipt(sequenceBefore, _coreCardMovementJournalMovementSequenceAccess._movementSequence)
            {
                Batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(batch => batch.ParentFrameId == frame.Id && batch.Movements.Count > 0 && batch.Movements.All(move => move.Sequence > sequenceBefore && move.Sequence <= _coreCardMovementJournalMovementSequenceAccess._movementSequence)).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RewardMovementReceipt = receipt });
            frame = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
        }

        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return;
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.RecoveryReplacement);
        ResumeRecoveryReplacementProducer(frame);
    }

    internal void ApplyRecoveryReplacement(RecoveryReplacementFrame frame, int? candidateIndex)
    {
        var attempt = frame.Attempt;
        if (frame.PendingApplication is null)
        {
            var selected = candidateIndex is { } selectedIndex ? attempt.Candidates[selectedIndex] : null;
            if (selected is not null && !IsRecoveryReplacementCandidateCurrent(frame, selected))
                selected = null;
            candidateIndex = selected is null ? null : candidateIndex;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingApplication = new(selected is null ? attempt.SourceSeat : attempt.TargetSeat, selected?.OwnerSeat ?? attempt.TargetSeat, selected?.RecoveryAmount ?? attempt.Amount, candidateIndex), Step = ResolutionFrameStep.ResolvingEffect });
        }

        if (_coreProgramRuntimeHooksOperations.RuntimeHooks.TryAdvance(ProgramRuntimePhase.BeforeRecoveryApply))
            return;
        frame = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
        var application = frame.PendingApplication ?? throw new InvalidOperationException("Recovery lost its prepared recipient and amount.");
        candidateIndex = application.CandidateIndex;
        var candidate = candidateIndex is { } index ? attempt.Candidates[index] : null;
        var recipient = _coreMatchParticipantsPlayersAccess._players[application.TargetSeat];
        var source = application.SourceSeat;
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoRecoveryChoice(frame, application))
            return;
        // No child or cost is opened between the live passive decision and this
        // single HP payment. Existing paid Cao Song support has already returned.
        var bonus = _coreRecoveryAmountBonusesOperations.Value.PrepareOwnRecoveryAmountBonus(application);
        var requested = bonus?.ModifiedRequestedAmount ?? application.RequestedAmount;
        var amount = recipient.IsAlive && recipient.HasHpValue && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? Math.Min(requested, Math.Max(0, recipient.MaxHp - recipient.Hp)) : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CandidateIndex = candidateIndex, Stage = RecoveryReplacementStage.RecoveryApplied, OriginalRecovered = candidate is null ? amount : 0, PendingApplication = null, AmountBonus = bonus, Step = ResolutionFrameStep.ResolvingEffect });
        if (candidate is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryReplacementChosenEvent(frame.Id, attempt.TargetSeat, candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.PolicyId, attempt.Amount, candidate.RecoveryAmount));
        if (amount > 0)
        {
            var child = _coreNativeRecoveryFramesOperations.BeginRecovery(frame.Id, source, recipient.Seat, amount);
            if (bonus is not null)
            {
                bonus = bonus with
                {
                    AppliedRecoveryFrameId = child
                };
                var owning = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == frame.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, owning with { AmountBonus = bonus });
            }

            recipient.Hp += amount;
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(recipient.Seat))
                _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(frame.Id, recipient.Seat, HpChangeKind.Recovery, frame.HappyQiaoHealth!.Before.Hp(frame.HappyQiaoHealth.Pool!.Value), recipient.Hp);
            if (bonus is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAmountBonusAppliedEvent(frame.Id, bonus));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(source, recipient.Seat, amount, recipient.Hp));
            _coreRuntimeOperations.PopResolutionFrame(child, ResolutionFrameKind.Recovery);
        }
        else
            _characterDaQiaoXiaoQiaoHappyGodOperations.ReleaseHappyQiaoHealthSelection(frame.Id);
        ApplyRecoveryAttemptProducerFacts(frame.Id, attempt, candidate is null ? amount : 0);
        ContinueRecoveryReplacement();
    }

    internal CommandResult SubmitRecoveryReplacementAnswer(PromptChoice selected) => _kernelCoreMainOperations.Accept(() =>
    {
        var frame = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
        var published = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Single(c => c.Id == selected.Id);
        int? index = published.Parameters.GetValueOrDefault("recovery-action") == "redirect" ? int.Parse(published.Parameters["candidate-index"], System.Globalization.CultureInfo.InvariantCulture) : null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        ApplyRecoveryReplacement(frame, index);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    });
    internal void ResolveAiRecoveryReplacement()
    {
        // A deterministic identity-role preference keeps this optional choice free of RNG consumption.
        var frame = (RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last();
        var choice = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Last();
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat];
        if (actor.Role == Role.Loyalist && _coreMatchParticipantsPlayersAccess._players[frame.Attempt.TargetSeat].Hp > 1)
            choice = _corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.FirstOrDefault(c => c.Targets.Count == 1 && _coreMatchParticipantsPlayersAccess._players[c.Targets[0]].Role == Role.Lord && _coreMatchParticipantsPlayersAccess._players[c.Targets[0]].Hp < _coreMatchParticipantsPlayersAccess._players[c.Targets[0]].MaxHp) ?? choice;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        int? index = choice.Parameters.GetValueOrDefault("recovery-action") == "redirect" ? int.Parse(choice.Parameters["candidate-index"], System.Globalization.CultureInfo.InvariantCulture) : null;
        ApplyRecoveryReplacement(frame, index);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ApplyRecoveryAttemptProducerFacts(long childId, RecoveryAttempt attempt, int recovered)
    {
        var completion = attempt.Completion;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == ((RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last()).ParentFrameId);
        if (completion.Producer == RecoveryAttemptProducer.ProgramMaximum && parent is ProgramSkillFrame maximum)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, maximum with { RecoveryReceipt = new(childId, completion.InstructionIndex, attempt.TargetSeat, recovered) });
        if (completion.Producer == RecoveryAttemptProducer.Program && parent is ProgramSkillFrame program && program.WindowContext?.JudgmentReplacement is { } judgment)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, program with { WindowContext = program.WindowContext with { JudgmentReplacement = judgment with { RecoveredHp = judgment.RecoveredHp + recovered } } });
        if (completion.Producer == RecoveryAttemptProducer.SilverLion)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SilverLionRemovedRecoveryEvent(childId, attempt.TargetSeat, completion.MoveReason!.Value, recovered, _coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].Hp));
        if (completion.Producer == RecoveryAttemptProducer.CardUse && recovered > 0)
            foreach (var policy in completion.Policies ?? [])
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRecoveryPolicyAppliedEvent(((RecoveryReplacementFrame)_resolutionStackOperations._resolutionStack.Last()).ParentFrameId, attempt.TargetSeat, attempt.SourceSeat, completion.CardId!.Value, attempt.Amount, policy.SkillId, policy.PolicyId));
        if (recovered > 0)
            _coreMatchLogOperations.AddLog("Recovered", $"{(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].Name)} 回复{(recovered)}点体力，至 {(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].Hp)}/{(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].MaxHp)}。", attempt.SourceSeat, attempt.TargetSeat);
    }

    internal void ResumeRecoveryReplacementProducer(RecoveryReplacementFrame completed)
    {
        if (_coreResponseCompletionHealthOperations.Value.ReturnCardResponseCompletionRecovery(completed))
            return;
        if (_coreCardSupplyCompletionOperations.ReturnCardSupplyCompletionRecovery(completed))
            return;
        if (completed.Return.Continuation == PostEventContinuation.DrawFundedDistinctBasic)
        {
            _coreDrawFundedDistinctBasicPaymentOperations.ResumeDrawFundedDistinctBasicRecovery(completed);
            return;
        }

        var attempt = completed.Attempt;
        var continuation = completed.Return;
        if (continuation.Continuation == PostEventContinuation.RecoveryProducer)
        {
            CompleteDeferredRecoveryProducer(attempt, completed.OriginalRecovered, completed.ParentFrameId);
            return;
        }

        if (TryBeginQueuedRecoveryReplacement(continuation.ResumeFrameId, continuation.Continuation, continuation.CardId, continuation.CardKind))
            return;
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(continuation.ResumeFrameId, continuation.Continuation, continuation.CardId, continuation.CardKind))
            return;
        switch (continuation.Continuation)
        {
            case PostEventContinuation.Program:
                _coreRuntimeOperations.AdvanceRuntimeProgram(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.SkillInvocation:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not SkillInvocationFrame invocation || !_characterHuaTuoAnniversaryGodOperations.IsSkillInvocationHealthChild2(invocation, completed))
                    throw new InvalidOperationException("A skill invocation lost its exact recovery producer return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(invocation.Id);
                break;
            case PostEventContinuation.KemingEcho:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame echo || echo.Id != continuation.ResumeFrameId || !_characterCaoPiAnniversaryGodOperations.KemingSkillEchoFrameRidesOn(completed, echo))
                    throw new InvalidOperationException("Keming echo lost its exact recovery producer return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(echo.Id);
                break;
            case PostEventContinuation.FriendDuelResponse:
                _coreFriendDuelResponsesOperations.ContinueFriendDuelResponse(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.AwaitedProgramMovement:
                if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                    _coreRuntimeOperations.ReturnRuntimeProgramMovement(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.CardUse:
                var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(continuation.ResumeFrameId!.Value, continuation.CardId!.Value);
                _coreNativeCardUseCompletionOperations.FinishCardUse(continuation.ResumeFrameId!.Value, card, continuation.CardKind);
                break;
            case PostEventContinuation.VirtualBasicCardUse:
                _coreVirtualBasicOffersOperations.FinishVirtualBasicUse(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.GroupRecovery:
                _coreNativeGroupRecoveryOperations.Value.CompleteGroupRecoveryTarget();
                break;
            case PostEventContinuation.ColdDamageReplacement:
                _coreRuntimeOperations.AdvanceRuntimeFrame(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.ShanheS5YimieRecovery:
                _coreRuntimeOperations.AdvanceRuntimeFrame(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.RecoveryReplacement:
                _coreRuntimeOperations.AdvanceRuntimeFrame(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.AmountSupport:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not (RecoveryReplacementFrame or BeforeDamageProgramWindowFrame))
                    throw new InvalidOperationException("Amount support lost its typed parent frame.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.RecoveryPaidCardUse:
                ContinueRecoveryPaidCardUse(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.CounterspellPayment:
                _coreUnrespondableCounterspellsOperations.ContinuePolicyCounterspellPayment(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.SoleIncomingRangeDraw:
                _coreSoleIncomingRangeDrawOperations.ContinueSoleIncomingRangeDraw(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.NativeCardEffectDraw:
                _coreNativeCardEffectDrawsOperations.ContinueNativeDrawTwo(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.WenheCardEffect:
                _coreWenheCardsOperations.ContinueWenheCardEffect(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.GuanduCardEffect:
                _coreGuanduCardsOperations.ContinueGuanduCardEffect(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.WumiaoFlood:
                _characterGuanYuWumiaoOperations.ContinueWumiaoFlood(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.DragonBoatCardEffect:
                _coreDragonBoatCardsOperations.ContinueDragonBoatCardEffect(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.ColorFireAttackPayment:
                _coreColorFireAttacksOperations.ContinueColorFireAttackPayment(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.HuxinMirrorPlacement:
                _coreHuxinMirrorOperations.ContinueHuxinMirrorPlacement(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.EquipmentRecast:
                _coreEquipmentRecastsOperations.ContinueEquipmentRecast(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.DrawPhaseObligation:
                _coreRuntimeOperations.AdvanceRuntimeFrame(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.FactionRequestCost:
                _coreFactionRequestCostRecoveriesOperations.ContinuePaidFactionRequestCost(continuation.ResumeFrameId!.Value);
                break;
            case PostEventContinuation.Boundary:
                _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow();
                break;
            default:
                throw new InvalidOperationException("A recovery attempt lost its typed producer continuation.");
        }
    }

    internal void CompleteDeferredRecoveryProducer(RecoveryAttempt attempt, int recovered, long parentId)
    {
        var c = attempt.Completion;
        if (c.Producer == RecoveryAttemptProducer.Hengye)
        {
            _coreActualTurnsOperations.ContinueTurnAfterHengye(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat]);
            return;
        }

        if (c.Producer == RecoveryAttemptProducer.SilverLion)
        {
            if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow())
                _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow();
            return;
        }

        if (c.Producer is not (RecoveryAttemptProducer.DyingBoundAlcohol or RecoveryAttemptProducer.DyingVirtualAlcohol))
            throw new InvalidOperationException("A root recovery attempt lost its producer.");
        if (c.Producer == RecoveryAttemptProducer.DyingBoundAlcohol)
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(parentId) ?? throw new InvalidOperationException("Bound Alcohol recovery lost its native use.");
            if (use.BoundDyingAlcoholUse is not { } token || token.ProgramFrameId != c.ProgramFrameId || token.DyingFrameId != c.DyingFrameId || token.Source.SkillId != c.SkillId || token.Source.OwnerSeat != c.SkillOwnerSeat || token.CardId != c.CardId || token.VictimSeat != attempt.TargetSeat || c.CardKind != CardKind.Alcohol || c.MoveReason != token.Reason || attempt.SourceSeat != token.VictimSeat)
                throw new InvalidOperationException("Bound Alcohol recovery returned outside its exact typed source/use.");
            _coreBoundDyingAlcoholReturnsOperations.RecordBoundDyingAlcoholPayment(parentId, recovered, attempt.Id);
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == c.CardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, c.MoveReason!.Value);
            _coreNativeCardUseCompletionOperations.FinishCardUse(parentId, card, CardKind.Alcohol);
            return; // The owning use returns the paused program after all completion children.
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDyingRescueEvent(c.DyingFrameId!.Value, c.SkillId!, c.SkillOwnerSeat!.Value, attempt.TargetSeat, c.CardId ?? 0, recovered, _coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].Hp));
        if (_coreActualTurnLegacyCompletionOperations.FinishActualTurnLegacyDyingAlcohol(parentId, c.ProgramFrameId!.Value))
            return;
        else
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(parentId);
        _coreRuntimeOperations.AdvanceRuntimeProgram(c.ProgramFrameId!.Value);
    }
}
