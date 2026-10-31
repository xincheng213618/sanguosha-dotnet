#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDiscardBudgetAndFactionGiftsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IDiscardBudgetAndFactionGiftHost.ResolveDiscardBudgetParticipants(ProgramSkillFrame f) => BeginDiscardBudget(f);
    SkillProgramStepOutcome global::CardGame.Core.IDiscardBudgetAndFactionGiftHost.PreventOwnPlayOutsideTargetRangeDamage(ProgramSkillFrame f) => PreventOwnPlayOutsideRange(f);
    SkillProgramStepOutcome global::CardGame.Core.IDiscardBudgetAndFactionGiftHost.DiscardOutsideRangeAfterInsufficientUses(ProgramSkillFrame f) => BeginOutsideRangeDiscard(f);
    SkillProgramStepOutcome global::CardGame.Core.IDiscardBudgetAndFactionGiftHost.OfferCompletedFactionCostGift(ProgramSkillFrame f, string faction) => BeginCompletedFactionGift(f, faction);
    internal bool BudgetGiftSourceValid(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId);
    internal Dictionary<string, string> BudgetGiftParameters(ProgramSkillFrame f, string action, string mode) => new()
    {
        ["program-action"] = action,
        ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
        ["mode"] = mode
    };
    internal void PublishBudgetGiftPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privacy = false)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = privacy,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal SkillProgramStepOutcome BeginDiscardBudget(ProgramSkillFrame f)
    {
        if (f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard)
            throw new InvalidOperationException("Discard budget requires its actual own completed discard phase.");
        var budget = _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber ? _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Count : 0;
        f = f with
        {
            DiscardBudgetDraft = new(budget, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new ProgramDiscardBudgetParticipant(p.Seat, p.Hp)).ToArray(), Selected: [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        PublishBudgetGiftPrompt(f, f.OwnerSeat, $"本弃牌阶段弃置了 {(budget)} 张牌，请选择效果。", DiscardBudgetChoices(f));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DiscardBudgetChoices(ProgramSkillFrame f)
    {
        var d = f.DiscardBudgetDraft!;
        var selected = d.Selected!;
        var choices = new List<PromptChoice>();
        void Add(string key, string label, IReadOnlyList<int> targets) => choices.Add(new(new($"discard-budget.{(f.Id)}.{(key)}"), label, [], targets, BudgetGiftParameters(f, "discard-budget", key)));
        if (d.Mode is null)
        {
            Add("draw", $"令至多 {(d.Budget)} 名角色各摸一张牌", []);
            if (d.Budget > 0)
                Add("damage", $"对体力值合计为 {(d.Budget)} 的角色各造成1点伤害", []);
            Add("skip", "不发动", []);
            return choices;
        }

        var sum = d.Participants.Where(p => selected.Contains(p.Seat)).Sum(p => p.Hp);
        foreach (var p in d.Participants.Where(p => !selected.Contains(p.Seat) && (d.Mode == "draw" ? selected.Count < d.Budget : p.Hp > 0 && sum + p.Hp <= d.Budget)))
            Add($"seat:{(p.Seat)}", $"选择 {(_coreMatchParticipantsPlayersAccess._players[p.Seat].Name)}（体力 {(p.Hp)}）", [p.Seat]);
        if (d.Mode == "draw" || selected.Count > 0 && sum == d.Budget)
            Add("finish", "确认目标并依次结算", selected);
        Add("reset", "重新选择模式", []);
        return choices;
    }

    [ProgramAnswer("discard-budget")]
    internal void ResolveDiscardBudgetChoice(PromptChoice c)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(long.Parse(c.Parameters["frame-id"]));
        var d = f.DiscardBudgetDraft!;
        c = DiscardBudgetChoices(f).Single(x => x.Id == c.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!BudgetGiftSourceValid(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var mode = c.Parameters["mode"];
        if (mode == "skip")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DiscardBudgetDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (mode == "reset")
            d = d with
            {
                Mode = null,
                Selected = []
            };
        else if (mode is "draw" or "damage")
            d = d with
            {
                Mode = mode,
                Selected = []
            };
        else if (mode == "finish")
        {
            d = d with
            {
                Committed = true,
                Cursor = 0
            };
            f = f with
            {
                DiscardBudgetDraft = d
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDiscardBudgetCommittedEvent(f.Id, f.OwnerSeat, d.Budget, d.Mode!, d.Selected!.Select(seat => d.Participants.Single(p => p.Seat == seat)).ToArray()));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }
        else
            d = d with
            {
                Selected = d.Selected!.Append(c.Targets.Single()).ToArray()
            };
        f = f with
        {
            DiscardBudgetDraft = d
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        PublishBudgetGiftPrompt(f, f.OwnerSeat, d.Mode == "draw" ? $"请选择至多 {(d.Budget)} 名角色。已选 {(d.Selected!.Count)} 名。" : $"请选择体力值合计为 {(d.Budget)} 的角色。已选体力合计 {(d.Participants.Where(p => d.Selected!.Contains(p.Seat)).Sum(p => p.Hp))}。", DiscardBudgetChoices(f));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 970)]
    internal bool ResumeBudgetGift(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.DiscardBudgetDraft is { Committed: true } d)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DiscardBudgetDraft = null });
                return false;
            }

            var cursor = d.Cursor;
            while (cursor < d.Selected!.Count && !_coreMatchParticipantsPlayersAccess._players[d.Selected[cursor]].IsAlive)
                cursor++;
            if (cursor >= d.Selected.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { DiscardBudgetDraft = null });
                return false;
            }

            var seat = d.Selected[cursor];
            f = f with
            {
                DiscardBudgetDraft = d with
                {
                    Cursor = cursor + 1
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f);
            if (d.Mode == "damage")
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, seat, 1);
            else
            {
                _coreProgramLifecycleOperations.DrawProgramCards(f.Id, seat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.discard-budget.draw"));
                if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            }

            return true;
        }

        if (f.CompletedFactionGiftDraft is { } gift && gift.Stage is "movement" or "draw")
        {
            if (!CompletedFactionGiftLive(f, gift))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedFactionGiftDraft = null });
                return false;
            }

            if (gift.Stage == "movement")
            {
                f = f with
                {
                    CompletedFactionGiftDraft = gift with
                    {
                        Stage = "reward"
                    }
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f);
                PublishBudgetGiftPrompt(f, f.OwnerSeat, "令赠牌角色摸一张牌且本回合杀次数+1？", CompletedFactionGiftChoices(f));
                return true;
            }

            var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, f.Id, f.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, 1, affectedSeat: gift.ProviderSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(granted));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedFactionGiftDraft = null });
            return false;
        }

        return false;
    }

    internal SkillProgramStepOutcome PreventOwnPlayOutsideRange(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target })
            throw new InvalidOperationException("Range prevention requires its real damage window.");
        if (source == f.OwnerSeat && source == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(target, source))
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
        return SkillProgramStepOutcome.Continue;
    }

    internal int[] OutsideRangePlayers(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(p.Seat, owner)).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> OutsideRangeDiscardChoices(ProgramSkillFrame f) => _coreProgramOtherOwnedCardDiscardsOperations.BuildOtherOwnedCardDiscardChoices(f.Id, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], new($"skill-program.{(f.SkillId)}.ChooseOtherOwnedCardDiscard")).Where(c => OutsideRangePlayers(f.OwnerSeat).Contains(c.Targets.Single())).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "outside-range-discard" } }).ToArray();
    internal SkillProgramStepOutcome BeginOutsideRangeDiscard(ProgramSkillFrame f)
    {
        if (f.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("Range discard requires its own actual play end.");
        var choices = OutsideRangeDiscardChoices(f);
        if (_coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(f.OwnerSeat) >= OutsideRangePlayers(f.OwnerSeat).Length || choices.Count == 0)
            return SkillProgramStepOutcome.Continue;
        PublishBudgetGiftPrompt(f, f.OwnerSeat, "使用牌数不足，弃置攻击范围不包含你的角色一张牌。", choices, true);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("outside-range-discard")]
    internal void ResolveOutsideRangeDiscard(PromptChoice c)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(long.Parse(c.Parameters["frame-id"]));
        if (_coreForeignEquipmentDiscardPoliciesOperations.HasForeignDiscardCapability && c.Targets is [var publishedOwner] && Enum.TryParse<CardZoneKind>(c.Parameters.GetValueOrDefault("source-zone"), out var publishedZone) && int.TryParse(c.Parameters.GetValueOrDefault("slot-index"), out var publishedSlot) && publishedSlot >= 0 && publishedSlot < _cardZonesOperations._cardZones.CardsAt(new(publishedZone, publishedOwner)).Count && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _cardZonesOperations._cardZones.CardsAt(new(publishedZone, publishedOwner))[publishedSlot], new(publishedZone, publishedOwner), OwnedCardMoveIntent.Discard))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "公布的弃牌选择已失效。");
            return;
        }

        c = OutsideRangeDiscardChoices(f).Single(x => x.Id == c.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!BudgetGiftSourceValid(f))
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var owner = c.Targets.Single();
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]);
        var slot = int.Parse(c.Parameters["slot-index"]);
        var from = new CardLocation(zone, owner);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "公布的弃牌选择已失效。");
            return;
        }

        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new($"skill-program.{(f.SkillId)}.ChooseOtherOwnedCardDiscard"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal CardActionContext FactionGiftAction(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } use } ? _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == f.WindowContext.ParentFrameId && w.Action.ActionId == use.CardActionId).Action : throw new InvalidOperationException("Faction gift lost its actual completed action.");
    internal string? CompletedFactionGiftFaction(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.ProviderFactionId;
    internal bool CompletedFactionGiftLive(ProgramSkillFrame f, ProgramCompletedFactionGiftDraft d) => BudgetGiftSourceValid(f) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId, Role.Lord) && _coreMatchParticipantsPlayersAccess._players[d.ProviderSeat].IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[d.ProviderSeat]) == CompletedFactionGiftFaction(f) && d.ProviderSeat != f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == d.ProviderSeat && d.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && FactionGiftAction(f)is { Type: CardActionType.Use } action && action.ActionId == d.ActionId && action.ActorSeat == d.ProviderSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind);
    internal int[] AvailableFactionGiftCosts(CardActionContext a) => a.PhysicalCards.Where(p => _cardZonesOperations._cardZones.GetLocation(p.CardId).Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile).Select(p => p.CardId).ToArray();
    internal bool FactionGiftUsed(int provider) => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is PhaseChangedEvent or TurnStartedEvent).OfType<CompletedFactionCostGiftedEvent>().Any(e => e.ProviderSeat == provider && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal SkillProgramStepOutcome BeginCompletedFactionGift(ProgramSkillFrame f, string faction)
    {
        var action = FactionGiftAction(f);
        var provider = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var ids = AvailableFactionGiftCosts(action);
        var d = new ProgramCompletedFactionGiftDraft(action.ActionId, provider.Seat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, ids);
        if (!CompletedFactionGiftLive(f, d) || _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) != faction || FactionGiftUsed(provider.Seat) || ids.Length == 0)
            return SkillProgramStepOutcome.Continue;
        f = f with
        {
            CompletedFactionGiftDraft = d
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        PublishBudgetGiftPrompt(f, provider.Seat, $"将本次【杀】的牌交给 {(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}？", CompletedFactionGiftChoices(f), true);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CompletedFactionGiftChoices(ProgramSkillFrame f)
    {
        var d = f.CompletedFactionGiftDraft!;
        var reward = d.Stage == "reward";
        var action = reward ? "reward" : "give";
        return[new(new($"faction-gift.{(f.Id)}.{(action)}"), reward ? "令赠牌角色摸一张牌且杀次数+1" : "交出本次【杀】的牌", reward ? [] : d.CardIds, [reward ? d.ProviderSeat : f.OwnerSeat], BudgetGiftParameters(f, "faction-cost-gift", action)), new(new($"faction-gift.{(f.Id)}.decline"), reward ? "不奖励" : "不赠予", [], [], BudgetGiftParameters(f, "faction-cost-gift", "decline"))];
    }

    [ProgramAnswer("faction-cost-gift")]
    internal void ResolveCompletedFactionGift(PromptChoice c)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(long.Parse(c.Parameters["frame-id"]));
        var d = f.CompletedFactionGiftDraft!;
        c = CompletedFactionGiftChoices(f).Single(x => x.Id == c.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (c.Parameters["mode"] == "decline" || !CompletedFactionGiftLive(f, d))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedFactionGiftDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (d.Stage == "reward")
        {
            f = f with
            {
                CompletedFactionGiftDraft = d with
                {
                    Stage = "draw"
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f);
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, d.ProviderSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.faction-cost-gift.reward"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[d.ProviderSeat]) != effect.ProviderFactionId || FactionGiftUsed(d.ProviderSeat) || !AvailableFactionGiftCosts(FactionGiftAction(f)).SequenceEqual(d.CardIds))
            throw new InvalidOperationException("Faction gift lost its exact phase, physical costs or faction.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedFactionCostGiftedEvent(f.Id, d.ActionId, d.ProviderSeat, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, d.PhaseInstanceId, d.CardIds));
        f = f with
        {
            CompletedFactionGiftDraft = d with
            {
                Stage = "movement"
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(d.CardIds, CardLocation.Hand(f.OwnerSeat), new("skill-program.faction-cost-gift.obtain"));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
