#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuXiOlRuntime
{
    internal const string DuXiObtainReason = "skill-program.du-xi.quxi.obtain";
    internal bool HasDuXiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQuxiAtPlayEnding);

    internal CardConversionSource DuXiIssuer(ProgramSkillFrame f) => new("ol:quxi", "markers", f.OwnerSeat, f.SkillInstanceId);
    internal DuXiQuxiMarkerMovedEvent[] DuXiCurrentMarkers() => !HasDuXiCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiMarkerMovedEvent>().GroupBy(e => (e.Source, e.Marker)).Select(g => g.Last()).ToArray();
    internal int? DuXiHolder(CardConversionSource source, PlayerMarkerKind marker) => _coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiMarkerMovedEvent>().LastOrDefault(e => e.Source == source && e.Marker == marker)?.Holder;
    internal bool DuXiHasPair(int owner) => _coreMatchParticipantsPlayersAccess._players.Any(a => a.IsAlive && a.Seat != owner && _coreMatchParticipantsPlayersAccess._players.Any(b => b.IsAlive && b.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(a).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(b).Count));
    SkillProgramStepOutcome global::CardGame.Core.IDuXiProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginDuXi(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26300)]
    internal bool CanOfferDuXi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var op = trigger.Effects.FirstOrDefault()?.Op;
        if (op == SkillProgramEffectOp.ResolveQuxiAtPlayEnding)
            return context.Window == SkillProgramTriggerWindow.PlayEnding && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && DuXiHasPair(candidate.OwnerSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiAcceptedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat);
        if (op == SkillProgramEffectOp.TransferQuxiAtRoundOrDeath)
        {
            var source = new CardConversionSource("ol:quxi", "markers", candidate.OwnerSeat, candidate.SkillInstanceId);
            var marks = DuXiCurrentMarkers().Where(e => e.Source == source).ToArray();
            if (marks.Length == 0)
                return false;
            if (context.Window == SkillProgramTriggerWindow.CharacterDied)
                return context.TargetSeat is { } dead && !_coreMatchParticipantsPlayersAccess._players[dead].IsAlive && marks.Any(e => e.Holder == dead);
            return context.Window == SkillProgramTriggerWindow.RoundStarting && _coreActualRoundProgressRoundTurnSeatsAccess._roundTurnSeats.Count == 1 && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra;
        }

        if (op == SkillProgramEffectOp.RecordBixiongActualDiscard)
            return context.MovementBatch is { } batch && DuXiBixiongMovements(batch, candidate.OwnerSeat).Any();
        return true;
    }

    internal SkillProgramStepOutcome BeginDuXi(ProgramSkillFrame input, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (operation == SkillProgramEffectOp.RecordBixiongActualDiscard)
        {
            if (f.WindowContext?.MovementBatch is { } batch)
                CaptureDuXiBixiongBatch(batch);
            return SkillProgramStepOutcome.Continue;
        }

        if (f.DuXiQuxi is not null || f.WindowContext is not { } context || f.InstructionIndex != 1 || f.SkillId != "ol:quxi" || !CanOfferDuXi(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Quxi requires its actual unpaid play ending, new round or marked holder death.");
        var initial = operation == SkillProgramEffectOp.ResolveQuxiAtPlayEnding;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuXiQuxi = new(DuXiIssuer(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, initial ? DuXiQuxiStage.FaceDownChildren : DuXiQuxiStage.TransferFeng, initial) });
        if (initial)
        {
            var wasFaceDown = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiQuxiAcceptedEvent(f.Id, DuXiIssuer(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, wasFaceDown));
            // The native face-state producer preserves an already face-down general.
            if (!wasFaceDown)
                _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, f.OwnerSeat, true);
        }

        ContinueDuXi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> DuXiChoices(ProgramSkillFrame f)
    {
        var r = f.DuXiQuxi!;
        var result = new List<PromptChoice>();
        void Add(string branch, string key, string label, IReadOnlyList<int>? targets = null) => result.Add(new(new($"du-xi.{(f.Id)}.{(r.Stage)}.{(branch)}.{(key)}"), label, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "du-xi-quxi", ["branch"] = branch }));
        if (r.Stage == DuXiQuxiStage.Pair)
        {
            foreach (var less in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                foreach (var more in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(less).Count))
                    Add("pair", $"{(less.Seat)}-{(more.Seat)}", $"令{(less.Name)}获得{(more.Name)}一张牌及“丰”，后者获得“歉”", [less.Seat, more.Seat]);
        }
        else if (r.Stage == DuXiQuxiStage.Obtain && r.LessHandSeat is { } recipient && r.MoreHandSeat is { } owner && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            result.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, recipient, owner, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Obtain).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "du-xi-quxi", ["branch"] = "obtain" } }));
        else if (r.Stage is DuXiQuxiStage.TransferFeng or DuXiQuxiStage.TransferQian)
        {
            var kind = r.Stage == DuXiQuxiStage.TransferFeng ? PlayerMarkerKind.QuxiFeng : PlayerMarkerKind.QuxiQian;
            if (DuXiHolder(r.Source, kind)is { } previous)
                foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != previous))
                    Add("transfer", player.Seat.ToString(CultureInfo.InvariantCulture), $"将“{(PlayerMarkerCatalog.GetDisplayName(kind))}”移给{(player.Name)}", [player.Seat]);
            Add("skip", "skip", "不移动此标记");
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishDuXi(ProgramSkillFrame f)
    {
        var r = f.DuXiQuxi!;
        var choices = DuXiChoices(f);
        if (choices.Count == 0)
        {
            FinishDuXi(f);
            return;
        }

        var chooser = r.Stage == DuXiQuxiStage.Obtain ? r.LessHandSeat!.Value : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == DuXiQuxiStage.Obtain ? "驱徙：获得另一名角色一张牌。" : "驱徙：选择标记的对象。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = r.Stage == DuXiQuxiStage.Obtain ? r.MoreHandSeat : null,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("du-xi-quxi")]
    internal void AnswerDuXi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuXiQuxi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == DuXiQuxiStage.Obtain ? r.LessHandSeat : f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], DuXiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Quxi lost its exact current pair, private opaque card slot or source-owned transfer choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "pair")
        {
            var less = choice.Targets[0];
            var more = choice.Targets[1];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiQuxiPairChosenEvent(f.Id, less, more, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[less]).Count, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[more]).Count));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuXiQuxi = r with { Stage = DuXiQuxiStage.Obtain, LessHandSeat = less, MoreHandSeat = more } });
        }
        else if (branch == "obtain")
        {
            var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), r.MoreHandSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var recipient = r.LessHandSeat!.Value;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuXiQuxi = r with { Stage = DuXiQuxiStage.ObtainChildren, ObtainedCardId = card.Id, ObtainedFrom = from, SequenceBefore = before, SequenceAfter = before } });
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, card.IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.Hand(recipient), new(DuXiObtainReason), movement =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { DuXiQuxi = current.DuXiQuxi!with { SequenceAfter = movement.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiQuxiCardObtainedEvent(f.Id, recipient, card.Id, from, before, movement.Sequence));
            });
        }
        else
        {
            var kind = r.Stage == DuXiQuxiStage.TransferFeng ? PlayerMarkerKind.QuxiFeng : PlayerMarkerKind.QuxiQian;
            if (branch == "transfer")
                SetDuXiMarker(f, kind, choice.Targets.Single());
            else if (branch != "skip")
                throw new InvalidOperationException("Unknown Quxi marker choice.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { DuXiQuxi = r with { Stage = r.Stage == DuXiQuxiStage.TransferFeng ? DuXiQuxiStage.TransferQian : DuXiQuxiStage.Finished } });
        }

        ContinueDuXi(f.Id);
    }

    internal void SetDuXiMarker(ProgramSkillFrame f, PlayerMarkerKind marker, int holder)
    {
        var r = f.DuXiQuxi!;
        var previous = DuXiHolder(r.Source, marker);
        if (!_coreMatchParticipantsPlayersAccess._players[holder].IsAlive || holder == previous)
            throw new InvalidOperationException("Quxi must move its own mark to a different living holder.");
        void Change(int seat, int delta)
        {
            var player = _coreMatchParticipantsPlayersAccess._players[seat];
            var key = (marker, r.Source.OwnerSeat);
            var attributed = player.MarkerSourceCounts.GetValueOrDefault(key) + delta;
            var total = player.Markers.GetValueOrDefault(marker) + delta;
            if (attributed < 0 || total < 0)
                throw new InvalidOperationException("Quxi cannot consume another source's mark.");
            if (attributed == 0)
                player.MarkerSourceCounts.Remove(key);
            else
                player.MarkerSourceCounts[key] = attributed;
            if (total == 0)
                player.Markers.Remove(marker);
            else
                player.Markers[marker] = total;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, seat, marker, delta, total, r.Source.OwnerSeat, "program.du-xi.quxi.marker"));
        }

        if (previous is { } old)
            Change(old, -1);
        Change(holder, 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiQuxiMarkerMovedEvent(f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, marker, previous, holder));
    }

    internal bool DrainDuXiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueDuXi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { DuXiQuxi: { } r } f && f.Id == id)
        {
            if (DrainDuXiChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.DuXiQuxi!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishDuXi(f);
                return;
            }

            if (r.Stage == DuXiQuxiStage.FaceDownChildren)
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DuXiQuxi = r with { Stage = DuXiQuxiStage.Pair } });
            else if (r.Stage == DuXiQuxiStage.ObtainChildren)
            {
                if (_coreMatchParticipantsPlayersAccess._players[r.LessHandSeat!.Value].IsAlive)
                    SetDuXiMarker(f, PlayerMarkerKind.QuxiFeng, r.LessHandSeat.Value);
                if (_coreMatchParticipantsPlayersAccess._players[r.MoreHandSeat!.Value].IsAlive)
                    SetDuXiMarker(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), PlayerMarkerKind.QuxiQian, r.MoreHandSeat.Value);
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { DuXiQuxi = r with { Stage = DuXiQuxiStage.Finished } });
            }
            else if (r.Stage == DuXiQuxiStage.Finished)
            {
                FinishDuXi(f);
                return;
            }
            else
            {
                PublishDuXi(f);
                return;
            }
        }
    }

    internal void FinishDuXi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DuXiQuxi = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26300)]
    internal bool ResumeDuXi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuXiQuxi: not null } f || f.Id != id)
            return false;
        ContinueDuXi(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26300)]
    internal bool ContinueIssuedDuXi(ProgramSkillFrame f) => f.DuXiQuxi is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveQuxiAtPlayEnding, SkillProgramEffectOp.TransferQuxiAtRoundOrDeath)]
    internal PromptChoice AiDuXi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "skip") ?? p.Choices.First();
    internal int DuXiDrawAdjustment(CharacterState player) => !player.IsAlive ? 0 : DuXiCurrentMarkers().Where(e => e.Holder == player.Seat).Sum(e => e.Marker == PlayerMarkerKind.QuxiFeng ? 1 : -1);
    internal bool TrySkipDuXiDiscardPhase(CharacterState player)
    {
        var receipt = _coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiAcceptedEvent>().SingleOrDefault(e => e.Source.OwnerSeat == player.Seat && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber);
        if (receipt is null)
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<DuXiQuxiDiscardSkippedEvent>().Any(e => e.AcceptedFrameId == receipt.FrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuXiQuxiDiscardSkippedEvent(receipt.FrameId, player.Seat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber));
        return true;
    }
}
