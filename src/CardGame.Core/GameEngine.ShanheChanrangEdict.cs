using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheChanrangSkill = "shanhe:chanrang-edict-1510";
    private static readonly CardMoveReason ShanheChanrangGiveReason = new("shanhe.chanrang-edict.give");
    private bool HasShanheChanrangCapability => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheChanrangEdict);

    private void CaptureShanheChanrangFirstGains(CardMovementBatchContext batch)
    {
        if (!HasShanheChanrangCapability || batch.TurnNumber <= 0 || batch.MovementTiming is not { } timing) return;
        var seen = ProgramEventHistory<ShanheChanrangFirstGainEvent>().Where(fact => fact.ActualTurn == batch.TurnNumber)
            .Select(fact => fact.RecipientSeat).ToHashSet();
        foreach (var seat in batch.Movements.Where(move => move.To.Zone == CardZoneKind.Hand && move.To.OwnerSeat is not null)
            .Select(move => move.To.OwnerSeat!.Value).Distinct())
        {
            if (seat == timing.ActualTurnOwnerSeat || seen.Contains(seat)) continue;
            var gains = ActualHandGainEntities(batch, seat);
            if (gains.Length != 0)
                AdvanceEventRulesAndQueueFact(new ShanheChanrangFirstGainEvent(batch.TurnNumber, timing.ActualTurnOwnerSeat,
                    seat, batch.Id, gains.Min(gain => gain.GainSequence)));
        }
    }

    private bool IsShanheChanrangCandidate(ProgramTriggerCandidate candidate) => candidate.SkillId == ShanheChanrangSkill && candidate.BindingId == "first-foreign-turn-gain";
    private Card? ShanheChanrangEquipment(int owner, string instance) => GetEquipment(_players[owner]).FirstOrDefault(card =>
        card.Kind == CardKind.ShanRangZhaoShu && IsActiveProgramSourceEquipmentCard(owner, ShanheChanrangSkill, instance, card));
    private int[] ShanheChanrangRecipients(CardMovementBatchContext batch, int owner) => ProgramEventHistory<ShanheChanrangFirstGainEvent>()
        .Where(fact => fact.BatchId == batch.Id && fact.ActualTurn == batch.TurnNumber && fact.RecipientSeat != owner &&
            _players[fact.RecipientSeat].IsAlive).OrderBy(fact => fact.FirstMovementSequence).ThenBy(fact => fact.RecipientSeat)
        .Select(fact => fact.RecipientSeat).ToArray();

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 5434)]
    private void CollectShanheChanrangCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!HasShanheChanrangCapability) return;
        candidates.RemoveAll(IsShanheChanrangCandidate);
        if (!ProgramEventHistory<ShanheChanrangFirstGainEvent>().Any(fact => fact.BatchId == batch.Id)) return;
        foreach (var player in _players.Where(player => player.IsAlive && GetEquipment(player).Any(card => card.Kind == CardKind.ShanRangZhaoShu) &&
            ShanheChanrangRecipients(batch, player.Seat).Length != 0))
            candidates.AddRange(CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.CardsMoved)
                .Where(candidate => IsShanheChanrangCandidate(candidate) && ShanheChanrangEquipment(player.Seat, candidate.SkillInstanceId) is not null));
    }

    private ProgramSkillWindowContext CreateShanheChanrangContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) =>
        new(SkillProgramTriggerWindow.CardsMoved, window.Id, candidate.OwnerSeat,
            Facts: CaptureProgramTriggerFacts(_players[candidate.OwnerSeat]), MovementBatch: window.Batch);

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 5434)]
    private bool IsIssuedShanheChanrang(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        IsShanheChanrangCandidate(candidate) && context is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } &&
        context.OwnerSeat == candidate.OwnerSeat && ShanheChanrangRecipients(batch, candidate.OwnerSeat).Length != 0 &&
        _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault() is { } window && window.Id == context.ParentFrameId &&
        window.Batch.Id == batch.Id && window.CandidateIndex < window.Candidates.Count && window.CandidateIndex >= 0 &&
        window.Candidates[window.CandidateIndex] == candidate && ShanheChanrangEquipment(candidate.OwnerSeat, candidate.SkillInstanceId) is not null;

    [ProgramCapability]
    private sealed class ShanheChanrangCapability(GameEngine engine) : IShanheChanrangHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => engine.BeginShanheChanrang(frame); }

    private SkillProgramStepOutcome BeginShanheChanrang(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheChanrangSkill || frame.ShanheChanrang is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } context ||
            ShanheChanrangEquipment(frame.OwnerSeat, frame.SkillInstanceId) is not { } equipment)
            throw new InvalidOperationException("The edict lost its real equipment grant or first-gain batch.");
        ReplaceRuntimeTop(frame = frame with { ShanheChanrang = new() { ParentWindowId = context.ParentFrameId,
            BatchId = batch.Id, ActualTurn = batch.TurnNumber, EquipmentId = equipment.Id, Recipients = ShanheChanrangRecipients(batch, frame.OwnerSeat) } });
        ContinueShanheChanrang(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private Card[] ShanheChanrangCards(int giver) => GetHand(_players[giver]).Concat(GetEquipment(_players[giver])).ToArray();
    private IReadOnlyList<PromptChoice> ShanheChanrangChoices(ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!;
        PromptChoice Choice(string branch, string label, int? card = null) => new(new ChoiceId($"chanrang.{frame.Id}.{draft.Cursor}.{branch}.{card}"), label,
            card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "shanhe-chanrang", ["branch"] = branch });
        if (draft.Stage == "card") return ShanheChanrangCards(draft.GiverSeat!.Value)
            .Select(card => Choice("card", "交给对方：" + card.DisplayName, card.Id)).ToArray();
        if (draft.Stage != "branch") return [];
        var recipient = draft.Recipients[draft.Cursor];
        var choices = new List<PromptChoice> { Choice("skip", "放弃") };
        if (ShanheChanrangCards(frame.OwnerSeat).Length != 0) choices.Add(Choice("give", "交给" + _players[recipient].Name + "一张牌"));
        if (ShanheChanrangCards(recipient).Length != 0) choices.Add(Choice("take", "令" + _players[recipient].Name + "交给你一张牌"));
        return choices;
    }

    private void PublishShanheChanrang(ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!; var choices = ShanheChanrangChoices(frame);
        if (choices.Count == 0) { AdvanceShanheChanrangRecipient(frame); return; }
        ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Choices = choices }, Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = draft.Stage == "card" ? draft.GiverSeat!.Value : frame.OwnerSeat;
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, chooser, "禅让诏书：选择交牌方式", choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, TargetSeat = draft.Recipients[draft.Cursor],
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shanhe-chanrang")]
    private void AnswerShanheChanrang(PromptChoice choice)
    {
        var frame = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The edict lost its original frame.");
        AssertShanheChanrang(frame); var draft = frame.ShanheChanrang!;
        if (!AssistedChoicesEqual([choice], ShanheChanrangChoices(frame).Where(candidate => candidate.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The edict choice is no longer its exact issued card or branch.");
        ClearPendingDecision(); ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "skip") { AdvanceShanheChanrangRecipient(frame); return; }
        if (draft.Stage == "branch")
        {
            var other = draft.Recipients[draft.Cursor];
            ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Stage = "card", GiverSeat = branch == "give" ? frame.OwnerSeat : other,
                RecipientSeat = branch == "give" ? other : frame.OwnerSeat, Choices = [] } });
            PublishShanheChanrang(frame); return;
        }
        var giver = draft.GiverSeat!.Value; var recipient = draft.RecipientSeat!.Value; var card = choice.Cards.Single();
        if (!_players[giver].IsAlive || !_players[recipient].IsAlive || !ShanheChanrangCards(giver).Any(candidate => candidate.Id == card))
            throw new InvalidOperationException("The edict payment lost its actual giver, living recipient or owned entity.");
        ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Stage = "paid", PaidCardId = card, Choices = [] } });
        MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(recipient), ShanheChanrangGiveReason,
            NativeGiveCompletionRecords(frame.Id, giver, recipient, [card], _movementSequence, (_, movements) =>
            {
                var move = movements.Single(item => item.CardId == card && item.To == CardLocation.Hand(recipient));
                var active = GetActiveProgramFrame(frame.Id);
                ReplaceRuntimeFrame(active.Id, active with { ShanheChanrang = active.ShanheChanrang! with { PaidMovementSequence = move.Sequence } });
                AdvanceEventRulesAndQueueFact(new ShanheChanrangPaidEvent(frame.Id, frame.OwnerSeat, giver, recipient, card, move.Sequence));
            }));
        ContinueShanheChanrang(frame.Id);
    }

    private void AdvanceShanheChanrangRecipient(ProgramSkillFrame frame)
    {
        ReplaceRuntimeTop(frame = frame with { ShanheChanrang = frame.ShanheChanrang! with { Cursor = frame.ShanheChanrang.Cursor + 1,
            Stage = "branch", GiverSeat = null, RecipientSeat = null, PaidCardId = null, PaidMovementSequence = null, Choices = [] }, PendingMovementContinuation = null });
        ContinueShanheChanrang(frame.Id);
    }
    private void ContinueShanheChanrang(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame top || top.Id != frameId) return;
        var frame = GetActiveProgramFrame(frameId); var draft = frame.ShanheChanrang!;
        if (draft.Stage == "paid")
        {
            if (TryBeginCardsMovedProgramWindow(frameId) || TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) ||
                TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program)) return;
            AdvanceShanheChanrangRecipient(frame); return;
        }
        while (draft.Cursor < draft.Recipients.Count && !_players[draft.Recipients[draft.Cursor]].IsAlive)
            draft = draft with { Cursor = draft.Cursor + 1 };
        if (draft.Cursor >= draft.Recipients.Count || !_players[frame.OwnerSeat].IsAlive ||
            ShanheChanrangEquipment(frame.OwnerSeat, frame.SkillInstanceId)?.Id != draft.EquipmentId)
        { ReplaceRuntimeTop(frame with { ShanheChanrang = null }); AdvanceRuntimeProgram(frame.Id); return; }
        ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft }); PublishShanheChanrang(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 5434)]
    private bool ResumeShanheChanrang(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheChanrang: not null } frame || frame.Id != frameId) return false;
        ContinueShanheChanrang(frameId); return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5434)]
    private bool ContinuePaidShanheChanrang(ProgramSkillFrame frame) => frame.ShanheChanrang is { Stage: "paid", PaidCardId: not null };

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 5434)]
    private void AssertShanheChanrang(ProgramSkillFrame frame)
    {
        if (frame.ShanheChanrang is not { } draft) return;
        if (frame.SkillId != ShanheChanrangSkill || frame.WindowContext is not { MovementBatch: { } batch } context ||
            draft.ParentWindowId != context.ParentFrameId || draft.BatchId != batch.Id || draft.ActualTurn != batch.TurnNumber ||
            draft.Recipients.Count == 0 || draft.Cursor < 0 || draft.Cursor >= draft.Recipients.Count ||
            draft.Recipients.Distinct().Count() != draft.Recipients.Count || draft.Recipients.Contains(frame.OwnerSeat) ||
            draft.Recipients.Any(recipient => !ProgramEventHistory<ShanheChanrangFirstGainEvent>().Any(fact =>
                fact.BatchId == draft.BatchId && fact.ActualTurn == draft.ActualTurn && fact.RecipientSeat == recipient)) ||
            draft.Stage is not ("branch" or "card" or "paid") ||
            draft.Stage == "paid" && (draft.PaidCardId is null || draft.PaidMovementSequence is null ||
                !ProgramEventHistory<ShanheChanrangPaidEvent>().Any(fact => fact.FrameId == frame.Id && fact.CardId == draft.PaidCardId && fact.MovementSequence == draft.PaidMovementSequence)))
            throw new InvalidOperationException("The edict lost its first-gain identity, original frame, cursor or once-paid receipt.");
        if (_resolutionStack.LastOrDefault()?.Id == frame.Id && _pendingDecision is { } prompt &&
            (prompt.PlayerSeat != (draft.Stage == "card" ? draft.GiverSeat : frame.OwnerSeat) || !prompt.IsPrivate ||
                !AssistedChoicesEqual(prompt.Choices, draft.Choices)))
            throw new InvalidOperationException("The edict's private payer is not the issued chooser.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheChanrangEdict)]
    private PromptChoice AiShanheChanrang(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!;
        if (draft.Stage == "card") return decision.Choices.OrderBy(choice => GetKeepValue(GetAdvancedCard(choice.Cards.Single()), _players[draft.GiverSeat!.Value])).First();
        var ally = AreProgramDistributionAllies(_players[frame.OwnerSeat], _players[draft.Recipients[draft.Cursor]]);
        return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == (ally ? "give" : "take")) ?? decision.Choices[0];
    }
}
