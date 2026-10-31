#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiYiOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ILiYiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginChanshuang(f, op);
    internal SkillProgramStepOutcome BeginChanshuang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Chanshuang is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || f.SkillId != "ol:chanshuang")
            throw new InvalidOperationException("Chanshuang requires its actual registered initial instruction.");
        var ending = op == SkillProgramEffectOp.ResolveChanshuangEnding;
        var mask = ending ? CurrentChanshuangMask(f.OwnerSeat) : 0;
        if (ending ? (f.TriggerId != "ending-first-items" || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } c || c.SourceSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || mask == 0) : (f.TriggerId is not null || f.ActivationId != "simultaneous-options" || f.SelectedTargetSeats is not [var peer] || peer == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[peer].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play))
            throw new InvalidOperationException("Chanshuang lost its real Play participant or Ending history.");
        var r = new ChanshuangReceipt(ChanshuangSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, ending ? f.OwnerSeat : f.SelectedTargetSeats.Single(), mask, ending ? ChanshuangStage.Ready : ChanshuangStage.Votes);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.ActualTurnOwner, r.PeerSeat, mask));
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        if (ending)
            ContinueChanshuang(f.Id);
        else
            PublishChanshuang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ChanshuangChoices(ProgramSkillFrame f)
    {
        var r = f.Chanshuang!;
        var actor = ChanshuangChooser(f);
        PromptChoice C(string option, string text, int? target = null) => new(new($"chanshuang.{(f.Id)}.{(r.Stage)}.{(r.Cursor)}.{(r.Votes.Count)}.{(r.Selected.Count)}.{(option)}.{(target)}"), text, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { { "program-action", "li-yi-chanshuang" }, { "option", option }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } });
        if (r.Stage == ChanshuangStage.Votes)
            return Array.AsReadOnly(new[] { C("recast", "1.重铸一张牌"), C("slash", "2.使用一张杀"), C("discard", "3.弃置两张牌") });
        if (r.Stage is ChanshuangStage.RecastCard or ChanshuangStage.DiscardCards)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, actor, actor, [CardZoneKind.Hand, CardZoneKind.Equipment], r.Stage == ChanshuangStage.DiscardCards ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer).Where(c => !r.Selected.Contains(c.Cards.Single())).Select(c => c with { Id = new($"chanshuang.{(f.Id)}.{(r.Cursor)}.{(r.Selected.Count)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "li-yi-chanshuang", ["option"] = "card" } }).ToArray());
        if (r.Stage == ChanshuangStage.SlashTarget)
            return Array.AsReadOnly(_coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashTargets(actor).Select(t => C("target", "指定杀的目标", t)).Append(C("decline", "不使用杀")).ToArray());
        if (r.Stage != ChanshuangStage.SlashChoice)
            return[];
        var view = f with
        {
            AssistedSlashRequest = new(actor, r.SlashTarget, ActorChoosesTarget: true)
        };
        var physical = _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashChoices(view).Where(c => c.Parameters.GetValueOrDefault("request-option") != "decline").Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "li-yi-chanshuang", ["option"] = "use" } });
        return Array.AsReadOnly(physical.Concat(_coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramSlashChoices(f, actor, [r.SlashTarget!.Value])).Concat(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(f, actor, [r.SlashTarget.Value])).Append(C("decline", "不使用杀")).ToArray());
    }

    internal void PublishChanshuang(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.Chanshuang!;
        if (r.Stage is ChanshuangStage.NativeUse or ChanshuangStage.SlashIssuing)
            return;
        var actor = ChanshuangChooser(f);
        if (!_coreMatchParticipantsPlayersAccess._players[actor].IsAlive)
        {
            CompleteChanshuangItem(f, false);
            return;
        }

        var choices = ChanshuangChoices(f);
        if (choices.Count == 0)
        {
            CompleteChanshuangItem(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, r.Stage == ChanshuangStage.Votes ? "缠双：分别密选，双方提交后同时公开。" : "缠双：执行当前已确定的选项。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("li-yi-chanshuang")]
    internal void AnswerChanshuang(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chanshuang: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != ChanshuangChooser(f) || !ChanshuangChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Chanshuang requires its real participant's exact private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = selected.Parameters["option"];
        if (r.Stage == ChanshuangStage.Votes)
        {
            var vote = new ChanshuangVote(ChanshuangChooser(f), option switch
            {
                "recast" => ChanshuangOption.Recast,
                "slash" => ChanshuangOption.Slash,
                _ => ChanshuangOption.Discard
            });
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Votes = r.Votes.Append(vote).ToArray() } });
            if (f.Chanshuang!.Votes.Count == 1)
            {
                PublishChanshuang(f);
                return;
            }

            foreach (var(v, i)in f.Chanshuang.Votes.Select((v, i) => (v, i)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangVoteRevealedEvent(f.Id, i, v.Seat, v.Option));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = f.Chanshuang with { Stage = ChanshuangStage.Ready } });
            ContinueChanshuang(f.Id);
            return;
        }

        if (option == "decline")
        {
            CompleteChanshuangItem(f, false);
            return;
        }

        if (r.Stage == ChanshuangStage.SlashTarget)
        {
            var target = selected.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangSlashSelectedEvent(f.Id, r.Cursor, ChanshuangItem(f).Seat, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Stage = ChanshuangStage.SlashChoice, SlashTarget = target } });
            PublishChanshuang(f);
            return;
        }

        if (r.Stage == ChanshuangStage.SlashChoice)
        {
            var actor = ChanshuangItem(f).Seat;
            var target = r.SlashTarget!.Value;
            if (selected.Parameters.GetValueOrDefault("request-option") == "faction")
            {
                BeginChanshuangFactionRequest(f, actor, target);
                return;
            }

            var(cards, kind, conversion) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(_coreMatchParticipantsPlayersAccess._players[actor], selected);
            if (!_coreAssistedPhysicalCardsOperations.AssistedSlashPaymentHasRange(_coreMatchParticipantsPlayersAccess._players[actor], target, cards) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], cards[0], conversion, kind, noEffectiveRank: cards.Count > 1, specificEffectiveRank: selected.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString() ? _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(_coreMatchParticipantsPlayersAccess._players[actor], cards) : null, physicalCardIds: cards.Select(c => c.Id).ToArray()))
                throw new InvalidOperationException("Chanshuang's Slash must retain ordinary range and native target legality.");
            PrepareChanshuangSuppliedSlash(f, actor, actor, target, kind, cards, conversion);
            _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], cards[0], kind, actor, physicalCards: cards, countsTowardSlashLimit: false, usesZhuqueFan: kind == CardKind.FireSlash && (cards[0].Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(_coreMatchParticipantsPlayersAccess._players[actor], cards[0])) && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[actor]), conversionSource: conversion, programSkillCardUseFrameId: f.Id);
            return;
        }

        if (r.Stage is not (ChanshuangStage.RecastCard or ChanshuangStage.DiscardCards) || selected.Cards is not [var id])
            throw new InvalidOperationException("Chanshuang lost its own HE selection stage.");
        var chosen = r.Selected.Append(id).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangMaterialSelectedEvent(f.Id, r.Cursor, r.Selected.Count, ChanshuangItem(f).Seat, id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Selected = chosen } });
        var remaining = ChanshuangChoices(f).Count;
        var required = r.Stage == ChanshuangStage.RecastCard ? 1 : 2;
        if (chosen.Length < required && remaining > 0)
        {
            PublishChanshuang(f);
            return;
        }

        PayChanshuangCards(f);
    }

    internal void PayChanshuangCards(ProgramSkillFrame f)
    {
        var r = f.Chanshuang!;
        var item = ChanshuangItem(f);
        var materials = r.Selected.Select(id =>
        {
            var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            return new ChanshuangMaterial(id, c.Kind, _cardZonesOperations._cardZones.GetLocation(id), c.IsGeneralWeapon);
        }).ToArray();
        if (materials.Length == 0 || materials.Any(c => c.From.OwnerSeat != item.Seat || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || materials.Length > (item.Option == ChanshuangOption.Recast ? 1 : 2))
            throw new InvalidOperationException("Chanshuang must freeze the executor's actual complete HE primary group.");
        var reason = item.Option == ChanshuangOption.Recast ? CardMoveReasons.RecastDiscard : new CardMoveReason(ChanshuangDiscardReason);
        var recast = item.Option == ChanshuangOption.Recast ? _coreNativeRecastInvocationsOperations.CaptureNativeRecastInvocation(f.OwnerSeat, item.Seat, r.Selected, f.Id, f.InstructionIndex, r.Cursor) : null;
        var discard = item.Option == ChanshuangOption.Discard ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, item.Seat, r.Selected, reason, f.Id) : null;
        var grain = materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(item.Seat)) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[item.Seat]).Select(c => c.Id).ToArray() : [];
        for (var i = 0; i < grain.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangGrainFrozenEvent(f.Id, r.Cursor, i, grain[i]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Stage = ChanshuangStage.CostChildren, Materials = materials, Grain = grain, Recast = recast, Discard = discard, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, CardLocation.DiscardPile, reason, (batch, moves) =>
        {
            for (var i = 0; i < materials.Length; i++)
            {
                var c = materials[i];
                var m = moves.Single(m => m.CardId == c.CardId);
                if (m.From != c.From || m.To != (c.GeneralWeapon && c.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) || m.Reason != reason)
                    throw new InvalidOperationException("Chanshuang changed a recast/discard primary destination.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangMaterialPaidEvent(f.Id, r.Cursor, i, item.Seat, c.CardId, c.Kind, m.From, m.To, m.Sequence, batch));
            }
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = f.Chanshuang!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discard);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangCostIssuedEvent(f.Id, r.Cursor, item.Seat, item.Option, materials.Length, (int)f.Chanshuang!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueChanshuang(f.Id);
    }

    internal bool DrainLiYi(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueChanshuang(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Chanshuang: { } r } f && f.Id == id)
        {
            if (r.Stage is ChanshuangStage.NativeUse or ChanshuangStage.SlashIssuing)
                return;
            if (r.Stage is ChanshuangStage.CostChildren or ChanshuangStage.DrawChildren or ChanshuangStage.NativeChildren)
            {
                if (DrainLiYi(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.Chanshuang!;
                var item = ChanshuangItem(f);
                if (r.Stage == ChanshuangStage.CostChildren && item.Option == ChanshuangOption.Recast)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Stage = ChanshuangStage.DrawChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                    var drawn = _coreMatchParticipantsPlayersAccess._players[item.Seat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[item.Seat], 1, true, CardMoveReasons.RecastDraw) : [];
                    f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = f.Chanshuang!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Drawn = drawn.Count } });
                    _coreNativeRecastInvocationsOperations.CompleteNativeRecastInvocation(r.Recast, drawn.Count);
                    var card = r.Materials.Single();
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(item.Seat, card.CardId, card.Kind, drawn.Count));
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangDrawIssuedEvent(id, r.Cursor, item.Seat, drawn.Count, (int)f.Chanshuang!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                    continue;
                }

                CompleteChanshuangItem(f, true);
                return;
            }

            if (r.Stage != ChanshuangStage.Ready)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishChanshuang(f);
                return;
            }

            if (r.Cursor >= ChanshuangItemCount(r) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishChanshuang(f);
                return;
            }

            var next = ChanshuangItem(f);
            if (!_coreMatchParticipantsPlayersAccess._players[next.Seat].IsAlive)
            {
                CompleteChanshuangItem(f, false);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = r with { Stage = next.Option == ChanshuangOption.Recast ? ChanshuangStage.RecastCard : next.Option == ChanshuangOption.Discard ? ChanshuangStage.DiscardCards : ChanshuangStage.SlashTarget } });
            PublishChanshuang(f);
            return;
        }
    }

    internal void CompleteChanshuangItem(ProgramSkillFrame f, bool actual)
    {
        var r = f.Chanshuang!;
        if (r.Stage == ChanshuangStage.Votes)
        {
            FinishChanshuang(f);
            return;
        }

        var item = ChanshuangItem(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangItemCompletedEvent(f.Id, r.Cursor, item.Seat, item.Option, actual, r.Native?.UseId));
        if (f.WeijieReturn is { } weijie && (r.Stage != ChanshuangStage.NativeChildren || !r.NativeReturned || r.Native?.UseId != weijie.UseId || !weijie.Returned))
            throw new InvalidOperationException("Chanshuang cannot release a supplier's paid lease before all native children returned.");
        if (f.HuanpeiReturn is { } huanpei && (r.Stage != ChanshuangStage.NativeChildren || !r.NativeReturned || r.Native?.UseId != huanpei.UseId || !huanpei.Returned))
            throw new InvalidOperationException("Chanshuang cannot release Huanpei before all native children returned.");
        if (f.ZonghuReturn is { } zonghu && (r.Stage != ChanshuangStage.NativeChildren || !r.NativeReturned || r.Native?.UseId != zonghu.UseId || !zonghu.Returned))
            throw new InvalidOperationException("Chanshuang cannot release its exact paid Zonghu lease before all native children returned.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, WeijieReturn = null, ZonghuReturn = null, HuanpeiReturn = null, Chanshuang = r with { Stage = ChanshuangStage.Ready, Cursor = r.Cursor + 1, Selected = [], Materials = [], Grain = [], Before = 0, After = 0, Drawn = 0, Recast = null, Discard = null, SlashTarget = null, Selection = null, Native = null, NativeReturned = false } });
        ContinueChanshuang(f.Id);
    }

    internal void FinishChanshuang(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChanshuangCompletedEvent(f.Id, f.Chanshuang!.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chanshuang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27480)]
    internal bool ResumeLiYi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chanshuang: not null } f || f.Id != id)
            return false;
        ContinueChanshuang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27480)]
    internal bool ReturnLiYiMovement(ProgramSkillFrame f)
    {
        if (f.Chanshuang is null)
            return false;
        ContinueChanshuang(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27480)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27480)]
    internal bool ContinueIssuedLiYi(ProgramSkillFrame f)
    {
        if (f.Chanshuang is not { } r || r.Stage == ChanshuangStage.Votes)
            return false;
        return r.Operation == SkillProgramEffectOp.ResolveChanshuangEnding || r.Votes.Count == 2;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices)]
    internal PromptChoice ChooseChanshuangActiveAi(PendingDecision p) => ChooseChanshuangAi(p);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveChanshuangEnding)]
    internal PromptChoice ChooseChanshuangEndingAi(PendingDecision p) => ChooseChanshuangAi(p);
    internal PromptChoice ChooseChanshuangAi(PendingDecision p)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.Chanshuang!;
        if (r.Stage == ChanshuangStage.Votes)
            return p.Choices.First(c => c.Parameters.GetValueOrDefault("option") == "recast");
        if (r.Stage == ChanshuangStage.SlashTarget)
            return p.Choices.Where(c => c.Targets.Count == 1).OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat), c.Targets[0], new(0, 0, 0, 0, 0, 1, false, false))).FirstOrDefault() ?? p.Choices.Last();
        return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") != "decline") ?? p.Choices.Last();
    }
}
