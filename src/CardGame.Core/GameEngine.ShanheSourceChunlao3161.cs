using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int[] MatchingShanheSourceChunlaoIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) =>
        batch.Movements.Select((move, index) => (move, index, origin: AdjacentDiscardOrigin(move)))
            .Where(x => x.move.To == CardLocation.DiscardPile && _cardZones.GetLocation(x.move.CardId) == CardLocation.DiscardPile &&
                x.origin is { } o && SlashKinds.Contains(o.EffectiveKind) && _players[o.SourceSeat].IsAlive &&
                GetCombatDistance(o.SourceSeat, candidate.OwnerSeat) <= 1).Select(x => x.index).ToArray();

    private SkillProgramStepOutcome BeginShanheSourceChunlaoStorage(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShanheSourceJiangdongTrioIds.Chunlao || f.ShanheSourceChunlaoStorage is not null ||
            f.WindowContext is not { MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count ||
            AdjacentDiscardOrigin(batch.Movements[index]) is not { } origin ||
            !MatchingShanheSourceChunlaoIndexes(batch, new(f.OwnerSeat, f.SkillId, GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0)).Contains(index))
            throw new InvalidOperationException("Source3161 requires its exact original distance-one discarded Slash.");
        ReplaceRuntimeTop(f = f with { ShanheSourceChunlaoStorage = new(batch.Movements[index].CardId, origin.SourceSeat, batch.Movements[index].Sequence) });
        var choices = ShanheSourceChunlaoStorageChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, origin.SourceSeat, $"醇醪：是否将此杀置于 {_players[f.OwnerSeat].Name} 的武将牌上？",
            [batch.Movements[index].CardId], [], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = false, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[origin.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private static IReadOnlyList<PromptChoice> ShanheSourceChunlaoStorageChoices(ProgramSkillFrame f) =>
        Array.AsReadOnly(new[] { "store", "decline" }.Select(branch => new PromptChoice(new($"source-chunlao.{f.Id}.{branch}"),
            branch == "store" ? "将此杀存为醇" : "不存入醇", [], [], new Dictionary<string, string>
            { ["program-action"] = "shanhe-source-chunlao-store", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
              ["branch"] = branch })).ToArray());

    [ProgramAnswer("shanhe-source-chunlao-store")]
    private void AnswerShanheSourceChunlaoStorage(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceChunlaoStorage: { Paid: false } r } f ||
            _pendingDecision?.PlayerSeat != r.SourceSeat || !ShanheSourceChunlaoStorageChoices(f).Any(c => AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Source3161 requires the original discarding player's exact choice.");
        ClearPendingDecision();
        if (choice.Parameters["branch"] == "decline" || !_players[r.SourceSeat].IsAlive || !_players[f.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _cardZones.GetLocation(r.CardId) != CardLocation.DiscardPile)
        { ReplaceRuntimeTop(f with { ShanheSourceChunlaoStorage = null }); AdvanceRuntimeProgram(f.Id); return; }
        ReplaceRuntimeTop(f = f with { ShanheSourceChunlaoStorage = r with { Paid = true }, Step = ResolutionFrameStep.ResolvingEffect });
        var card = _cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == r.CardId);
        MoveCard(card, CardLocation.DiscardPile, new(CardZoneKind.Chunlao, f.OwnerSeat), new("shanhe.source-chunlao-3161.store"), moved =>
        {
            var current = GetActiveProgramFrame(f.Id);
            ReplaceRuntimeTop(current with { ShanheSourceChunlaoStorage = current.ShanheSourceChunlaoStorage! with { StoreSequence = moved.Sequence } });
            AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoStoredEvent(f.Id, f.OwnerSeat, r.SourceSeat, r.CardId, r.DiscardSequence, moved.Sequence));
        });
        AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24921)]
    private bool ResumeShanheSourceChunlaoStorage(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != id || f.ShanheSourceChunlaoStorage is not { } r) return false;
        if (!r.Paid) return true;
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id)) return true;
        ReplaceRuntimeTop(f with { ShanheSourceChunlaoStorage = null }); return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24921)]
    private void AssertShanheSourceChunlaoStorage(ProgramSkillFrame f)
    {
        if (f.ShanheSourceChunlaoStorage is not { } r) return;
        if (f.SkillId != ShanheSourceJiangdongTrioIds.Chunlao || f.WindowContext is not
                { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } ||
            index < 0 || index >= batch.Movements.Count || batch.Movements[index].CardId != r.CardId ||
            batch.Movements[index].Sequence != r.DiscardSequence || AdjacentDiscardOrigin(batch.Movements[index])?.SourceSeat != r.SourceSeat ||
            r.Paid && !_cardMovements.Any(m => m.Sequence == r.StoreSequence && m.CardId == r.CardId && m.From == CardLocation.DiscardPile &&
                m.To == new CardLocation(CardZoneKind.Chunlao, f.OwnerSeat) && m.Reason.Value == "shanhe.source-chunlao-3161.store"))
            throw new InvalidOperationException("Source3161 storage lost its original discard or exact committed pile transfer.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferShanheSourceChunlaoStorage)]
    private PromptChoice AiShanheSourceChunlaoStorage(PendingDecision p, ProgramSkillFrame f) =>
        p.Choices.Single(c => c.Parameters["branch"] == (f.ShanheSourceChunlaoStorage!.SourceSeat == f.OwnerSeat ||
            AreProgramDistributionAllies(_players[f.ShanheSourceChunlaoStorage.SourceSeat], _players[f.OwnerSeat]) ? "store" : "decline"));
}
