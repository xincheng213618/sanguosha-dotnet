namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryMobileDangxian = "boundary:dangxian-mobile", BoundaryMobileFuli = "boundary:fuli-mobile",
        BoundaryLiaoHuaDiscardAction = "boundary-liao-hua-discard-slash", BoundaryLiaoHuaDiscardReason = "skill-program.boundary:liao-hua-mobile.discard-slash";

    [ProgramCapability]
    private sealed class BoundaryLiaoHuaMobileHost(GameEngine engine) : IBoundaryLiaoHuaMobileHost
    {
        public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) =>
            op == SkillProgramEffectOp.ObtainBoundaryLiaoHuaDiscardSlash ? engine.BeginBoundaryLiaoHuaDiscardSlash(frame) : engine.TurnOverBoundaryLiaoHuaIfUniqueHighestHp(frame);
    }

    private SkillProgramStepOutcome BeginBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != BoundaryMobileDangxian || frame.TriggerId != "discard-slash-extra-play" || frame.InstructionIndex != 1 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || frame.BoundaryLiaoHuaDiscardSlash is not null)
            throw new InvalidOperationException("Mobile Dangxian requires its fresh original own-turn starting instruction.");
        var cards = _cardZones.CardsAt(CardLocation.DiscardPile).Where(card => IsSlashCard(card.Kind)).Select(card => card.Id).ToArray();
        if (cards.Length == 0 || !_players[frame.OwnerSeat].IsAlive) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame = frame with { BoundaryLiaoHuaDiscardSlash = new(new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId),
            frame.GameplayHash, context.ParentFrameId, frame.InstructionIndex) { Candidates = cards } });
        PublishBoundaryLiaoHuaDiscardSlash(frame); return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> BoundaryLiaoHuaDiscardChoices(ProgramSkillFrame frame) => Array.AsReadOnly(
        frame.BoundaryLiaoHuaDiscardSlash!.Candidates.Select(id => {
            var card = _cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == id);
            return new PromptChoice(new($"boundary-liao-hua.frame-{frame.Id}.card-{id}"), $"获得弃牌堆的【{card.DisplayName}】（{card.RankText}）", [id], [],
                new Dictionary<string, string> { ["program-action"] = BoundaryLiaoHuaDiscardAction });
        }).ToArray());

    private void PublishBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame)
    {
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "当先：选择获得弃牌堆中的一张杀。", frame.BoundaryLiaoHuaDiscardSlash!.Candidates, [], frame.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = frame.OwnerSeat, Choices = BoundaryLiaoHuaDiscardChoices(frame), SkillPrompt = new(frame.SkillId, skill.Name, "当先 · 获得杀", skill.Description) };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryLiaoHuaDiscardAction)]
    private void AnswerBoundaryLiaoHuaDiscardSlash(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryLiaoHuaDiscardSlash.CardId: null } frame ||
            !ValidBoundaryLiaoHuaDiscardSlash(frame) || _pendingDecision?.PlayerSeat != frame.OwnerSeat ||
            !AssistedChoicesEqual([selected], BoundaryLiaoHuaDiscardChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mobile Dangxian requires its current original owner's published actual discarded Slash.");
        ClearPendingDecision();
        if (!_players[frame.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        { CancelProgramBindingAndCleanup(frame, "当先获得杀之前原技能来源已失效。"); return; }
        var receipt = frame.BoundaryLiaoHuaDiscardSlash! with { CardId = selected.Cards.Single(), Before = _movementSequence };
        ReplaceRuntimeTop(frame = frame with { BoundaryLiaoHuaDiscardSlash = receipt });
        MoveProgramCardsFromMultipleSources([receipt.CardId.Value], CardLocation.Hand(frame.OwnerSeat), new(BoundaryLiaoHuaDiscardReason), (batch, moves) => {
            var active = GetActiveProgramFrame(frame.Id);
            ReplaceRuntimeTop(active with { BoundaryLiaoHuaDiscardSlash = receipt with { BatchId = batch, After = _movementSequence } });
            AdvanceEventRulesAndQueueFact(new BoundaryLiaoHuaDiscardSlashObtainedEvent(frame.Id, receipt.InstructionIndex, frame.OwnerSeat,
                receipt.CardId.Value, batch, receipt.Before, _movementSequence));
        });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
        { ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id) with { BoundaryLiaoHuaDiscardSlash = null }); AdvanceRuntimeProgram(frame.Id); }
    }

    private bool ValidBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiaoHuaDiscardSlash is not { } receipt || frame.SkillId != BoundaryMobileDangxian || frame.TriggerId != "discard-slash-extra-play" ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.ParentFrameId != receipt.WindowFrameId ||
            receipt.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash ||
            receipt.InstructionIndex != frame.InstructionIndex || receipt.Candidates.Count == 0 || receipt.Candidates.Distinct().Count() != receipt.Candidates.Count) return false;
        if (receipt.CardId is null)
            return receipt.Candidates.All(id => _cardZones.CardsAt(CardLocation.DiscardPile).Any(card => card.Id == id && IsSlashCard(card.Kind)));
        var moves = _cardMovements.Where(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After).ToArray();
        return receipt.Candidates.Contains(receipt.CardId.Value) && receipt.BatchId > 0 && moves is [var paid] && paid.CardId == receipt.CardId &&
            paid.From == CardLocation.DiscardPile && paid.To == CardLocation.Hand(frame.OwnerSeat) && IsSlashCard(paid.CardKind) && paid.Reason.Value == BoundaryLiaoHuaDiscardReason &&
            ProgramEventHistory<BoundaryLiaoHuaDiscardSlashObtainedEvent>().Count(fact => fact.FrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex &&
                fact.OwnerSeat == frame.OwnerSeat && fact.CardId == receipt.CardId && fact.BatchId == receipt.BatchId && fact.Before == receipt.Before && fact.After == receipt.After) == 1;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 55600)]
    private bool IsBoundaryLiaoHuaDiscardMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ObtainBoundaryLiaoHuaDiscardSlash && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null &&
        frame.BoundaryLiaoHuaDiscardSlash?.CardId is not null && ValidBoundaryLiaoHuaDiscardSlash(frame);

    [ProgramContinuation(ProgramContinuationKind.Movement, 55600)]
    private bool ReturnBoundaryLiaoHuaDiscardMovement(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiaoHuaDiscardSlash?.CardId is null) return false;
        if (!ValidBoundaryLiaoHuaDiscardSlash(frame)) throw new InvalidOperationException("Mobile Dangxian lost its exact paid discard gain and original instruction.");
        ReplaceRuntimeTop(frame with { BoundaryLiaoHuaDiscardSlash = null, PendingMovementContinuation = null }); AdvanceRuntimeProgram(frame.Id); return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 55600)]
    private bool ResumeBoundaryLiaoHuaDiscard(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryLiaoHuaDiscardSlash: { CardId: null } } frame || frame.Id != id) return false;
        if (!ValidBoundaryLiaoHuaDiscardSlash(frame)) throw new InvalidOperationException("Mobile Dangxian lost its original frozen public discard candidates.");
        if (_pendingDecision is null) PublishBoundaryLiaoHuaDiscardSlash(frame); return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55600)]
    private void AssertBoundaryLiaoHuaDiscard(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiaoHuaDiscardSlash is not { } receipt) return;
        if (!ValidBoundaryLiaoHuaDiscardSlash(frame) || receipt.Candidates is not System.Collections.IList { IsReadOnly: true } ||
            receipt.CardId is null && (_resolutionStack.LastOrDefault()?.Id != frame.Id || _pendingDecision?.PlayerSeat != frame.OwnerSeat ||
                !AssistedChoicesEqual(_pendingDecision.Choices, BoundaryLiaoHuaDiscardChoices(frame))))
            throw new InvalidOperationException("Mobile Dangxian lost its original frame, immutable candidates, current prompt or strict native movement invoice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ObtainBoundaryLiaoHuaDiscardSlash)]
    private PromptChoice ChooseBoundaryLiaoHuaDiscard(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.First();

    private SkillProgramStepOutcome TurnOverBoundaryLiaoHuaIfUniqueHighestHp(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != BoundaryMobileFuli || frame.TriggerId != "self-dying-recovery" || frame.InstructionIndex != 2 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse, TargetSeat: { } target } || target != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Fuli compares current HP only after its original self-dying recovery and child windows.");
        var owner = _players[frame.OwnerSeat];
        if (owner.IsAlive && owner.HasHpValue && _winner == Winner.None &&
            _players.Where(player => player.IsAlive && player.HasHpValue && player.Seat != owner.Seat).All(player => player.Hp < owner.Hp))
            SetProgramTargetFaceState(frame.Id, owner.Seat, owner.Seat, !owner.IsFaceDown);
        return SkillProgramStepOutcome.Continue;
    }
}
