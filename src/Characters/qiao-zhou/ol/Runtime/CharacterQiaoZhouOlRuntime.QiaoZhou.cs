#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoZhouOlRuntime
{
    internal const string QiaoZhouTableBind = "xingbu-public-three";
    internal const string QiaoZhouRevealReason = "skill-program.ol:xingbu.forecast.reveal";
    internal const string QiaoZhouCleanupReason = "skill-program.ol:xingbu.forecast.discard";
    internal const string QiaoZhouPreparationReason = "skill-program.ol:xingbu.preparation-discard";
    internal bool HasQiaoZhouCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXingbuThreeCardForecast);

    internal CardConversionSource QiaoZhouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IQiaoZhouProgramHost.Forecast(ProgramSkillFrame frame) => BeginQiaoZhouForecast(frame);
    internal QiaoZhouForecastGrantedEvent[] QiaoZhouMarks()
    {
        if (!HasQiaoZhouCapability)
            return[];
        var expired = _coreProgramEventHistoryOperations.ProgramEventHistory<QiaoZhouForecastExpiredEvent>().Select(e => e.GrantId).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<QiaoZhouForecastGrantedEvent>().Where(e => !expired.Contains(e.GrantId)).ToArray();
    }

    internal QiaoZhouForecastActivatedEvent? QiaoZhouActivation(long grantId) => _coreProgramEventHistoryOperations.ProgramEventHistory<QiaoZhouForecastActivatedEvent>().SingleOrDefault(e => e.GrantId == grantId);
    internal bool QiaoZhouRecipientAvailable(int owner, int recipient) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) && recipient != owner && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && !QiaoZhouMarks().Any(e => e.RecipientSeat == recipient);
    internal SkillProgramStepOutcome BeginQiaoZhouForecast(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.QiaoZhouForecast is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } ending || ending.OwnerSeat != f.OwnerSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("Xingbu requires its owner's actual ending boundary.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QiaoZhouForecast = new(QiaoZhouSource(f), f.GameplayHash, f.InstructionIndex, ending.Id, QiaoZhouForecastStage.RevealChildren, 0, before, before) });
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, 3, null, QiaoZhouTableBind, SkillProgramCardSetVisibility.Public);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, QiaoZhouTableBind).CardIds.Select(id => new QiaoZhouRevealedCard(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id).Sequence)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { QiaoZhouForecast = f.QiaoZhouForecast!with { Cards = cards, RedCount = cards.Count(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(c.Suit)), SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueQiaoZhouForecast(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainQiaoZhouChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueQiaoZhouForecast(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { QiaoZhouForecast: { } r } f && f.Id == id)
        {
            if (DrainQiaoZhouChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.QiaoZhouForecast!;
            if (r.Stage == QiaoZhouForecastStage.CleanupChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { QiaoZhouForecast = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CleanupQiaoZhouForecast(f);
                return;
            }

            if (r.Stage == QiaoZhouForecastStage.RevealChildren)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QiaoZhouForecast = r with { Stage = QiaoZhouForecastStage.Recipient } });
            PublishQiaoZhouForecast(f);
            return;
        }
    }

    internal IReadOnlyList<PromptChoice> QiaoZhouForecastChoices(ProgramSkillFrame f)
    {
        var r = f.QiaoZhouForecast!;
        var result = _coreMatchParticipantsPlayersAccess._players.Where(p => QiaoZhouRecipientAvailable(f.OwnerSeat, p.Seat)).Select(p => new PromptChoice(new($"qiao-zhou.{(f.Id)}.recipient.{(p.Seat)}"), $"令{(p.Name)}于下个行动回合获得星卜{(Math.Max(1, r.RedCount))}效果", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "qiao-zhou-forecast", ["option"] = "recipient" })).ToList();
        result.Add(new(new($"qiao-zhou.{(f.Id)}.finish"), "不赋予星卜效果", [], [], new Dictionary<string, string> { ["program-action"] = "qiao-zhou-forecast", ["option"] = "finish" }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishQiaoZhouForecast(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = QiaoZhouForecastChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"星卜：已亮出{(f.QiaoZhouForecast!.RedCount)}张红色牌，可选择另一名角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("qiao-zhou-forecast")]
    internal void AnswerQiaoZhouForecast(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QiaoZhouForecast.Stage: QiaoZhouForecastStage.Recipient } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], QiaoZhouForecastChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xingbu lost its exact living, unmarked recipient choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["option"] == "recipient")
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiaoZhouForecastGrantedEvent(f.Id, QiaoZhouSource(f), f.GameplayHash, choice.Targets.Single(), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, f.QiaoZhouForecast!.RedCount));
        CleanupQiaoZhouForecast(f);
    }

    internal void CleanupQiaoZhouForecast(ProgramSkillFrame f)
    {
        if (f.QiaoZhouForecast!.Stage != QiaoZhouForecastStage.CleanupChildren)
        {
            var r = f.QiaoZhouForecast;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var ids = r.Cards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == c.CardId).Sequence == c.MovementSequence).Select(c => c.CardId).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QiaoZhouForecast = r with { Stage = QiaoZhouForecastStage.CleanupChildren, SequenceBefore = before, SequenceAfter = before } });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, new(QiaoZhouCleanupReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { QiaoZhouForecast = f.QiaoZhouForecast!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        }

        ContinueQiaoZhouForecast(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26220)]
    internal bool ResumeQiaoZhouForecast(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QiaoZhouForecast: not null } f || f.Id != id)
            return false;
        ContinueQiaoZhouForecast(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26220)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26220)]
    internal bool ContinueIssuedQiaoZhouForecast(ProgramSkillFrame f) => f.QiaoZhouForecast is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveXingbuThreeCardForecast)]
    internal PromptChoice AiQiaoZhouForecast(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["option"] == "recipient") ?? p.Choices.Single(c => c.Parameters["option"] == "finish");
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26220)]
    internal void ExpireQiaoZhouForecasts(IGameEvent payload)
    {
        if (!HasQiaoZhouCapability || payload is not (PlayerDiedEvent or TurnEndedEvent))
            return;
        foreach (var grant in QiaoZhouMarks())
        {
            var expired = payload is PlayerDiedEvent died && (died.VictimSeat == grant.Source.OwnerSeat || died.VictimSeat == grant.RecipientSeat) || payload is TurnEndedEvent ended && QiaoZhouActivation(grant.GrantId)is { } active && active.ActualTurnNumber == ended.TurnNumber && active.RecipientSeat == ended.ActorSeat;
            if (!expired)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiaoZhouForecastExpiredEvent(grant.GrantId, grant.RecipientSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, payload is PlayerDiedEvent ? "participant-death" : "recipient-turn-ended"));
            var ids = _turnCardUseEffectsOperations._turnCardUseEffects.RevokeQiaoZhouForecastModifiers(grant.GrantId);
            if (ids.Count > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnCardUseEffectsExpiredEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, ids));
        }
    }

    internal bool TryBeginQiaoZhouPreparation(CharacterState current)
    {
        if (!HasQiaoZhouCapability)
            return false;
        var grants = QiaoZhouMarks().Where(g => g.RecipientSeat == current.Seat && QiaoZhouActivation(g.GrantId)is null).ToArray();
        if (grants.Length == 0)
            return false;
        if (grants.Length != 1 || current.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || current.IsFaceDown || !current.IsAlive || _resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || grants[0].IssuedTurn >= _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("Xingbu lost its one pending mark at the recipient's actual preparation.");
        var grant = grants.Single();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiaoZhouForecastActivatedEvent(grant.GrantId, current.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        if (grant.RedCount >= 2)
        {
            var source = new CardUseEffectSource(grant.Source.SkillId, grant.Source.BindingId, grant.Source.OwnerSeat, grant.Source.SkillInstanceId);
            void Add(SkillRuleQuery query, int amount, int index)
            {
                var modifier = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, grant.GrantId, index, source, query, SkillRuleOperation.Add, amount, affectedSeat: current.Seat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(modifier));
            }

            Add(SkillRuleQuery.SlashLimit, grant.RedCount == 3 ? 1 : -1, 0);
            if (grant.RedCount == 3)
                Add(SkillRuleQuery.DrawCount, 2, 1);
            return false;
        }

        var frame = new QiaoZhouPreparationFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, current.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, grant);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        ContinueQiaoZhouPreparation(frame.Id);
        return true;
    }

    internal bool TrySkipQiaoZhouDiscardPhase(CharacterState current)
    {
        if (!HasQiaoZhouCapability)
            return false;
        var mark = QiaoZhouMarks().SingleOrDefault(g => g.RecipientSeat == current.Seat && g.RedCount == 2 && QiaoZhouActivation(g.GrantId)is { } a && a.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        if (mark is null)
            return false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QiaoZhouDiscardPhaseSkippedEvent(mark.GrantId, current.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        return true;
    }
}
