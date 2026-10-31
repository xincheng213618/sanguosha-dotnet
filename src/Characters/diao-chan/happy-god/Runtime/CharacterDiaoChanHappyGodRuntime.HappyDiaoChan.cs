#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanHappyGodRuntime
{
    internal const string HappyDcAction = "happy-diao-chan", HappyDcPindian = "happy-huoxin-pindian", HappyDcGive = "skill-program.happy:diao-chan.give", HappyDcTake = "skill-program.happy:diao-chan.take", HappyDcDiscard = "skill-program.happy:huoxin.cost";
    SkillProgramStepOutcome global::CardGame.Core.IHappyDiaoChanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyDc(f, op);
    internal CardConversionSource HappyDcSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IEnumerable<(int Source, int Opponent)> HappyDcPairs(int? discarded = null)
    {
        var seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Any(c => c.Id != discarded)).Select(p => p.Seat).ToArray();
        foreach (var source in seats)
            foreach (var opponent in seats)
                if (source != opponent && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(source, opponent))
                    yield return (source, opponent);
    }

    internal IReadOnlyList<PromptChoice> HappyDcCosts(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => HappyDcPairs(HappyDcCard(c, f.OwnerSeat).Id).Any()).ToArray();
    internal Card HappyDcCard(PromptChoice c, int seat) => _cardZonesOperations._cardZones.CardsAt(new(Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]), seat))[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)];
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 42220, SkillProgramEffectOp.ResolveHappyDiaoChanHuoxin)]
    internal bool CanStartHappyHuoxin(CharacterState actor) => _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).Any(card => HappyDcPairs(card.Id).Any() && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(actor.Seat, card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), OwnedCardMoveIntent.Discard));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42220)]
    internal bool CanOfferHappyMeihun(ProgramTriggerLegalityContext gate) => gate.SkillId != HappyDiaoChanComposition.Meihun || gate.Trigger.Id != "first-slash-target" || gate.WindowContext is { } context && HappyDcFirstSlash(gate.OwnerSeat, context);
    internal bool HappyDcFirstSlash(int owner, ProgramSkillWindowContext context)
    {
        if (context.CardUse is not { } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.EffectiveKind))
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(facts, e => e is TurnStartedEvent);
        var since = facts.Skip(start + 1).ToArray();
        var first = since.OfType<TargetsConfirmedEvent>().FirstOrDefault(e => e.TargetSeats.Contains(owner) && since.OfType<CardUseDeclaredEvent>().Any(d => d.ResolutionId == e.ResolutionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(d.CardKind)));
        return first?.ResolutionId == use.ParentCardUseFrameId && !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyDiaoChanStartedEvent>().Any(e => e.Operation == SkillProgramEffectOp.ResolveHappyDiaoChanMeihun && e.Source.OwnerSeat == owner && e.Source.BindingId == "first-slash-target" && e.ParentId == context.ParentFrameId);
    }

    internal SkillProgramStepOutcome BeginHappyDc(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyDiaoChan is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || operation == SkillProgramEffectOp.ResolveHappyDiaoChanMeihun && !HappyDcOriginalCandidate(f) || operation == SkillProgramEffectOp.ResolveHappyDiaoChanHuoxin && (f.SkillId != HappyDiaoChanComposition.Huoxin || f.ActivationId != "charm-contest" || f.TriggerId is not null))
            throw new InvalidOperationException("Happy Diao Chan requires her original native opportunity or zero-input paid activation.");
        var r = new HappyDiaoChanReceipt(HappyDcSource(f), f.GameplayHash, operation, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, operation == SkillProgramEffectOp.ResolveHappyDiaoChanMeihun ? HappyDiaoChanStage.Target : HappyDiaoChanStage.Cost);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanStartedEvent(f.Id, r.Source, r.GameplayHash, operation, r.ParentId, r.ActualTurn, r.ActualTurnOwner));
        PublishHappyDc(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice HappyDcChoice(ProgramSkillFrame f, string mode, string text, IReadOnlyList<int>? targets = null, IReadOnlyList<int>? cards = null, string? value = null) => new(new($"happy-diao.{(f.Id)}.{(f.HappyDiaoChan!.Stage)}.{(f.HappyDiaoChan.NonwinnerIndex)}.{(mode)}.{(value)}.{(string.Join('-', targets ?? []))}.{(string.Join('-', cards ?? []))}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", HappyDcAction }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) }, { "mode", mode }, { "value", value ?? "" } });
    internal int HappyDcChooser(ProgramSkillFrame f) => f.HappyDiaoChan is { Stage: HappyDiaoChanStage.Nonwinner } r ? r.Nonwinners[r.NonwinnerIndex] : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> HappyDcChoices(ProgramSkillFrame f)
    {
        var r = f.HappyDiaoChan!;
        var choices = new List<PromptChoice>();
        if (r.Stage == HappyDiaoChanStage.Target)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                choices.Add(HappyDcChoice(f, "target", $"选择{(p.Name)}", [p.Seat]));
        if (r.Stage is HappyDiaoChanStage.MeihunSuit or HappyDiaoChanStage.HuoxinSuit)
        {
            foreach (var suit in new[]
            {
                Suit.Spade,
                Suit.Heart,
                Suit.Club,
                Suit.Diamond
            }

            )
                choices.Add(HappyDcChoice(f, "suit", $"声明{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(suit))}", value: suit.ToString()));
            if (r.Stage == HappyDiaoChanStage.HuoxinSuit)
                choices.Add(HappyDcChoice(f, "decline", "不声明花色"));
        }

        if (r.Stage == HappyDiaoChanStage.ViewHand)
        {
            var allowed = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Target, [CardZoneKind.Hand], OwnedCardMoveIntent.Obtain).Select(c => HappyDcCard(c, r.Target).Id).ToHashSet();
            foreach (var id in r.Viewed.Where(allowed.Contains))
                choices.Add(HappyDcChoice(f, "view-take", $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [r.Target], [id]));
        }

        if (r.Stage == HappyDiaoChanStage.Cost)
            foreach (var c in HappyDcCosts(f))
                choices.Add(c with { Id = new($"happy-cost.{(f.Id)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = HappyDcAction, ["mode"] = "cost" } });
        if (r.Stage == HappyDiaoChanStage.Pair)
            foreach (var pair in HappyDcPairs())
                choices.Add(HappyDcChoice(f, "pair", $"令{(_coreMatchParticipantsPlayersAccess._players[pair.Source].Name)}与{(_coreMatchParticipantsPlayersAccess._players[pair.Opponent].Name)}拼点", [pair.Source, pair.Opponent]));
        if (r.Stage == HappyDiaoChanStage.Nonwinner)
        {
            choices.Add(HappyDcChoice(f, "give", $"交给貂蝉所有{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(r.Suit))}牌"));
            choices.Add(HappyDcChoice(f, "charm", $"获得{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(r.Suit))}魅惑，至自己回合结束不能使用或打出该花色牌"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishHappyDc(ProgramSkillFrame f)
    {
        var choices = HappyDcChoices(f);
        if (choices.Count == 0)
        {
            FinishHappyDc(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, HappyDcChooser(f), choices, f.SkillId == HappyDiaoChanComposition.Meihun ? "【魅魂】请选择。" : "【惑心】请选择。", _coreTableSeatQueriesOperations.IsValidPlayerSeat(f.HappyDiaoChan!.Target) ? f.HappyDiaoChan.Target : f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(HappyDcAction)]
    internal void AnswerHappyDc(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyDiaoChan: { } r } f || !ValidHappyDc(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != HappyDcChooser(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], HappyDcChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Diao Chan requires the exact current choice of its real participant.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var mode = answer.Parameters["mode"];
        if (mode == "decline")
        {
            FinishHappyDc(f);
            return;
        }

        if (r.Stage == HappyDiaoChanStage.Target)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { Target = answer.Targets.Single(), Stage = HappyDiaoChanStage.MeihunSuit } });
            PublishHappyDc(f);
            return;
        }

        if (r.Stage is HappyDiaoChanStage.MeihunSuit or HappyDiaoChanStage.HuoxinSuit)
        {
            var suit = Enum.Parse<Suit>(answer.Parameters["value"]);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r = r with { Suit = suit } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanDeclaredEvent(f.Id, r.Target, suit));
            if (r.Stage == HappyDiaoChanStage.HuoxinSuit)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { Stage = HappyDiaoChanStage.Nonwinner } });
                ContinueHappyDc(f.Id);
                return;
            }

            var matching = HappyDcSuitCards(r.Target, suit);
            if (matching.Length > 0)
            {
                GiveHappyDc(f, r.Target, matching, false);
                return;
            }

            var viewed = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Select(c => c.Id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { Stage = HappyDiaoChanStage.ViewHand, Viewed = viewed } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPrivateHandViewedEvent(f.Id, f.SkillId, f.OwnerSeat, r.Target, viewed.Length));
            PublishHappyDc(f);
            return;
        }

        if (r.Stage == HappyDiaoChanStage.ViewHand)
        {
            GiveHappyDc(f, r.Target, answer.Cards.ToArray(), true);
            return;
        }

        if (r.Stage == HappyDiaoChanStage.Cost)
        {
            var card = HappyDcCard(answer, f.OwnerSeat);
            var from = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
            f = HappyDcChildren(f, HappyDiaoChanStage.CostChildren);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = f.HappyDiaoChan!with { Cost = new(card.Id, card.Kind, from), CostBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(HappyDcDiscard), (batch, moves) =>
            {
                var paid = moves.Single();
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { HappyDiaoChan = active.HappyDiaoChan!with { CostAfter = paid.Sequence, CostBatch = batch } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanCostPaidEvent(f.Id, batch, card.Id, paid.From, paid.To, paid.Sequence));
            });
            ContinueHappyDc(f.Id);
            return;
        }

        if (r.Stage == HappyDiaoChanStage.Pair)
        {
            var source = answer.Targets[0];
            var opponent = answer.Targets[1];
            var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { Stage = HappyDiaoChanStage.Pindian, PairSource = source, PairOpponent = opponent, PindianId = id, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanPindianIssuedEvent(f.Id, id, source, opponent));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name, skill.Description), source, opponent, programResultBind: HappyDcPindian);
            return;
        }

        if (r.Stage == HappyDiaoChanStage.Nonwinner)
        {
            var target = r.Nonwinners[r.NonwinnerIndex];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanNonwinnerChosenEvent(f.Id, r.NonwinnerIndex, target, r.Suit, mode == "give"));
            if (mode == "give")
            {
                GiveHappyDc(f, target, HappyDcSuitCards(target, r.Suit), false);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanCharmIssuedEvent(f.Id, r.Source, r.GameplayHash, target, r.Suit, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
            SyncHappyDcCharmMarker(f.Id, target, r.Suit);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { NonwinnerIndex = r.NonwinnerIndex + 1 } });
            ContinueHappyDc(f.Id);
            return;
        }

        throw new InvalidOperationException("Diao Chan cannot pay a completed branch twice.");
    }

    internal int[] HappyDcSuitCards(int seat, Suit suit) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], c) == suit).Select(c => c.Id).ToArray();
    internal void GiveHappyDc(ProgramSkillFrame f, int giver, IReadOnlyList<int> proposed, bool take)
    {
        var allowed = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, take ? f.OwnerSeat : giver, giver, [CardZoneKind.Hand, CardZoneKind.Equipment], take ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Transfer);
        var ids = allowed.Select(c => HappyDcCard(c, giver).Id).Where(proposed.Contains).Distinct().ToArray();
        f = HappyDcChildren(f, HappyDiaoChanStage.GiveChildren);
        var r = f.HappyDiaoChan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r with { Target = giver, IsViewedTake = take, TransferOrdinal = r.TransferOrdinal + 1, Moves = [] } });
        if (ids.Length == 0 || giver == f.OwnerSeat)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanTransferIssuedEvent(f.Id, r.TransferOrdinal + 1, giver, f.OwnerSeat, 0, take, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueHappyDc(f.Id);
            return;
        }

        var nativeGive = take ? null : _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, giver, f.OwnerSeat, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(take ? HappyDcTake : HappyDcGive), (batch, moves) =>
        {
            nativeGive?.Invoke(batch, moves);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = active.HappyDiaoChan!;
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { HappyDiaoChan = receipt with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Moves = moves.Select(m => new HappyDiaoChanMove(m.CardId, m.From, m.To, m.Sequence)).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanTransferIssuedEvent(f.Id, receipt.TransferOrdinal, giver, f.OwnerSeat, moves.Count, take, receipt.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        ContinueHappyDc(f.Id);
    }

    internal ProgramSkillFrame HappyDcChildren(ProgramSkillFrame f, HappyDiaoChanStage stage)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = f.HappyDiaoChan!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal bool DrainHappyDc(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueHappyDc(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyDiaoChan: { } r } f && f.Id == id)
        {
            if (r.Stage == HappyDiaoChanStage.Pindian)
            {
                var result = _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Single(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId).Result;
                var losers = new[]
                {
                    result.SourceSeat,
                    result.OpponentSeat
                }.Where(s => !result.WonBy(s)).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r = r with { Stage = HappyDiaoChanStage.PindianChildren, Result = result, Nonwinners = losers, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDiaoChanPindianReturnedEvent(f.Id, r.PindianId, result.SourceSeat, result.OpponentSeat, result.SourceRank, result.OpponentRank));
            }

            if (!ValidHappyDc(f))
                throw new InvalidOperationException("Diao Chan lost her original paid contest, exact movement or pending participant.");
            if (r.Stage is HappyDiaoChanStage.CostChildren or HappyDiaoChanStage.PindianChildren or HappyDiaoChanStage.GiveChildren)
            {
                if (DrainHappyDc(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.HappyDiaoChan!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                if (r.Stage == HappyDiaoChanStage.GiveChildren)
                {
                    if (r.Operation == SkillProgramEffectOp.ResolveHappyDiaoChanMeihun)
                    {
                        FinishHappyDc(f);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r = r with { Stage = HappyDiaoChanStage.Nonwinner, NonwinnerIndex = r.NonwinnerIndex + 1 } });
                }
                else
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyDiaoChan = r = r with { Stage = r.Stage == HappyDiaoChanStage.CostChildren ? HappyDiaoChanStage.Pair : HappyDiaoChanStage.HuoxinSuit } });
            }

            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage == HappyDiaoChanStage.Nonwinner && r.NonwinnerIndex >= r.Nonwinners.Count)
            {
                FinishHappyDc(f);
                return;
            }

            if (r.Stage == HappyDiaoChanStage.Nonwinner && !_coreMatchParticipantsPlayersAccess._players[r.Nonwinners[r.NonwinnerIndex]].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyDiaoChan = r with { NonwinnerIndex = r.NonwinnerIndex + 1 } });
                continue;
            }

            PublishHappyDc(f);
            return;
        }
    }

    internal void FinishHappyDc(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    [ProgramContinuation(ProgramContinuationKind.Resume, 42220)]
    internal bool ResumeHappyDc(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyDiaoChan: not null } f || f.Id != id)
            return false;
        ContinueHappyDc(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42220)]
    internal bool ReturnHappyDc(ProgramSkillFrame f) => f.HappyDiaoChan is not null && ResumeHappyDc(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyDiaoChanMeihun, SkillProgramEffectOp.ResolveHappyDiaoChanHuoxin)]
    internal PromptChoice AiHappyDc(PendingDecision p, ProgramSkillFrame f) => f.HappyDiaoChan!.Stage == HappyDiaoChanStage.Nonwinner ? p.Choices.First(c => c.Parameters["mode"] == "charm") : p.Choices.First();
}
