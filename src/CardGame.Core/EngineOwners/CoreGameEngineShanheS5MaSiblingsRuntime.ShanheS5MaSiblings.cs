#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5MaSiblingsRuntime
{
    internal const string ShanheS5TieqiDiscardReason = "skill-program.shanhe-tieqi.discard";
    SkillProgramStepOutcome IShanheS5TieqiProgramHost.SuppressAndJudgeAllColor(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreSlashSuitDiscardOperations.Value.BeginProgramSlashSuitDiscard(frame, effect);
    internal bool IsShanheS5Tieqi(ProgramSkillFrame frame) => frame.TriggerId is not null && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger(frame)).HasOperation(SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeColorAllDiscard);
    internal Card[] ShanheS5TieqiColorCards(ProgramSkillFrame frame)
    {
        var draft = frame.SlashSuitDiscard!;
        if (draft.FinalSuit is null or Suit.None)
            return[];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat])).Where(card => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat], card)is var suit && suit != Suit.None && CoreNativeCardClassificationsRuntime.IsRedSuit(suit) == CoreNativeCardClassificationsRuntime.IsRedSuit(draft.FinalSuit.Value)).ToArray();
    }

    internal bool CanPayShanheS5Tieqi(ProgramSkillFrame frame, Card[] cards) => cards.Length > 0 && cards.All(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.SlashSuitDiscard!.TargetSeat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    internal IReadOnlyList<PromptChoice> ShanheS5TieqiChoices(ProgramSkillFrame frame)
    {
        var cards = ShanheS5TieqiColorCards(frame);
        var choices = new List<PromptChoice>();
        if (CanPayShanheS5Tieqi(frame, cards))
            choices.Add(new(new($"shanhe-tieqi.{(frame.Id)}.pay"), $"弃置全部{((CoreNativeCardClassificationsRuntime.IsRedSuit(frame.SlashSuitDiscard!.FinalSuit!.Value) ? "红" : "黑"))}色手牌及装备牌（{(cards.Length)}张），使此杀可被抵消。", cards.Select(card => card.Id).ToArray(), [], new Dictionary<string, string> { ["program-action"] = "shanhe-tieqi-color" }));
        choices.Add(new(new($"shanhe-tieqi.{(frame.Id)}.decline"), "不弃置，不能抵消此杀。", [], [], new Dictionary<string, string> { ["program-action"] = "shanhe-tieqi-skip" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShanheS5Tieqi(ProgramSkillFrame frame)
    {
        var draft = frame.SlashSuitDiscard!;
        var choices = ShanheS5TieqiChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, $"【{(skill.Name)}】是否弃置全部与判定颜色相同的手牌及装备牌？", choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.TargetSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool ResumeShanheS5Tieqi(ProgramSkillFrame frame)
    {
        var draft = frame.SlashSuitDiscard!;
        if (frame.PendingMovementContinuation is not null)
            return true;
        _ = _coreSlashSuitDiscardOperations.Value.SlashSuitDiscardAttack(frame);
        if (draft.Stage is ProgramSlashSuitDiscardStage.Judging or ProgramSlashSuitDiscardStage.Choosing)
            return true;
        if (draft.Stage == ProgramSlashSuitDiscardStage.PaymentMovement)
        {
            if (!ValidShanheS5TieqiPayment(frame))
                throw new InvalidOperationException("The whole-color Tieqi cost lost its original real discard cohort.");
            FinishShanheS5Tieqi(frame, true);
            return false;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || draft.FinalSuit is null or Suit.None || !CanPayShanheS5Tieqi(frame, ShanheS5TieqiColorCards(frame)))
        {
            FinishShanheS5Tieqi(frame, false);
            return false;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SlashSuitDiscard = draft with { Stage = ProgramSlashSuitDiscardStage.Choosing } });
        PublishShanheS5Tieqi(frame);
        return true;
    }

    [ProgramAnswer("shanhe-tieqi-color", "shanhe-tieqi-skip")]
    internal void ResolveShanheS5TieqiChoice(PromptChoice choice)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (!IsShanheS5Tieqi(frame) || frame.SlashSuitDiscard is not { Stage: ProgramSlashSuitDiscardStage.Choosing } draft || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.TargetSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ShanheS5TieqiChoices(frame).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Whole-color Tieqi requires its exact published target-private cohort.");
        _ = _coreSlashSuitDiscardOperations.Value.SlashSuitDiscardAttack(frame);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters.GetValueOrDefault("program-action") == "shanhe-tieqi-skip" || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishShanheS5Tieqi(frame, false);
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var cards = ShanheS5TieqiColorCards(frame);
        if (!CanPayShanheS5Tieqi(frame, cards) || !cards.Select(card => card.Id).SequenceEqual(choice.Cards))
            throw new InvalidOperationException("Whole-color Tieqi cannot partially pay a changed or protected cohort.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SlashSuitDiscard = draft with { Stage = ProgramSlashSuitDiscardStage.PaymentMovement }, ShanheS5Tieqi = new(before, before, 0, cards.Select(card => card.Id).ToArray(), cards.Select(card => _cardZonesOperations._cardZones.GetLocation(card.Id)).ToArray()) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, draft.TargetSeat, choice.Cards, new(ShanheS5TieqiDiscardReason), (batch, moves) =>
        {
            var owner = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(owner with { ShanheS5Tieqi = owner.ShanheS5Tieqi!with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        });
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheS5Tieqi = frame.ShanheS5Tieqi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5TieqiPaidEvent(frame.Id, draft.CardUseFrameId, draft.TargetSeat, choice.Cards.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool ValidShanheS5TieqiPayment(ProgramSkillFrame frame)
    {
        if (frame.ShanheS5Tieqi is not { } paid || frame.SlashSuitDiscard is not { Stage: ProgramSlashSuitDiscardStage.PaymentMovement } draft || paid.CardIds.Count == 0 || paid.CardIds.Count != paid.From.Count || paid.CardIds.Distinct().Count() != paid.CardIds.Count || paid.BatchId <= 0 || paid.Before >= paid.After || paid.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || paid.From.Any(from => from.OwnerSeat != draft.TargetSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return false;
        var logical = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == frame.Id && e.InitiatorSeat == frame.OwnerSeat && e.ExecutorSeat == draft.TargetSeat && e.SubjectSeat == draft.TargetSeat && e.ActualCount == paid.CardIds.Count && e.SequenceBefore == paid.Before && e.SequenceAfter == paid.After && e.Reason.Value == ShanheS5TieqiDiscardReason).ToArray();
        if (logical.Length != 1 || paid.CardIds.Select((id, index) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.InvocationId == logical[0].InvocationId && e.CardId == id && e.From == paid.From[index] && e.CompletionSequence > paid.Before && e.CompletionSequence <= paid.After) == 1).Any(valid => !valid))
            return false;
        return paid.CardIds.Select((id, index) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.CardId == id && move.Sequence > paid.Before && move.Sequence <= paid.After && move.Reason.Value == ShanheS5TieqiDiscardReason && (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move) == paid.From[index] || move.From == paid.From[index] && move.From.Zone == CardZoneKind.Equipment && move.To == CardLocation.OutsideGame && (_coreCardAttackStateOperations.GetAttackCard(id).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(move)))) == 1).All(value => value) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5TieqiPaidEvent>().Count(e => e.FrameId == frame.Id && e.CardUseFrameId == draft.CardUseFrameId && e.TargetSeat == draft.TargetSeat && e.Count == paid.CardIds.Count && e.Before == paid.Before && e.After == paid.After) == 1;
    }

    internal void FinishShanheS5Tieqi(ProgramSkillFrame frame, bool paid)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheS5Tieqi = null });
        _coreSlashSuitDiscardOperations.Value.FinishProgramSlashSuitDiscard(frame, paid);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5760)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5760)]
    internal bool CanContinuePaidShanheS5Tieqi(ProgramSkillFrame frame) => frame.ShanheS5Tieqi is not null && IsShanheS5Tieqi(frame) && ValidShanheS5TieqiPayment(frame);
    [ProgramAiChoice(SkillProgramEffectOp.SuppressCurrentSlashTargetAndJudgeColorAllDiscard)]
    internal PromptChoice SelectAiShanheS5Tieqi(PendingDecision decision, ProgramSkillFrame frame)
    {
        var cards = ShanheS5TieqiColorCards(frame);
        var player = _coreMatchParticipantsPlayersAccess._players[frame.SlashSuitDiscard!.TargetSeat];
        var pay = decision.Choices.FirstOrDefault(choice => choice.Cards.Count > 0);
        // Only the payer's own cards enter this estimate; hidden deck order is irrelevant.
        var hasRemainingDodge = _corePhysicalOwnedCardQueriesOperations.GetHand(player).Any(card => card.Kind == CardKind.Dodge && !cards.Any(cost => cost.Id == card.Id));
        return pay is not null && hasRemainingDodge && cards.Sum(card => CoreNativeAiDecisionsRuntime.GetKeepValue(card, player)) <= 8d ? pay : decision.Choices.Last();
    }
}
