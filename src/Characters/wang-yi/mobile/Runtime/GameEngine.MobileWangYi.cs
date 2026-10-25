namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 73010)]
    private IEnumerable<CardSnapshot> MobileWangYiPrivateMijiCards(int viewerSeat)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: "mobile:miji", InstructionIndex: 4 } frame ||
            frame.OwnerSeat != viewerSeat || _pendingDecision?.PlayerSeat != viewerSeat ||
            frame.CardSetBindings.SingleOrDefault(binding => binding.Name == "miji-cards") is not
                { Visibility: SkillProgramCardSetVisibility.Private } cards)
            return [];
        return cards.CardIds.Where(id => _cardZones.GetLocation(id) == CardLocation.Processing)
            .Select(id => ToSnapshot(_cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id))).ToArray();
    }

    // The ordinary mobile retrial commits to the unknown top card. Its menu
    // must never publish the future material before the player accepts.
    private static bool IsDeckTopJudgmentReplacement(SkillProgramTrigger trigger) =>
        trigger.Effects is [{ Op: SkillProgramEffectOp.ReplaceJudgment, Zones: [CardZoneKind.DrawPile] }];

    private bool HasDeckTopJudgmentReplacementMaterial() =>
        _cardZones.Count(CardLocation.DrawPile) + _cardZones.Count(CardLocation.DiscardPile) > 0;

    private void BeginDeckTopJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate,
        SkillProgram program, SkillProgramTrigger trigger)
    {
        var owner = _players[candidate.OwnerSeat];
        var old = GetJudgmentCard(pending) ?? throw new InvalidOperationException("The deck-top retrial lost its current judgment.");
        SetJudgmentFrameStep(pending.Id, ResolutionFrameStep.AwaitingResponse);
        AdvanceEventRulesAndQueueFact(new JudgmentReplacementRequestedEvent(pending.Id, pending.Id,
            pending.TargetSeat, owner.Seat, pending.Reason, old.Id, old.Kind, EffectiveSuit(_players[pending.TargetSeat], old)));
        _pendingDecision = new PendingDecision(DecisionKind.ProgramJudgmentReplacement, owner.Seat,
            $"【{_contentRegistry.GetSkill(program.Id).Name}】：是否亮出未知的牌堆顶牌代替当前判定牌？", [], [], pending.TargetSeat, old.Kind)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = pending.TargetSeat,
            SkillPrompt = new(program.Id, _contentRegistry.GetSkill(program.Id).Name, "以未知顶牌改判", _contentRegistry.GetSkill(program.Id).Description),
            Choices = Array.AsReadOnly(new[]
            {
                new PromptChoice(new($"program-judgment-top.{program.Id}.{trigger.Id}.activate"), "亮出牌堆顶的一张牌代替之。", [], [],
                    new Dictionary<string, string> { ["action"] = "program-judgment-top-replace" }),
                new PromptChoice(new($"program-judgment-top.{program.Id}.{trigger.Id}.skip"), "不发动，保留当前判定牌。", [], [],
                    new Dictionary<string, string> { ["action"] = "program-judgment-replace-skip" })
            })
        };
        _status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }

    private bool IsDeckTopJudgmentReplacementPromptValid(JudgmentFrame judgment, PendingDecision? decision)
    {
        var candidate = CurrentJudgmentCandidate(judgment);
        if (candidate is null || decision is not { Kind: DecisionKind.ProgramJudgmentReplacement, IsPrivate: true,
                ValidCardIds.Count: 0, ValidTargetSeats.Count: 0, Choices.Count: 2 } ||
            decision.PlayerSeat != candidate.OwnerSeat || decision.SourceSeat != judgment.TargetSeat || decision.TargetSeat != judgment.TargetSeat)
            return false;
        var (program, trigger) = GetProgramJudgmentReplacement(candidate);
        return IsDeckTopJudgmentReplacement(trigger) && decision.SkillPrompt?.SkillId == program.Id &&
            decision.Choices.All(choice => choice.Cards.Count == 0 && choice.Targets.Count == 0) &&
            decision.Choices[0].Id.Value == $"program-judgment-top.{program.Id}.{trigger.Id}.activate" &&
            decision.Choices[0].Parameters.GetValueOrDefault("action") == "program-judgment-top-replace" &&
            decision.Choices[1].Id.Value == $"program-judgment-top.{program.Id}.{trigger.Id}.skip" &&
            decision.Choices[1].Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip";
    }

    private bool TryResolveDeckTopJudgmentReplacement(PromptChoice selected, JudgmentFrame pending,
        JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger)
    {
        if (!IsDeckTopJudgmentReplacement(trigger) || selected.Parameters.GetValueOrDefault("action") != "program-judgment-top-replace") return false;
        if (!IsDeckTopJudgmentReplacementPromptValid(pending, _pendingDecision) ||
            !_players[candidate.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[candidate.OwnerSeat], program.Id, candidate.SkillInstanceId!) ||
            !MatchesProgramJudgmentReplacement(_players[candidate.OwnerSeat], trigger, pending.TargetSeat, pending.Reason))
            throw new InvalidOperationException("The accepted deck-top retrial lost its original owning judgment candidate.");
        ClearPendingDecision();
        if (!EnsureDrawPile()) { AdvanceJudgmentCandidate(pending); return true; }
        var old = GetJudgmentCard(pending) ?? throw new InvalidOperationException("The accepted retrial lost its old judgment card.");
        var replacement = _cardZones.CardsAt(CardLocation.DrawPile)[^1];
        var shared = new ProgramTriggerCandidate(candidate.OwnerSeat, program.Id, trigger.Id,
            candidate.SkillInstanceId!, program.GameplayHash, trigger.Priority);
        BeginProgramBinding(shared, new(SkillProgramTriggerWindow.JudgmentReplacing, pending.Id, candidate.OwnerSeat,
            SourceSeat: pending.SourceSeat, TargetSeat: pending.TargetSeat, Facts: CaptureProgramTriggerFacts(_players[candidate.OwnerSeat]),
            JudgmentReplacement: new(pending.Id, pending.TargetSeat, pending.Reason, old.Id, replacement.Id)));
        return true;
    }

    private bool IsDeckTopJudgmentReplacementInput(JudgmentFrame judgment, CharacterState owner, Card card)
    {
        var candidate = CurrentJudgmentCandidate(judgment);
        return candidate is not null && candidate.OwnerSeat == owner.Seat && judgment.TargetSeat == owner.Seat &&
            IsDeckTopJudgmentReplacement(GetProgramJudgmentReplacement(candidate).Trigger) &&
            _cardZones.GetLocation(card.Id) == CardLocation.DrawPile &&
            _cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()?.Id == card.Id;
    }

    private bool TryResolveAiDeckTopJudgmentReplacement()
    {
        if (ActiveJudgment is not { } judgment || CurrentJudgmentCandidate(judgment) is not { } candidate ||
            !IsDeckTopJudgmentReplacement(GetProgramJudgmentReplacement(candidate).Trigger)) return false;
        var current = GetJudgmentCard(judgment)!;
        // The choice only uses the current public judgment, never the hidden
        // replacement material. Miji wants black; an already-black result stays.
        var activate = judgment.Reason == "skill.mobile-wang-yi.miji" &&
            EffectiveSuit(_players[judgment.TargetSeat], current) is Suit.Heart or Suit.Diamond;
        ResolveProgramJudgmentReplacementChoice(_pendingDecision!.Choices[activate ? 0 : 1]);
        AdvanceRulesAndPublishState();
        return true;
    }
}
