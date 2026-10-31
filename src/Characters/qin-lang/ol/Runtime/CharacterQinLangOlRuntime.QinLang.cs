#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQinLangOlRuntime
{
    internal const string QinLangDrawReason = "skill-program.qin-lang.xianying.draw", QinLangDiscardReason = "skill-program.qin-lang.xianying.discard";
    internal bool HasQinLangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXianyingDrawDiscard);

    internal static CardConversionSource QinLangSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qin_lang_ol_QinLang.QinLangSource(f);
    internal static bool SameQinLangSource(CardConversionSource a, CardConversionSource b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qin_lang_ol_QinLang.SameQinLangSource(a, b);
    internal bool QinLangCountUsed(int owner, int round, int count) => _coreProgramEventHistoryOperations.ProgramEventHistory<QinLangXianyingDiscardPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.RoundNumber == round && e.Count == count);
    internal bool QinLangGrantConsumed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<QinLangXianyingGrantConsumedEvent>().Any(e => e.GrantFrameId == id);
    internal IEnumerable<QinLangXianyingGrantedEvent> OutstandingQinLangGrants(int turn) => _coreProgramEventHistoryOperations.ProgramEventHistory<QinLangXianyingGrantedEvent>().Where(e => e.ActualTurn == turn && !QinLangGrantConsumed(e.FrameId));
    internal IReadOnlyList<QinLangXianyingCard> QinLangDiscardable(ProgramSkillFrame f) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.OwnerSeat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c)).Select(c => new QinLangXianyingCard(c.Id, c.Kind, _cardZonesOperations._cardZones.GetLocation(c.Id), QinLangName(_cardZonesOperations._cardZones.GetLocation(c.Id).Zone == CardZoneKind.Hand ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c) : c.Kind))).ToArray());
    SkillProgramStepOutcome global::CardGame.Core.IQinLangHost.Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => BeginQinLang(f, effect.Op == SkillProgramEffectOp.ResolveXianyingTurnEndUses);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 27170)]
    internal bool CollectQinLang(SkillProgramTrigger t) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveXianyingTurnEndUses }];
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeQinLangXianyingAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (!HasQinLangCapability)
            return ordinary;
        var items = ordinary.Where(i => i.Candidate is not { SkillId: "ol:xianying", BindingId: "turn-end-own" or "turn-end-foreign" }).ToList();
        foreach (var group in OutstandingQinLangGrants(parent.TurnNumber).Where(e => _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive).GroupBy(e => (e.Source.OwnerSeat, e.Source.SkillInstanceId, e.GameplayHash)))
        {
            var c = new ProgramTriggerCandidate(group.Key.OwnerSeat, "ol:xianying", group.Key.OwnerSeat == parent.OwnerSeat ? "turn-end-own" : "turn-end-foreign", group.Key.SkillInstanceId, group.Key.GameplayHash, 0);
            items.Add(new(TurnEndingBoundaryItemKind.Program, c.Priority, $"program:{(c.SkillId)}:{(c.BindingId)}:{(c.SkillInstanceId)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }

        return Array.AsReadOnly(items.OrderBy(i => _coreTableOrderOperations.RelativeTableOrder(parent.OwnerSeat, i.Candidate!.OwnerSeat)).ThenByDescending(i => i.Priority).ThenBy(i => i.StableIdentity, StringComparer.Ordinal).ToArray());
    }

    [ProgramIssuedTrigger(530)]
    internal bool IsIssuedQinLangXianyingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasQinLangCapability || c.SkillId != "ol:xianying" || c.BindingId is not ("turn-end-own" or "turn-end-foreign") || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || context.Window != SkillProgramTriggerWindow.AfterTurnEnded || context.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent) != c || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.Facts != parent.AfterTurnEnded!.Items[parent.AfterTurnEnded.ItemIndex].Facts || c.BindingId != (c.OwnerSeat == parent.OwnerSeat ? "turn-end-own" : "turn-end-foreign") || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program is not { } program || program.GameplayHash != c.GameplayHash)
            return false;
        return OutstandingQinLangGrants(parent.TurnNumber).Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WindowContext == context && f.QinLangXianying is { } r && r.Grants.Count > 0 && r.Source == QinLangSource(f) && r.GameplayHash == c.GameplayHash && r.Source.SkillInstanceId == c.SkillInstanceId);
    }

    internal SkillProgramStepOutcome BeginQinLang(ProgramSkillFrame input, bool ending)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.QinLangXianying is not null || f.SkillId != "ol:xianying" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Xianying requires its actual preparation/damage or issued actual-turn-end instruction.");
        if (ending)
        {
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
            if (!IsIssuedQinLangXianyingCandidate(c, context))
                throw new InvalidOperationException("Xianying lost its exact frozen ending candidate.");
        }
        else if (context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterDamageApplied) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Unpaid Xianying requires its living original source and actual trigger.");
        var r = new QinLangXianyingReceipt(QinLangSource(f), f.GameplayHash, 1, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, ending ? QinLangXianyingStage.EndChoice : QinLangXianyingStage.DrawChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Grants = ending ? OutstandingQinLangGrants(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber).Where(e => SameQinLangSource(e.Source, QinLangSource(f)) && e.GameplayHash == f.GameplayHash).ToArray() : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.RoundNumber, ending));
        if (!ending)
        {
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new(QinLangDrawReason)).Count;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = f.QinLangXianying!with { Paid = true, ActualDrawCount = count, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingDrawnEvent(f.Id, f.OwnerSeat, count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        ContinueQinLang(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static CardKind QinLangName(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qin_lang_ol_QinLang.QinLangName(kind);
    internal static bool QinLangImitable(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qin_lang_ol_QinLang.QinLangImitable(kind);
    internal IReadOnlyList<PromptChoice> QinLangChoices(ProgramSkillFrame f)
    {
        var r = f.QinLangXianying!;
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "qin-lang-xianying",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        if (r.Stage == QinLangXianyingStage.EndChoice)
        {
            var choices = new List<PromptChoice>();
            foreach (var grant in r.Grants.Where(g => !QinLangGrantConsumed(g.FrameId)))
            {
                var options = QinLangUseOptions(f, grant);
                for (var i = 0; i < options.Count; i++)
                {
                    var option = options[i];
                    var args = Args("use");
                    args["grant-id"] = grant.FrameId.ToString(CultureInfo.InvariantCulture);
                    args["option-index"] = i.ToString(CultureInfo.InvariantCulture);
                    choices.Add(new(new($"xianying.{(f.Id)}.use.{(grant.FrameId)}.{(i)}"), $"贤膺：{(option.Description)}", [], option.TargetSeats, args));
                }

                var skip = Args("skip");
                skip["grant-id"] = grant.FrameId.ToString(CultureInfo.InvariantCulture);
                choices.Add(new(new($"xianying.{(f.Id)}.skip.{(grant.FrameId)}"), $"放弃本次【{(CardCatalog.Get(grant.Name).DisplayName)}】", [], [], skip));
            }

            choices.Add(new(new($"xianying.{(f.Id)}.finish"), "结束贤膺结算，放弃剩余用牌", [], [], Args("finish")));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage != QinLangXianyingStage.DiscardChoice)
            return[];
        var legal = QinLangDiscardable(f);
        var result = new List<PromptChoice>();
        foreach (var c in r.Cards.Where(legal.Contains))
        {
            var selected = r.Selected.Contains(c);
            result.Add(new(new($"xianying.{(f.Id)}.card.{(c.CardId)}"), (selected ? "取消" : "选择") + $"【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).DisplayName)}】", [c.CardId], [], Args("card")));
        }

        if (!QinLangCountUsed(f.OwnerSeat, r.RoundNumber, r.Selected.Count))
            result.Add(new(new($"xianying.{(f.Id)}.pay.{(r.Selected.Count)}"), $"确认弃置{(r.Selected.Count)}张", [], [], Args("pay")));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishQinLang(ProgramSkillFrame f)
    {
        var choices = QinLangChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Xianying cannot expose an empty frozen choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.QinLangXianying!.Stage == QinLangXianyingStage.DiscardChoice ? "贤膺：弃置任意张手牌或装备牌，本轮已使用过的张数不能重复。" : "贤膺：选择独立承诺及其当前合法用法。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("qin-lang-xianying")]
    internal void AnswerQinLang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QinLangXianying: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !QinLangChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xianying changed its private chooser, frozen payment or actual legal use.");
        AssertQinLang(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "card")
        {
            var card = r.Cards.Single(c => c.CardId == choice.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r with { Selected = r.Selected.Contains(card) ? r.Selected.Where(c => c != card).ToArray() : r.Selected.Append(card).ToArray() } });
            PublishQinLang(f);
            return;
        }

        if (branch == "pay")
        {
            PayQinLang(f);
            ContinueQinLang(f.Id);
            return;
        }

        if (branch == "finish")
        {
            foreach (var g in r.Grants.Where(g => !QinLangGrantConsumed(g.FrameId)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingGrantConsumedEvent(g.FrameId, f.Id, f.OwnerSeat, r.ActualTurn, false));
            FinishQinLang(f);
            return;
        }

        var grant = r.Grants.Single(g => g.FrameId == long.Parse(choice.Parameters["grant-id"], CultureInfo.InvariantCulture) && !QinLangGrantConsumed(g.FrameId));
        if (branch == "skip")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingGrantConsumedEvent(grant.FrameId, f.Id, f.OwnerSeat, r.ActualTurn, false));
            ContinueQinLang(f.Id);
            return;
        }

        var option = QinLangUseOptions(f, grant)[int.Parse(choice.Parameters["option-index"], CultureInfo.InvariantCulture)];
        var origin = new QinLangXianyingUseOrigin(f.Id, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, grant.FrameId, r.Source, r.GameplayHash, r.ActualTurn, grant.Name, f.OwnerSeat, option.Kind, option.TargetCardId, option.RequiredKind, string.Join(',', option.TargetSeats));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r with { Stage = QinLangXianyingStage.NativeUse, Use = origin, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingGrantConsumedEvent(grant.FrameId, f.Id, f.OwnerSeat, r.ActualTurn, true));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingUseIssuedEvent(origin));
        BeginQinLangXianyingUse(f, origin, option.TargetSeats);
    }

    internal void PayQinLang(ProgramSkillFrame f)
    {
        var r = f.QinLangXianying!;
        var selected = r.Selected;
        if (r.Stage != QinLangXianyingStage.DiscardChoice || QinLangCountUsed(f.OwnerSeat, r.RoundNumber, selected.Count) || selected.Any(c => !QinLangDiscardable(f).Contains(c)))
            throw new InvalidOperationException("Xianying cannot reuse a round count or substitute a paid HE entity.");
        var grain = selected.Where(c => c.Kind == CardKind.WoodenOx && c.From.Zone == CardZoneKind.Equipment).SelectMany(c => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(x => new QinLangXianyingCard(x.Id, x.Kind, CardLocation.WoodenOxGrain(f.OwnerSeat)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r with { Stage = QinLangXianyingStage.DiscardChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Auxiliaries = grain } });
        if (selected.Count > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, selected.Select(c => c.CardId).ToArray(), new(QinLangDiscardReason), (_, moves) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { QinLangXianying = current.QinLangXianying!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, PaidBatchId = _activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Peek() } });
                foreach (var m in moves)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingEntityDiscardedEvent(f.Id, m.CardId, m.CardKind, m.From, m.Sequence, selected.Single(c => c.CardId == m.CardId).EffectiveName!.Value));
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = f.QinLangXianying!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingDiscardPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.RoundNumber, selected.Count, f.QinLangXianying!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        var names = selected.Select(c => c.EffectiveName!.Value).Distinct().ToArray();
        if (names is [var name] && QinLangImitable(name))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingGrantedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, selected.Count, name));
    }

    internal bool DrainQinLang(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueQinLang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QinLangXianying: { } r } f || f.Id != id)
            return;
        AssertQinLang(f);
        if (r.Stage is QinLangXianyingStage.DrawChildren or QinLangXianyingStage.DiscardChildren or QinLangXianyingStage.NativeChildren)
        {
            if (DrainQinLang(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.QinLangXianying!;
            if (r.Stage == QinLangXianyingStage.DiscardChildren)
            {
                FinishQinLang(f);
                return;
            }

            if (r.Stage == QinLangXianyingStage.DrawChildren)
            {
                var cards = QinLangDiscardable(f);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r with { Stage = QinLangXianyingStage.DiscardChoice, Cards = cards } });
                r = f.QinLangXianying!;
                if (Enumerable.Range(0, cards.Count + 1).All(n => QinLangCountUsed(f.OwnerSeat, r.RoundNumber, n)))
                {
                    FinishQinLang(f);
                    return;
                }
            }
            else
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = r with { Stage = QinLangXianyingStage.EndChoice, Use = null } });
                r = f.QinLangXianying!;
            }
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishQinLang(f);
            return;
        }

        if (r.Stage == QinLangXianyingStage.EndChoice && !r.Grants.Any(g => !QinLangGrantConsumed(g.FrameId)))
        {
            FinishQinLang(f);
            return;
        }

        if (r.Stage is QinLangXianyingStage.DiscardChoice or QinLangXianyingStage.EndChoice)
            PublishQinLang(f);
    }

    internal void FinishQinLang(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27170)]
    internal bool ResumeQinLang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QinLangXianying: not null } f || f.Id != id)
            return false;
        ContinueQinLang(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27170)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27170)]
    internal bool ContinuePaidQinLang(ProgramSkillFrame f) => f.QinLangXianying is { Paid: true } && ValidQinLang(f) || f.QinLangXianying is { Grants.Count: > 0 } && ValidQinLang(f) || f.QinLangXianying is null && f.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && IsIssuedQinLangXianyingCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
}
