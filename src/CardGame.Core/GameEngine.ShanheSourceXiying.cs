using System.Globalization;
using CardGame.Core.Campaigns;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string SourceXiyingDiscard = "skill-program.source-xiying-1180.discard", SourceXiyingReward = "skill-program.source-xiying-1180.reward";
    private static CardConversionSource SourceXiyingSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private bool SourceXiyingMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    [ProgramCapability] private sealed class ShanheSourceXiyingCapability(GameEngine e) : IShanheSourceXiyingHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => e.BeginSourceXiying(f); }

    private bool SourceXiyingPlayParent(int owner, ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => SourceXiyingMode &&
        c.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == owner && c.SourceSeat == owner && c.TargetSeat == owner &&
        owner == _currentSeat && _phase == TurnPhase.Play && _turnNumber > 0 && _cardUseDebitPhaseInstanceId > 0 &&
        _resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId) is { } p && p.OwnerSeat == owner &&
        p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate == candidate;

    private IReadOnlyList<PromptChoice> SourceXiyingPaymentChoices(ProgramSkillFrame f, int actor, bool initial) =>
        BuildOwnedCardPaymentChoices(f.Id, actor, actor, initial ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard)
            .Where(c => c.Cards.Count == 1 && (!initial || CardCatalog.Get(GetAdvancedCard(c.Cards.Single()).Kind).CategoryName != "基本牌")).ToArray();

    private SkillProgramStepOutcome BeginSourceXiying(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShanheSourceXiyingContract.Skill || f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId ||
            f.ShanheSourceXiying is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c)
            throw new InvalidOperationException("Source1180 requires its original zero-input native phase binding.");
        if (f.TriggerId == ShanheSourceXiyingContract.Ending) return BeginSourceXiyingReward(f);
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!SourceXiyingPlayParent(f.OwnerSeat, candidate, c) || !_players[f.OwnerSeat].IsAlive || SourceXiyingPaymentChoices(f, f.OwnerSeat, true).Count == 0)
            throw new InvalidOperationException("Source1180 must discard one actual non-basic hand entity at its real own Play start.");
        ReplaceRuntimeTop(f = f with { ShanheSourceXiying = new(c.ParentFrameId, _turnNumber, _cardUseDebitPhaseInstanceId, ShanheSourceXiyingStage.Cost)
        { Seats = _players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray() } });
        ContinueSourceXiying(f.Id); return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> SourceXiyingChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceXiying!; var actor = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index];
        PromptChoice Make(string branch, string label, IReadOnlyList<int> cards) => new(new($"source-xiying.{f.Id}.{r.Stage}.{r.Index}.{branch}.{string.Join('-', cards)}"), label,
            cards, [], new Dictionary<string, string> { ["program-action"] = "source-xiying", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["index"] = r.Index.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        var choices = SourceXiyingPaymentChoices(f, actor, r.Stage == ShanheSourceXiyingStage.Cost)
            .Select(c => Make("discard", c.Label, c.Cards)).ToList();
        if (r.Stage == ShanheSourceXiyingStage.Targets) choices.Add(Make("ban", "本回合不能使用或打出牌", []));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }
    private void PublishSourceXiying(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceXiying!; var choices = SourceXiyingChoices(f);
        if (choices.Count == 0) { FinishSourceXiying(f); return; }
        var actor = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index]; var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, actor, actor == f.OwnerSeat ? "袭营：弃置手中一张非基本牌。" : "袭营：弃置一张牌，或本回合不能使用、打出牌。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = actor, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("source-xiying")]
    private void AnswerSourceXiying(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceXiying: { } r } f ||
            r.Stage is not (ShanheSourceXiyingStage.Cost or ShanheSourceXiyingStage.Targets) ||
            selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) ||
            !SourceXiyingChoices(f).Any(c => AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Source1180 lost its exact current owner/other-character payment choice.");
        AssertSourceXiyingFrame(f); ClearPendingDecision();
        var target = r.Stage == ShanheSourceXiyingStage.Cost ? f.OwnerSeat : r.Seats[r.Index];
        if (selected.Parameters["branch"] == "ban")
        {
            if (r.Stage != ShanheSourceXiyingStage.Targets || target == f.OwnerSeat) throw new InvalidOperationException("Only the current other character can accept this exact ban.");
            ValidateProgramTurnEffectGrant(f);
            var ban = _turnCardUseEffects.GrantTargetRestriction(_turnNumber, _currentSeat, f.Id, -r.Index - 1,
                CreateProgramTurnEffectSource(f), SkillProgramCardTargetRestriction.CannotUseOrPlayForTurn, target);
            AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(ban));
            ReplaceRuntimeTop(f with { ShanheSourceXiying = r with { Index = r.Index + 1 } });
        }
        else
        {
            var id = selected.Cards.Single(); var before = _movementSequence; var initial = r.Stage == ShanheSourceXiyingStage.Cost;
            ReplaceRuntimeTop(f = f with { ShanheSourceXiying = initial
                ? r with { Stage = ShanheSourceXiyingStage.CostChildren, CostCardId = id, CostBefore = before, CostAfter = before }
                : r with { Stage = ShanheSourceXiyingStage.TargetChildren, TargetSeat = target, TargetCardId = id, TargetBefore = before, TargetAfter = before } });
            MoveProgramLogicalDiscard(f, target, [id], new(SourceXiyingDiscard), (_, moves) =>
            {
                var active = GetActiveProgramFrame(f.Id); var current = active.ShanheSourceXiying!;
                if (moves.Count != 1 || moves[0].CardId != id || moves[0].To != CardLocation.DiscardPile && moves[0].To != CardLocation.OutsideGame)
                    throw new InvalidOperationException("Source1180 must really discard its exact selected physical card once.");
                ReplaceRuntimeTop(active with { ShanheSourceXiying = initial ? current with { CostAfter = _movementSequence } : current with { TargetAfter = _movementSequence } });
                if (initial) AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingPaidEvent(f.Id, SourceXiyingSource(active), active.GameplayHash,
                    current.ActualTurn, current.PlayPhaseInstance, id, before, _movementSequence));
                else AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingTargetPaidEvent(f.Id, target, id, before, _movementSequence));
            });
        }
        ContinueSourceXiying(f.Id);
    }

    private bool DrainSourceXiying(long id) => TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) ||
        TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) ||
        TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id);
    private void ContinueSourceXiying(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceXiying: { } r } f || f.Id != id) return;
        AssertSourceXiyingFrame(f);
        if (r.Stage is ShanheSourceXiyingStage.CostChildren or ShanheSourceXiyingStage.TargetChildren or ShanheSourceXiyingStage.RewardChildren)
        {
            if (DrainSourceXiying(id)) return; f = GetActiveProgramFrame(id); r = f.ShanheSourceXiying!;
            if (r.Stage == ShanheSourceXiyingStage.RewardChildren) { FinishSourceXiying(f); return; }
            ReplaceRuntimeTop(f = f with { ShanheSourceXiying = r = r with { Stage = ShanheSourceXiyingStage.Targets,
                Index = r.Stage == ShanheSourceXiyingStage.TargetChildren ? r.Index + 1 : r.Index } });
        }
        if (_winner != Winner.None) { FinishSourceXiying(f); return; }
        if (r.Stage == ShanheSourceXiyingStage.Targets)
        {
            while (r.Index < r.Seats.Count && !_players[r.Seats[r.Index]].IsAlive)
                ReplaceRuntimeTop(f = f with { ShanheSourceXiying = r = r with { Index = r.Index + 1 } });
            if (r.Index == r.Seats.Count) { FinishSourceXiying(f); return; }
        }
        if (_pendingDecision is null) PublishSourceXiying(f);
    }
    private void FinishSourceXiying(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f = f with { ShanheSourceXiying = null }); FinishProgramSkill(f, true); }
    [ProgramContinuation(ProgramContinuationKind.Resume, 62063)]
    private bool ResumeSourceXiying(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceXiying: not null } f || f.Id != id) return false; ContinueSourceXiying(id); return true; }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceXiying)]
    private PromptChoice SelectSourceXiyingAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Cards.Count == 1) ?? p.Choices.Single();
}
