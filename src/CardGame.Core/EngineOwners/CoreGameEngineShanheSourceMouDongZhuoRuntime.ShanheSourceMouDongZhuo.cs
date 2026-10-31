#nullable enable

using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoRuntime
{
    internal const string SourceMouXiongni = "shanhe:source-xiongni-14206", SourceMouFengshang = "shanhe:source-fengshang-14207";
    internal const string SourceMouZhibing = "shanhe:source-zhibing-14208", SourceMouZhixi = "shanhe:source-zhixi-14209";
    internal const string SourceMouCost = "skill.shanhe-source-mou.discard", SourceMouGift = "skill.shanhe-source-mou.give";
    internal bool IsSourceMouMode => _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.ShanheBattle is not null;

    SkillProgramStepOutcome IShanheSourceMouHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => StartSourceMou(frame, operation);
    internal CardConversionSource SourceMouSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal ShanheSourceMouCard SourceMouCard(int seat, Card card, CardLocation from, bool effective) => new(card.Id, card.Kind, card.Suit, card.Rank, effective ? _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card) : card.Suit, from);
    internal ShanheSourceMouCard[] SourceMouDiscardable(int seat, bool handOnly = false) => (handOnly ? new[]
    {
        CardZoneKind.Hand
    }

    : [CardZoneKind.Hand, CardZoneKind.Equipment]).SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, seat)).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card, new(zone, seat), OwnedCardMoveIntent.Discard)).Select(card => SourceMouCard(seat, card, new(zone, seat), true))).ToArray();
    internal ShanheSourceMouCard[] SourceMouDiscardPile() => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(card => card.Suit != Suit.None && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.CardId == card.Id && move.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && move.To == CardLocation.DiscardPile)).Select(card => SourceMouCard(0, card, CardLocation.DiscardPile, false)).ToArray();
    internal bool SourceMouParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == SourceMouFengshang && f.ActivationId == "same-suit-allocation" && f.WindowContext is null;
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[index - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            PlayPhaseStartingBoundaryFrame p => p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count && p.Items[p.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate),
            ProgramCardTriggerWindowFrame p => w.CardUse is { } use && p.Action.ActionId == use.CardActionId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(p.Candidates[p.CandidateIndex])),
            _ => false
        };
    }

    internal bool ValidSourceMou(ProgramSkillFrame f) => f.ShanheSourceMou is { } r && IsSourceMouMode && r.Source == SourceMouSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && r.Instruction == f.InstructionIndex && r.ParentId == (f.WindowContext?.ParentFrameId ?? 0) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == r.Operation && SourceMouParent(f) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceMouStartedEvent>().Count(e => e == new ShanheSourceMouStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.Turn, r.TurnOwner, r.ParentId, r.Instruction)) == 1 && r.Cards.Select(card => card.Id).Distinct().Count() == r.Cards.Count && r.Cursor >= 0 && r.Cursor <= r.Participants.Count && r.Before >= 0 && r.After >= r.Before;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62053, SkillProgramEffectOp.ResolveShanheSourceXiongni, SkillProgramEffectOp.ResolveShanheSourceFengshang, SkillProgramEffectOp.ResolveShanheSourceZhibing)]
    internal bool SourceMouTrigger(ProgramTriggerLegalityContext c) => IsSourceMouMode && c.WindowContext is { } window && (c.SkillId switch
    {
        SourceMouXiongni => window.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && SourceMouDiscardable(c.OwnerSeat).Length > 0,
        SourceMouFengshang => SourceMouDiscardPile().Length > 0,
        SourceMouZhibing => _coreShanheSourceMouDongZhuoZhibingOperations.SourceMouThreshold(c.OwnerSeat, window)is not null,
        _ => false
    });
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 62054, SkillProgramEffectOp.ResolveShanheSourceFengshang)]
    internal bool SourceMouActivation(CharacterState owner) => IsSourceMouMode && SourceMouDiscardPile().Length > 0;
    internal SkillProgramStepOutcome StartSourceMou(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsSourceMouMode || f.ShanheSourceMou is not null || f.InstructionIndex != 1 || !SourceMouParent(f) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != operation || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Source Mou Dong Zhuo lost its exact unpaid native binding.");
        var threshold = operation == SkillProgramEffectOp.ResolveShanheSourceZhibing ? _coreShanheSourceMouDongZhuoZhibingOperations.SourceMouThreshold(f.OwnerSeat, f.WindowContext!) : null;
        if (operation == SkillProgramEffectOp.ResolveShanheSourceZhibing && threshold is null)
            return SkillProgramStepOutcome.Continue;
        var r = new ShanheSourceMouReceipt(SourceMouSource(f), f.GameplayHash, operation, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, f.WindowContext?.ParentFrameId ?? 0, f.InstructionIndex)
        {
            Stage = operation == SkillProgramEffectOp.ResolveShanheSourceXiongni ? ShanheSourceMouStage.OwnChoice : operation == SkillProgramEffectOp.ResolveShanheSourceFengshang ? ShanheSourceMouStage.GiftChoice : ShanheSourceMouStage.ThresholdChildren,
            Participants = Enumerable.Range(1, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount - 1).Select(offset => (offset + f.OwnerSeat) % _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActionId = threshold?.Use.ActionId ?? 0,
            Actor = threshold?.Use.Actor ?? -1,
            Ordinal = threshold?.Ordinal ?? 0,
            Paid = threshold is not null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouStartedEvent(f.Id, r.Source, r.Hash, operation, r.Turn, r.TurnOwner, r.ParentId, r.Instruction));
        if (threshold is not null)
            _coreShanheSourceMouDongZhuoZhibingOperations.ApplySourceMouThreshold(f);
        ContinueSourceMou(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int SourceMouChooser(ProgramSkillFrame f) => f.ShanheSourceMou!.Stage == ShanheSourceMouStage.PeerChoice ? f.ShanheSourceMou.Participants[f.ShanheSourceMou.Cursor] : f.OwnerSeat;
    internal PromptChoice SourceMouChoice(ProgramSkillFrame f, string branch, string label, int card = 0, int target = -1) => new(new($"shanhe-source-mou.{(f.Id)}.{(f.ShanheSourceMou!.Cursor)}.{(branch)}.{(card)}.{(target)}.{(f.ShanheSourceMou.Cards.Count)}"), label, card > 0 ? [card] : [], target >= 0 ? [target] : [], new Dictionary<string, string> { ["program-action"] = "shanhe-source-mou", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> SourceMouChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceMou!;
        var choices = new List<PromptChoice>();
        var chooser = SourceMouChooser(f);
        if (r.Stage is ShanheSourceMouStage.OwnChoice or ShanheSourceMouStage.PeerChoice or ShanheSourceMouStage.HandDiscardChoice)
        {
            var cards = SourceMouDiscardable(chooser, r.Stage == ShanheSourceMouStage.HandDiscardChoice).Where(card => !r.Cards.Any(picked => picked.Id == card.Id) && (r.Stage != ShanheSourceMouStage.PeerChoice || card.Suit == r.Suit)).ToArray();
            if (r.Stage != ShanheSourceMouStage.PeerChoice || cards.Length + r.Cards.Count >= 5)
                choices.AddRange(cards.Select(card => SourceMouChoice(f, "discard", r.Stage == ShanheSourceMouStage.PeerChoice ? $"选择同花色牌（{(r.Cards.Count)}/5）" : "选择弃置的牌", card.Id)));
            if (r.Stage == ShanheSourceMouStage.OwnChoice)
                choices.Add(SourceMouChoice(f, "cancel", "不发动凶逆"));
            if (r.Stage == ShanheSourceMouStage.PeerChoice)
                choices.Add(SourceMouChoice(f, "maximum", "不弃五张牌，体力上限-1"));
        }
        else if (r.Stage == ShanheSourceMouStage.GiftChoice)
        {
            choices.AddRange(SourceMouDiscardPile().Where(card => !r.Cards.Any(picked => picked.Id == card.Id) && (r.Cards.Count == 0 || card.Suit == r.Suit)).Select(card => SourceMouChoice(f, "gift-card", "选择同花色弃牌", card.Id)));
            if (r.Cards.Count > 0)
                choices.Add(SourceMouChoice(f, "gift-finish", $"选择接收者（{(r.Cards.Count)}张）"));
        }
        else if (r.Stage == ShanheSourceMouStage.GiftRecipient)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => SourceMouChoice(f, "recipient", "交给" + player.Name, target: player.Seat)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSourceMou(ProgramSkillFrame f)
    {
        var choices = SourceMouChoices(f);
        if (choices.Count == 0)
        {
            FinishSourceMou(f);
            return;
        }

        var seat = SourceMouChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, skill.Name + "：选择真实牌与效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-source-mou")]
    internal void AnswerSourceMou(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceMou: { } r } f || !ValidSourceMou(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != SourceMouChooser(f) || !SourceMouChoices(f).Any(published => published.Id == choice.Id && published.Cards.SequenceEqual(choice.Cards) && published.Targets.SequenceEqual(choice.Targets) && published.Parameters.Count == choice.Parameters.Count && published.Parameters.All(pair => choice.Parameters.GetValueOrDefault(pair.Key) == pair.Value)))
            throw new InvalidOperationException("Source Mou Dong Zhuo's actual private choice changed.");
        var chooser = SourceMouChooser(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["branch"])
        {
            case "cancel":
                FinishSourceMou(f);
                return;
            case "discard":
                var card = SourceMouDiscardable(chooser, r.Stage == ShanheSourceMouStage.HandDiscardChoice).Single(c => c.Id == choice.Cards.Single());
                var selected = r.Cards.Append(card).ToArray();
                var required = r.Stage == ShanheSourceMouStage.OwnChoice ? 1 : r.Stage == ShanheSourceMouStage.PeerChoice ? 5 : r.Required;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cards = selected, Payer = chooser, Suit = r.Stage == ShanheSourceMouStage.OwnChoice ? card.Suit : r.Suit } });
                if (selected.Length < required)
                {
                    PublishSourceMou(f);
                    return;
                }

                PaySourceMouDiscard(f, r.Stage == ShanheSourceMouStage.OwnChoice ? ShanheSourceMouStage.OwnChildren : r.Stage == ShanheSourceMouStage.PeerChoice ? ShanheSourceMouStage.PeerChildren : ShanheSourceMouStage.ThresholdChildren);
                break;
            case "maximum":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.MaximumChildren, Cards = [], Payer = chooser } });
                ReduceSourceMouMaximum(f, chooser);
                break;
            case "gift-card":
                var gift = SourceMouDiscardPile().Single(c => c.Id == choice.Cards.Single());
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cards = r.Cards.Append(gift).ToArray(), Suit = gift.Suit } });
                PublishSourceMou(f);
                return;
            case "gift-finish":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.GiftRecipient } });
                PublishSourceMou(f);
                return;
            case "recipient":
                GiveSourceMou(f, choice.Targets.Single());
                break;
            default:
                throw new InvalidOperationException("Unknown source Mou Dong Zhuo choice.");
        }

        ContinueSourceMou(f.Id);
    }

    internal void PaySourceMouDiscard(ProgramSkillFrame f, ShanheSourceMouStage stage)
    {
        var r = f.ShanheSourceMou!;
        if (!r.Cards.All(card => SourceMouDiscardable(r.Payer, r.Stage == ShanheSourceMouStage.HandDiscardChoice).Contains(card)) || r.Stage == ShanheSourceMouStage.PeerChoice && (r.Cards.Count != 5 || r.Cards.Any(card => card.Suit != r.Suit)))
            throw new InvalidOperationException("Source Xiongni requires the exact frozen payable same-suit entities.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = stage, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Payer, r.Cards.Select(card => card.Id).ToArray(), new(SourceMouCost), (_, records) =>
        {
            var active = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == f.Id);
            var receipt = active.ShanheSourceMou!;
            if (records.Count != r.Cards.Count || records.Any(move => !r.Cards.Any(card => card.Id == move.CardId && card.From == move.From)))
                throw new InvalidOperationException("Source Mou discarded entities lost their frozen payment.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(active.Id, active with { ShanheSourceMou = receipt with { After = records.Max(move => move.Sequence) } });
            foreach (var move in records)
            {
                var card = r.Cards.Single(c => c.Id == move.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouPaidEvent(f.Id, r.Payer, card.Id, card.From, card.Kind, card.PrintedSuit, card.Rank, card.Suit, move.Sequence));
            }
        });
    }

    internal void LoseSourceMouGuanbian(ProgramSkillFrame f)
    {
        if (f.ShanheSourceMou!.GuanbianRemoved)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = f.ShanheSourceMou with { GuanbianRemoved = true } });
        var grants = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(grant => grant.SkillId == "mou-ol:guanbian" && !grant.IsModeRule).Select(grant => grant.GrantId).ToArray();
        _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, 0, f);
        _coreProgramSkillChangesOperations.RemoveProgramExplicitSkillSources(f, grants);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouGuanbianLostEvent(f.Id, f.OwnerSeat));
    }

    internal void GiveSourceMou(ProgramSkillFrame f, int recipient)
    {
        var r = f.ShanheSourceMou!;
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || r.Cards.Count == 0 || r.Cards.Any(card => card.Suit != r.Suit || !SourceMouDiscardPile().Contains(card)))
            throw new InvalidOperationException("Source Fengshang lost its original nonempty same-suit current-turn discard collection.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.GiftChildren, Paid = true, Recipient = recipient, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = r.Cards.Select(card => card.Id).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(SourceMouGift), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, before, null), disclosure: CardMovementDisclosure.FaceUp);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShanheSourceMou = active.ShanheSourceMou!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceMouAllocationEvent(f.Id, f.OwnerSeat, recipient, ids.Length, r.Suit, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        LoseSourceMouGuanbian(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ReduceSourceMouMaximum(ProgramSkillFrame f, int targetSeat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive)
            return;
        if (target.MaxHp > 1)
        {
            _coreProgramLifecycleOperations.ChangeProgramParticipantMaximumHp(f, targetSeat, -1);
            return;
        }

        var after = _coreWushuangTacticRulesOperations.HasWushuangTactic(targetSeat, 2020) ? 7 : target.HasShenPangTongKunyu ? 1 : 0;
        if (!_characterDaQiaoXiaoQiaoHappyGodOperations.TryApplyHappyQiaoMaximumHp(f.Id, targetSeat, after, f.SkillId))
        {
            var hp = target.Hp;
            var before = target.MaxHp;
            target.MaxHp = after;
            target.Hp = Math.Min(hp, after);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(targetSeat, after - before, after, f.SkillId) { HpBeforeClamp = hp });
        }

        if (after == 0 && target.IsAlive)
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, target, null, null, null, causingProgramSkillFrameId: f.Id);
    }

    internal bool DrainSourceMou(ProgramSkillFrame f)
    {
        // A real discard/gain invoice owns its movement continuation. Atomic
        // HP loss/maximum changes without that invoice return to the program.
        var continuation = CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(f) ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, continuation) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, continuation) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    }

    internal void ContinueSourceMou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceMou: { } r } f || f.Id != id)
            return;
        if (!ValidSourceMou(f))
            throw new InvalidOperationException("Source Mou lost its original frame-owned continuation.");
        if (r.Stage is ShanheSourceMouStage.OwnChoice or ShanheSourceMouStage.PeerChoice or ShanheSourceMouStage.GiftChoice or ShanheSourceMouStage.GiftRecipient or ShanheSourceMouStage.HandDiscardChoice)
        {
            PublishSourceMou(f);
            return;
        }

        if (r.Stage == ShanheSourceMouStage.Wine)
            return;
        if (DrainSourceMou(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.ShanheSourceMou!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishSourceMou(f);
            return;
        }

        if (r.Stage == ShanheSourceMouStage.OwnChildren)
        {
            LoseSourceMouGuanbian(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShanheSourceMou!;
            // Keep OwnChildren while lost-skill children run: on return
            // the first frozen participant must still be index0.
            if (DrainSourceMou(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                return;
            OfferSourceMouPeer(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), 0);
            return;
        }

        if (r.Stage is ShanheSourceMouStage.PeerChildren or ShanheSourceMouStage.MaximumChildren)
        {
            OfferSourceMouPeer(f, r.Cursor + 1);
            return;
        }

        if (r.Stage == ShanheSourceMouStage.GiftChildren)
        {
            var ownerGot = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > r.Before && move.Sequence <= r.After && r.Cards.Any(card => card.Id == move.CardId) && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(f.OwnerSeat));
            if (ownerGot)
            {
                FinishSourceMou(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Stage = ShanheSourceMouStage.Wine } });
            if (_coreShanheSourceMouDongZhuoWineOperations.BeginShanheSourceMouWine(f))
                return;
        }

        FinishSourceMou(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return;
    }

    internal void OfferSourceMouPeer(ProgramSkillFrame f, int cursor)
    {
        var r = f.ShanheSourceMou!;
        while (cursor < r.Participants.Count && !_coreMatchParticipantsPlayersAccess._players[r.Participants[cursor]].IsAlive)
            cursor++;
        if (cursor >= r.Participants.Count)
        {
            FinishSourceMou(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = r with { Cursor = cursor, Stage = ShanheSourceMouStage.PeerChoice, Cards = [] } });
        PublishSourceMou(f);
    }

    internal void FinishSourceMou(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceMou = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62053)]
    internal bool ResumeSourceMou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceMou: not null } f || f.Id != id)
            return false;
        ContinueSourceMou(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62053)]
    internal bool ReturnSourceMouMovement(ProgramSkillFrame f) => f.ShanheSourceMou is not null && ResumeSourceMou(f.Id);
    internal bool SourceMouMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.ShanheSourceMou is { Paid: true } r && effect?.Op == r.Operation && ValidSourceMou(f) && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62053)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62053)]
    internal bool KeepPaidSourceMou(ProgramSkillFrame f) => f.ShanheSourceMou is { Paid: true } && ValidSourceMou(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceXiongni, SkillProgramEffectOp.ResolveShanheSourceFengshang, SkillProgramEffectOp.ResolveShanheSourceZhibing)]
    internal PromptChoice SourceMouAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShanheSourceMou!;
        var selfDying = _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.VictimSeat == f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp <= 0;
        if (r.Stage == ShanheSourceMouStage.GiftRecipient)
        {
            var self = decision.Choices.FirstOrDefault(choice => choice.Targets.SequenceEqual([f.OwnerSeat]));
            if (!selfDying && self is not null)
                return self;
            var others = decision.Choices.Where(choice => !selfDying || !choice.Targets.Contains(f.OwnerSeat)).ToArray();
            return others.OrderByDescending(choice => _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()])).ThenByDescending(choice => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), choice.Targets.Single(), new(0, 0, 0, 1, 0, 0, false, false))).FirstOrDefault() ?? self ?? decision.Choices.First();
        }

        var finish = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "gift-finish");
        if (r.Stage == ShanheSourceMouStage.GiftChoice)
        {
            // Rescue needs the issued wine promptly; otherwise collect the
            // complete selected suit before assigning those real cards.
            if (selfDying && finish is not null)
                return finish;
            var available = SourceMouDiscardPile();
            return decision.Choices.Where(choice => choice.Cards.Count == 1).OrderByDescending(choice => available.Count(card => card.Suit == available.Single(picked => picked.Id == choice.Cards[0]).Suit)).ThenBy(choice => selfDying ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : -CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? finish ?? decision.Choices.First();
        }

        return decision.Choices.Where(choice => choice.Cards.Count > 0).OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat])).FirstOrDefault() ?? decision.Choices.First();
    }
}
