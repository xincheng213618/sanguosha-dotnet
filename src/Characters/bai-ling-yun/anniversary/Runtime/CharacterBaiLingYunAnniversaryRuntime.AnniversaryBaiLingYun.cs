#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaiLingYunAnniversaryRuntime
{
    internal static CardConversionSource AnniversaryBlySource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardLocation AnniversaryBlyLease(ProgramSkillFrame f) => new(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"linghui:{(f.Id)}");
    internal static CardMoveReason AnniversaryBlyReason(string step) => new("skill.anniversary.linghui." + step);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 43600, SkillProgramEffectOp.ResolveAnniversaryBaiLingYunLinghui)]
    internal bool CanOfferAnniversaryBly(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<PlayerDyingEvent>().Any();
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryBaiLingYunHost.Begin(ProgramSkillFrame frame) => BeginAnniversaryBly(frame);
    internal SkillProgramStepOutcome BeginAnniversaryBly(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryBly is not null || f.InstructionIndex != 1 || f.SkillId != AnniversaryBaiLingYunComposition.Linghui || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } w)
            throw new InvalidOperationException("Linghui requires its accepted original ending candidate.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = new AnniversaryBlyReceipt(AnniversaryBlySource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, AnniversaryBlyStage.Viewing)
        {
            Before = before,
            After = before,
            ViewBefore = before,
            ViewAfter = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var lease = AnniversaryBlyLease(f);
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(lease);
        var ids = new List<int>();
        for (var i = 0; i < 3 && _corePhysicalDrawFacadesOperations.EnsureDrawPile(); i++)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
            ids.Add(card.Id);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, lease, AnniversaryBlyReason("view"), disclosure: CardMovementDisclosure.FaceDown);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AnniversaryBly = r with { Viewed = ids, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ViewAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "linghui-private", ids, SkillProgramCardSetVisibility.Private);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryBlyStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, ids.Count));
        ContinueAnniversaryBly(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal ProgramSkillFrame? AnniversaryBlyQuery(int actor, int? card = null) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryBly: { Stage: AnniversaryBlyStage.Choice or AnniversaryBlyStage.UsePayment } r } f && f.OwnerSeat == actor && (card is null || r.Viewed.Contains(card.Value)) && ValidAnniversaryBly(f) ? f : null;
    internal bool AnniversaryBlyQueryableCard(ProgramSkillFrame f, int id) => f.AnniversaryBly!.Viewed.Contains(id) && (_cardZonesOperations._cardZones.GetLocation(id) == AnniversaryBlyLease(f) || f.AnniversaryBly is { Stage: AnniversaryBlyStage.UsePayment, Plan.CardId: var selected } && selected == id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing);
    internal bool IsAnniversaryBaiLingYunExactCardQuery(CharacterState actor, int? selectedPhysicalCardId) => selectedPhysicalCardId is { } id && AnniversaryBlyQuery(actor.Seat, id)is { } frame && AnniversaryBlyQueryableCard(frame, id);
    internal Card? FindAnniversaryBaiLingYunViewedCard(CharacterState actor, int id) => AnniversaryBlyQuery(actor.Seat, id)is { } f && AnniversaryBlyQueryableCard(f, id) ? _coreAdvancedLifecycleOperations.GetAdvancedCard(id) : null;
    internal IReadOnlyList<Card> AppendAnniversaryBaiLingYunPlayableCards(CharacterState actor, IReadOnlyList<Card> cards) => AnniversaryBlyQuery(actor.Seat)is { } f ? Array.AsReadOnly(cards.Concat(f.AnniversaryBly!.Viewed.Where(id => AnniversaryBlyQueryableCard(f, id)).Select(_coreAdvancedLifecycleOperations.GetAdvancedCard)).DistinctBy(c => c.Id).ToArray()) : cards;
    internal IReadOnlyList<LegalAction> AnniversaryBlyUseActions(ProgramSkillFrame f) => AnniversaryBlyQuery(f.OwnerSeat)is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? [] : Array.AsReadOnly(f.AnniversaryBly!.Viewed.Where(id => AnniversaryBlyQueryableCard(f, id)).SelectMany(id => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], selectedPhysicalCardId: id, includeProgramActions: false).Where(a => a.CardId == id && a.ConversionSource is null && a.AdditionalConversionSources is not { Count: > 0 } && a.Kind is not (LegalActionKind.Recast or LegalActionKind.EndPlay or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect) && ((a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind) == _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind || _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.Slash && a.PlayedCardKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])))).ToArray());
    internal IReadOnlyList<PromptChoice> AnniversaryBlyChoices(ProgramSkillFrame f) => Array.AsReadOnly(AnniversaryBlyUseActions(f).Select((a, i) => new PromptChoice(new($"linghui.{(f.Id)}.use.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string> { ["program-action"] = "anniversary-linghui", ["index"] = i.ToString(System.Globalization.CultureInfo.InvariantCulture) })).Append(new(new($"linghui.{(f.Id)}.skip"), "不使用牌，继续随机获得一张", [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-linghui", ["index"] = "skip" })).ToArray());
    internal void PublishAnniversaryBly(ProgramSkillFrame f)
    {
        var choices = AnniversaryBlyChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "灵慧：仅你可见暂存的顶牌，选择真实牌及合法目标，或不使用。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, "灵慧", "灵慧", _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryBaiLingYunLinghui)]
    internal PromptChoice ChooseAnniversaryBly(PendingDecision p) => p.Choices.First();
    [ProgramAnswer("anniversary-linghui")]
    internal void AnswerAnniversaryBly(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryBly.Stage: AnniversaryBlyStage.Choice } f || !AnniversaryBlyChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Linghui rejects a stale private entity or target choice.");
        RequireAnniversaryBly(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["index"] == "skip")
        {
            GainAndReturnAnniversaryBly(f);
            return;
        }

        var a = AnniversaryBlyUseActions(f)[int.Parse(choice.Parameters["index"], System.Globalization.CultureInfo.InvariantCulture)];
        var r = f.AnniversaryBly!;
        var plan = new AnniversaryBlyPlan(a.CardId!.Value, a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.GetAdvancedCard(a.CardId.Value).Kind, a.Kind, a.TargetCardId, a.ProgramActivationId)
        {
            Targets = a.TargetSeats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.UsePayment, Plan = plan, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(plan.CardId), AnniversaryBlyLease(f), CardLocation.Processing, AnniversaryBlyReason("use"), disclosure: CardMovementDisclosure.FaceUp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AnniversaryBly = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).AnniversaryBly!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueAnniversaryBly(f.Id);
    }

    internal bool DrainAnniversaryBly(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 43600)]
    internal bool ContinueAnniversaryBly(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryBly: { } r } f || f.Id != id)
            return false;
        RequireAnniversaryBly(f);
        if (r.Stage is AnniversaryBlyStage.IssuingUse or AnniversaryBlyStage.NativeUse)
            return true;
        if (DrainAnniversaryBly(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryBly!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == AnniversaryBlyStage.Viewing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                GainAndReturnAnniversaryBly(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.Choice } });
            PublishAnniversaryBly(f);
            return true;
        }

        if (r.Stage == AnniversaryBlyStage.UsePayment)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Plan is { } p && AnniversaryBlyUseActions(f).Any(a => AnniversaryBlyActionMatches(a, p)))
            {
                IssueAnniversaryBlyUse(f);
                return true;
            }

            GainAndReturnAnniversaryBly(f);
            return true;
        }

        if (r.Stage == AnniversaryBlyStage.NativeTail)
        {
            GainAndReturnAnniversaryBly(f);
            return true;
        }

        if (r.Stage == AnniversaryBlyStage.GainChildren)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        PublishAnniversaryBly(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 43600)]
    internal bool ReturnAnniversaryBlyMovement(ProgramSkillFrame f) => f.AnniversaryBly is not null && ContinueAnniversaryBly(f.Id);
    internal static bool AnniversaryBlyActionMatches(LegalAction a, AnniversaryBlyPlan p) => a.CardId == p.CardId && a.Kind == p.ActionKind && a.TargetCardId == p.TargetCardId && a.ProgramActivationId == p.Adjustment && a.TargetSeats.SequenceEqual(p.Targets);
    internal void GainAndReturnAnniversaryBly(ProgramSkillFrame f)
    {
        var r = f.AnniversaryBly!;
        var lease = AnniversaryBlyLease(f);
        var available = r.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == lease).ToArray();
        var gain = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && available.Length > 0 ? available[_randomOperations._random.Next(available.Length)] : (int? )null;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryBly = r with { Stage = AnniversaryBlyStage.GainChildren, GainId = gain, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (gain is { } chosen)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(chosen), lease, CardLocation.Hand(f.OwnerSeat), AnniversaryBlyReason("gain"), disclosure: CardMovementDisclosure.FaceDown);
        // The reserve is an actual private viewing area, so native draws during
        // the chosen use cannot consume the two remaining viewed entities.
        foreach (var id in r.Viewed.Reverse().Where(id => _cardZonesOperations._cardZones.GetLocation(id) == lease))
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), lease, CardLocation.DrawPile, AnniversaryBlyReason("return"), disclosure: CardMovementDisclosure.FaceDown);
        if (r.Plan is { } unpaid && r.UseId == 0 && _cardZonesOperations._cardZones.GetLocation(unpaid.CardId) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(unpaid.CardId), CardLocation.Processing, CardLocation.DiscardPile, AnniversaryBlyReason("cancelled"), disclosure: CardMovementDisclosure.FaceUp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AnniversaryBly = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).AnniversaryBly!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryBlyGainCompletedEvent(f.Id, gain is null ? 0 : 1, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueAnniversaryBly(f.Id);
    }
}
