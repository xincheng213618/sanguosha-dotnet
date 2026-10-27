using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string SourceHuanfuDiscard = "skill-program.source-huanfu-3156.discard";
    private const string SourceHuanfuDraw = "skill-program.source-huanfu-3156.draw";
    private static CardConversionSource SourceHuanfuSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    private CardUseFrame? SourceHuanfuUse(ProgramSkillWindowContext context) => context.CardUse is { } card
        ? _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == card.ParentCardUseFrameId &&
            u.Action is { Type: CardActionType.Use } a && a.ActionId == card.CardActionId && a.ActorSeat == card.ActorSeat &&
            u.SourceSeat == card.ActorSeat && IsSlashCard(u.CardKind) && u.CardKind == card.EffectiveKind) : null;

    private IReadOnlyList<PromptChoice> SourceHuanfuCosts(ProgramSkillFrame f) => BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat,
        [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1).ToArray();

    private bool SourceHuanfuArmQualifies(int owner, string binding, ProgramSkillWindowContext context) => SourceExecutionerMode &&
        context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _players[owner].IsAlive && _players[owner].MaxHp > 0 &&
        SourceHuanfuUse(context) is { Action: { } a } use && use.SourceExecutionerHuanfuBets?.Any(b => b.Source.OwnerSeat == owner) != true &&
        (binding == "actor-wager" ? a.ActorSeat == owner : binding == "target-wager" && a.ActorSeat != owner && a.TargetSeats.Contains(owner)) &&
        GetHand(_players[owner]).Concat(GetEquipment(_players[owner])).Any(card =>
            !IsSelfHandCategoryDiscardForbidden(owner, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) &&
            !IsForeignEquipmentDiscardPrevented(owner, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));

    private CardUseFrame ExactSourceHuanfuParent(ProgramSkillFrame f)
    {
        if (f.SkillId != SourceExecutionerContract.Huanfu || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context ||
            SourceHuanfuUse(context) is not { Action: { } action } use || !SourceExecutionerMode)
            throw new InvalidOperationException("Source3156 Huanfu lost its original real Slash Use.");
        var i = _resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2 || _resolutionStack[i - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.ParentFrameId != use.Id || _resolutionStack[i - 2].Id != use.Id || window.Action.ActionId != action.ActionId ||
            window.Action.Type != CardActionType.Use || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(f, ToSharedCandidate(window.Candidates[window.CandidateIndex])) ||
            CreateCardActionProgramContext(window, window.Candidates[window.CandidateIndex]) != context)
            throw new InvalidOperationException("Source3156 Huanfu lost its original card window and exact instance candidate.");
        return use;
    }

    private SkillProgramStepOutcome BeginSourceExecutionerHuanfu(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var use = ExactSourceHuanfuParent(f);
        if (f.SourceExecutionerHuanfu is not null) throw new InvalidOperationException("Huanfu cannot begin its native payment twice.");
        if (f.TriggerId == "settle-wager") return SettleSourceHuanfu(f, use);
        if (!SourceHuanfuArmQualifies(f.OwnerSeat, f.TriggerId!, f.WindowContext!) || SourceHuanfuCosts(f).Count == 0)
            throw new InvalidOperationException("Huanfu requires the actor or original target's actual payable HE wager.");
        ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = new(use.Id, use.Action!.ActionId, SourceExecutionerHuanfuStage.Cards,
            f.TriggerId == "actor-wager", _players[f.OwnerSeat].MaxHp) });
        ContinueSourceHuanfu(f.Id); return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> SourceHuanfuChoices(ProgramSkillFrame f)
    {
        var r = f.SourceExecutionerHuanfu!; var choices = new List<PromptChoice>();
        void Add(string key, string label, string branch, IReadOnlyList<int> cards) => choices.Add(new(new($"source-huanfu.{f.Id}.{key}"), label, cards, [],
            new Dictionary<string, string> { ["program-action"] = "source-huanfu-wager", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        foreach (var id in r.Cards) Add("unpick." + id, "取消已选牌", "unpick", [id]);
        var limit = Math.Min(r.Limit, _players[f.OwnerSeat].MaxHp);
        if (r.Cards.Count < limit) foreach (var c in SourceHuanfuCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single())))
            Add("card." + c.Cards.Single(), c.Label, "card", c.Cards);
        if (r.Cards.Count > 0 && r.Cards.Count <= limit) Add("pay", $"弃置{r.Cards.Count}张牌下注", "pay", []);
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    [ProgramAnswer("source-huanfu-wager")]
    private void AnswerSourceHuanfu(PromptChoice selected)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Huanfu lost its own selection frame.");
        var r = f.SourceExecutionerHuanfu ?? throw new InvalidOperationException("Huanfu lost its wager receipt.");
        if (r.Stage != SourceExecutionerHuanfuStage.Cards || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) ||
            !AssistedChoicesEqual([selected], SourceHuanfuChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Huanfu's real HE wager or maximum HP changed.");
        ExactSourceHuanfuParent(f); ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "card": ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } }); break;
            case "unpick": ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } }); break;
            case "pay":
                var available = SourceHuanfuCosts(f).SelectMany(c => c.Cards).ToHashSet(); var before = _movementSequence;
                if (r.Cards.Count < 1 || r.Cards.Count > Math.Min(r.Limit, _players[f.OwnerSeat].MaxHp) || r.Cards.Any(c => !available.Contains(c)))
                    throw new InvalidOperationException("Huanfu must discard one to current maximum HP actual HE entities.");
                ReplaceRuntimeTop(f = f with { SourceExecutionerHuanfu = r with { Stage = SourceExecutionerHuanfuStage.CostChildren,
                    Limit = _players[f.OwnerSeat].MaxHp, SequenceBefore = before, SequenceAfter = before } });
                var invocation = CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, r.Cards, new(SourceHuanfuDiscard), f.Id);
                MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.DiscardPile, new(SourceHuanfuDiscard), (_, moves) =>
                {
                    var current = GetActiveProgramFrame(f.Id); var paid = current.SourceExecutionerHuanfu! with { SequenceAfter = _movementSequence,
                        ActualCount = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) };
                    ReplaceRuntimeTop(current with { SourceExecutionerHuanfu = paid });
                    var use = ExactSourceHuanfuParent(current);
                    var bet = new SourceExecutionerHuanfuBet(use.Id, use.Action!.ActionId, SourceHuanfuSource(current), current.GameplayHash,
                        current.Id, use.SourceSeat, paid.ActorWager, paid.Limit, paid.ActualCount, before, paid.SequenceAfter)
                    { Cards = paid.Cards, Targets = use.Action.TargetSeats };
                    ReplaceRuntimeFrame(use.Id, use with { SourceExecutionerHuanfuBets = (use.SourceExecutionerHuanfuBets ?? []).Append(bet).ToArray() });
                    AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuPaidEvent(current.Id, use.Id, bet.ActionId, bet.Source,
                        bet.GameplayHash, bet.Amount, bet.MaxHpAtPayment, before, bet.SequenceAfter));
                });
                CompleteNativeLogicalDiscardInvocation(invocation); break;
            default: throw new InvalidOperationException("Unknown Huanfu wager selection.");
        }
        ContinueSourceHuanfu(f.Id);
    }

    private void FinishSourceHuanfu(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = null }); AdvanceRuntimeProgram(f.Id); }

    private void ContinueSourceHuanfu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SourceExecutionerHuanfu: { } r } f || f.Id != id || f.NativeDraw is not null) return;
        ExactSourceHuanfuParent(f);
        if (r.Stage == SourceExecutionerHuanfuStage.Cards)
        {
            if (!_players[f.OwnerSeat].IsAlive) { FinishSourceHuanfu(f); return; }
            var choices = SourceHuanfuChoices(f); if (choices.Count == 0) { FinishSourceHuanfu(f); return; }
            var skill = _contentRegistry.GetSkill(f.SkillId);
            _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"宦浮：弃置至多{Math.Min(r.Limit, _players[f.OwnerSeat].MaxHp)}张手牌或装备牌下注。",
                choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
            _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            AdvanceRulesAndPublishState(); return;
        }
        if (r.Stage == SourceExecutionerHuanfuStage.CostChildren)
        {
            if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
                TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return;
            FinishSourceHuanfu(GetActiveProgramFrame(id));
        }
    }

    private SkillProgramStepOutcome SettleSourceHuanfu(ProgramSkillFrame f, CardUseFrame use)
    {
        var bet = SourceHuanfuSettlement(f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, use) ??
            throw new InvalidOperationException("Huanfu cannot settle without its original paid wager.");
        if (bet.Settled) throw new InvalidOperationException("Huanfu cannot settle the same wager twice.");
        var requested = bet.Amount > 0 && bet.DamageTotal == bet.Amount && _players[f.OwnerSeat].IsAlive ? checked(bet.Amount * 2) : 0;
        ReplaceRuntimeFrame(use.Id, use with { SourceExecutionerHuanfuBets = use.SourceExecutionerHuanfuBets!.Select(b => b == bet ? b with { Settled = true } : b).ToArray() });
        AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuSettledEvent(f.Id, bet.ProgramFrameId, use.Id, f.OwnerSeat, bet.Amount, bet.DamageTotal, requested));
        if (requested == 0) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { SourceExecutionerHuanfu = new(use.Id, use.Action!.ActionId, SourceExecutionerHuanfuStage.Drawing,
            bet.ActorWager, bet.MaxHpAtPayment, bet.SequenceBefore, bet.SequenceAfter, bet.Amount, bet.ProgramFrameId) });
        BeginOperationProgramDraw(f, f.OwnerSeat, requested, new(SourceHuanfuDraw)); return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62060)]
    private bool ResumeSourceHuanfu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SourceExecutionerHuanfu: not null } f || f.Id != id) return false;
        ContinueSourceHuanfu(id); return true;
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)]
    private void ReturnSourceHuanfuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = GetActiveProgramFrame(supplied.Id); var r = f.SourceExecutionerHuanfu ?? throw new InvalidOperationException("Huanfu's native draw lost its paid wager.");
        if (r.Stage != SourceExecutionerHuanfuStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat ||
            outcome.RequestedCount != checked(r.ActualCount * 2) || outcome.ActualDrawIds.Count > outcome.RequestedCount)
            throw new InvalidOperationException("Huanfu lost its exact twice-wager native draw return.");
        AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuDrawReturnedEvent(f.Id, r.UseId, outcome.RequestedCount, outcome.ActualDrawIds.Count));
        FinishSourceHuanfu(f);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)]
    private PromptChoice ChooseSourceHuanfuAi(PendingDecision d, ProgramSkillFrame f) =>
        d.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? d.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "card");
}
