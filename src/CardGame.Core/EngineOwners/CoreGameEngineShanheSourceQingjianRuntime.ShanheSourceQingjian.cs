#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceQingjianRuntime
{
    internal const string ShanheSourceQingjianSkill = "shanhe:source-qingjian-721";
    internal const string ShanheSourceQingjianGiveReason = "skill-program.shanhe-source-qingjian-721.give";
    internal IReadOnlyList<int> SourceQingjianOriginalGains(int owner, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch } || batch.MovementTiming is null || batch.MovementTiming is { Phase: TurnPhase.Draw, PhaseActorSeat: var actor } && actor == owner)
            return[];
        return Array.AsReadOnly(batch.Movements.Select((move, index) => (move, index)).GroupBy(item => item.move.CardId).Select(group => group.OrderBy(item => item.move.Sequence).Last()).Where(item => item.move.To == CardLocation.Hand(owner) && item.move.From != item.move.To && _cardZonesOperations._cardZones.GetLocation(item.move.CardId) == CardLocation.Hand(owner) && !_coreAdvancedLifecycleOperations.GetAdvancedCard(item.move.CardId).IsGeneralWeapon).OrderBy(item => item.index).Select(item => item.move.CardId).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24750, SkillProgramEffectOp.DistributeShanheSourceQingjianGains)]
    internal bool CanOfferSourceQingjian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && SourceQingjianOriginalGains(candidate.OwnerSeat, context).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != candidate.OwnerSeat);
    internal SkillProgramStepOutcome DistributeSourceQingjianGains(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var original = SourceQingjianOriginalGains(frame.OwnerSeat, frame.WindowContext);
        if (frame.ShanheSourceQingjian is not null || frame.SkillId != ShanheSourceQingjianSkill || frame.WindowContext?.MovementBatch is not { } batch || original.Count == 0 || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not CardsMovedTriggerWindowFrame window || frame.WindowContext.ParentFrameId != window.Id || window.Batch != batch || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]))
            throw new InvalidOperationException("Source Qingjian requires its original actual gain batch and owned movement candidate.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = new(batch.Id, frame.InstructionIndex, Array.AsReadOnly(original.ToArray()), Array.AsReadOnly(Array.Empty<int>())) });
        PublishSourceQingjian(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SourceQingjianChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheSourceQingjian!;
        var cards = receipt.OriginalCardIds.Where(id => !receipt.GivenCardIds.Contains(id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)).Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        var choices = cards.SelectMany(card => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat).Select(player => new PromptChoice(new ChoiceId($"source-qingjian.{(frame.Id)}.{(receipt.GivenCardIds.Count)}.{(card.Id)}.{(player.Seat)}"), $"将本次获得的【{(card.DisplayName)}】交给 {(player.Name)}", [card.Id], [player.Seat], new Dictionary<string, string> { ["program-action"] = "source-qingjian-distribute", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "give" }))).ToList();
        choices.Add(new(new ChoiceId($"source-qingjian.{(frame.Id)}.{(receipt.GivenCardIds.Count)}.finish"), receipt.GivenCardIds.Count == 0 ? "取消分配" : "完成分配", [], [], new Dictionary<string, string> { ["program-action"] = "source-qingjian-distribute", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "finish" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSourceQingjian(ProgramSkillFrame frame)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            FinishSourceQingjian(frame);
            return;
        }

        var choices = SourceQingjianChoices(frame);
        if (!choices.Any(choice => choice.Cards.Count != 0))
        {
            FinishSourceQingjian(frame);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "清俭：分配本批仍在手中的原牌，或结束分配。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("source-qingjian-distribute")]
    internal void AnswerSourceQingjian(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceQingjian: { ReturningChild: false } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || receipt.InstructionIndex != frame.InstructionIndex || !SourceQingjianChoices(frame).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Source Qingjian requires its current original-gain entity and exact recipient choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters["branch"] == "finish")
        {
            FinishSourceQingjian(frame);
            return;
        }

        var cardId = selected.Cards.Single();
        var target = selected.Targets.Single();
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = receipt with { ReturningChild = true, GivenCardIds = Array.AsReadOnly(receipt.GivenCardIds.Append(cardId).ToArray()), PaidCardId = cardId, RecipientSeat = target, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreHappyWeiYanGainDestinationsOperations.MoveNativeGivenCards(frame.Id, frame.OwnerSeat, target, [cardId], new(ShanheSourceQingjianGiveReason), settlement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var after = settlement.Movements.Select(move => (long)move.Sequence).DefaultIfEmpty(before).Max();
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { ShanheSourceQingjian = current.ShanheSourceQingjian!with { After = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceQingjianGivenEvent(frame.Id, receipt.BatchId, frame.OwnerSeat, target, cardId, before, after));
        });
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = frame.ShanheSourceQingjian!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
            ReturnSourceQingjianMovement(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24750)]
    internal bool ContinueSourceQingjian(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceQingjian is not { } receipt)
            return false;
        if (frame.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
                ReturnSourceQingjianMovement(frame);
            return true;
        }

        if (receipt.ReturningChild)
            throw new InvalidOperationException("Source Qingjian must return its original real give children first.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishSourceQingjian(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24750)]
    internal bool ReturnSourceQingjianMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceQingjian is not { ReturningChild: true } receipt)
            return false;
        if (frame.PendingMovementContinuation is not { } pending || !SourceQingjianMovement(frame, SourceQingjianPausedEffect(frame), pending))
            throw new InvalidOperationException("Source Qingjian lost its exact paid give movement return.");
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, ShanheSourceQingjian = receipt with { ReturningChild = false } });
        PublishSourceQingjian(frame);
        return true;
    }

    internal SkillProgramEffect? SourceQingjianPausedEffect(ProgramSkillFrame frame) => frame.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
    internal bool SourceQingjianMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => frame.ShanheSourceQingjian is { ReturningChild: true } receipt && receipt.InstructionIndex == frame.InstructionIndex && effect?.Op == SkillProgramEffectOp.DistributeShanheSourceQingjianGains && effect == SourceQingjianPausedEffect(frame) && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && receipt.OriginalCardIds.Contains(receipt.PaidCardId) && receipt.GivenCardIds.Contains(receipt.PaidCardId) && receipt.RecipientSeat != frame.OwnerSeat && receipt.After >= receipt.Before;
    internal void FinishSourceQingjian(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.DistributeShanheSourceQingjianGains)]
    internal PromptChoice ChooseSourceQingjian(PendingDecision decision, ProgramSkillFrame frame)
    {
        // Human choices remain legal; AI keeps a Qingjian-origin gift rather
        // than bouncing the same original entity between two skill owners.
        if (frame.WindowContext?.MovementBatch?.OriginSkillId == ShanheSourceQingjianSkill)
            return decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
        var choices = decision.Choices.Where(choice => choice.Cards.Count == 1 && choice.Targets.Count == 1 && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[choice.Targets[0]])).ToArray();
        return choices.OrderBy(choice => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards[0]).Kind).HandKeepValue).FirstOrDefault() ?? decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
    }

    SkillProgramStepOutcome IShanheSourceQingjianHost.DistributeOriginalGains(ProgramSkillFrame frame) => DistributeSourceQingjianGains(frame);
}
