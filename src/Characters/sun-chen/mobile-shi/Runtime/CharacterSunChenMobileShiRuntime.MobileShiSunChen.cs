#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunChenMobileShiRuntime
{
    internal const string ShiScNigu = "mobile:shi-nigu", ShiScLulian = "mobile:shi-lulian";
    internal static CardMoveReason ShiScDiscardReason => new("skill.mobile-shi-sun-chen.nigu.discard");
    internal static CardMoveReason ShiScGiveReason => new("skill.mobile-shi-sun-chen.nigu.give");

    internal const string ShiScDrawReason = "skill.mobile-shi-sun-chen.lulian.draw";
    internal static CardConversionSource ShiScSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IShiSunChenHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiSc(f, op);
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 49100)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 49100)]
    internal bool ShiScImplicitDrawTwo(ProgramTargetCardWindowQuery q) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiSunChenLulian);
    internal static SkillProgramCardCategory ShiScCategory(CardKind kind) => EquipmentCatalog.IsEquipment(kind) ? SkillProgramCardCategory.Equipment : global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(kind, [SkillProgramCardCategory.Trick]) ? SkillProgramCardCategory.Trick : SkillProgramCardCategory.Basic;
    internal global::CardGame.Core.GameEngine.ShiScCompletedUse? ShiScCompleted(ProgramSkillWindowContext w, int owner)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != owner || w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(p) != SkillProgramTriggerWindow.CardUseCompleted || p.Action.ActorSeat != owner || info.ActorSeat != owner || p.Action.ActionId != info.CardActionId || p.Action.EffectiveKind != info.EffectiveKind || info.ParentCardUseFrameId != p.ParentFrameId || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return null;
        var a = p.Action;
        var nativeId = p.ParentFrameId;
        if (a.Type == CardActionType.Use)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(nativeId)is not { Step: ResolutionFrameStep.Completed, Action: { } original } use || original.ActionId != a.ActionId || use.SourceSeat != owner)
                return null;
        }
        else if (p.CompletedResponseReturn is not { IsCommitted: false } response || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(a, response.Kind) || !_coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(p))
            return null;
        if (a.PhysicalCards.Count == 0 || a.PhysicalCards.Any(c => c.From.Zone != CardZoneKind.Hand || c.From.OwnerSeat != owner && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.Actor == owner && e.UseId == (a.Type == CardActionType.Use ? nativeId : 0) && e.ActionId == a.ActionId && e.CardId == c.CardId && e.From == c.From)))
            return null;
        var targets = a.Type == CardActionType.Use ? _coreCardUseLifecycleOperations.LifecycleCardUse(nativeId)!.Action!.EffectiveDesignatedTargetSeats : a.TargetSeats;
        return new(nativeId, a, targets.Distinct().Where(_coreTableSeatQueriesOperations.IsValidPlayerSeat).ToArray());
    }

    internal bool ShiScSourceWindow(ProgramSkillFrame f) => f.SkillId == ShiScLulian && f.TriggerId == "completed-hand-category" && f.ActivationId == f.TriggerId && f.WindowContext is { } w && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is ProgramCardTriggerWindowFrame p && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(p.Candidates[p.CandidateIndex]));
    internal bool ShiScNoCategory(int owner, CardKind used) => !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Any(c => ShiScCategory(c.Kind) == ShiScCategory(used));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49100)]
    internal bool CanOfferShiSc(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (c.SkillId != ShiScLulian)
            return true;
        if (ShiScCompleted(w, c.OwnerSeat)is not { } use || !ShiScNoCategory(c.OwnerSeat, use.Action.EffectiveKind))
            return false;
        var targets = use.Targets.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray();
        return targets.Any(s => _coreMatchParticipantsPlayersAccess._players[s].Hp <= _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp) || targets.Any(s => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count <= _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count);
    }

    internal SkillProgramStepOutcome BeginShiSc(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = new ShiScReceipt(ShiScSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.WindowContext?.ParentFrameId, 0, 0, ShiScStage.Cards)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (op == SkillProgramEffectOp.ResolveMobileShiSunChenNigu)
        {
            if (f.SkillId != ShiScNigu || f.TriggerId is not null || f.WindowContext is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play)
                throw new InvalidOperationException("Nigu requires the owner's actual once-Play activation.");
        }
        else
        {
            if (op != SkillProgramEffectOp.ResolveMobileShiSunChenLulian || !ShiScSourceWindow(f) || f.WindowContext is not { } w || ShiScCompleted(w, f.OwnerSeat)is not { } use || !ShiScNoCategory(f.OwnerSeat, use.Action.EffectiveKind))
                throw new InvalidOperationException("Lulian requires the original completed physical Hand use and current absence of its category.");
            var alive = use.Targets.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray();
            r = r with
            {
                UseId = use.NativeId,
                ActionId = use.Action.ActionId,
                Targets = use.Targets,
                Stage = ShiScStage.Chain,
                Chain = alive.Any(s => _coreMatchParticipantsPlayersAccess._players[s].Hp <= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp),
                Draw = alive.Any(s => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count <= _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count)
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.Turn, r.TurnOwner, r.PhaseId, r.WindowId, r.UseId, r.ActionId));
        if (op == SkillProgramEffectOp.ResolveMobileShiSunChenLulian)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScLulianBoundEvent(f.Id, r.UseId, r.ActionId, r.Chain, r.Draw, r.Targets.Count));
            for (var i = 0; i < r.Targets.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScTargetEvent(f.Id, i, r.Targets[i]));
            ContinueShiSc(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishShiSc(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShiScOwnedChoices(ProgramSkillFrame f, int owner, bool discard) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, owner, owner, [CardZoneKind.Hand, CardZoneKind.Equipment], discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, new(zone, owner), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer));
    internal int ShiScChooser(ProgramSkillFrame f) => f.ShiSunChen is { } r && r.Stage is ShiScStage.Votes or ShiScStage.GiveCard ? r.Participants[r.Stage == ShiScStage.Votes ? r.Votes.Count : r.Cursor] : f.OwnerSeat;
    internal IReadOnlyList<int> ShiScMomentumTargets()
    {
        var live = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
        return live.Length == 0 ? [] : live.Where(p => p.Hp > live.Min(x => x.Hp)).Select(p => p.Seat).ToArray();
    }

    internal IReadOnlyList<PromptChoice> ShiScChoices(ProgramSkillFrame f)
    {
        var r = f.ShiSunChen!;
        PromptChoice C(string branch, string text, int? target = null) => new(new($"shi-sun-chen.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target ?? -1)}"), text, [], target is { } s ? [s] : [], new Dictionary<string, string> { ["program-action"] = "shi-sun-chen", ["branch"] = branch });
        if (r.Stage == ShiScStage.Cards)
        {
            var suits = r.Cards.Select(id => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToHashSet();
            return ShiScOwnedChoices(f, f.OwnerSeat, true).Where(c => r.Cards.Contains(c.Cards.Single()) || !suits.Contains(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single())))).Select(c => c with { Id = new($"shi-sun-chen.{(f.Id)}.cost.{(c.Cards.Single())}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "shi-sun-chen", ["branch"] = r.Cards.Contains(c.Cards.Single()) ? "remove" : "add" } }).Concat([C("pay", $"确认弃置{(r.Cards.Count)}张牌（可为零张）")]).Concat(r.Cards.Count > 0 ? [C("reset", "重新选择弃牌")] : Array.Empty<PromptChoice>()).ToArray();
        }

        if (r.Stage == ShiScStage.Votes)
            return (ShiScOwnedChoices(f, ShiScChooser(f), false).Count > 0 ? new[]
            {
                C("give", "私下承诺交给其一张牌")
            }

            : []).Concat([C("refuse", "不交牌")]).ToArray();
        if (r.Stage == ShiScStage.GiveCard)
            return ShiScOwnedChoices(f, ShiScChooser(f), false).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "shi-sun-chen", ["branch"] = "gift" } }).ToArray();
        return r.Stage == ShiScStage.Momentum ? ShiScMomentumTargets().Select(s => C("fire", "乘势：对 " + _coreMatchParticipantsPlayersAccess._players[s].Name + " 造成1点火焰伤害", s)).ToArray() : [];
    }

    internal void PublishShiSc(ProgramSkillFrame f)
    {
        var r = f.ShiSunChen!;
        var choices = ShiScChoices(f);
        if (choices.Count == 0)
        {
            FinishShiSc(f);
            return;
        }

        var chooser = ShiScChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage switch
        {
            ShiScStage.Cards => "逆固：多选不同花色的手牌或装备牌后确认，可以零张。",
            ShiScStage.Votes => "逆固：独立私下承诺，全部答复后同时公开。",
            ShiScStage.GiveCard => "逆固：交给孙綝一张手牌或装备牌。",
            _ => "戮连乘势：必须选择一名体力值不为最小的角色（可含自己）。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shi-sun-chen")]
    internal void AnswerShiSc(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiSunChen: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != ShiScChooser(f) || !ShiScChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shi Sun Chen rejects an unpublished chooser, original material or nonminimal fire target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == ShiScStage.Cards)
        {
            if (branch is "add" or "remove" or "reset")
            {
                int[] selected = branch == "reset" ? [] : branch == "add" ? r.Cards.Append(choice.Cards.Single()).ToArray() : r.Cards.Where(id => id != choice.Cards.Single()).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Cards = selected } });
                PublishShiSc(f);
                return;
            }

            if (branch != "pay")
                throw new InvalidOperationException("Nigu requires the explicit whole cost confirmation.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.DiscardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.Cards.Count > 0)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Cards, ShiScDiscardReason);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = f.ShiSunChen!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScCostPaidEvent(f.Id, r.Cards.Count, f.ShiSunChen!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueShiSc(f.Id);
            return;
        }

        if (r.Stage == ShiScStage.Votes)
        {
            var votes = r.Votes.Append(new ShiScVote(ShiScChooser(f), branch == "give")).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Votes = votes } });
            if (votes.Length < r.Participants.Count)
            {
                PublishShiSc(f);
                return;
            }

            for (var i = 0; i < votes.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScVoteRevealedEvent(f.Id, i, votes[i].Seat, votes[i].Give));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = f.ShiSunChen!with { Stage = ShiScStage.Gifts } });
            ContinueShiSc(f.Id);
            return;
        }

        if (r.Stage == ShiScStage.GiveCard)
        {
            if (branch != "gift")
                throw new InvalidOperationException("Nigu requires its originally committed giver's real card.");
            var id = choice.Cards.Single();
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            var material = new ShiScMaterial(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, from);
            var payer = ShiScChooser(f);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.GiftChildren, Material = material, Given = false, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(f.OwnerSeat), ShiScGiveReason, (batch, moves) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var move = moves.Single(m => m.CardId == id);
                if (move.From != from || move.CardKind != material.Kind)
                    throw new InvalidOperationException("Nigu changed its committed giver's original physical source.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, current with { ShiSunChen = current.ShiSunChen!with { After = move.Sequence, Given = move.To == CardLocation.Hand(f.OwnerSeat) } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScGiftPaidEvent(f.Id, r.Cursor, payer, id, from, move.To, move.Sequence));
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, payer, f.OwnerSeat, [id], before);
            });
            ContinueShiSc(f.Id);
            return;
        }

        if (r.Stage != ShiScStage.Momentum || branch != "fire")
            throw new InvalidOperationException("Lulian lost its forced momentum target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.DamageChildren, Target = choice.Targets.Single() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScMomentumIssuedEvent(f.Id, choice.Targets.Single()));
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Damage(f, choice.Targets.Single(), 1, nature: DamageNature.Fire);
    }

    internal bool DrainShiSc(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 49100)]
    internal bool ContinueShiSc(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShiSunChen: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (f.NativeDraw is not null || f.AttackAttempt is not null || r.Stage == ShiScStage.Drawing)
                return true;
            if (DrainShiSc(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShiSunChen!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShiSc(f);
                return true;
            }

            if (r.Stage == ShiScStage.DiscardChildren)
            {
                var participants = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.Votes, Participants = participants } });
                for (var i = 0; i < participants.Length; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScParticipantEvent(id, i, participants[i]));
                if (participants.Length > 0)
                {
                    PublishShiSc(f);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = f.ShiSunChen!with { Stage = ShiScStage.Gifts } });
                continue;
            }

            if (r.Stage == ShiScStage.GiftChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiSunChen = r with { Stage = ShiScStage.Gifts, Cursor = r.Cursor + 1, Ungiven = r.Ungiven + (r.Given ? 0 : 1), Material = null } });
                continue;
            }

            if (r.Stage == ShiScStage.Gifts)
            {
                if (r.Cursor >= r.Participants.Count)
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScArmedEvent(id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.Ungiven));
                    FinishShiSc(f);
                    return true;
                }

                var seat = r.Participants[r.Cursor];
                if (!r.Votes[r.Cursor].Give || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || ShiScOwnedChoices(f, seat, false).Count == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiSunChen = r with { Cursor = r.Cursor + 1, Ungiven = r.Ungiven + 1 } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.GiveCard } });
                PublishShiSc(f);
                return true;
            }

            if (r.Stage == ShiScStage.ChainChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiSunChen = r with { Stage = ShiScStage.Chain } });
                continue;
            }

            if (r.Stage == ShiScStage.Chain)
            {
                if (r.Chain && r.Cursor < r.Targets.Count)
                {
                    var target = r.Targets[r.Cursor];
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.ChainChildren, Cursor = r.Cursor + 1 } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScChainIssuedEvent(id, r.Cursor, target));
                    if (_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                        _coreProgramLifecycleOperations.SetProgramChainedState(f, true, target);
                    continue;
                }

                if (r.Draw && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.Drawing } });
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(ShiScDrawReason));
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiSunChen = r with { Stage = ShiScStage.DrawChildren } });
                continue;
            }

            if (r.Stage == ShiScStage.DrawChildren)
            {
                if (r.Chain && r.Draw && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && ShiScMomentumTargets().Count > 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.Momentum } });
                    PublishShiSc(f);
                    return true;
                }

                FinishShiSc(f);
                return true;
            }

            if (r.Stage == ShiScStage.DamageChildren)
            {
                FinishShiSc(f);
                return true;
            }

            PublishShiSc(f);
            return true;
        }

        return _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is ProgramSkillFrame { ShiSunChen: not null });
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiSunChenLulian)]
    internal void ReturnShiScDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.ShiSunChen!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = r with { Stage = ShiScStage.DrawChildren, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter) }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScDrawPaidEvent(f.Id, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShiSc(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49100)]
    internal bool ReturnShiScMovement(ProgramSkillFrame f) => f.ShiSunChen is not null && ContinueShiSc(f.Id);
    internal void FinishShiSc(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunChen = f.ShiSunChen!with { Stage = ShiScStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal PendingDecision? ProjectShiScCardSelection(PendingDecision decision, ProgramSkillFrame f) => f.ShiSunChen is { Stage: ShiScStage.Cards } r && decision.PlayerSeat == f.OwnerSeat ? _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(decision, f, "shi-sun-chen:cost", "branch", ["add"], ["pay"], r.Cards, 0, ShiScChoices(f).Where(c => c.Parameters.GetValueOrDefault("branch") == "add").Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()))).Distinct().Count(), resetValues: ["reset"]) : null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiSunChenNigu, SkillProgramEffectOp.ResolveMobileShiSunChenLulian)]
    internal PromptChoice ChooseShiSc(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.ShiSunChen!;
        if (r.Stage == ShiScStage.Cards)
            return p.Choices.Single(c => c.Parameters["branch"] == "pay");
        if (r.Stage == ShiScStage.Votes)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == (global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat], _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? "give" : "refuse")) ?? p.Choices.Last();
        if (r.Stage == ShiScStage.GiveCard)
            return p.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).First();
        return p.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets.Single(), new(0, 0, 0, 1, 0, 0, false, false))).First();
    }
}
