#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuKuiAnniversaryRuntime
{
    internal static CardConversionSource DuKuiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static string DuKuiShownBind(DuKuiReceipt r) => "du-kui.shown-" + r.Iteration.ToString(CultureInfo.InvariantCulture);
    internal static bool IsDuKuiFanyinTargetGrant(CardConversionSource source) => source.SkillId == AnniversaryDuKuiComposition.Fanyin;
    internal bool DuKuiAllMutualRanges() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(a => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != a.Seat).All(t => _coreNativeRuleQueriesOperations.GetCombatDistance(a.Seat, t.Seat) <= _coreNativeRuleQueriesOperations.GetAttackRange(a.Seat)));
    internal ProgramSkillFrame? DuKuiQuery(int actor, int? id = null) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { DuKui: { Operation: SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin, Stage: DuKuiStage.Choice, CardId: > 0 } r } f && f.OwnerSeat == actor && (id is null || r.CardId == id) && _cardZonesOperations._cardZones.Contains(CardLocation.Processing, r.CardId) && ValidDuKuiFrame(f) ? f : null;
    internal Card? FindDuKuiFanyinRevealedCard(CharacterState actor, int cardId) => DuKuiQuery(actor.Seat, cardId)is { } f ? _coreAdvancedLifecycleOperations.GetAdvancedCard(f.DuKui!.CardId) : null;
    internal IReadOnlyList<Card> AppendDuKuiFanyinPlayableCards(CharacterState actor, IReadOnlyList<Card> cards) => DuKuiQuery(actor.Seat)is { } f ? Array.AsReadOnly(cards.Append(_coreAdvancedLifecycleOperations.GetAdvancedCard(f.DuKui!.CardId)).DistinctBy(c => c.Id).ToArray()) : cards;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 43200, SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin, SkillProgramEffectOp.ResolveAnniversaryDuKuiPeiqi)]
    internal bool CanOfferDuKui(ProgramTriggerLegalityContext q) => q.Features.HasOperation(SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin) ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Any(c => c.Rank is >= 1 and <= 13) : DuKuiFieldChoices(null).Any();
    internal SkillProgramStepOutcome BeginDuKui(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.DuKui is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w || (op == SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin ? f.SkillId != AnniversaryDuKuiComposition.Fanyin || w.Window != SkillProgramTriggerWindow.PlayPhaseStarting || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat : f.SkillId != AnniversaryDuKuiComposition.Peiqi || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.TargetSeat != f.OwnerSeat))
            throw new InvalidOperationException("Du Kui requires his original unpaid actual Play-start or damaged-owner candidate.");
        var r = new DuKuiReceipt(DuKuiSource(f), f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, op == SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin ? DuKuiStage.RevealMovement : DuKuiStage.MoveChoice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuKuiStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, op));
        if (op == SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin)
            RevealNextDuKuiCard(f);
        else
            PublishDuKui(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RevealNextDuKuiCard(ProgramSkillFrame f)
    {
        var r = f.DuKui!;
        var pile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
        var rank = r.Iteration == 0 ? pile.Where(c => c.Rank is >= 1 and <= 13).Select(c => c.Rank).DefaultIfEmpty(0).Min() : r.Rank * 2;
        var candidates = pile.Where(c => c.Rank == rank && rank is >= 1 and <= 13).ToArray();
        if (candidates.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishDuKui(f);
            return;
        }

        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        r = r with
        {
            Stage = DuKuiStage.RevealMovement,
            Iteration = r.Iteration + 1,
            Rank = rank,
            CardId = card.Id,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            MovementSequence = 0,
            From = CardLocation.DrawPile,
            To = CardLocation.Processing,
            UseId = 0,
            ActionId = 0,
            NativeReturned = false,
            NativeBatches = [],
            UseTargets = [],
            UseAction = null,
            UseKind = null,
            TargetCardId = null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, DuKuiShownBind(r), [card.Id], SkillProgramCardSetVisibility.Public);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new("anniversary.fanyin.reveal"), moved =>
        {
            var p = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(p with { DuKui = p.DuKui!with { After = moved.Sequence, MovementSequence = moved.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuKuiRevealedEvent(f.Id, r.Iteration, card.Id, card.Rank, moved.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
        ContinueDuKui(f.Id);
    }

    internal bool DrainDuKui(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 43200)]
    internal bool ContinueDuKui(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuKui: { } r } f || f.Id != id)
            return false;
        if (r.Stage is DuKuiStage.NativeIssuing or DuKuiStage.NativeUse)
            return true;
        if (r.Stage is DuKuiStage.RevealMovement or DuKuiStage.DeclineMovement or DuKuiStage.MoveChildren or DuKuiStage.NativeTail)
        {
            if (DrainDuKui(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.DuKui!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishDuKui(f);
                return true;
            }

            if (r.Stage is DuKuiStage.DeclineMovement or DuKuiStage.NativeTail)
            {
                RevealNextDuKuiCard(f);
                return true;
            }

            if (r.Stage == DuKuiStage.MoveChildren)
            {
                if (r.Moves >= 2 || !DuKuiAllMutualRanges())
                {
                    FinishDuKui(f);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r with { Stage = DuKuiStage.MoveChoice } });
                PublishDuKui(f);
                return true;
            }

            if (!_cardZonesOperations._cardZones.Contains(CardLocation.Processing, r.CardId))
            {
                RevealNextDuKuiCard(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r with { Stage = DuKuiStage.Choice } });
        }

        PublishDuKui(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 43200)]
    internal bool DuKuiMovementReturned(ProgramSkillFrame f) => f.DuKui is not null && ContinueDuKui(f.Id);
    internal PromptChoice DuKuiChoice(ProgramSkillFrame f, string verb, string suffix, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"du-kui.{(f.Id)}.{(f.DuKui!.Iteration)}.{(f.DuKui.Moves)}.{(verb)}.{(suffix)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "anniversary-du-kui", ["verb"] = verb, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
    internal IEnumerable<PromptChoice> DuKuiFieldChoices(ProgramSkillFrame? f)
    {
        foreach (var source in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var zone in new[]
            {
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(new(zone, source.Seat)))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != source.Seat))
                    {
                        if (!(zone == CardZoneKind.Equipment ? !card.IsGeneralWeapon && _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(target.Seat, card) : _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(card, zone, target.Seat)))
                            continue;
                        if (f is null)
                        {
                            yield return new(new("du-kui.field-probe"), "移动场上牌", [card.Id], [target.Seat], new Dictionary<string, string>());
                            yield break;
                        }

                        yield return DuKuiChoice(f, "field", $"{(card.Id)}.{(target.Seat)}", $"将{(source.Name)}{((zone == CardZoneKind.Equipment ? "装备区" : "判定区"))}的牌移至{(target.Name)}", [card.Id], [target.Seat]);
                    }

        if (f?.DuKui is { Moves: > 0 })
            yield return DuKuiChoice(f, "finish", "none", "不再移动牌");
    }

    internal IReadOnlyList<LegalAction> DuKuiUseActions(ProgramSkillFrame f) => DuKuiQuery(f.OwnerSeat)is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], selectedPhysicalCardId: f.DuKui!.CardId, includeProgramActions: false).Where(a => a.CardId == f.DuKui.CardId && a.Kind != LegalActionKind.Recast && a.ConversionSource is null && a.AdditionalConversionSources is not { Count: > 0 } && ((a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.GetAdvancedCard(f.DuKui.CardId).Kind) == _coreAdvancedLifecycleOperations.GetAdvancedCard(f.DuKui.CardId).Kind || _coreAdvancedLifecycleOperations.GetAdvancedCard(f.DuKui.CardId).Kind == CardKind.Slash && a.PlayedCardKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))).ToArray()) : [];
    internal IReadOnlyList<PromptChoice> DuKuiChoices(ProgramSkillFrame f) => f.DuKui!.Stage switch
    {
        DuKuiStage.MoveChoice => Array.AsReadOnly(DuKuiFieldChoices(f).ToArray()),
        DuKuiStage.Choice => Array.AsReadOnly(DuKuiUseActions(f).Select((a, i) => DuKuiChoice(f, "use", i.ToString(CultureInfo.InvariantCulture), a.Description, [f.DuKui.CardId], a.TargetSeats)).Append(DuKuiChoice(f, "extra", "next", "不使用亮出的牌，本回合下一张牌可额外选择一个目标", [f.DuKui.CardId])).ToArray()),
        _ => []
    };
    internal void PublishDuKui(ProgramSkillFrame f)
    {
        var choices = DuKuiChoices(f);
        if (choices.Count == 0)
        {
            FinishDuKui(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.SkillId == AnniversaryDuKuiComposition.Fanyin ? "泛音：使用本次亮出的牌，或令下一张牌增加一个目标。" : "配器：选择一张场上牌及合法迁入区域。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryDuKuiFanyin)]
    internal PromptChoice AiDuKuiFanyin(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("verb") == "use") ?? p.Choices.Last();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryDuKuiPeiqi)]
    internal PromptChoice AiDuKuiPeiqi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer("anniversary-du-kui")]
    internal void AnswerDuKui(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuKui: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], DuKuiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Du Kui lost the original published entity and currently legal targets.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["verb"])
        {
            case "finish":
                FinishDuKui(f);
                return;
            case "use":
                var actions = DuKuiUseActions(f);
                var selected = DuKuiChoices(f).Where(c => c.Parameters["verb"] == "use").Select((c, i) => (c, i)).Single(x => x.c.Id == choice.Id);
                IssueDuKuiUse(f, actions[selected.i]);
                return;
            case "extra":
                _coreNextCardTargetAdjustmentsOperations.GrantNextCardTargetAdjustment(f);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuKuiTargetGrantedEvent(f.Id, r.Iteration, r.CardId));
                MutateDuKui(f, DuKuiStage.DeclineMovement, _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), CardLocation.Processing, CardLocation.DiscardPile, new("anniversary.fanyin.unused"));
                return;
            case "field":
                var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
                var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
                var to = new CardLocation(from.Zone, choice.Targets.Single());
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r with { Moves = r.Moves + 1, CardId = card.Id } });
                MutateDuKui(f, DuKuiStage.MoveChildren, card, from, to, new("anniversary.peiqi.move"));
                return;
            default:
                throw new InvalidOperationException("Du Kui cannot invent a choice branch.");
        }
    }

    internal void MutateDuKui(ProgramSkillFrame f, DuKuiStage stage, Card card, CardLocation from, CardLocation to, CardMoveReason reason)
    {
        var r = f.DuKui!with
        {
            Stage = stage,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            MovementSequence = 0,
            From = from,
            To = to
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuKui = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason, moved =>
        {
            var p = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(p with { DuKui = p.DuKui!with { After = moved.Sequence, MovementSequence = moved.Sequence, To = moved.To } });
            if (stage == DuKuiStage.MoveChildren)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuKuiFieldMovedEvent(f.Id, r.Moves, card.Id, moved.From, moved.To, moved.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
        ContinueDuKui(f.Id);
    }

    internal void FinishDuKui(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryDuKuiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginDuKui(f, op);
}
