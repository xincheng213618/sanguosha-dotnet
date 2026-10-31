#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengHuoMouMobileRuntime
{
    internal const string MouMhDiscardReason = "skill-program.mou-mobile:zaiqi.discard", MouMhDrawReason = "skill-program.mou-mobile:zaiqi.draw";
    internal PromptChoice MouMhChoice(ProgramSkillFrame f, string branch, string label, int? seat = null, int? card = null) => new(new($"mou-meng-huo.{(f.Id)}.{(branch)}.{(seat)}.{(card)}"), label, card is { } id ? [id] : [], seat is { } target ? [target] : [], new Dictionary<string, string> { ["program-action"] = "mou-meng-huo-zaiqi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MouMhChoices(ProgramSkillFrame f)
    {
        var r = f.MouMengHuo!;
        var choices = new List<PromptChoice>();
        if (r.Stage == MouMengHuoStage.Selecting)
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (r.Targets.Contains(p.Seat) || r.Targets.Count < Math.Min(r.Budget, MouMhCharge(f.OwnerSeat)))))
                choices.Add(MouMhChoice(f, "toggle", (r.Targets.Contains(target.Seat) ? "取消" : "选择") + target.Name, target.Seat));
            if (r.Targets.Count > 0)
                choices.Add(MouMhChoice(f, "confirm", $"消耗{(r.Targets.Count)}点蓄力，依次结算"));
            choices.Add(MouMhChoice(f, "skip", "不发动再起"));
        }
        else if (r.Stage == MouMengHuoStage.RecipientChoice && r.Cursor < r.Targets.Count)
        {
            var seat = r.Targets[r.Cursor];
            choices.Add(MouMhChoice(f, "draw", $"令{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}摸一张牌"));
            foreach (var zone in new[]
            {
                CardZoneKind.Hand,
                CardZoneKind.Equipment
            }

            )
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(new(zone, seat)).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, new(zone, seat), OwnedCardMoveIntent.Discard)))
                    choices.Add(MouMhChoice(f, "discard", $"弃置【{(CardCatalog.Get(card.Kind).DisplayName)}】，令{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)}回复1点体力", card: card.Id));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouMh(ProgramSkillFrame f)
    {
        var r = f.MouMengHuo!;
        var chooser = r.Stage == MouMengHuoStage.Selecting ? f.OwnerSeat : r.Targets[r.Cursor];
        var choices = MouMhChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Zaiqi cannot publish an empty recipient choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouMengHuoStage.Selecting ? $"再起：拥有{(MouMhCharge(f.OwnerSeat))}点蓄力，选择角色。" : "再起：选择令其摸牌，或实际弃置自己的一张手牌/装备令其回复体力。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-meng-huo-zaiqi")]
    internal void AnswerMouMh(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouMengHuo;
        if (r is null || !ValidMouMh(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouMengHuoStage.Selecting ? f.OwnerSeat : r.Targets[r.Cursor]) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouMhChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Zaiqi accepts only the actual chooser's published private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        if (r.Stage == MouMengHuoStage.Selecting)
        {
            if (!_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                FinishMouMh(f);
                return;
            }

            if (branch == "skip")
            {
                FinishMouMh(f);
                return;
            }

            if (branch == "toggle")
            {
                var target = selected.Targets.Single();
                var targets = r.Targets.Contains(target) ? r.Targets.Where(t => t != target).ToArray() : r.Targets.Append(target).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Targets = targets } });
                PublishMouMh(f);
                return;
            }

            if (branch != "confirm" || r.Targets.Count == 0 || r.Targets.Count > MouMhCharge(f.OwnerSeat) || r.Targets.Any(s => !_coreMatchParticipantsPlayersAccess._players[s].IsAlive))
                throw new InvalidOperationException("Zaiqi cannot debit unpayable recipient count.");
            var before = MouMhCharge(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Accepted = true, Stage = MouMengHuoStage.RecipientChoice } });
            ChangeMouMhCharge(f, -r.Targets.Count);
            for (var i = 0; i < r.Targets.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoTargetSelectedEvent(f.Id, i, r.Targets[i]));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoChargeSpentEvent(f.Id, f.OwnerSeat, r.Targets.Count, before, before - r.Targets.Count));
            ContinueMouMh(f.Id);
            return;
        }

        var recipient = r.Targets[r.Cursor];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoRecipientDecidedEvent(f.Id, r.Cursor, recipient, branch));
        if (branch == "draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Stage = MouMengHuoStage.Drawing, Draw = null, Payment = null }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(MouMhDrawReason));
            return;
        }

        if (branch != "discard" || selected.Cards is not [var id])
            throw new InvalidOperationException("Zaiqi requires one actual recipient-owned HE payment.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var beforeMove = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (from.OwnerSeat != recipient || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(recipient, card, from, OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Zaiqi cannot discard another chooser's entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Stage = MouMengHuoStage.DiscardChildren, Before = beforeMove, After = beforeMove, Draw = null, Payment = null }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var invocation = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(recipient, recipient, [id], new(MouMhDiscardReason), f.Id);
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.DiscardPile, new(MouMhDiscardReason));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(invocation);
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > beforeMove && m.CardId == id && m.From == from && m.Reason.Value == MouMhDiscardReason);
        var payment = new MouMengHuoPayment(id, card.Kind, from, move.To, move.Sequence, invocation?.InvocationId ?? 0);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = f.MouMengHuo!with { Payment = payment } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoDiscardPaidEvent(f.Id, r.Cursor, recipient, payment));
        SealMouMh(f.Id, beforeMove);
        ContinueMouMh(f.Id);
    }

    internal void ContinueMouMh(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouMengHuo: { } r } f || f.Id != id)
            return;
        if (!ValidMouMh(f))
            throw new InvalidOperationException("Mou Meng Huo lost its owned phase and paid recipient receipt.");
        if (r.Stage is MouMengHuoStage.Drawing or MouMengHuoStage.Selecting)
            return;
        if (DrainMouMh(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouMengHuo!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Obtain)
        {
            FinishMouMh(f);
            return;
        }

        if (r.Stage == MouMengHuoStage.DiscardChildren)
        {
            if (r.Payment is not { } p || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.InvocationId == p.InvocationId && e.ParentFrameId == f.Id && e.ActualCount == 1))
                throw new InvalidOperationException("Zaiqi's recovery needs its actual logical recipient discard.");
            var recipient = r.Targets[r.Cursor];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Stage = MouMengHuoStage.RecoveryChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoRecoveryIssuedEvent(f.Id, r.Cursor, recipient, f.OwnerSeat, 1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, recipient, f.OwnerSeat, 1, null, null);
            SealMouMh(f.Id, before);
            ContinueMouMh(id);
            return;
        }

        if (r.Stage is MouMengHuoStage.RecoveryChildren or MouMengHuoStage.DrawChildren)
        {
            r = r with
            {
                Cursor = r.Cursor + 1,
                Stage = MouMengHuoStage.RecipientChoice,
                Payment = null,
                Draw = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r, PendingMovementContinuation = null });
        }

        while (r.Cursor < r.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive)
        {
            r = r with
            {
                Cursor = r.Cursor + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r });
        }

        if (r.Cursor == r.Targets.Count)
        {
            FinishMouMh(f);
            return;
        }

        PublishMouMh(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouMengHuoZaiqi)]
    internal void ReturnMouMhDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidMouMh(f) || f.MouMengHuo is not { Stage: MouMengHuoStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Zaiqi cannot return a different or repeated native Draw1.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = r with { Stage = MouMengHuoStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoDrawReturnedEvent(f.Id, r.Cursor, 1, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        SealMouMh(f.Id, outcome.SequenceBefore);
        ContinueMouMh(f.Id);
    }

    internal void FinishMouMh(ProgramSkillFrame f)
    {
        var r = f.MouMengHuo!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouMengHuoCompletedEvent(f.Id, r.Obtain, r.Accepted, r.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouMengHuo = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouMengHuoZaiqi)]
    internal PromptChoice AiMouMh(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouMengHuo!;
        if (r.Stage == MouMengHuoStage.Selecting)
        {
            var next = p.Choices.Where(c => c.Parameters["branch"] == "toggle" && !r.Targets.Contains(c.Targets.Single()) && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).OrderBy(c => c.Targets.Single()).FirstOrDefault();
            return next ?? p.Choices.Single(c => c.Parameters["branch"] == (r.Targets.Count > 0 ? "confirm" : "skip"));
        }

        var cheapest = p.Choices.Where(c => c.Parameters["branch"] == "discard").OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).FirstOrDefault();
        return cheapest is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp < _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat], _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) && CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(cheapest.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]) <= 4 ? cheapest : p.Choices.Single(c => c.Parameters["branch"] == "draw");
    }
}
