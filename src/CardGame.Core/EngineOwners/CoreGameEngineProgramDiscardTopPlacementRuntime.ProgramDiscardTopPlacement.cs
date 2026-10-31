#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementRuntime
{
    SkillProgramStepOutcome IProgramDiscardTopPlacementHost.PutDiscardedCardsOnDrawPileTop(ProgramSkillFrame frame) => PutProgramDiscardedCardsOnDrawPileTop(frame);

    internal static bool IsDiscardMovementReason(CardMoveReason reason) => RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.IsDiscardMovementReason(reason);
    internal SkillProgramStepOutcome PutProgramDiscardedCardsOnDrawPileTop(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } || active.DiscardTopPlacement is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Discard-top placement requires a clean frozen movement window.");
        var ids = batch.Movements.Where(move => _coreNeighborDiscardOpportunitiesOperations.Value.IsProgramDiscardTopMovement(active, move) && _cardZonesOperations._cardZones.GetLocation(move.CardId) == CardLocation.DiscardPile).Select(move => move.CardId).Distinct().ToArray();
        if (ids.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        active = active with
        {
            DiscardTopPlacement = new(ids, [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramDiscardTopPlacement(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramDiscardTopPlacement(ProgramSkillFrame frame)
    {
        var state = frame.DiscardTopPlacement ?? throw new InvalidOperationException("The discard-top draft is missing.");
        var chosen = state.SelectedCardIds.ToHashSet();
        var choices = state.CandidateCardIds.Where(id => !chosen.Contains(id)).Select(id =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == id);
            return new PromptChoice(new ChoiceId($"program-discard-top.{(frame.Id)}.select.{(id)}"), $"选择【{(card.DisplayName)}】置于牌堆顶（按下一张先摸的顺序）。", [id], [], new Dictionary<string, string> { ["program-action"] = "discard-top-select" });
        }).ToList();
        choices.Add(new PromptChoice(new ChoiceId($"program-discard-top.{(frame.Id)}.finish"), "完成排序，其余牌留在弃牌堆。", [], [], new Dictionary<string, string> { ["program-action"] = "discard-top-finish" }));
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, "依次选择要置于牌堆顶的弃牌，可随时完成。", state.CandidateCardIds, [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, skill.Name, $"{(skill.Name)} · 弃牌排序", "所选第一张将最先摸到。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("discard-top-select", "discard-top-finish")]
    internal void ResolveProgramDiscardTopPlacementChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Discard-top placement lost its frame.");
        var state = frame.DiscardTopPlacement ?? throw new InvalidOperationException("Discard-top placement lost its draft.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat || !decision.Choices.Any(item => item.Id == choice.Id))
            throw new InvalidOperationException("Discard-top placement choice is not current.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || state.CandidateCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DiscardPile))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "弃牌已变化，牌堆顶放置取消。");
            return;
        }

        var action = choice.Parameters.GetValueOrDefault("program-action");
        if (action == "discard-top-select" && choice.Cards is [var id] && state.CandidateCardIds.Contains(id) && !state.SelectedCardIds.Contains(id))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            frame = frame with
            {
                DiscardTopPlacement = state with
                {
                    SelectedCardIds = [..state.SelectedCardIds, id]
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishProgramDiscardTopPlacement(frame);
            return;
        }

        if (action != "discard-top-finish" || choice.Cards.Count != 0)
            throw new InvalidOperationException("Invalid discard-top placement action.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DiscardTopPlacement = null });
        var cards = state.SelectedCardIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(card => card.Id == id)).ToArray();
        if (cards.Length > 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DiscardPile, CardLocation.DrawPile, new CardMoveReason($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.PutDiscardedCardsOnDrawPileTop)}"));
            _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(state.SelectedCardIds);
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
