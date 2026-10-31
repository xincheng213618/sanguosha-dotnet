#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunXiuBoundaryMobileRuntime
{
    internal const string BoundaryXingxueAction = "boundary-sun-xiu-xingxue", BoundaryXingxueDraw = "skill.boundary-sun-xiu.xingxue.draw", BoundaryXingxueTop = "skill.boundary-sun-xiu.xingxue.top", BoundaryXingxueGive = "skill.boundary-sun-xiu.xingxue.give";
    internal static CardConversionSource BoundarySunXiuSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IBoundarySunXiuHost.Resolve(ProgramSkillFrame frame) => BeginBoundaryXingxue(frame);
    bool global::CardGame.Core.IBoundarySunXiuSelectionQuery.HasOwnedRegionCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Count + _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(seat)).Count > 0;
    internal bool IsBoundarySunXiuFactionRangeTarget(int actorSeat, int targetSeat) => _coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && owner.Seat != actorSeat && _coreNationalSkillsOperations.HasRuntimeSkill(owner, BoundarySunXiuComposition.Zhaofu) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, BoundarySunXiuComposition.Zhaofu, null, Role.Lord) && string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[actorSeat]), "wu", StringComparison.Ordinal) && _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, targetSeat) == 1);
    internal SkillProgramStepOutcome BeginBoundaryXingxue(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.BoundarySunXiu is not null || frame.SkillId != BoundarySunXiuComposition.Upgraded || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatAccess._currentSeat)
            throw new InvalidOperationException("Upgraded Xingxue requires its original owner ending.");
        var receipt = new BoundarySunXiuReceipt(BoundarySunXiuSource(frame), frame.GameplayHash, context.ParentFrameId, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp), BoundarySunXiuStage.Participants);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.EndingId, receipt.Quota));
        PublishBoundaryXingxue(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int BoundaryXingxueChooser(ProgramSkillFrame frame) => frame.BoundarySunXiu!.Stage == BoundarySunXiuStage.Card ? frame.BoundarySunXiu.Participants[frame.BoundarySunXiu.Index] : frame.OwnerSeat;
    internal PromptChoice BoundaryXingxueChoice(ProgramSkillFrame frame, string branch, string key, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"boundary-xingxue.{(frame.Id)}.{(frame.BoundarySunXiu!.Index)}.{(branch)}.{(key)}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", BoundaryXingxueAction }, { "branch", branch }, { "frame-id", frame.Id.ToString(CultureInfo.InvariantCulture) } });
    internal IReadOnlyList<PromptChoice> BoundaryXingxueChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundarySunXiu!;
        var choices = new List<PromptChoice>();
        if (receipt.Stage == BoundarySunXiuStage.Participants)
        {
            if (receipt.Participants.Count < receipt.Quota)
                choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !receipt.Participants.Contains(p.Seat)).Select(p => BoundaryXingxueChoice(frame, "participant", p.Seat.ToString(CultureInfo.InvariantCulture), "选择 " + p.Name, targets: [p.Seat])));
            choices.Add(BoundaryXingxueChoice(frame, "finish", "done", $"开始依次结算（已选{(receipt.Participants.Count)}/{(receipt.Quota)}）"));
        }
        else if (receipt.Stage == BoundarySunXiuStage.Card)
        {
            var actor = receipt.Participants[receipt.Index];
            foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, actor, actor, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.DrawPileTop))
            {
                var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
                choices.Add(BoundaryXingxueChoice(frame, "top", card.Id.ToString(CultureInfo.InvariantCulture), $"将【{(card.DisplayName)}】置于牌堆顶", [card.Id]));
            }

            foreach (var recipient in receipt.Participants.Where(s => s != actor && _coreMatchParticipantsPlayersAccess._players[s].IsAlive))
                foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, actor, actor, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: recipient))
                {
                    var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
                    choices.Add(BoundaryXingxueChoice(frame, "give", $"{(card.Id)}.{(recipient)}", $"将【{(card.DisplayName)}】交给 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}", [card.Id], [recipient]));
                }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishBoundaryXingxue(ProgramSkillFrame frame)
    {
        var choices = BoundaryXingxueChoices(frame);
        if (choices.Count == 0)
        {
            NextBoundaryXingxue(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = BoundaryXingxueChooser(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, frame.BoundarySunXiu!.Stage == BoundarySunXiuStage.Participants ? $"兴学2级：选择至多{(frame.BoundarySunXiu.Quota)}名角色，然后按行动顺序结算。" : "兴学2级：选择自己的牌置顶，或交给本次其他入选角色。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryXingxueAction)]
    internal void AnswerBoundaryXingxue(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundarySunXiu: { } receipt } frame || !ValidBoundaryXingxue(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != BoundaryXingxueChooser(frame) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], BoundaryXingxueChoices(frame).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Xingxue requires its original participant chooser, current owned card and sealed recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = answer.Parameters["branch"];
        if (receipt.Stage == BoundarySunXiuStage.Participants)
        {
            if (branch == "participant")
            {
                var target = answer.Targets.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt = receipt with { Participants = receipt.Participants.Append(target).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuParticipantEvent(frame.Id, receipt.Participants.Count - 1, target));
                if (receipt.Participants.Count < receipt.Quota)
                {
                    PublishBoundaryXingxue(frame);
                    return;
                }
            }

            var ordered = receipt.Participants.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, s)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt with { Participants = ordered, Sealed = true, Index = 0, Stage = BoundarySunXiuStage.Drawing } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuSealedEvent(frame.Id, ordered.Length));
            BeginBoundaryXingxueDraw(frame);
            return;
        }

        var actor = receipt.Participants[receipt.Index];
        var cardId = answer.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var recipient = branch == "give" ? answer.Targets.Single() : -1;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt with { Stage = BoundarySunXiuStage.MoveChildren, CardId = cardId, From = from, Recipient = recipient, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        var native = recipient < 0 ? null : _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, actor, recipient, [cardId], before, null);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([cardId], recipient < 0 ? CardLocation.DrawPile : CardLocation.Hand(recipient), new(recipient < 0 ? BoundaryXingxueTop : BoundaryXingxueGive), (batch, moves) =>
        {
            native?.Invoke(batch, moves);
            if (recipient < 0 && moves.Any(m => m.CardId == cardId && m.To == CardLocation.DrawPile))
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop([cardId]);
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BoundarySunXiu = active.BoundarySunXiu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuCardMovedEvent(frame.Id, receipt.Index, actor, cardId, from, recipient, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        ContinueBoundaryXingxue(frame.Id);
    }

    internal void BeginBoundaryXingxueDraw(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundarySunXiu!;
        while (receipt.Index < receipt.Participants.Count && !_coreMatchParticipantsPlayersAccess._players[receipt.Participants[receipt.Index]].IsAlive)
            receipt = receipt with
            {
                Index = receipt.Index + 1
            };
        if (receipt.Index >= receipt.Participants.Count)
        {
            CompleteBoundaryXingxue(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt with { Stage = BoundarySunXiuStage.Drawing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawAttempt = null, CardId = 0, From = null, Recipient = -1 } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, receipt.Participants[receipt.Index], 1, new(BoundaryXingxueDraw));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundarySunXiuXingxue)]
    internal void ReturnBoundaryXingxueDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidBoundaryXingxue(frame) || frame.BoundarySunXiu is not { Stage: BoundarySunXiuStage.Drawing } receipt || outcome.RecipientSeat != receipt.Participants[receipt.Index] || outcome.RequestedCount != 1 || outcome.SequenceBefore < receipt.Before)
            throw new InvalidOperationException("Xingxue draw must return once to its original current participant.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt with { Stage = BoundarySunXiuStage.DrawChildren, After = outcome.SequenceAfter, DrawAttempt = outcome.AttemptFrameId }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuDrawReturnedEvent(frame.Id, receipt.Index, outcome.RecipientSeat, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryXingxue(frame.Id);
    }

    internal void NextBoundaryXingxue(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = frame.BoundarySunXiu!with { Index = frame.BoundarySunXiu.Index + 1, Stage = BoundarySunXiuStage.Drawing }, PendingMovementContinuation = null });
        BeginBoundaryXingxueDraw(frame);
    }

    internal void CompleteBoundaryXingxue(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = frame.BoundarySunXiu!with { Stage = BoundarySunXiuStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunXiuCompletedEvent(frame.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53500)]
    internal bool ContinueBoundaryXingxue(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundarySunXiu: { } receipt } frame || frame.Id != id || receipt.Stage == BoundarySunXiuStage.Complete)
            return false;
        if (!ValidBoundaryXingxue(frame))
            throw new InvalidOperationException("Xingxue lost its ending, frozen recipients or native payment cursor.");
        if (receipt.Stage is BoundarySunXiuStage.Participants or BoundarySunXiuStage.Card)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBoundaryXingxue(frame);
            return true;
        }

        if (receipt.Stage == BoundarySunXiuStage.Drawing)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.BoundarySunXiu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
        if (receipt.Stage == BoundarySunXiuStage.MoveChildren)
        {
            NextBoundaryXingxue(frame);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundarySunXiu = receipt with { Stage = BoundarySunXiuStage.Card } });
        PublishBoundaryXingxue(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53500)]
    internal bool ReturnBoundaryXingxueMovement(ProgramSkillFrame frame) => frame.BoundarySunXiu is not null && ContinueBoundaryXingxue(frame.Id);
    internal bool ValidBoundaryXingxue(ProgramSkillFrame frame) => frame.BoundarySunXiu is { } receipt && frame.InstructionIndex == 1 && frame.SkillId == BoundarySunXiuComposition.Upgraded && frame.TriggerId == "ending-draw-and-give-or-top" && receipt.Source == BoundarySunXiuSource(frame) && receipt.GameplayHash == frame.GameplayHash && receipt.EndingId == frame.WindowContext?.ParentFrameId && frame.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding && receipt.Participants.Count <= receipt.Quota && receipt.Participants.Distinct().Count() == receipt.Participants.Count && receipt.Participants.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && receipt.Index >= 0 && receipt.Index <= receipt.Participants.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunXiuStartedEvent>().Count(e => e.FrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.EndingId == receipt.EndingId && e.Quota == receipt.Quota) == 1 && (!receipt.Sealed || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunXiuSealedEvent>().Count(e => e.FrameId == frame.Id && e.Count == receipt.Participants.Count) == 1 && receipt.Participants.SequenceEqual(_coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunXiuParticipantEvent>().Where(e => e.FrameId == frame.Id).OrderBy(e => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, e.Participant)).Select(e => e.Participant)));
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 53500)]
    internal void AssertBoundaryXingxue(ProgramSkillFrame frame)
    {
        if (frame.BoundarySunXiu is not null && !ValidBoundaryXingxue(frame))
            throw new InvalidOperationException("Xingxue changed its exact original frozen ending participants.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 53500)]
    internal bool BoundaryXingxuePaidMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ResolveBoundarySunXiuXingxue || pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || !ValidBoundaryXingxue(frame) || frame.BoundarySunXiu is not { Sealed: true } r)
            return false;
        if (r.Stage == BoundarySunXiuStage.DrawChildren)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunXiuDrawReturnedEvent>().Count(e => e.FrameId == frame.Id && e.Index == r.Index && e.Participant == r.Participants[r.Index] && e.Attempt == r.DrawAttempt && e.After == r.After) == 1;
        return r.Stage == BoundarySunXiuStage.MoveChildren && r.From is { } from && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundarySunXiuCardMovedEvent>().Count(e => e.FrameId == frame.Id && e.Index == r.Index && e.Participant == r.Participants[r.Index] && e.CardId == r.CardId && e.From == from && e.Recipient == r.Recipient && e.Before == r.Before && e.After == r.After) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == r.CardId && m.From == from && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == (r.Recipient < 0 ? BoundaryXingxueTop : BoundaryXingxueGive));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53500)]
    internal bool KeepIssuedBoundaryXingxue(ProgramSkillFrame frame) => frame.BoundarySunXiu is { Sealed: true } && ValidBoundaryXingxue(frame);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundarySunXiuXingxue)]
    internal PromptChoice BoundaryXingxueAi(PendingDecision pending, ProgramSkillFrame frame)
    {
        var receipt = frame.BoundarySunXiu!;
        var chooser = BoundaryXingxueChooser(frame);
        if (receipt.Stage == BoundarySunXiuStage.Participants)
            return pending.Choices.FirstOrDefault(c => c.Targets.Count == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[chooser], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? pending.Choices.Single(c => c.Parameters["branch"] == "finish");
        var friendly = pending.Choices.Where(c => c.Parameters["branch"] == "give" && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[chooser], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])).ToArray();
        return (friendly.Length > 0 ? friendly : pending.Choices.Where(c => c.Parameters["branch"] == "top")).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[chooser])).First();
    }
}
