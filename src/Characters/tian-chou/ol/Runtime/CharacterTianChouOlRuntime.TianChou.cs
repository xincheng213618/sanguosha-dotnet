#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianChouOlRuntime
{
    internal const string TianChouTopReason = "skill-program.tian-chou.shandao.top";
    internal static CardConversionSource TianChouSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tian_chou_ol_TianChou.TianChouSource(f);
    internal bool TianChouHasCards(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(seat)).Any();
    SkillProgramStepOutcome global::CardGame.Core.ITianChouHost.Resolve(ProgramSkillFrame f) => BeginTianChou(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27090, SkillProgramEffectOp.ResolveShandaoTopAndGlobalTricks)]
    internal bool CanActivateTianChou(CharacterState owner) => _coreMatchParticipantsPlayersAccess._players.Any(p => TianChouHasCards(p.Seat));
    internal SkillProgramStepOutcome BeginTianChou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TianChouShandao is not null || f.SkillId != "ol:shandao" || f.ActivationId != "top-and-two-tricks" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Shandao requires its own actual once-per-Play living source.");
        var candidates = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(TianChouHasCards).ToArray();
        if (candidates.Length == 0)
            throw new InvalidOperationException("Shandao needs at least one actual HE owner.");
        var r = new TianChouShandaoReceipt(TianChouSource(f), f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Candidates = candidates
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, r.StartedAt, candidates.Length));
        for (var i = 0; i < candidates.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoCandidateEvent(f.Id, i, candidates[i]));
        PublishTianChou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> TianChouChoices(ProgramSkillFrame f)
    {
        var r = f.TianChouShandao!;
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "tian-chou-shandao",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        if (r.Stage == TianChouShandaoStage.Targets)
        {
            var result = r.Candidates.Where(s => !r.Targets.Contains(s) && TianChouHasCards(s)).Select(s => new PromptChoice(new($"shandao.{(f.Id)}.target.{(s)}"), $"选择{(_coreMatchParticipantsPlayersAccess._players[s].Name)}（五谷丰登目标）", [], [s], Args("target"))).ToList();
            if (r.Targets.Count > 0)
                result.Add(new(new($"shandao.{(f.Id)}.confirm"), "确认所选角色，逐人置顶一张牌", [], [], Args("confirm")));
            return Array.AsReadOnly(result.ToArray());
        }

        if (r.Stage != TianChouShandaoStage.Card || r.Cursor >= r.Targets.Count || !_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive)
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Targets[r.Cursor], [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.DrawPileTop).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "tian-chou-shandao", ["branch"] = "card", ["cursor"] = r.Cursor.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    }

    internal void PublishTianChou(ProgramSkillFrame f)
    {
        var choices = TianChouChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Shandao cannot publish an empty current choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var r = f.TianChouShandao!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == TianChouShandaoStage.Targets ? "善刀：选择至少一名有手牌或装备牌的角色，然后确认。" : $"善刀：将{(_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].Name)}的一张手牌或装备牌公开置于牌堆顶。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == TianChouShandaoStage.Card,
            Choices = choices,
            TargetSeat = r.Stage == TianChouShandaoStage.Card ? r.Targets[r.Cursor] : null,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("tian-chou-shandao")]
    internal void AnswerTianChou(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TianChouShandao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !TianChouChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shandao requires the owner's exact currently published target or private card slot.");
        AssertTianChou(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Tops.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("An unpaid Shandao choice lost its living source.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == TianChouShandaoStage.Targets)
        {
            if (choice.Parameters["branch"] == "target")
            {
                var seat = choice.Targets.Single();
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoSelectedEvent(f.Id, r.Targets.Count, seat));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r with { Targets = r.Targets.Append(seat).ToArray() } });
                PublishTianChou(f);
                return;
            }

            var ordered = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(r.Targets.Contains).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r with { Stage = TianChouShandaoStage.Card, TargetsFrozen = true, Targets = ordered } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoTargetsFrozenEvent(f.Id, ordered.Length));
            for (var i = 0; i < ordered.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoTargetOrderedEvent(f.Id, i, ordered[i]));
            ContinueTianChou(f.Id);
            return;
        }

        var owner = r.Targets[r.Cursor];
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var from = new CardLocation(zone, owner);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var grain = card.Kind == CardKind.WoodenOx && zone == CardZoneKind.Equipment ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r with { Stage = TianChouShandaoStage.TopChildren } });
        // A selected foreign hand remains hidden until this real card is revealed and moved.
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicHandRevealSelection = new("shandao-hand-top", f.InstructionIndex, 0, card.Id, card.Kind, from, CardLocation.DrawPile, f.OwnerSeat, f.OwnerSeat) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "shandao-hand-top"))
            return;
        ContinueShandaoAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal bool DrainTianChou(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueTianChou(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TianChouShandao: { } r } f && f.Id == id)
        {
            AssertTianChou(f);
            if (r.Stage is TianChouShandaoStage.TopChildren or TianChouShandaoStage.NativeChildren)
            {
                if (DrainTianChou(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.TianChouShandao!;
                r = r.Stage == TianChouShandaoStage.TopChildren ? r with
                {
                    Stage = TianChouShandaoStage.Card,
                    Cursor = r.Cursor + 1
                }

                : r with
                {
                    Stage = TianChouShandaoStage.TrickReady,
                    NativeUse = null
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishTianChou(f, "owner-dead-or-game-ended");
                return;
            }

            if (r.Stage == TianChouShandaoStage.NativeUse)
                return;
            if (r.Stage == TianChouShandaoStage.Targets)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishTianChou(f);
                return;
            }

            if (r.Stage == TianChouShandaoStage.Card)
            {
                if (r.Cursor < r.Targets.Count)
                {
                    if (TianChouChoices(f).Count > 0)
                    {
                        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                            PublishTianChou(f);
                        return;
                    }

                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoSkippedEvent(f.Id, r.Cursor, r.Targets[r.Cursor]));
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { TianChouShandao = r with { Cursor = r.Cursor + 1 } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r = r with { Stage = TianChouShandaoStage.TrickReady } });
            }

            if (r.TrickIndex >= 2)
            {
                FinishTianChou(f, "completed");
                return;
            }

            var kind = r.TrickIndex == 0 ? CardKind.FiveGrains : CardKind.ArrowBarrage;
            var nominated = r.TrickIndex == 0 ? r.Targets : Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(s => !r.Targets.Contains(s)).ToArray();
            var targets = TianChouShandaoTrickTargets(f.OwnerSeat, kind, nominated);
            if (targets.Count == 0)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoTrickSkippedEvent(f.Id, kind));
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { TianChouShandao = r with { TrickIndex = r.TrickIndex + 1 } });
                continue;
            }

            var origin = new TianChouShandaoUseOrigin(f.Id, r.InstructionIndex, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, f.OwnerSeat, kind)
            {
                TargetSeats = targets
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = r with { Stage = TianChouShandaoStage.NativeUse, NativeUse = origin } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoNativeIssuedEvent(f.Id, origin.CardUseFrameId, kind, f.OwnerSeat, targets.Count));
            for (var i = 0; i < targets.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoNativeTargetEvent(f.Id, origin.CardUseFrameId, i, targets[i]));
            BeginTianChouShandaoTrick(f, origin);
            return;
        }
    }

    internal void FinishTianChou(ProgramSkillFrame f, string reason)
    {
        var r = f.TianChouShandao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoCompletedEvent(f.Id, r.Tops.Count, _coreProgramEventHistoryOperations.ProgramEventHistory<TianChouShandaoNativeIssuedEvent>().Count(e => e.FrameId == f.Id), reason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TianChouShandao = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool MatchesTianChouShandao(ProgramSkillFrame f, TianChouShandaoUseOrigin o) => f.TianChouShandao is { Stage: TianChouShandaoStage.NativeUse, NativeUse: { } expected } r && o.ProgramFrameId == f.Id && o.InstructionIndex == f.InstructionIndex && o.Source == r.Source && o.GameplayHash == f.GameplayHash && o.ActualTurn == r.ActualTurn && o.PhaseInstanceId == r.PhaseInstanceId && o.ActorSeat == f.OwnerSeat && o.CardUseFrameId == expected.CardUseFrameId && o.CardKind == expected.CardKind && o.TargetSeats.SequenceEqual(expected.TargetSeats) && ValidTianChou(f);
    internal void CompleteTianChouShandaoTrick(ProgramSkillFrame f, TianChouShandaoUseOrigin origin)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !MatchesTianChouShandao(f, origin))
            throw new InvalidOperationException("Shandao lost its exact native global-trick return.");
        var r = f.TianChouShandao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoNativeReturnedEvent(f.Id, origin.CardUseFrameId, origin.CardKind));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TianChouShandao = r with { Stage = TianChouShandaoStage.NativeChildren, TrickIndex = r.TrickIndex + 1 } });
        ContinueTianChou(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27090)]
    internal bool ResumeTianChou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TianChouShandao: not null } f || f.Id != id)
            return false;
        ContinueTianChou(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27090)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27090)]
    internal bool ContinuePaidTianChou(ProgramSkillFrame f) => f.TianChouShandao is { Tops.Count: > 0 } && ValidTianChou(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShandaoTopAndGlobalTricks)]
    internal PromptChoice AiTianChou(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.TianChouShandao!;
        if (r.Stage == TianChouShandaoStage.Targets)
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
            var benefit = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
            var next = p.Choices.Where(c => c.Targets.Count == 1).Select(c => (Choice: c, Score: _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, c.Targets[0], benefit))).OrderByDescending(x => x.Score).ThenBy(x => x.Choice.Targets[0]).FirstOrDefault();
            return next.Choice is not null && (r.Targets.Count == 0 || next.Score > 0) ? next.Choice : p.Choices.Single(c => c.Parameters["branch"] == "confirm");
        }

        // Visible equipment can deliberately be stripped; a foreign hand is only a blind slot.
        return p.Choices.OrderBy(c => c.Cards is [var id] ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    [PublicHandRevealReturn("shandao-hand-top")]
    internal void ContinueShandaoAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.TianChouShandao ?? throw new InvalidOperationException("Shandao lost its actual selection cursor.");
        var payment = f.PublicHandRevealSelection ?? throw new InvalidOperationException("Shandao lost its selected entity.");
        if (payment.ReturnKey != "shandao-hand-top" || payment.InstructionIndex != f.InstructionIndex || r.Stage != TianChouShandaoStage.TopChildren)
            throw new InvalidOperationException("Shandao changed its selected-source obligation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicHandRevealSelection = null });
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(payment.CardId);
        var from = payment.From;
        var owner = from.OwnerSeat!.Value;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != from)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoSkippedEvent(f.Id, r.Cursor, owner));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { TianChouShandao = r with { Stage = TianChouShandaoStage.Card, Cursor = r.Cursor + 1 } });
            ContinueTianChou(f.Id);
            return;
        }

        var grain = card.Kind == CardKind.WoodenOx && from.Zone == CardZoneKind.Equipment ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c => c.Id).ToArray() : [];
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.DrawPile, new(TianChouTopReason));
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == card.Id && m.From == from && m.Reason.Value == TianChouTopReason);
        var top = new TianChouShandaoTop(r.Cursor, owner, card.Id, card.Kind, from, move.To, before, after, move.Sequence)
        {
            WoodenOxGrainIds = grain
        };
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TianChouShandao = f.TianChouShandao!with { Tops = r.Tops.Append(top).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianChouShandaoTopPaidEvent(f.Id, r.Cursor, owner, card.Id, card.Kind, from, move.To, before, after, move.Sequence));
        ContinueTianChou(f.Id);
    }
}
