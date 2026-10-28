using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourceQingjianSkill = "shanhe:source-qingjian-721";
    private const string ShanheSourceQingjianGiveReason = "skill-program.shanhe-source-qingjian-721.give";

    private IReadOnlyList<int> SourceQingjianOriginalGains(int owner, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } batch } ||
            batch.MovementTiming is null || batch.MovementTiming is { Phase: TurnPhase.Draw, PhaseActorSeat: var actor } && actor == owner)
            return [];
        return Array.AsReadOnly(batch.Movements.Select((move, index) => (move, index)).GroupBy(item => item.move.CardId)
            .Select(group => group.OrderBy(item => item.move.Sequence).Last())
            .Where(item => item.move.To == CardLocation.Hand(owner) && item.move.From != item.move.To &&
                _cardZones.GetLocation(item.move.CardId) == CardLocation.Hand(owner) && !GetAdvancedCard(item.move.CardId).IsGeneralWeapon)
            .OrderBy(item => item.index).Select(item => item.move.CardId).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24750,
        SkillProgramEffectOp.DistributeShanheSourceQingjianGains)]
    private bool CanOfferSourceQingjian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        _players[candidate.OwnerSeat].IsAlive && SourceQingjianOriginalGains(candidate.OwnerSeat, context).Count > 0 &&
        _players.Any(player => player.IsAlive && player.Seat != candidate.OwnerSeat);

    private SkillProgramStepOutcome DistributeSourceQingjianGains(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var original = SourceQingjianOriginalGains(frame.OwnerSeat, frame.WindowContext);
        if (frame.ShanheSourceQingjian is not null || frame.SkillId != ShanheSourceQingjianSkill ||
            frame.WindowContext?.MovementBatch is not { } batch || original.Count == 0 ||
            _resolutionStack.Count < 2 || _resolutionStack[^2] is not CardsMovedTriggerWindowFrame window ||
            frame.WindowContext.ParentFrameId != window.Id || window.Batch != batch ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]))
            throw new InvalidOperationException("Source Qingjian requires its original actual gain batch and owned movement candidate.");
        ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = new(batch.Id, frame.InstructionIndex,
            Array.AsReadOnly(original.ToArray()), Array.AsReadOnly(Array.Empty<int>())) });
        PublishSourceQingjian(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> SourceQingjianChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheSourceQingjian!;
        var cards = receipt.OriginalCardIds.Where(id => !receipt.GivenCardIds.Contains(id) &&
            _cardZones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)).Select(GetAdvancedCard).ToArray();
        var choices = cards.SelectMany(card => _players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat)
            .Select(player => new PromptChoice(new ChoiceId($"source-qingjian.{frame.Id}.{receipt.GivenCardIds.Count}.{card.Id}.{player.Seat}"),
                $"将本次获得的【{card.DisplayName}】交给 {player.Name}", [card.Id], [player.Seat],
                new Dictionary<string, string> { ["program-action"] = "source-qingjian-distribute", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "give" }))).ToList();
        choices.Add(new(new ChoiceId($"source-qingjian.{frame.Id}.{receipt.GivenCardIds.Count}.finish"),
            receipt.GivenCardIds.Count == 0 ? "取消分配" : "完成分配", [], [],
            new Dictionary<string, string> { ["program-action"] = "source-qingjian-distribute", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "finish" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    private void PublishSourceQingjian(ProgramSkillFrame frame)
    {
        if (!_players[frame.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        { FinishSourceQingjian(frame); return; }
        var choices = SourceQingjianChoices(frame);
        if (!choices.Any(choice => choice.Cards.Count != 0)) { FinishSourceQingjian(frame); return; }
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "清俭：分配本批仍在手中的原牌，或结束分配。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("source-qingjian-distribute")]
    private void AnswerSourceQingjian(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceQingjian: { ReturningChild: false } receipt } frame ||
            _pendingDecision?.PlayerSeat != frame.OwnerSeat || receipt.InstructionIndex != frame.InstructionIndex ||
            !SourceQingjianChoices(frame).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Source Qingjian requires its current original-gain entity and exact recipient choice.");
        ClearPendingDecision();
        ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters["branch"] == "finish") { FinishSourceQingjian(frame); return; }
        var cardId = selected.Cards.Single(); var target = selected.Targets.Single(); var before = _movementSequence;
        ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = receipt with
        { ReturningChild = true, GivenCardIds = Array.AsReadOnly(receipt.GivenCardIds.Append(cardId).ToArray()),
            PaidCardId = cardId, RecipientSeat = target, Before = before, After = before },
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        MoveNativeGivenCards(frame.Id, frame.OwnerSeat, target, [cardId], new(ShanheSourceQingjianGiveReason), settlement =>
        {
            var current = GetActiveProgramFrame(frame.Id);
            var after = settlement.Movements.Select(move => (long)move.Sequence).DefaultIfEmpty(before).Max();
            ReplaceRuntimeTop(current with { ShanheSourceQingjian = current.ShanheSourceQingjian! with { After = after } });
            AdvanceEventRulesAndQueueFact(new ShanheSourceQingjianGivenEvent(frame.Id, receipt.BatchId, frame.OwnerSeat, target, cardId, before, after));
        });
        frame = GetActiveProgramFrame(frame.Id);
        ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = frame.ShanheSourceQingjian! with { After = _movementSequence } });
        if (!TryDrainFireTargetMovement(frame)) ReturnSourceQingjianMovement(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24750)]
    private bool ContinueSourceQingjian(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceQingjian is not { } receipt) return false;
        if (frame.PendingMovementContinuation is not null)
        { if (!TryDrainFireTargetMovement(frame)) ReturnSourceQingjianMovement(frame); return true; }
        if (receipt.ReturningChild) throw new InvalidOperationException("Source Qingjian must return its original real give children first.");
        if (_pendingDecision is null) PublishSourceQingjian(frame);
        return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 24750)]
    private bool ReturnSourceQingjianMovement(ProgramSkillFrame frame)
    {
        if (frame.ShanheSourceQingjian is not { ReturningChild: true } receipt) return false;
        if (frame.PendingMovementContinuation is not { } pending || !SourceQingjianMovement(frame, SourceQingjianPausedEffect(frame), pending))
            throw new InvalidOperationException("Source Qingjian lost its exact paid give movement return.");
        if (TryDrainFireTargetMovement(frame)) return true;
        ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null,
            ShanheSourceQingjian = receipt with { ReturningChild = false } });
        PublishSourceQingjian(frame);
        return true;
    }

    private SkillProgramEffect? SourceQingjianPausedEffect(ProgramSkillFrame frame) => frame.InstructionIndex < 1 ? null :
        ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 24750)]
    private bool SourceQingjianMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        frame.ShanheSourceQingjian is { ReturningChild: true } receipt && receipt.InstructionIndex == frame.InstructionIndex &&
        effect?.Op == SkillProgramEffectOp.DistributeShanheSourceQingjianGains && effect == SourceQingjianPausedEffect(frame) &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null &&
        receipt.OriginalCardIds.Contains(receipt.PaidCardId) && receipt.GivenCardIds.Contains(receipt.PaidCardId) &&
        receipt.RecipientSeat != frame.OwnerSeat && receipt.After >= receipt.Before;

    private void FinishSourceQingjian(ProgramSkillFrame frame)
    { ReplaceRuntimeTop(frame = frame with { ShanheSourceQingjian = null }); FinishProgramSkill(frame, completed: true); }

    [ProgramAiChoice(SkillProgramEffectOp.DistributeShanheSourceQingjianGains)]
    private PromptChoice ChooseSourceQingjian(PendingDecision decision, ProgramSkillFrame frame)
    {
        // Human choices remain legal; AI keeps a Qingjian-origin gift rather
        // than bouncing the same original entity between two skill owners.
        if (frame.WindowContext?.MovementBatch?.OriginSkillId == ShanheSourceQingjianSkill)
            return decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
        var choices = decision.Choices.Where(choice => choice.Cards.Count == 1 && choice.Targets.Count == 1 &&
            OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], _players[choice.Targets[0]])).ToArray();
        return choices.OrderBy(choice => CardCatalog.Get(GetAdvancedCard(choice.Cards[0]).Kind).HandKeepValue).FirstOrDefault() ??
            decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
    }
    [ProgramCapability]
    private sealed class ShanheSourceQingjianHost(GameEngine engine) : IShanheSourceQingjianHost
    { public SkillProgramStepOutcome DistributeOriginalGains(ProgramSkillFrame frame) => engine.DistributeSourceQingjianGains(frame); }
}
