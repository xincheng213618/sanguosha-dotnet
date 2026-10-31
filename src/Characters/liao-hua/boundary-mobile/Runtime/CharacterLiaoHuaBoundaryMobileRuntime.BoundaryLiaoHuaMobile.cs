#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiaoHuaBoundaryMobileRuntime
{
    internal const string BoundaryMobileDangxian = "boundary:dangxian-mobile", BoundaryLiaoHuaDiscardAction = "boundary-liao-hua-discard-slash", BoundaryLiaoHuaDiscardReason = "skill-program.boundary:liao-hua-mobile.discard-slash";
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryLiaoHuaMobileHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginBoundaryLiaoHuaDiscardSlash(frame);
    internal SkillProgramStepOutcome BeginBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != BoundaryMobileDangxian || frame.TriggerId != "discard-slash-extra-play" || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || frame.BoundaryLiaoHuaDiscardSlash is not null)
            throw new InvalidOperationException("Mobile Dangxian requires its fresh original own-turn starting instruction.");
        var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind)).Select(card => card.Id).ToArray();
        if (cards.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryLiaoHuaDiscardSlash = new(new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, context.ParentFrameId, frame.InstructionIndex) { Candidates = cards } });
        PublishBoundaryLiaoHuaDiscardSlash(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryLiaoHuaDiscardChoices(ProgramSkillFrame frame) => Array.AsReadOnly(frame.BoundaryLiaoHuaDiscardSlash!.Candidates.Select(id =>
    {
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == id);
        return new PromptChoice(new($"boundary-liao-hua.frame-{(frame.Id)}.card-{(id)}"), $"获得弃牌堆的【{(card.DisplayName)}】（{(card.RankText)}）", [id], [], new Dictionary<string, string> { ["program-action"] = BoundaryLiaoHuaDiscardAction });
    }).ToArray());
    internal void PublishBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "当先：选择获得弃牌堆中的一张杀。", frame.BoundaryLiaoHuaDiscardSlash!.Candidates, [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = BoundaryLiaoHuaDiscardChoices(frame),
            SkillPrompt = new(frame.SkillId, skill.Name, "当先 · 获得杀", skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryLiaoHuaDiscardAction)]
    internal void AnswerBoundaryLiaoHuaDiscardSlash(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiaoHuaDiscardSlash.CardId: null } frame || !ValidBoundaryLiaoHuaDiscardSlash(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], BoundaryLiaoHuaDiscardChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mobile Dangxian requires its current original owner's published actual discarded Slash.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "当先获得杀之前原技能来源已失效。");
            return;
        }

        var receipt = frame.BoundaryLiaoHuaDiscardSlash!with
        {
            CardId = selected.Cards.Single(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryLiaoHuaDiscardSlash = receipt });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([receipt.CardId.Value], CardLocation.Hand(frame.OwnerSeat), new(BoundaryLiaoHuaDiscardReason), (batch, moves) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BoundaryLiaoHuaDiscardSlash = receipt with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiaoHuaDiscardSlashObtainedEvent(frame.Id, receipt.InstructionIndex, frame.OwnerSeat, receipt.CardId.Value, batch, receipt.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { BoundaryLiaoHuaDiscardSlash = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
    }

    internal bool ValidBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiaoHuaDiscardSlash is not { } receipt || frame.SkillId != BoundaryMobileDangxian || frame.TriggerId != "discard-slash-extra-play" || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.ParentFrameId != receipt.WindowFrameId || receipt.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || receipt.InstructionIndex != frame.InstructionIndex || receipt.Candidates.Count == 0 || receipt.Candidates.Distinct().Count() != receipt.Candidates.Count)
            return false;
        if (receipt.CardId is null)
            return receipt.Candidates.All(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Any(card => card.Id == id && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind)));
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After).ToArray();
        return receipt.Candidates.Contains(receipt.CardId.Value) && receipt.BatchId > 0 && moves is [var paid] && paid.CardId == receipt.CardId && paid.From == CardLocation.DiscardPile && paid.To == CardLocation.Hand(frame.OwnerSeat) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(paid.CardKind) && paid.Reason.Value == BoundaryLiaoHuaDiscardReason && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiaoHuaDiscardSlashObtainedEvent>().Count(fact => fact.FrameId == frame.Id && fact.InstructionIndex == receipt.InstructionIndex && fact.OwnerSeat == frame.OwnerSeat && fact.CardId == receipt.CardId && fact.BatchId == receipt.BatchId && fact.Before == receipt.Before && fact.After == receipt.After) == 1;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 55600)]
    internal bool ReturnBoundaryLiaoHuaDiscardMovement(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiaoHuaDiscardSlash?.CardId is null)
            return false;
        if (!ValidBoundaryLiaoHuaDiscardSlash(frame))
            throw new InvalidOperationException("Mobile Dangxian lost its exact paid discard gain and original instruction.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryLiaoHuaDiscardSlash = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 55600)]
    internal bool ResumeBoundaryLiaoHuaDiscard(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiaoHuaDiscardSlash: { CardId: null } } frame || frame.Id != id)
            return false;
        if (!ValidBoundaryLiaoHuaDiscardSlash(frame))
            throw new InvalidOperationException("Mobile Dangxian lost its original frozen public discard candidates.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishBoundaryLiaoHuaDiscardSlash(frame);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ObtainBoundaryLiaoHuaDiscardSlash)]
    internal PromptChoice ChooseBoundaryLiaoHuaDiscard(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.First();
}
