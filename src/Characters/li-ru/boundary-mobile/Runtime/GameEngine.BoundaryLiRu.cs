namespace CardGame.Core;
public sealed partial class GameEngine
{
    private const string BoundaryLiRuGiftReason = "skill.boundary-li-ru.mieji.give", BoundaryLiRuDiscardReason = "skill.boundary-li-ru.mieji.discard";
    [ProgramCapability] private sealed class BoundaryLiRuQueries(GameEngine engine) : IBoundaryLiRuQueries
    {
        public IReadOnlyList<int> LostCardTargets(int owner) => engine.BoundaryLiRuLostCardTargets(owner);
        public bool HasHeCards(int target) => engine.IsValidPlayerSeat(target) && engine._players[target].IsAlive && engine.GetHand(engine._players[target]).Count + engine.GetEquipment(target).Count > 0;
    }
    [ProgramCapability] private sealed class BoundaryLiRuHost(GameEngine engine) : IBoundaryLiRuHost
    { public SkillProgramStepOutcome ResolveMieji(ProgramSkillFrame frame) => engine.ResolveBoundaryLiRuMieji(frame); }
    private IReadOnlyList<int> BoundaryLiRuLostCardTargets(int owner) => _players.Where(p => p.IsAlive && p.Seat != owner && _turnNumber > 0 &&
        _cardMovements.Any(m => m.TurnNumber == _turnNumber && m.From.OwnerSeat == p.Seat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.From != m.To))
        .Select(p => p.Seat).ToArray();
    private CardConversionSource BoundaryLiRuSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private IEnumerable<(Card Card, CardLocation From)> BoundaryLiRuMiejiCards(int target, bool gift) =>
        (gift ? new[] { CardZoneKind.Hand } : new[] { CardZoneKind.Hand, CardZoneKind.Equipment }).SelectMany(zone =>
            _cardZones.CardsAt(new(zone, target)).Where(c => gift ? GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Trick :
                GetProgramCardCategory(c.Kind) != SkillProgramCardCategory.Trick && !IsForeignEquipmentDiscardPrevented(target, c, new(zone, target), OwnedCardMoveIntent.Discard))
            .Select(c => (c, new CardLocation(zone, target))));
    private SkillProgramStepOutcome ResolveBoundaryLiRuMieji(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != BoundaryLiRuComposition.Mieji || f.InstructionIndex != 4 || f.SelectedTargetSeats is not [var target] || !IsValidPlayerSeat(target) || target == f.OwnerSeat)
            throw new InvalidOperationException("Mieji requires its original paid black-Trick activation and one other target.");
        if (!_players[target].IsAlive) return SkillProgramStepOutcome.Continue;
        if (f.BoundaryLiRuMieji is null)
        {
            ReplaceRuntimeTop(f = f with { BoundaryLiRuMieji = new(target, BoundaryLiRuSource(f), f.GameplayHash, BoundaryLiRuMiejiStage.Branch) });
            AdvanceEventRulesAndQueueFact(new BoundaryLiRuMiejiStartedEvent(f.Id, f.OwnerSeat, target, f.SkillInstanceId, f.GameplayHash));
        }
        if (!ValidBoundaryLiRuMieji(f)) throw new InvalidOperationException("Mieji lost its original target, private selection or actual payment.");
        return PublishBoundaryLiRuMieji(f);
    }
    private SkillProgramStepOutcome PublishBoundaryLiRuMieji(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiRuMieji!; var choices = new List<PromptChoice>();
        void Add(string action, string label, int? card = null) => choices.Add(new(new ChoiceId($"boundary-mieji.{f.Id}.{r.Stage}.{action}.{card}"), label,
            card is { } id ? [id] : [], card is null ? [] : [r.TargetSeat], new Dictionary<string, string>
            { ["program-action"] = "boundary-li-ru-mieji", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["stage"] = r.Stage.ToString(), ["branch"] = action, ["selection-index"] = r.Cards.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        if (r.Stage == BoundaryLiRuMiejiStage.Branch)
        {
            if (BoundaryLiRuMiejiCards(r.TargetSeat, true).Any()) Add("gift", "交给李儒一张锦囊牌。");
            var count = BoundaryLiRuMiejiCards(r.TargetSeat, false).Take(2).Count();
            if (count > 0) Add("discard", count == 1 ? "弃置一张非锦囊牌（不足两张）。" : "弃置两张非锦囊牌。");
        }
        else if (r.Stage is BoundaryLiRuMiejiStage.Gift or BoundaryLiRuMiejiStage.Discard)
            foreach (var item in BoundaryLiRuMiejiCards(r.TargetSeat, r.Gift).Where(c => !r.Cards.Contains(c.Card.Id))) Add("card", (r.Gift ? "交给李儒" : "选择弃置") + "【" + item.Card.DisplayName + "】。", item.Card.Id);
        else throw new InvalidOperationException("A paid Mieji cannot reopen an unpaid private choice.");
        if (choices.Count == 0) { ReplaceRuntimeTop(f with { BoundaryLiRuMieji = null }); return SkillProgramStepOutcome.Continue; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, r.TargetSeat, r.Stage == BoundaryLiRuMiejiStage.Branch ? "【灭计】交给李儒锦囊牌，或依次弃置非锦囊牌。" : $"【灭计】选择第{r.Payments.Count + 1}张牌（共{r.Required}张）。", choices.SelectMany(c => c.Cards).ToArray(), [r.TargetSeat], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = r.TargetSeat, Choices = Array.AsReadOnly(choices.ToArray()), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[r.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }
    [ProgramAnswer("boundary-li-ru-mieji")]
    private void AnswerBoundaryLiRuMieji(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryLiRuMieji: { } r } f || !ValidBoundaryLiRuMieji(f) ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != r.TargetSeat || !p.Choices.Any(c => c.Id == choice.Id) ||
            choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("stage") != r.Stage.ToString() ||
            choice.Parameters.GetValueOrDefault("selection-index") != r.Cards.Count.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Mieji's answer must belong to the exact original target and paused frame.");
        if (!_players[r.TargetSeat].IsAlive || !_players[f.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        { ClearPendingDecision(); CancelProgramBindingAndCleanup(f, "灭计参与者或技能实例已失效。"); return; }
        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (r.Stage == BoundaryLiRuMiejiStage.Branch)
        {
            if (branch is not ("gift" or "discard") || choice.Cards.Count != 0 || choice.Targets.Count != 0) throw new InvalidOperationException("Mieji requires an available cost branch.");
            var gift = branch == "gift"; var count = BoundaryLiRuMiejiCards(r.TargetSeat, gift).Take(2).Count();
            if (count == 0) throw new InvalidOperationException("Mieji's chosen branch has no legal actual card.");
            ClearPendingDecision(); r = r with { Gift = gift, Required = gift ? 1 : 2, Stage = gift ? BoundaryLiRuMiejiStage.Gift : BoundaryLiRuMiejiStage.Discard };
            ReplaceRuntimeTop(f = f with { BoundaryLiRuMieji = r }); AdvanceEventRulesAndQueueFact(new BoundaryLiRuMiejiBranchEvent(f.Id, r.TargetSeat, r.Gift, r.Required));
            PublishBoundaryLiRuMieji(f); return;
        }
        if (branch != "card" || choice.Cards is not [var card] || choice.Targets is not [var seat] || seat != r.TargetSeat || r.Cards.Contains(card) ||
            !BoundaryLiRuMiejiCards(r.TargetSeat, r.Gift).Any(c => c.Card.Id == card)) throw new InvalidOperationException("Mieji requires a still-owned, category-correct cost card.");
        ClearPendingDecision(); r = r with { Cards = r.Cards.Append(card).ToArray(), From = r.From.Append(_cardZones.GetLocation(card)).ToArray() };
        ReplaceRuntimeTop(f = f with { BoundaryLiRuMieji = r });
        PayBoundaryLiRuMieji(f);
    }
    private void PayBoundaryLiRuMieji(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiRuMieji!;
        if (r.Cards.Count != 1 || r.Payments.Count >= r.Required || r.Cards.Select((id, index) => (id, index)).Any(c => _cardZones.GetLocation(c.id) != r.From[c.index] ||
            !BoundaryLiRuMiejiCards(r.TargetSeat, r.Gift).Any(item => item.Card.Id == c.id))) throw new InvalidOperationException("Mieji lost a privately selected card before its next sequential payment.");
        r = r with { Stage = BoundaryLiRuMiejiStage.Movement, Before = _movementSequence, After = _movementSequence };
        ReplaceRuntimeTop(f = f with { BoundaryLiRuMieji = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var reason = new CardMoveReason(r.Gift ? BoundaryLiRuGiftReason : BoundaryLiRuDiscardReason);
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            if (moves.Count != r.Cards.Count || moves.Any(m => !r.Cards.Contains(m.CardId) || m.From != r.From[r.Cards.ToList().IndexOf(m.CardId)] || m.Sequence <= r.Before ||
                m.To != (r.Gift ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile) && m.To != CardLocation.OutsideGame)) throw new InvalidOperationException("Mieji lost its actual gift/discard movement invoice.");
            var paid = r with { After = moves.Max(m => m.Sequence), Batch = batch,
                Payments = r.Payments.Append(new BoundaryLiRuMiejiPayment(r.Cards.Single(), r.From.Single(), r.Before, moves.Max(m => m.Sequence), batch)).ToArray() };
            ReplaceRuntimeFrame(f.Id, GetActiveProgramFrame(f.Id) with { BoundaryLiRuMieji = paid });
            AdvanceEventRulesAndQueueFact(new BoundaryLiRuMiejiPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.Gift, paid.Payments.Count, 1, r.Before, paid.After, batch));
        }
        if (r.Gift) MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(f.OwnerSeat), reason, NativeGiveCompletionRecords(f.Id, r.TargetSeat, f.OwnerSeat, r.Cards, r.Before, Paid));
        else MoveProgramLogicalDiscard(f, r.TargetSeat, r.Cards, reason, Paid);
        if (AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            ReturnBoundaryLiRuMieji(GetActiveProgramFrame(f.Id));
    }
    private bool ValidBoundaryLiRuMieji(ProgramSkillFrame f)
    {
        if (f.BoundaryLiRuMieji is not { } r || f.SkillId != BoundaryLiRuComposition.Mieji || f.InstructionIndex != 4 || f.WindowContext is not null || f.TriggerId is not null ||
            f.SelectedTargetSeats is not [var target] || target != r.TargetSeat || r.Source != BoundaryLiRuSource(f) || r.GameplayHash != f.GameplayHash || r.Cards.Count != r.From.Count ||
            r.Cards.Count > 1 || r.Payments.Count > r.Required || r.Required is < 0 or > 2 ||
            ProgramEventHistory<BoundaryLiRuMiejiStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == target && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash) != 1) return false;
        if (r.Stage == BoundaryLiRuMiejiStage.Branch) return r.Required == 0 && r.Cards.Count == 0 && r.Payments.Count == 0 && !r.Gift && r.Before == 0 && r.After == 0;
        if (ProgramEventHistory<BoundaryLiRuMiejiBranchEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == target && e.Gift == r.Gift && e.Required == r.Required) != 1 || r.Required == 0 || r.Gift && r.Required != 1) return false;
        if (!r.Payments.Select((paid, i) => (paid, i)).All(item => item.paid.After > item.paid.Before && item.paid.Batch > 0 &&
            ProgramEventHistory<BoundaryLiRuMiejiPaidEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == target && e.Gift == r.Gift && e.Ordinal == item.i + 1 && e.Count == 1 && e.Before == item.paid.Before && e.After == item.paid.After && e.Batch == item.paid.Batch) == 1 &&
            _cardMovements.Count(m => m.CardId == item.paid.CardId && m.From == item.paid.From && m.Sequence > item.paid.Before && m.Sequence <= item.paid.After &&
                m.Reason.Value == (r.Gift ? BoundaryLiRuGiftReason : BoundaryLiRuDiscardReason) && (m.To == (r.Gift ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile) || m.To == CardLocation.OutsideGame)) == 1)) return false;
        if (r.Stage is BoundaryLiRuMiejiStage.Gift or BoundaryLiRuMiejiStage.Discard) return (r.Stage == BoundaryLiRuMiejiStage.Gift) == r.Gift && r.Cards.Count == 0 && r.Payments.Count < r.Required && r.Before == 0 && r.After == 0;
        return r.Stage == BoundaryLiRuMiejiStage.Movement && r.Cards is [var id] && r.From is [var from] && r.Payments.LastOrDefault() == new BoundaryLiRuMiejiPayment(id, from, r.Before, r.After, r.Batch);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 55700)]
    private bool BoundaryLiRuMiejiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) =>
        effect?.Op == SkillProgramEffectOp.ResolveBoundaryLiRuMieji && f.BoundaryLiRuMieji is { Stage: BoundaryLiRuMiejiStage.Movement } r && ValidBoundaryLiRuMieji(f) &&
        p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 55700)]
    private bool ReturnBoundaryLiRuMieji(ProgramSkillFrame f)
    {
        if (f.BoundaryLiRuMieji is not { Stage: BoundaryLiRuMiejiStage.Movement }) return false;
        if (!ValidBoundaryLiRuMieji(f)) throw new InvalidOperationException("Mieji returned without its exact committed gift/discard invoice.");
        var r = f.BoundaryLiRuMieji;
        if (!r.Gift && r.Payments.Count < r.Required && _players[r.TargetSeat].IsAlive && _players[f.OwnerSeat].IsAlive &&
            HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && BoundaryLiRuMiejiCards(r.TargetSeat, false).Any())
        {
            var next = f with { BoundaryLiRuMieji = r with { Stage = BoundaryLiRuMiejiStage.Discard, Cards = [], From = [], Before = 0, After = 0, Batch = 0 }, PendingMovementContinuation = null };
            ReplaceRuntimeFrame(f.Id, next); PublishBoundaryLiRuMieji(next); return true;
        }
        ReplaceRuntimeFrame(f.Id, f with { BoundaryLiRuMieji = null, PendingMovementContinuation = null }); AdvanceRuntimeProgram(f.Id); return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 55700)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 55700)]
    private bool KeepPaidBoundaryLiRuMieji(ProgramSkillFrame f) => f.BoundaryLiRuMieji is { Stage: BoundaryLiRuMiejiStage.Movement } && ValidBoundaryLiRuMieji(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55700)]
    private void AssertBoundaryLiRuMieji(ProgramSkillFrame f)
    { if (f.BoundaryLiRuMieji is not null && !ValidBoundaryLiRuMieji(f)) throw new InvalidOperationException("Mieji lost its original activation, private choice or paid movement receipt."); }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryLiRuMieji)]
    private PromptChoice AiBoundaryLiRuMieji(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "gift") ??
        p.Choices.OrderBy(c => c.Cards.Count == 1 ? GetKeepValue(_cardZones.CardsAt(_cardZones.GetLocation(c.Cards[0])).Single(card => card.Id == c.Cards[0]), _players[p.PlayerSeat]) : 0).First();
}
