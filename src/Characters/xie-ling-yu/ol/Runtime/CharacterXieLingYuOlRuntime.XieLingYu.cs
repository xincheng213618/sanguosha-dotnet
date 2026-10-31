#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal const string YuandiDrawReason = "skill-program.xie-ling-yu.yuandi.draw", XinyouDrawReason = "skill-program.xie-ling-yu.xinyou.draw", XinyouDiscardReason = "skill-program.xie-ling-yu.xinyou.discard";
    SkillProgramStepOutcome global::CardGame.Core.IXieLingYuHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginXieLingYu(frame, op);
    internal SkillProgramStepOutcome BeginXieLingYu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XieLingYu is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !XieLingYuComposition.Handles(op))
            throw new InvalidOperationException("Xie Ling Yu requires her unissued exact owning instruction.");
        XieLingYuComposition.ValidateProgram(f.SkillId, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var r = new XieLingYuReceipt(XieLingYuSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, XieLingYuStage.Cost);
        if (op == SkillProgramEffectOp.ResolveYuandiDiscardNames)
        {
            if (f.WindowContext is not { } c)
                throw new InvalidOperationException("Yuandi lost its actual discard parent.");
            var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
            if (!IsIssuedYuandiCandidate(candidate, c) || CurrentYuandiOpportunity(candidate, c)is not { } opportunity)
                throw new InvalidOperationException("Yuandi lost its exact first full printed-name group.");
            r = r with
            {
                Paid = true,
                Stage = XieLingYuStage.Drawing,
                NamesMask = opportunity.NamesMask,
                NonUse = opportunity.NonUse,
                OpportunitySequence = opportunity.CompletionSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuandiPaidEvent(f.Id, f.OwnerSeat, r.WindowId, r.ActualTurn, r.TurnOwner, r.OpportunitySequence, r.NamesMask, r.NonUse));
        }
        else
        {
            if (f.WindowContext is not null || f.TriggerId is not null || f.ActivationId != "basic-cycle" || XinyouActualPlay(f.OwnerSeat)is not { } phase || XinyouPhaseSpent(f.OwnerSeat, phase))
                throw new InvalidOperationException("Xinyou requires an unspent actual Play draft.");
            r = r with
            {
                PlayPhase = phase
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r });
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieLingYuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.TurnOwner, r.PlayPhase?.Token ?? 0));
        ContinueXieLingYu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> XieLingYuChoices(ProgramSkillFrame f)
    {
        var r = f.XieLingYu!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, CardKind? name = null)
        {
            var data = new Dictionary<string, string>
            {
                ["program-action"] = "xie-ling-yu",
                ["branch"] = branch
            };
            if (name is { } k)
                data["name"] = k.ToString();
            return new(new($"xie-ling-yu.{(f.Id)}.{(branch)}.{((name?.ToString() ?? (cards is [var id] ? id.ToString(CultureInfo.InvariantCulture) : "0")))}"), label, cards ?? [], [], data);
        }

        if (r.Stage == XieLingYuStage.RemoveName)
        {
            foreach (var name in XieLingYuNames(YuandiRemainingMask(f.OwnerSeat)))
                choices.Add(Choice("remove-name", $"移除【{(CardCatalog.Get(name).DisplayName)}】牌名", name: name));
        }
        else if (r.Stage == XieLingYuStage.Cost)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                foreach (var card in XinyouCostCards(f.OwnerSeat).Where(c => !r.Selected.Contains(c.Id)))
                    choices.Add(Choice("cost", $"选择弃置【{(card.DisplayName)}】", [card.Id]));
                if (r.Selected.Count > 0)
                    choices.Add(Choice("pay", $"弃置选中的{(r.Selected.Count)}张基本牌，开始心幽"));
            }

            choices.Add(Choice("cancel", "取消心幽（未弃牌，不消耗本阶段次数）"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishXieLingYu(ProgramSkillFrame f)
    {
        var choices = XieLingYuChoices(f);
        if (choices.Count == 0)
        {
            FinishXieLingYu(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.XieLingYu!.Stage == XieLingYuStage.Cost ? "心幽：选择至少一张手牌中的基本牌，然后确认整批弃置。" : "元嫡：移除一个剩余牌名。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("xie-ling-yu")]
    internal void AnswerXieLingYu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XieLingYu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !XieLingYuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xie Ling Yu requires her exact current private cost or remaining-name choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "cancel")
        {
            FinishXieLingYu(f);
            return;
        }

        if (r.Stage == XieLingYuStage.RemoveName)
        {
            var name = Enum.Parse<CardKind>(choice.Parameters["name"]);
            var before = YuandiRemainingMask(f.OwnerSeat);
            var after = before & ~XieLingYuNameBit(name);
            if (after == 0 || after == before || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                throw new InvalidOperationException("Yuandi must remove one existing name while retaining at least one.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuandiNameRemovedEvent(f.Id, f.OwnerSeat, name, before, after));
            _coreMatchLogOperations.AddLog("Skill", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}发动“元嫡”，移除【{(CardCatalog.Get(name).DisplayName)}】牌名。", f.OwnerSeat);
            FinishXieLingYu(f);
            return;
        }

        if (r.Stage != XieLingYuStage.Cost || r.Paid)
            throw new InvalidOperationException("Xinyou cannot repeat its paid initial discard.");
        if (branch == "cost")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r with { Selected = r.Selected.Append(choice.Cards.Single()).ToArray() } });
            PublishXieLingYu(f);
            return;
        }

        if (branch != "pay" || r.Selected.Count == 0 || r.PlayPhase is not { } phase || XinyouActualPlay(f.OwnerSeat) != phase || XinyouPhaseSpent(f.OwnerSeat, phase) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Xinyou cannot pay a canceled, stale or already spent draft.");
        var current = XinyouCostCards(f.OwnerSeat);
        var materials = r.Selected.Select(id => current.Single(c => c.Id == id)).Select(c => new XieLingYuMaterial(c.Id, c.Kind, global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)), CardLocation.Hand(f.OwnerSeat))).ToArray();
        var mask = materials.Aggregate(0, (m, c) => m | XieLingYuNameBit(c.BasicName));
        var beforeSequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r with { Paid = true, Stage = XieLingYuStage.CostChildren, Materials = materials, NamesMask = mask, Before = beforeSequence, After = beforeSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < materials.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XinyouMaterialFrozenEvent(f.Id, i, materials[i].CardId, materials[i].PrintedKind, materials[i].BasicName));
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, materials.Select(c => c.CardId).ToArray(), new(XinyouDiscardReason), (_, movements) =>
        {
            var paidFrame = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(paidFrame with { XieLingYu = paidFrame.XieLingYu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            for (var i = 0; i < materials.Length; i++)
            {
                var m = movements.Single(m => m.CardId == materials[i].CardId && m.From == materials[i].From && (m.To == CardLocation.DiscardPile || _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(m, materials[i].From, new(XinyouDiscardReason))) && m.Reason.Value == XinyouDiscardReason);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XinyouMaterialDiscardedEvent(f.Id, i, m.CardId, m.Sequence));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XinyouPaidEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.TurnOwner, phase.Token, mask, materials.Length, beforeSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        ContinueXieLingYu(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveYuandiDiscardNames, SkillProgramEffectOp.ResolveXinyouBasicCycle)]
    internal void ReturnXieLingYuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.XieLingYu is not { Paid: true, Stage: XieLingYuStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || r.Requests is < 1 or > 5 || _coreProgramEventHistoryOperations.ProgramEventHistory<XieLingYuDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Request == r.Requests))
            throw new InvalidOperationException("Xie Ling Yu's draw must return to its unique real request cursor.");
        var ids = outcome.ActualDrawIds.Select(id => (Id: id, Replacement: false)).Concat(outcome.ReplacementGainedIds.Select(id => (Id: id, Replacement: true))).ToArray();
        if (ids.Select(c => c.Id).Distinct().Count() != ids.Length || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0)
            throw new InvalidOperationException("A replacement cannot invent ordinary DrawPile materials.");
        var gains = ids.Select(x =>
        {
            var before = x.Replacement ? outcome.Replacement!.SequenceBefore : outcome.SequenceBefore;
            var after = x.Replacement ? outcome.Replacement!.SequenceAfter : outcome.SequenceAfter;
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == x.Id && m.Sequence > before && m.Sequence <= after && m.To == CardLocation.Hand(f.OwnerSeat) && m.From != m.To && (x.Replacement || m.From == CardLocation.DrawPile && m.Reason.Value == (r.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames ? YuandiDrawReason : XinyouDrawReason))) ?? throw new InvalidOperationException("A draw outcome entity lacks its actual original gain movement.");
            return new XieLingYuGain(x.Id, m.CardKind, m.Sequence, x.Replacement);
        }).ToArray();
        var stop = gains.Length == 0 || r.Requests >= 5 || gains.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.PrintedKind) == SkillProgramCardCategory.Basic && (XieLingYuNameBit(c.PrintedKind) & r.NamesMask) == 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r with { Stage = XieLingYuStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Gains = gains, Outcome = outcome, Stop = stop }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Op == SkillProgramEffectOp.ResolveXinyouBasicCycle)
            for (var i = 0; i < gains.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XinyouGainedEntityEvent(f.Id, r.Requests, i, f.OwnerSeat, r.ActualTurn, r.TurnOwner, gains[i].CardId, gains[i].PrintedKind, gains[i].MovementSequence, gains[i].Replacement));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieLingYuDrawReturnedEvent(f.Id, r.Requests, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, stop));
        ContinueXieLingYu(f.Id);
    }

    internal void ContinueXieLingYu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { XieLingYu: { } r } f && f.Id == id)
        {
            if (r.Stage is XieLingYuStage.Cost or XieLingYuStage.RemoveName)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !r.Paid && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
                {
                    FinishXieLingYu(f);
                    return;
                }

                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishXieLingYu(f);
                return;
            }

            if (r.Stage is XieLingYuStage.CostChildren or XieLingYuStage.DrawChildren)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.XieLingYu!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishXieLingYu(f);
                    return;
                }

                if (r.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames)
                {
                    if (r.NonUse && XieLingYuNames(YuandiRemainingMask(f.OwnerSeat)).Length > 1)
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r with { Stage = XieLingYuStage.RemoveName } });
                        PublishXieLingYu(f);
                        return;
                    }

                    FinishXieLingYu(f);
                    return;
                }

                if (r.Stage == XieLingYuStage.DrawChildren && r.Stop)
                {
                    FinishXieLingYu(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r = r with { Stage = XieLingYuStage.Drawing, Outcome = null, Gains = [] } });
            }

            if (r.Stage != XieLingYuStage.Drawing || f.NativeDraw is not null)
                return;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishXieLingYu(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = r with { Requests = r.Requests + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(r.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames ? YuandiDrawReason : XinyouDrawReason));
            return;
        }
    }

    internal void FinishXieLingYu(ProgramSkillFrame f)
    {
        var r = f.XieLingYu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieLingYuCompletedEvent(f.Id, r.Paid, r.Requests));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XieLingYu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27581)]
    internal bool ResumeXieLingYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XieLingYu: not null } f || f.Id != id)
            return false;
        ContinueXieLingYu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27581)]
    internal bool ReturnXieLingYuMovement(ProgramSkillFrame f)
    {
        if (f.XieLingYu is null)
            return false;
        ContinueXieLingYu(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveYuandiDiscardNames, SkillProgramEffectOp.ResolveXinyouBasicCycle)]
    internal PromptChoice ChooseXieLingYuAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.XieLingYu!.Stage == XieLingYuStage.RemoveName)
            return prompt.Choices.First();
        return prompt.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? prompt.Choices.Where(c => c.Parameters["branch"] == "cost").OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? prompt.Choices.Single(c => c.Parameters["branch"] == "cancel");
    }
}
