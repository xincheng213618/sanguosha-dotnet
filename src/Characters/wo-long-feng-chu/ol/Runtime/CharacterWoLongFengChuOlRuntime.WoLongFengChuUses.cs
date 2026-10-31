#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWoLongFengChuOlRuntime
{
    internal const string EquipmentSlotNamedTier = "equipment-slot-named-tier";
    internal static string EquipmentSlotNamedLabel(EquipmentSlot slot) => RuntimeSkills.RuntimeRules_wo_long_feng_chu_ol_WoLongFengChuUses.EquipmentSlotNamedLabel(slot);
    internal bool IsEquipmentSlotNamedSkill(string skill) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skill)?.Program?.Activations.Any(a => ProgramInstructionResolver.Default.Features(a).HasOperation(SkillProgramEffectOp.PayEquipmentSlotForNamedUse)) == true;
    internal EquipmentSlotNamedUsePayment? UnconsumedEquipmentSlotPayment(CardConversionSource source) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUsePaidEvent>().Select(e => e.Payment).LastOrDefault(p => p.Source == source && p.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && p.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && p.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUseConsumedEvent>().Any(e => e.PaymentFrameId == p.FrameId) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUseCancelledEvent>().Any(e => e.PaymentFrameId == p.FrameId));
    internal bool EquipmentSlotNamedYin(int seat, string skill, string instance) => _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.GetValueOrDefault(new GameEngine.ProgramBooleanStateKey(seat, skill, instance, "yin"));
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> PaidEquipmentSlotNamedZeroRules(CharacterState owner, CardKind? output, bool responseUse, bool dyingUse)
    {
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            yield break;
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUsePaidEvent>())
        {
            var p = fact.Payment;
            if (p.Source.OwnerSeat != owner.Seat || UnconsumedEquipmentSlotPayment(p.Source) != p || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == p.FrameId) || _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(i => i.SkillId == p.Source.SkillId && i.SkillInstanceId == p.Source.SkillInstanceId) || _corePlayCardHistoryOperations.ViewAsRule(p.Source)is not { InputCount: 0, UseOnly: true, TieredRoundConversion: not null } rule || output is { } kind && kind != rule.OutputKind || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, rule.OutputKind, CardActionType.Use) || (dyingUse ? rule.NoDying || !(rule.ForResponse || rule.ForPlay && rule.UseOnly) : responseUse ? !rule.ForResponse : !rule.ForPlay))
                continue;
            yield return (p.Source, rule, p.GameplayHash);
        }
    }

    internal bool EquipmentSlotNamedNameUsed(int seat, CardKind kind) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Reverse().TakeWhile(e => e is not EquipmentSlotNamedNamesResetEvent reset || reset.OwnerSeat != seat).OfType<EquipmentSlotNamedUseIssuedEvent>().Any(e => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUsePaidEvent>().Any(p => p.Payment.FrameId == e.PaymentFrameId && p.Payment.Source.OwnerSeat == seat && p.Payment.EffectiveKind == kind));
    internal bool TryGetEquipmentSlotNamedConversionAvailability(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool responseUse, bool dyingUse, out bool available)
    {
        available = false;
        if (!IsEquipmentSlotNamedSkill(instance.SkillId))
            return false;
        var source = new CardConversionSource(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !actor.IsAlive || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || rule.TieredRoundConversion is not { } policy || rule.InputCount != 0 || !rule.UseOnly || _coreProgramTieredRoundConversionOperations.TieredRoundTier(actor.Seat, policy.StateId) != 2 || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, rule.OutputKind, CardActionType.Use))
            return true;
        var paid = UnconsumedEquipmentSlotPayment(source);
        if (paid is not null && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId))
        {
            available = true;
            return true;
        }

        var yin = EquipmentSlotNamedYin(actor.Seat, source.SkillId, source.SkillInstanceId);
        var basic = CardUseCategoryCatalog.Get(rule.OutputKind) == CardUseCategories.Basic;
        available = yin == basic && !EquipmentSlotNamedNameUsed(actor.Seat, rule.OutputKind) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(actor.Seat, source.SkillId, CoreProgramTieredRoundConversionRuntime.TieredRoundUsage(policy), SkillUsageScope.Round) == 0 && Enum.GetValues<EquipmentSlot>().Any(slot => actor.EquipmentSlotCapacity(slot) > 0);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 26100)]
    internal void InitializeEquipmentSlotNamedNativeTier()
    {
        foreach (var actor in _coreMatchParticipantsPlayersAccess._players)
            if (_coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Any(i => IsEquipmentSlotNamedSkill(i.SkillId)))
                _coreProgramSkillChangesOperations.CommitConfiguredConversionTier(actor, EquipmentSlotNamedTier, 2);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 26100)]
    internal bool HideInternalEquipmentSlotPayment(CharacterState owner, SkillProgramActivation activation) => !ProgramInstructionResolver.Default.Features(activation).HasOperation(SkillProgramEffectOp.PayEquipmentSlotForNamedUse);
    internal bool TryBeginEquipmentSlotNamedPlay(CharacterState actor, LegalAction action)
    {
        if (action.ConversionSource is not { } source || !IsEquipmentSlotNamedSkill(source.SkillId) || UnconsumedEquipmentSlotPayment(source)is { } paid && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId))
            return false;
        var original = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? new PendingDecision(DecisionKind.PlayCard, actor.Seat, "游龙", [], []);
        BeginEquipmentSlotNamedUse(actor, source, action.PlayedCardKind!.Value, EquipmentSlotNamedUseIntent.Play, original, action, null);
        return true;
    }

    internal bool TryBeginEquipmentSlotNamedResponse(CharacterState actor, CardConversionSource source, PromptChoice choice, bool dyingUse)
    {
        if (!IsEquipmentSlotNamedSkill(source.SkillId) || UnconsumedEquipmentSlotPayment(source)is { } paid && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId))
            return false;
        var kind = _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind;
        var intent = dyingUse ? kind == CardKind.Peach ? EquipmentSlotNamedUseIntent.DyingPeach : EquipmentSlotNamedUseIntent.DyingAlcohol : kind == CardKind.Nullification ? EquipmentSlotNamedUseIntent.Nullification : EquipmentSlotNamedUseIntent.Dodge;
        BeginEquipmentSlotNamedUse(actor, source, kind, intent, _corePendingDecisionPolicyPipelineOperations._pendingDecision!, null, choice);
        return true;
    }

    internal bool TryBeginEquipmentSlotNamedForcedSlash(CharacterState actor, CardConversionSource source, CardKind kind, PromptChoice choice, bool qinglong)
    {
        if (!IsEquipmentSlotNamedSkill(source.SkillId) || UnconsumedEquipmentSlotPayment(source)is { } paid && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId))
            return false;
        BeginEquipmentSlotNamedUse(actor, source, kind, qinglong ? EquipmentSlotNamedUseIntent.Qinglong : EquipmentSlotNamedUseIntent.BorrowedSword, _corePendingDecisionPolicyPipelineOperations._pendingDecision!, null, choice);
        return true;
    }

    internal static string EquipmentSlotNamedTargetSignature(LegalAction? action, PromptChoice? choice) => RuntimeSkills.RuntimeRules_wo_long_feng_chu_ol_WoLongFengChuUses.EquipmentSlotNamedTargetSignature(action, choice);
    internal static string EquipmentSlotNamedDecisionSignature(PendingDecision decision) => RuntimeSkills.RuntimeRules_wo_long_feng_chu_ol_WoLongFengChuUses.EquipmentSlotNamedDecisionSignature(decision);
    internal bool EquipmentSlotNamedChoiceIsPayable(CharacterState actor, CardConversionSource source, CardKind kind, EquipmentSlotNamedUseIntent intent, IReadOnlyList<int> targets, EquipmentSlot slot)
    {
        if (actor.EquipmentSlotCapacity(slot) <= 0 || intent is EquipmentSlotNamedUseIntent.BorrowedSword or EquipmentSlotNamedUseIntent.Qinglong && slot == EquipmentSlot.Weapon)
            return false;
        if (kind == CardKind.Snatch)
            return targets.Count == 1 && (_coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance2(actor.Seat, Suit.None) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind) || _coreProvenanceCardPoliciesOperations.HasProvenanceUseDistance2(actor, []) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, kind) || CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], excludedEquipmentSlot: slot)) == 1);
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return true;
        var logical = new Card(0, kind, Suit.None, 0);
        return targets.Count > 0 && targets.All(target => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], logical, source, kind, noEffectiveRank: true, physicalCardIds: [], excludedEquipmentSlot: slot) && (intent is EquipmentSlotNamedUseIntent.Qinglong or EquipmentSlotNamedUseIntent.BorrowedSword or EquipmentSlotNamedUseIntent.SayingSlash || _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[target], false, kind, effectiveRank: 0, excludedEquipmentSlot: slot, physicalCardIds: [])));
    }

    internal bool IsEquipmentSlotNamedActionPayable(CharacterState actor, LegalAction action) => action.ConversionSource is not { } source || !IsEquipmentSlotNamedSkill(source.SkillId) || Enum.GetValues<EquipmentSlot>().Any(slot => EquipmentSlotNamedChoiceIsPayable(actor, source, action.PlayedCardKind!.Value, EquipmentSlotNamedUseIntent.Play, action.TargetSeats, slot));
    internal bool IsEquipmentSlotNamedForcedSlashPayable(CharacterState actor, CardConversionSource source, CardKind kind, CharacterState target, bool qinglong) => !IsEquipmentSlotNamedSkill(source.SkillId) || UnconsumedEquipmentSlotPayment(source)is { } paid && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId) || Enum.GetValues<EquipmentSlot>().Any(slot => EquipmentSlotNamedChoiceIsPayable(actor, source, kind, qinglong ? EquipmentSlotNamedUseIntent.Qinglong : EquipmentSlotNamedUseIntent.BorrowedSword, [target.Seat], slot));
    internal void BeginEquipmentSlotNamedUse(CharacterState actor, CardConversionSource source, CardKind kind, EquipmentSlotNamedUseIntent intent, PendingDecision original, LegalAction? action, PromptChoice? choice)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        var rule = _corePlayCardHistoryOperations.ViewAsRule(source)!;
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Single(i => i.SkillId == source.SkillId && i.SkillInstanceId == source.SkillInstanceId);
        if (!TryGetEquipmentSlotNamedConversionAvailability(actor, instance, rule, intent != EquipmentSlotNamedUseIntent.Play, intent is EquipmentSlotNamedUseIntent.DyingPeach or EquipmentSlotNamedUseIntent.DyingAlcohol, out var available) || !available)
            throw new InvalidOperationException("Youlong lost its actual current face, unused name, round allowance or remaining equipment slot.");
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id;
        var frame = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, actor.Seat, source.SkillId, "equipment-slot-payment", program.GameplayHash, 1, [], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            EquipmentSlotNamedUse = new()
            {
                Source = source,
                GameplayHash = program.GameplayHash,
                Intent = intent,
                EffectiveKind = kind,
                Yin = EquipmentSlotNamedYin(actor.Seat, source.SkillId, source.SkillInstanceId),
                RoundNumber = _coreActualRoundProgressRoundNumberAccess._roundNumber,
                ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber,
                ActualTurnOwner = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat,
                PhaseInstance = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
                OriginalDecision = original,
                PlayAction = action,
                ResponseChoice = choice,
                OriginalParentId = parent,
                TargetSignature = EquipmentSlotNamedTargetSignature(action, choice),
                OriginalDecisionSignature = EquipmentSlotNamedDecisionSignature(original)
            }
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseStartedEvent(frame.Id, source, program.GameplayHash, intent, kind, frame.EquipmentSlotNamedUse.Yin, _coreActualRoundProgressRoundNumberAccess._roundNumber, original.PromptId, original.Revision, parent, frame.EquipmentSlotNamedUse.TargetSignature, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) { OriginalDecisionSignature = frame.EquipmentSlotNamedUse.OriginalDecisionSignature });
        PublishEquipmentSlotNamedChoice(frame);
    }

    internal void PublishEquipmentSlotNamedChoice(ProgramSkillFrame frame)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var draft = frame.EquipmentSlotNamedUse!;
        var targets = draft.PlayAction?.TargetSeats ?? draft.ResponseChoice?.Targets ?? [];
        var choices = Enum.GetValues<EquipmentSlot>().Where(s => EquipmentSlotNamedChoiceIsPayable(actor, draft.Source, draft.EffectiveKind, draft.Intent, targets, s)).Select(slot => new PromptChoice(new($"equipment-slot-use.{(frame.Id)}.{(slot)}"), $"废除{(EquipmentSlotNamedLabel(slot))}栏，视为使用【{(CardCatalog.Get(draft.EffectiveKind).DisplayName)}】", [], [], new Dictionary<string, string> { ["program-action"] = "equipment-slot-named-use", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["equipment-slot"] = slot.ToString() })).ToArray();
        if (choices.Length == 0)
            throw new InvalidOperationException("An accepted slot conversion lost every payable equipment slot before payment.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "游龙：选择废除的装备栏。", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(frame.SkillId, "游龙", "游龙", _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = actor.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PayEquipmentSlotForNamedUse)]
    internal PromptChoice SelectAiEquipmentSlotNamedUse(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.First();
    [ProgramAnswer("equipment-slot-named-use")]
    internal void PayEquipmentSlotNamedUse(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { EquipmentSlotNamedUse: { Stage: EquipmentSlotNamedUseStage.ChoosingSlot } draft } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || choice.Cards.Count != 0 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || !Enum.TryParse<EquipmentSlot>(choice.Parameters.GetValueOrDefault("equipment-slot"), out var slot) || !EquipmentSlotNamedChoiceIsPayable(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], draft.Source, draft.EffectiveKind, draft.Intent, draft.PlayAction?.TargetSeats ?? draft.ResponseChoice?.Targets ?? [], slot))
            throw new InvalidOperationException("Youlong requires its exact remaining slot choice on its original unpaid frame.");
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var policy = _corePlayCardHistoryOperations.ViewAsRule(draft.Source)!.TieredRoundConversion!;
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(actor.Seat, draft.Source.SkillId, CoreProgramTieredRoundConversionRuntime.TieredRoundUsage(policy), SkillUsageScope.Round, 1))
            throw new InvalidOperationException("A Youlong face cannot pay its actual-round allowance twice.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var previous = actor.EquipmentSlotCapacity(slot);
        var payment = new EquipmentSlotNamedUsePayment(frame.Id, draft.Source, draft.GameplayHash, draft.RoundNumber, draft.ActualTurn, draft.ActualTurnOwner, draft.PhaseInstance, draft.Yin, draft.EffectiveKind, slot, previous, before, before, draft.Intent, draft.OriginalDecision.PromptId, draft.OriginalDecision.Revision, draft.OriginalParentId, draft.TargetSignature);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { EquipmentSlotNamedUse = draft with { Stage = EquipmentSlotNamedUseStage.MovementChildren, Payment = payment } });
        _coreProgramPersistentStateOperations.SetProgramBooleanState(actor.Seat, draft.Source.SkillId, draft.Source.SkillInstanceId, "yin", !draft.Yin);
        _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(actor, slot, 0);
        payment = payment with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { EquipmentSlotNamedUse = frame.EquipmentSlotNamedUse!with { Payment = payment }, PendingMovementContinuation = payment.After > payment.Before ? new(frame.OwnerSeat, 0, null) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUsePaidEvent(payment));
        ResumeEquipmentSlotNamedUse(frame.Id);
    }

    internal bool TryConsumePaidEquipmentSlotNamedConversion(CardConversionSource source)
    {
        if (!IsEquipmentSlotNamedSkill(source.SkillId))
            return false;
        var paid = UnconsumedEquipmentSlotPayment(source) ?? throw new InvalidOperationException("The native Youlong use has no exact unconsumed paid slot invoice.");
        var policy = _corePlayCardHistoryOperations.ViewAsRule(source)!.TieredRoundConversion!;
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.FrameId) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(source.OwnerSeat, source.SkillId, CoreProgramTieredRoundConversionRuntime.TieredRoundUsage(policy), SkillUsageScope.Round) != 1)
            throw new InvalidOperationException("The native Youlong use cannot start before its once-paid cost frame returns.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseConsumedEvent(paid.FrameId));
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26100)]
    internal void ObserveEquipmentSlotNamedNativeIssue(IGameEvent payload)
    {
        if (payload is not TieredRoundConversionUseIssuedEvent { Receipt: var receipt } || !IsEquipmentSlotNamedSkill(receipt.Source.SkillId))
            return;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUsePaidEvent>().Select(e => e.Payment).LastOrDefault(p => p.Source == receipt.Source && p.RoundNumber == receipt.RoundNumber && p.EffectiveKind == receipt.EffectiveKind && p.GameplayHash == receipt.GameplayHash && p.ActualTurn == receipt.ActualTurnNumber && p.ActualTurnOwner == receipt.ActualTurnOwnerSeat && p.PhaseInstance == receipt.PhaseInstanceId && receipt.ActorSeat == p.Source.OwnerSeat && receipt.MaterialCount == 0 && receipt.FrozenTier == 2 && receipt.StateId == EquipmentSlotNamedTier && receipt.UsageId == (p.Yin ? "yin" : "yang") && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUseConsumedEvent>().Any(e => e.PaymentFrameId == p.FrameId) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentSlotNamedUseIssuedEvent>().Any(e => e.PaymentFrameId == p.FrameId)) ?? throw new InvalidOperationException("An issued native Youlong use lost its exact consumed slot payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseIssuedEvent(paid.FrameId, receipt.OwnerFrameId, receipt.CardActionId));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26100)]
    internal bool ResumeEquipmentSlotNamedUse(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { EquipmentSlotNamedUse: { } draft } frame || frame.Id != id)
            return false;
        if (draft.Stage == EquipmentSlotNamedUseStage.ChoosingSlot)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishEquipmentSlotNamedChoice(frame);
            return true;
        }

        if (draft.Payment is not { } paid)
            throw new InvalidOperationException("The paid slot stage lost its own invoice.");
        if (DrainEquipmentSlotNamedChildren(frame.Id))
            return true;
        _coreSkillProgramsOperations.FinishProgramSkill(frame with { PendingMovementContinuation = null }, true);
        if (_coreWenyiPeachSuppliesOperations.ResumeWenyiEquipmentSlotPeach(draft))
            return true;
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseCancelledEvent(paid.FrameId));
            if (draft.Intent == EquipmentSlotNamedUseIntent.SayingSlash && draft.OriginalParentId is { } sayingParent && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame saying && saying.Id == sayingParent)
            {
                _characterSunYuanOlOperations.FinishSayingSuppliedSlashFailure(saying);
                return true;
            }

            if (draft.OriginalParentId is { } pingduanParent)
                _characterKeBiNengOlOperations.TryCancelKeBiNengPingduanBasicPayment(pingduanParent);
            if (draft.OriginalParentId is { } liwenParent)
                _characterKongRongMouOlOperations.Value.TryCancelMouKrLiwenPayment(liwenParent);
            return true;
        }

        if (draft.Intent is EquipmentSlotNamedUseIntent.DyingPeach or EquipmentSlotNamedUseIntent.DyingAlcohol && _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } rescued && _coreMatchParticipantsPlayersAccess._players[rescued.VictimSeat].Hp > 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseCancelledEvent(paid.FrameId));
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeDyingOperations.CompleteDying(rescued, survived: true);
            return true;
        }

        if (draft.Intent == EquipmentSlotNamedUseIntent.Play && !EquipmentSlotNamedPlayStillLegal(actor, draft))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseCancelledEvent(paid.FrameId));
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
            if (draft.OriginalParentId is { } pingduanParent && _characterKeBiNengOlOperations.TryCancelKeBiNengPingduanBasicPayment(pingduanParent))
                return true;
            if (draft.OriginalParentId is { } liwenParent && _characterKongRongMouOlOperations.Value.TryCancelMouKrLiwenPayment(liwenParent))
                return true;
            if (actor.IsHuman && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor.Seat)
                _coreNativePromptBoundaryOperations.RequestHumanPlay();
            return true;
        }

        if (draft.Intent == EquipmentSlotNamedUseIntent.SayingSlash && !_coreSayingTieredSuppliesOperations.SayingEquipmentSlotUseStillLegal(actor, draft))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedUseCancelledEvent(paid.FrameId));
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
            if (draft.OriginalParentId is { } sayingParent && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame saying && saying.Id == sayingParent)
                _characterSunYuanOlOperations.FinishSayingSuppliedSlashFailure(saying);
            return true;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = draft.OriginalDecision;
        if (draft.Intent == EquipmentSlotNamedUseIntent.SayingSlash)
            _coreSayingTieredSuppliesOperations.ResolveSayingTieredSlash(draft.ResponseChoice!);
        else if (draft.Intent == EquipmentSlotNamedUseIntent.Play)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreTieredRoundZeroUsesOperations.ExecuteTieredRoundZeroPlay(actor, draft.PlayAction!);
        }
        else if (draft.Intent == EquipmentSlotNamedUseIntent.Dodge)
            _coreTieredRoundZeroResponsesOperations.ResolveTieredRoundZeroDodge(actor, draft.Source);
        else if (draft.Intent == EquipmentSlotNamedUseIntent.Nullification)
            _coreTieredRoundZeroResponsesOperations.ResolveTieredRoundZeroCounterspell(actor, draft.Source);
        else if (draft.Intent is EquipmentSlotNamedUseIntent.DyingPeach or EquipmentSlotNamedUseIntent.DyingAlcohol)
            _coreTieredRoundZeroResponsesOperations.ResolveTieredRoundZeroResponse(draft.ResponseChoice!, false);
        else
            _coreTieredRoundZeroUsesOperations.TryResolveTieredRoundZeroForcedSlash(draft.ResponseChoice!, false);
        return true;
    }

    internal bool EquipmentSlotNamedPlayStillLegal(CharacterState actor, EquipmentSlotNamedUseDraft draft)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, draft.EffectiveKind, CardActionType.Use) || draft.PlayAction is not { } action)
            return false;
        var kind = draft.EffectiveKind;
        if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true).Any(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId && o.TargetSeats.SequenceEqual(action.TargetSeats));
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return action.TargetSeats.Count > 0 && action.TargetSeats.All(t => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], new(0, kind, Suit.None, 0), draft.Source, kind, noEffectiveRank: true, physicalCardIds: []) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[t], _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, draft.Source), kind, effectiveRank: 0, physicalCardIds: []));
        if (kind == CardKind.Peach)
            return _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None);
        return kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26100)]
    internal bool ReturnEquipmentSlotNamedUseMovement(ProgramSkillFrame frame)
    {
        if (frame.EquipmentSlotNamedUse is null)
            return false;
        return ResumeEquipmentSlotNamedUse(frame.Id);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26100)]
    internal bool ContinuePaidWoLongFengChu(ProgramSkillFrame frame) => frame.EquipmentSlotNamedUse?.Payment is not null || frame.Luanfeng is not null;
    internal bool DrainEquipmentSlotNamedChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
}
