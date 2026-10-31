#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiMouMobileRuntime
{
    internal const string MouZhenJiLuoshenId = "mou-mobile:luoshen", MouZhenJiObtainReason = "skill-program.mou-mobile:luoshen.obtain", MouZhenJiDiscardReason = "skill-program.mou-mobile:luoshen.discard";
    internal bool HasMouZhenJiLuoshenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouZhenJiLuoshen);

    internal SkillProgramStepOutcome StartMouZhenJiLuoshen(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouZhenJiLuoshen is not null || f.SkillId != MouZhenJiLuoshenId || f.TriggerId != "reveal-seated-hands" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } c || c.OwnerSeat != f.OwnerSeat || c.SourceSeat != f.OwnerSeat)
            throw new InvalidOperationException("Mou Luoshen requires its actual owner's native preparation trigger.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = new(new(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, c.ParentFrameId, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) });
        PublishMouZhenJiLuoshen(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool ValidMouZhenJiLuoshen(ProgramSkillFrame f)
    {
        if (f.MouZhenJiLuoshen is not { } r || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || f.WindowContext?.ParentFrameId != r.WindowId || !Enum.IsDefined(r.Stage) || r.Cursor < 0 || r.Cursor > r.Seats.Count || r.Seats.Distinct().Count() != r.Seats.Count || r.Seats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat) || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        if (r.StartingSeat is null)
            return r.Stage == MouZhenJiLuoshenStage.StartingSeat && r.Seats.Count == 0 && r.Cursor == 0;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhenJiLuoshenStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.TurnNumber == r.TurnNumber && e.TurnOwnerSeat == r.TurnOwnerSeat && e.StartingSeat == r.StartingSeat && e.Count == r.Seats.Count) == 1 && r.Seats.Select((s, i) => new MouZhenJiLuoshenSeatQueuedEvent(f.Id, i, s)).All(e => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhenJiLuoshenSeatQueuedEvent>().Count(v => v == e) == 1);
    }

    internal IReadOnlyList<PromptChoice> MouZhenJiLuoshenChoices(ProgramSkillFrame f)
    {
        var r = f.MouZhenJiLuoshen!;
        if (r.Stage == MouZhenJiLuoshenStage.StartingSeat)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new PromptChoice(new($"mou-zhen-ji.{(f.Id)}.start.{(p.Seat)}"), $"从{(p.Name)}开始依次展示手牌", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mou-zhen-ji-luoshen", ["branch"] = "start", ["seat"] = p.Seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
        var seat = r.Seats[r.Cursor];
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => new PromptChoice(new($"mou-zhen-ji.{(f.Id)}.show.{(c.Id)}"), $"展示【{(CardCatalog.Get(c.Kind).DisplayName)}】", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "mou-zhen-ji-luoshen", ["branch"] = "reveal", ["card-id"] = c.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishMouZhenJiLuoshen(ProgramSkillFrame f)
    {
        var r = f.MouZhenJiLuoshen!;
        var chooser = r.Stage == MouZhenJiLuoshenStage.StartingSeat ? f.OwnerSeat : r.Seats[r.Cursor];
        var choices = MouZhenJiLuoshenChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouZhenJiLuoshenStage.StartingSeat ? "洛神：选择起始角色。" : "洛神：展示一张手牌。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
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

    [ProgramAnswer("mou-zhen-ji-luoshen")]
    internal void AnswerMouZhenJiLuoshen(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouZhenJiLuoshen!;
        var chooser = r.Stage == MouZhenJiLuoshenStage.StartingSeat ? f.OwnerSeat : r.Seats[r.Cursor];
        if (!ValidMouZhenJiLuoshen(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != chooser || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouZhenJiLuoshenChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Luoshen requires the current actual participant's published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouZhenJiLuoshenStage.StartingSeat)
        {
            var start = selected.Targets.Single();
            var count = (_coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) + 1) / 2;
            var seats = Enumerable.Range(0, _coreMatchParticipantsPlayersAccess._players.Count).Select(i => _coreTableOrderOperations.TableStepSeat(start, i)).Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Take(count).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = r with { StartingSeat = start, Seats = seats, Stage = MouZhenJiLuoshenStage.RevealChoice } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhenJiLuoshenStartedEvent(f.Id, r.Source, r.TurnNumber, r.TurnOwnerSeat, start, seats.Length));
            for (var i = 0; i < seats.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhenJiLuoshenSeatQueuedEvent(f.Id, i, seats[i]));
            ContinueMouZhenJiLuoshen(f.Id);
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
        var from = CardLocation.Hand(chooser);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            throw new InvalidOperationException("Luoshen cannot show a departed hand entity.");
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[chooser], card);
        var red = CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = r with { RevealedCardId = card.Id, RevealedSuit = suit, Stage = MouZhenJiLuoshenStage.MovementChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId!, f.OwnerSeat, "luoshen-reveal", [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhenJiLuoshenRevealedEvent(f.Id, r.Cursor, chooser, card.Id, suit));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "luoshen-selected-hand"))
            return;
        ContinueLuoshenAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal bool DrainMouZhenJiLuoshen(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueMouZhenJiLuoshen(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhenJiLuoshen: { } r } f || f.Id != id)
            return;
        if (!ValidMouZhenJiLuoshen(f))
            throw new InvalidOperationException("Luoshen lost its owning cursor or frozen seat route.");
        if (r.Stage == MouZhenJiLuoshenStage.MovementChildren)
        {
            if (DrainMouZhenJiLuoshen(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MouZhenJiLuoshen!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = r = r with { Cursor = r.Cursor + 1, Stage = MouZhenJiLuoshenStage.RevealChoice, RevealedCardId = null, RevealedSuit = null }, PendingMovementContinuation = null });
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishMouZhenJiLuoshen(f);
            return;
        }

        while (r.Cursor < r.Seats.Count && (!_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Cursor]].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Cursor]]).Count == 0))
            r = r with
            {
                Cursor = r.Cursor + 1
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = r });
        if (r.Cursor >= r.Seats.Count)
        {
            FinishMouZhenJiLuoshen(f);
            return;
        }

        PublishMouZhenJiLuoshen(f);
    }

    internal void FinishMouZhenJiLuoshen(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhenJiLuoshenCompletedEvent(f.Id, f.MouZhenJiLuoshen!.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhenJiLuoshen = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouZhenJiLuoshen)]
    internal PromptChoice AiMouZhenJiLuoshen(PendingDecision p, ProgramSkillFrame f) => f.MouZhenJiLuoshen!.Stage == MouZhenJiLuoshenStage.StartingSeat ? p.Choices.OrderBy(c => (c.Targets.Single() - f.OwnerSeat + _coreMatchParticipantsPlayersAccess._players.Count) % _coreMatchParticipantsPlayersAccess._players.Count).First() : p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).ThenBy(c => c.Cards.Single()).First();
    SkillProgramStepOutcome IMouZhenJiLuoshenHost.Start(ProgramSkillFrame f) => StartMouZhenJiLuoshen(f);
    [PublicHandRevealReturn("luoshen-selected-hand")]
    internal void ContinueLuoshenAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.MouZhenJiLuoshen ?? throw new InvalidOperationException("Luoshen lost its exact public Hand selection.");
        var chooser = r.Seats[r.Cursor];
        var from = CardLocation.Hand(chooser);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.RevealedCardId!.Value);
        var red = CoreGameEnginePublicPileColorsRuntime.SuitColor(r.RevealedSuit!.Value);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) == from && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[chooser].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            if (red == false)
                _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(MouZhenJiObtainReason));
            else if (red == true && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, card, from, OwnedCardMoveIntent.Discard))
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, chooser, card, from, new(MouZhenJiDiscardReason));
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence > before && m.CardId == card.Id && m.From == from && m.Reason.Value == (red == false ? MouZhenJiObtainReason : MouZhenJiDiscardReason));
            if (m is not null)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhenJiLuoshenMovedEvent(f.Id, r.Cursor, card.Id, from, m.To, m.Sequence, red == false));
                if (red == false && m.To == CardLocation.Hand(f.OwnerSeat) && !_turnCardUseEffectsOperations._turnCardUseEffects.GetHandLimitExemptCardIds(r.TurnNumber, r.TurnOwnerSeat, f.OwnerSeat).Contains(card.Id))
                {
                    var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandLimitExemptCard(r.TurnNumber, r.TurnOwnerSeat, f.Id, f.InstructionIndex - 1, 0, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), f.OwnerSeat, card.Id, m.Sequence);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnHandLimitExemptCardGrantedEvent(grant));
                }
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZhenJiLuoshen = f.MouZhenJiLuoshen!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouZhenJiLuoshen(f.Id);
    }
}
