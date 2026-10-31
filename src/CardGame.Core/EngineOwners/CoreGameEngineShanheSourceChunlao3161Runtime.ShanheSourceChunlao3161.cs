#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceChunlao3161Runtime
{
    internal int[] MatchingShanheSourceChunlaoIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => batch.Movements.Select((move, index) => (move, index, origin: _coreAdjacentDiscardStorageOperations.AdjacentDiscardOrigin(move))).Where(x => x.move.To == CardLocation.DiscardPile && _cardZonesOperations._cardZones.GetLocation(x.move.CardId) == CardLocation.DiscardPile && x.origin is { } o && global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Contains(o.EffectiveKind) && _coreMatchParticipantsPlayersAccess._players[o.SourceSeat].IsAlive && _coreNativeRuleQueriesOperations.GetCombatDistance(o.SourceSeat, candidate.OwnerSeat) <= 1).Select(x => x.index).ToArray();
    internal SkillProgramStepOutcome BeginShanheSourceChunlaoStorage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShanheSourceJiangdongTrioIds.Chunlao || f.ShanheSourceChunlaoStorage is not null || f.WindowContext is not { MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || _coreAdjacentDiscardStorageOperations.AdjacentDiscardOrigin(batch.Movements[index])is not { } origin || !MatchingShanheSourceChunlaoIndexes(batch, new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0)).Contains(index))
            throw new InvalidOperationException("Source3161 requires its exact original distance-one discarded Slash.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceChunlaoStorage = new(batch.Movements[index].CardId, origin.SourceSeat, batch.Movements[index].Sequence) });
        var choices = ShanheSourceChunlaoStorageChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, origin.SourceSeat, $"醇醪：是否将此杀置于 {(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的武将牌上？", [batch.Movements[index].CardId], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[origin.SourceSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static IReadOnlyList<PromptChoice> ShanheSourceChunlaoStorageChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { "store", "decline" }.Select(branch => new PromptChoice(new($"source-chunlao.{(f.Id)}.{(branch)}"), branch == "store" ? "将此杀存为醇" : "不存入醇", [], [], new Dictionary<string, string> { ["program-action"] = "shanhe-source-chunlao-store", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch })).ToArray());
    [ProgramAnswer("shanhe-source-chunlao-store")]
    internal void AnswerShanheSourceChunlaoStorage(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceChunlaoStorage: { Paid: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.SourceSeat || !ShanheSourceChunlaoStorageChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Source3161 requires the original discarding player's exact choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "decline" || !_coreMatchParticipantsPlayersAccess._players[r.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(r.CardId) != CardLocation.DiscardPile)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceChunlaoStorage = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceChunlaoStorage = r with { Paid = true }, Step = ResolutionFrameStep.ResolvingEffect });
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == r.CardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, new(CardZoneKind.Chunlao, f.OwnerSeat), new("shanhe.source-chunlao-3161.store"), moved =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { ShanheSourceChunlaoStorage = current.ShanheSourceChunlaoStorage!with { StoreSequence = moved.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceChunlaoStoredEvent(f.Id, f.OwnerSeat, r.SourceSeat, r.CardId, r.DiscardSequence, moved.Sequence));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24921)]
    internal bool ResumeShanheSourceChunlaoStorage(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ShanheSourceChunlaoStorage is not { } r)
            return false;
        if (!r.Paid)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceChunlaoStorage = null });
        return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24921)]
    internal void AssertShanheSourceChunlaoStorage(ProgramSkillFrame f)
    {
        if (f.ShanheSourceChunlaoStorage is not { } r)
            return;
        if (f.SkillId != ShanheSourceJiangdongTrioIds.Chunlao || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || batch.Movements[index].CardId != r.CardId || batch.Movements[index].Sequence != r.DiscardSequence || _coreAdjacentDiscardStorageOperations.AdjacentDiscardOrigin(batch.Movements[index])?.SourceSeat != r.SourceSeat || r.Paid && !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.StoreSequence && m.CardId == r.CardId && m.From == CardLocation.DiscardPile && m.To == new CardLocation(CardZoneKind.Chunlao, f.OwnerSeat) && m.Reason.Value == "shanhe.source-chunlao-3161.store"))
            throw new InvalidOperationException("Source3161 storage lost its original discard or exact committed pile transfer.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferShanheSourceChunlaoStorage)]
    internal PromptChoice AiShanheSourceChunlaoStorage(PendingDecision p, ProgramSkillFrame f) => p.Choices.Single(c => c.Parameters["branch"] == (f.ShanheSourceChunlaoStorage!.SourceSeat == f.OwnerSeat || global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.ShanheSourceChunlaoStorage.SourceSeat], _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? "store" : "decline"));
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 1100)]
    internal int[]? SelectWindowIndexes_MatchingShanheSourceChunlaoIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.OfferShanheSourceChunlaoStorage))
            return MatchingShanheSourceChunlaoIndexes(batch, candidate);
        return null;
    }
}
