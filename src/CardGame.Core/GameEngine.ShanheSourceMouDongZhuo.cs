using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string SourceMouXiongni = "shanhe:source-xiongni-14206", SourceMouFengshang = "shanhe:source-fengshang-14207";
    private const string SourceMouZhibing = "shanhe:source-zhibing-14208", SourceMouZhixi = "shanhe:source-zhixi-14209";
    private const string SourceMouCost = "skill.shanhe-source-mou.discard", SourceMouGift = "skill.shanhe-source-mou.give";
    private bool IsSourceMouMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    [ProgramCapability]
    private sealed class SourceMouCapability(GameEngine engine) : IShanheSourceMouHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => engine.StartSourceMou(frame, operation); }
    private CardConversionSource SourceMouSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private ShanheSourceMouCard SourceMouCard(int seat, Card card, CardLocation from, bool effective) =>
        new(card.Id, card.Kind, card.Suit, card.Rank, effective ? GetProgramEffectiveSuit(_players[seat], card) : card.Suit, from);
    private ShanheSourceMouCard[] SourceMouDiscardable(int seat, bool handOnly = false) =>
        (handOnly ? new[] { CardZoneKind.Hand } : [CardZoneKind.Hand, CardZoneKind.Equipment]).SelectMany(zone =>
            _cardZones.CardsAt(new(zone, seat)).Where(card => !IsForeignEquipmentDiscardPrevented(seat, card, new(zone, seat), OwnedCardMoveIntent.Discard))
                .Select(card => SourceMouCard(seat, card, new(zone, seat), true))).ToArray();
    private ShanheSourceMouCard[] SourceMouDiscardPile() => _cardZones.CardsAt(CardLocation.DiscardPile)
        .Where(card => card.Suit != Suit.None && _cardMovements.Any(move => move.CardId == card.Id && move.TurnNumber == _turnNumber && move.To == CardLocation.DiscardPile))
        .Select(card => SourceMouCard(0, card, CardLocation.DiscardPile, false)).ToArray();
    private bool SourceMouParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null) return f.SkillId == SourceMouFengshang && f.ActivationId == "same-suit-allocation" && f.WindowContext is null;
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId) return false;
        var index = _resolutionStack.FindIndex(item => item.Id == f.Id);
        if (index < 1 || _resolutionStack[index - 1].Id != w.ParentFrameId) return false;
        return _resolutionStack[index - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            PlayPhaseStartingBoundaryFrame p => p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } candidate && MountObserverCandidateMatches(f, candidate),
            ProgramCardTriggerWindowFrame p => w.CardUse is { } use && p.Action.ActionId == use.CardActionId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && MountObserverCandidateMatches(f, ToSharedCandidate(p.Candidates[p.CandidateIndex])),
            _ => false
        };
    }
    private bool ValidSourceMou(ProgramSkillFrame f) => f.ShanheSourceMou is { } r && IsSourceMouMode &&
        r.Source == SourceMouSource(f) && r.Hash == f.GameplayHash && r.Turn == _turnNumber && r.TurnOwner == _currentSeat &&
        r.Instruction == f.InstructionIndex && r.ParentId == (f.WindowContext?.ParentFrameId ?? 0) && GetPausedPrivateOfferEffect(f)?.Op == r.Operation && SourceMouParent(f) &&
        ProgramEventHistory<ShanheSourceMouStartedEvent>().Count(e => e == new ShanheSourceMouStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.Turn, r.TurnOwner, r.ParentId, r.Instruction)) == 1 &&
        r.Cards.Select(card => card.Id).Distinct().Count() == r.Cards.Count && r.Cursor >= 0 && r.Cursor <= r.Participants.Count && r.Before >= 0 && r.After >= r.Before;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62053, SkillProgramEffectOp.ResolveShanheSourceXiongni, SkillProgramEffectOp.ResolveShanheSourceFengshang, SkillProgramEffectOp.ResolveShanheSourceZhibing)]
    private bool SourceMouTrigger(ProgramTriggerLegalityContext c) => IsSourceMouMode && c.WindowContext is { } window && (c.SkillId switch
    {
        SourceMouXiongni => window.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == _currentSeat && SourceMouDiscardable(c.OwnerSeat).Length > 0,
        SourceMouFengshang => SourceMouDiscardPile().Length > 0,
        SourceMouZhibing => SourceMouThreshold(c.OwnerSeat, window) is not null,
        _ => false
    });
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 62054, SkillProgramEffectOp.ResolveShanheSourceFengshang)]
    private bool SourceMouActivation(CharacterState owner) => IsSourceMouMode && SourceMouDiscardPile().Length > 0;
    private SkillProgramStepOutcome StartSourceMou(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (!IsSourceMouMode || f.ShanheSourceMou is not null || f.InstructionIndex != 1 || !SourceMouParent(f) ||
            GetPausedPrivateOfferEffect(f)?.Op != operation || !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Source Mou Dong Zhuo lost its exact unpaid native binding.");
        var threshold = operation == SkillProgramEffectOp.ResolveShanheSourceZhibing ? SourceMouThreshold(f.OwnerSeat, f.WindowContext!) : null;
        if (operation == SkillProgramEffectOp.ResolveShanheSourceZhibing && threshold is null) return SkillProgramStepOutcome.Continue;
        var r = new ShanheSourceMouReceipt(SourceMouSource(f), f.GameplayHash, operation, _turnNumber, _currentSeat,
            f.WindowContext?.ParentFrameId ?? 0, f.InstructionIndex)
        {
            Stage = operation == SkillProgramEffectOp.ResolveShanheSourceXiongni ? ShanheSourceMouStage.OwnChoice :
                operation == SkillProgramEffectOp.ResolveShanheSourceFengshang ? ShanheSourceMouStage.GiftChoice : ShanheSourceMouStage.ThresholdChildren,
            Participants = Enumerable.Range(1, _playerCount - 1).Select(offset => (offset + f.OwnerSeat) % _playerCount).Where(seat => _players[seat].IsAlive).ToArray(),
            Before = _movementSequence, After = _movementSequence, ActionId = threshold?.Use.ActionId ?? 0,
            Actor = threshold?.Use.Actor ?? -1, Ordinal = threshold?.Ordinal ?? 0,
            Paid = threshold is not null
        };
        ReplaceRuntimeTop(f = f with { ShanheSourceMou = r });
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouStartedEvent(f.Id, r.Source, r.Hash, operation, r.Turn, r.TurnOwner, r.ParentId, r.Instruction));
        if (threshold is not null) ApplySourceMouThreshold(f);
        ContinueSourceMou(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }

    private int SourceMouChooser(ProgramSkillFrame f) => f.ShanheSourceMou!.Stage == ShanheSourceMouStage.PeerChoice
        ? f.ShanheSourceMou.Participants[f.ShanheSourceMou.Cursor] : f.OwnerSeat;
    private PromptChoice SourceMouChoice(ProgramSkillFrame f, string branch, string label, int card = 0, int target = -1) => new(
        new($"shanhe-source-mou.{f.Id}.{f.ShanheSourceMou!.Cursor}.{branch}.{card}.{target}.{f.ShanheSourceMou.Cards.Count}"), label,
        card > 0 ? [card] : [], target >= 0 ? [target] : [], new Dictionary<string, string>
        { ["program-action"] = "shanhe-source-mou", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    private IReadOnlyList<PromptChoice> SourceMouChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceMou!; var choices = new List<PromptChoice>(); var chooser = SourceMouChooser(f);
        if (r.Stage is ShanheSourceMouStage.OwnChoice or ShanheSourceMouStage.PeerChoice or ShanheSourceMouStage.HandDiscardChoice)
        {
            var cards = SourceMouDiscardable(chooser, r.Stage == ShanheSourceMouStage.HandDiscardChoice)
                .Where(card => !r.Cards.Any(picked => picked.Id == card.Id) && (r.Stage != ShanheSourceMouStage.PeerChoice || card.Suit == r.Suit)).ToArray();
            if (r.Stage != ShanheSourceMouStage.PeerChoice || cards.Length + r.Cards.Count >= 5)
                choices.AddRange(cards.Select(card => SourceMouChoice(f, "discard", r.Stage == ShanheSourceMouStage.PeerChoice ? $"选择同花色牌（{r.Cards.Count}/5）" : "选择弃置的牌", card.Id)));
            if (r.Stage == ShanheSourceMouStage.OwnChoice) choices.Add(SourceMouChoice(f, "cancel", "不发动凶逆"));
            if (r.Stage == ShanheSourceMouStage.PeerChoice) choices.Add(SourceMouChoice(f, "maximum", "不弃五张牌，体力上限-1"));
        }
        else if (r.Stage == ShanheSourceMouStage.GiftChoice)
        {
            choices.AddRange(SourceMouDiscardPile().Where(card => !r.Cards.Any(picked => picked.Id == card.Id) &&
                (r.Cards.Count == 0 || card.Suit == r.Suit)).Select(card => SourceMouChoice(f, "gift-card", "选择同花色弃牌", card.Id)));
            if (r.Cards.Count > 0) choices.Add(SourceMouChoice(f, "gift-finish", $"选择接收者（{r.Cards.Count}张）"));
        }
        else if (r.Stage == ShanheSourceMouStage.GiftRecipient)
            choices.AddRange(_players.Where(player => player.IsAlive).Select(player => SourceMouChoice(f, "recipient", "交给" + player.Name, target: player.Seat)));
        return Array.AsReadOnly(choices.ToArray());
    }
    private void PublishSourceMou(ProgramSkillFrame f)
    {
        var choices = SourceMouChoices(f);
        if (choices.Count == 0) { FinishSourceMou(f); return; }
        var seat = SourceMouChooser(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, seat, skill.Name + "：选择真实牌与效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
    [ProgramAnswer("shanhe-source-mou")]
    private void AnswerSourceMou(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceMou: { } r } f || !ValidSourceMou(f) ||
            _pendingDecision?.PlayerSeat != SourceMouChooser(f) || !SourceMouChoices(f).Any(published => published.Id == choice.Id &&
                published.Cards.SequenceEqual(choice.Cards) && published.Targets.SequenceEqual(choice.Targets) && published.Parameters.Count == choice.Parameters.Count &&
                published.Parameters.All(pair => choice.Parameters.GetValueOrDefault(pair.Key) == pair.Value)))
            throw new InvalidOperationException("Source Mou Dong Zhuo's actual private choice changed.");
        var chooser = SourceMouChooser(f); ClearPendingDecision(); ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["branch"])
        {
            case "cancel": FinishSourceMou(f); return;
            case "discard":
                var card = SourceMouDiscardable(chooser, r.Stage == ShanheSourceMouStage.HandDiscardChoice).Single(c => c.Id == choice.Cards.Single());
                var selected = r.Cards.Append(card).ToArray();
                var required = r.Stage == ShanheSourceMouStage.OwnChoice ? 1 : r.Stage == ShanheSourceMouStage.PeerChoice ? 5 : r.Required;
                ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cards = selected, Payer = chooser,
                    Suit = r.Stage == ShanheSourceMouStage.OwnChoice ? card.Suit : r.Suit } });
                if (selected.Length < required) { PublishSourceMou(f); return; }
                PaySourceMouDiscard(f, r.Stage == ShanheSourceMouStage.OwnChoice ? ShanheSourceMouStage.OwnChildren :
                    r.Stage == ShanheSourceMouStage.PeerChoice ? ShanheSourceMouStage.PeerChildren : ShanheSourceMouStage.ThresholdChildren);
                break;
            case "maximum":
                ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.MaximumChildren, Cards = [], Payer = chooser } });
                ReduceSourceMouMaximum(f, chooser); break;
            case "gift-card":
                var gift = SourceMouDiscardPile().Single(c => c.Id == choice.Cards.Single());
                ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cards = r.Cards.Append(gift).ToArray(), Suit = gift.Suit } });
                PublishSourceMou(f); return;
            case "gift-finish": ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.GiftRecipient } }); PublishSourceMou(f); return;
            case "recipient": GiveSourceMou(f, choice.Targets.Single()); break;
            default: throw new InvalidOperationException("Unknown source Mou Dong Zhuo choice.");
        }
        ContinueSourceMou(f.Id);
    }
    private void PaySourceMouDiscard(ProgramSkillFrame f, ShanheSourceMouStage stage)
    {
        var r = f.ShanheSourceMou!;
        if (!r.Cards.All(card => SourceMouDiscardable(r.Payer, r.Stage == ShanheSourceMouStage.HandDiscardChoice).Contains(card)) ||
            r.Stage == ShanheSourceMouStage.PeerChoice && (r.Cards.Count != 5 || r.Cards.Any(card => card.Suit != r.Suit)))
            throw new InvalidOperationException("Source Xiongni requires the exact frozen payable same-suit entities.");
        ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = stage, Paid = true, Before = _movementSequence, After = _movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MoveProgramLogicalDiscard(f, r.Payer, r.Cards.Select(card => card.Id).ToArray(), new(SourceMouCost), (_, records) =>
        {
            var active = _resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == f.Id); var receipt = active.ShanheSourceMou!;
            if (records.Count != r.Cards.Count || records.Any(move => !r.Cards.Any(card => card.Id == move.CardId && card.From == move.From)))
                throw new InvalidOperationException("Source Mou discarded entities lost their frozen payment.");
            ReplaceRuntimeFrame(active.Id, active with { ShanheSourceMou = receipt with { After = records.Max(move => move.Sequence) } });
            foreach (var move in records) { var card = r.Cards.Single(c => c.Id == move.CardId);
                AdvanceEventRulesAndQueueFact(new ShanheSourceMouPaidEvent(f.Id, r.Payer, card.Id, card.From, card.Kind, card.PrintedSuit, card.Rank, card.Suit, move.Sequence)); }
        });
    }
    private void LoseSourceMouGuanbian(ProgramSkillFrame f)
    {
        if (f.ShanheSourceMou!.GuanbianRemoved) return;
        ReplaceRuntimeTop(f = f with { ShanheSourceMou = f.ShanheSourceMou with { GuanbianRemoved = true } });
        var grants = _players[f.OwnerSeat].SkillGrants.Grants.Where(grant => grant.SkillId == "mou-ol:guanbian" && !grant.IsModeRule).Select(grant => grant.GrantId).ToArray();
        SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, 0, f);
        RemoveProgramExplicitSkillSources(f, grants);
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouGuanbianLostEvent(f.Id, f.OwnerSeat));
    }
    private void GiveSourceMou(ProgramSkillFrame f, int recipient)
    {
        var r = f.ShanheSourceMou!;
        if (!_players[recipient].IsAlive || r.Cards.Count == 0 || r.Cards.Any(card => card.Suit != r.Suit || !SourceMouDiscardPile().Contains(card)))
            throw new InvalidOperationException("Source Fengshang lost its original nonempty same-suit current-turn discard collection.");
        ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.GiftChildren, Paid = true, Recipient = recipient,
            Before = _movementSequence, After = _movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = r.Cards.Select(card => card.Id).ToArray(); var before = _movementSequence;
        MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(SourceMouGift),
            NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, before, null), disclosure: CardMovementDisclosure.FaceUp);
        var active = GetActiveProgramFrame(f.Id);
        ReplaceRuntimeTop(active with { ShanheSourceMou = active.ShanheSourceMou! with { After = _movementSequence } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceMouAllocationEvent(f.Id, f.OwnerSeat, recipient, ids.Length, r.Suit, _turnNumber));
        LoseSourceMouGuanbian(GetActiveProgramFrame(f.Id));
    }
    private void ReduceSourceMouMaximum(ProgramSkillFrame f, int targetSeat)
    {
        var target = _players[targetSeat];
        if (!target.IsAlive) return;
        if (target.MaxHp > 1) { ChangeProgramParticipantMaximumHp(f, targetSeat, -1); return; }
        var after = HasWushuangTactic(targetSeat, 2020) ? 7 : target.HasShenPangTongKunyu ? 1 : 0;
        if (!TryApplyHappyQiaoMaximumHp(f.Id, targetSeat, after, f.SkillId))
        {
            var hp = target.Hp; var before = target.MaxHp; target.MaxHp = after; target.Hp = Math.Min(hp, after);
            AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(targetSeat, after - before, after, f.SkillId) { HpBeforeClamp = hp });
        }
        if (after == 0 && target.IsAlive) BeginPlayerDeath(f.Id, target, null, null, null, causingProgramSkillFrameId: f.Id);
    }
    private bool DrainSourceMou(ProgramSkillFrame f)
    {
        // A real discard/gain invoice owns its movement continuation. Atomic
        // HP loss/maximum changes without that invoice return to the program.
        var continuation = IsAwaitingProgramMovement(f) ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        return TryBeginQueuedRecoveryReplacement(f.Id, continuation) ||
            TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(f.Id) ||
            TryBeginHpChangedProgramWindow(f.Id, continuation) || TryBeginCardsMovedProgramWindow(f.Id);
    }
    private void ContinueSourceMou(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceMou: { } r } f || f.Id != id) return;
        if (!ValidSourceMou(f)) throw new InvalidOperationException("Source Mou lost its original frame-owned continuation.");
        if (r.Stage is ShanheSourceMouStage.OwnChoice or ShanheSourceMouStage.PeerChoice or ShanheSourceMouStage.GiftChoice or ShanheSourceMouStage.GiftRecipient or ShanheSourceMouStage.HandDiscardChoice)
        { PublishSourceMou(f); return; }
        if (r.Stage == ShanheSourceMouStage.Wine) return;
        if (DrainSourceMou(f)) return;
        f = GetActiveProgramFrame(id); r = f.ShanheSourceMou!;
        ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive) { FinishSourceMou(f); return; }
        if (r.Stage == ShanheSourceMouStage.OwnChildren)
        {
            LoseSourceMouGuanbian(f); f = GetActiveProgramFrame(id); r = f.ShanheSourceMou!;
            // Keep OwnChildren while lost-skill children run: on return
            // the first frozen participant must still be index0.
            if (DrainSourceMou(GetActiveProgramFrame(id))) return;
            OfferSourceMouPeer(GetActiveProgramFrame(id), 0); return;
        }
        if (r.Stage is ShanheSourceMouStage.PeerChildren or ShanheSourceMouStage.MaximumChildren)
        { OfferSourceMouPeer(f, r.Cursor + 1); return; }
        if (r.Stage == ShanheSourceMouStage.GiftChildren)
        {
            var ownerGot = _cardMovements.Any(move => move.Sequence > r.Before && move.Sequence <= r.After &&
                r.Cards.Any(card => card.Id == move.CardId) && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(f.OwnerSeat));
            if (ownerGot) { FinishSourceMou(f); return; }
            ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.Wine } });
            if (BeginShanheSourceMouWine(f)) return;
        }
        FinishSourceMou(GetActiveProgramFrame(id)); return;
    }
    private void OfferSourceMouPeer(ProgramSkillFrame f, int cursor)
    {
        var r = f.ShanheSourceMou!;
        while (cursor < r.Participants.Count && !_players[r.Participants[cursor]].IsAlive) cursor++;
        if (cursor >= r.Participants.Count) { FinishSourceMou(f); return; }
        ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cursor = cursor, Stage = ShanheSourceMouStage.PeerChoice, Cards = [] } });
        PublishSourceMou(f);
    }
    private void FinishSourceMou(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f = f with { ShanheSourceMou = null, PendingMovementContinuation = null }); FinishProgramSkill(f, true); }
    [ProgramContinuation(ProgramContinuationKind.Resume, 62053)]
    private bool ResumeSourceMou(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceMou: not null } f || f.Id != id) return false; ContinueSourceMou(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 62053)]
    private bool ReturnSourceMouMovement(ProgramSkillFrame f) => f.ShanheSourceMou is not null && ResumeSourceMou(f.Id);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62053)]
    private void AssertSourceMou(ProgramSkillFrame f) { if (f.ShanheSourceMou is not null && !ValidSourceMou(f)) throw new InvalidOperationException("Invalid source Mou receipt."); }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 62053)]
    private bool SourceMouMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        f.ShanheSourceMou is { Paid: true } r && effect?.Op == r.Operation && ValidSourceMou(f) && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62053)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62053)]
    private bool KeepPaidSourceMou(ProgramSkillFrame f) => f.ShanheSourceMou is { Paid: true } && ValidSourceMou(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceXiongni, SkillProgramEffectOp.ResolveShanheSourceFengshang, SkillProgramEffectOp.ResolveShanheSourceZhibing)]
    private PromptChoice SourceMouAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShanheSourceMou!;
        var selfDying = ActiveDying is { } dying && dying.VictimSeat == f.OwnerSeat && _players[f.OwnerSeat].Hp <= 0;
        if (r.Stage == ShanheSourceMouStage.GiftRecipient)
        {
            var self = decision.Choices.FirstOrDefault(choice => choice.Targets.SequenceEqual([f.OwnerSeat]));
            if (!selfDying && self is not null) return self;
            var others = decision.Choices.Where(choice => !selfDying || !choice.Targets.Contains(f.OwnerSeat)).ToArray();
            return others.OrderByDescending(choice => OtherRevealedPublicTeamAlly(_players[f.OwnerSeat], _players[choice.Targets.Single()]))
                .ThenByDescending(choice => _aiBrains[f.OwnerSeat].ScoreProgramTarget(CreateSnapshot(f.OwnerSeat), choice.Targets.Single(), new(0, 0, 0, 1, 0, 0, false, false)))
                .FirstOrDefault() ?? self ?? decision.Choices.First();
        }
        var finish = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "gift-finish");
        if (r.Stage == ShanheSourceMouStage.GiftChoice)
        {
            // Rescue needs the issued wine promptly; otherwise collect the
            // complete selected suit before assigning those real cards.
            if (selfDying && finish is not null) return finish;
            var available = SourceMouDiscardPile();
            return decision.Choices.Where(choice => choice.Cards.Count == 1)
                .OrderByDescending(choice => available.Count(card => card.Suit == available.Single(picked => picked.Id == choice.Cards[0]).Suit))
                .ThenBy(choice => selfDying ? GetKeepValue(GetAdvancedCard(choice.Cards[0]), _players[f.OwnerSeat]) : -GetKeepValue(GetAdvancedCard(choice.Cards[0]), _players[f.OwnerSeat]))
                .FirstOrDefault() ?? finish ?? decision.Choices.First();
        }
        return decision.Choices.Where(choice => choice.Cards.Count > 0)
            .OrderBy(choice => GetKeepValue(GetAdvancedCard(choice.Cards.Single()), _players[decision.PlayerSeat])).FirstOrDefault() ?? decision.Choices.First();
    }
}
