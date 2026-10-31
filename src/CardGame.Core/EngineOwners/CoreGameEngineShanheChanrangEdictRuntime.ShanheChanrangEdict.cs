#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheChanrangEdictRuntime
{
    internal const string ShanheChanrangSkill = "shanhe:chanrang-edict-1510";
    internal bool HasShanheChanrangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheChanrangEdict);

    internal void CaptureShanheChanrangFirstGains(CardMovementBatchContext batch)
    {
        if (!HasShanheChanrangCapability || batch.TurnNumber <= 0 || batch.MovementTiming is not { } timing)
            return;
        var seen = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheChanrangFirstGainEvent>().Where(fact => fact.ActualTurn == batch.TurnNumber).Select(fact => fact.RecipientSeat).ToHashSet();
        foreach (var seat in batch.Movements.Where(move => move.To.Zone == CardZoneKind.Hand && move.To.OwnerSeat is not null).Select(move => move.To.OwnerSeat!.Value).Distinct())
        {
            if (seat == timing.ActualTurnOwnerSeat || seen.Contains(seat))
                continue;
            var gains = _coreActualHandGainProgramsOperations.ActualHandGainEntities(batch, seat);
            if (gains.Length != 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheChanrangFirstGainEvent(batch.TurnNumber, timing.ActualTurnOwnerSeat, seat, batch.Id, gains.Min(gain => gain.GainSequence)));
        }
    }

    internal bool IsShanheChanrangCandidate(ProgramTriggerCandidate candidate) => candidate.SkillId == ShanheChanrangSkill && candidate.BindingId == "first-foreign-turn-gain";
    internal Card? ShanheChanrangEquipment(int owner, string instance) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).FirstOrDefault(card => card.Kind == CardKind.ShanRangZhaoShu && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, ShanheChanrangSkill, instance, card));
    internal int[] ShanheChanrangRecipients(CardMovementBatchContext batch, int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheChanrangFirstGainEvent>().Where(fact => fact.BatchId == batch.Id && fact.ActualTurn == batch.TurnNumber && fact.RecipientSeat != owner && _coreMatchParticipantsPlayersAccess._players[fact.RecipientSeat].IsAlive).OrderBy(fact => fact.FirstMovementSequence).ThenBy(fact => fact.RecipientSeat).Select(fact => fact.RecipientSeat).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 5434)]
    internal void CollectShanheChanrangCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!HasShanheChanrangCapability)
            return;
        candidates.RemoveAll(IsShanheChanrangCandidate);
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheChanrangFirstGainEvent>().Any(fact => fact.BatchId == batch.Id))
            return;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Any(card => card.Kind == CardKind.ShanRangZhaoShu) && ShanheChanrangRecipients(batch, player.Seat).Length != 0))
            candidates.AddRange(_coreProgramLifecycleOperations.CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.CardsMoved).Where(candidate => IsShanheChanrangCandidate(candidate) && ShanheChanrangEquipment(player.Seat, candidate.SkillInstanceId)is not null));
    }

    internal ProgramSkillWindowContext CreateShanheChanrangContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, window.Id, candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: window.Batch);
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 5434)]
    internal bool IsIssuedShanheChanrang(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsShanheChanrangCandidate(candidate) && context is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } && context.OwnerSeat == candidate.OwnerSeat && ShanheChanrangRecipients(batch, candidate.OwnerSeat).Length != 0 && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == context.ParentFrameId && window.Batch.Id == batch.Id && window.CandidateIndex < window.Candidates.Count && window.CandidateIndex >= 0 && window.Candidates[window.CandidateIndex] == candidate && ShanheChanrangEquipment(candidate.OwnerSeat, candidate.SkillInstanceId)is not null;
    SkillProgramStepOutcome IShanheChanrangHost.Resolve(ProgramSkillFrame frame) => BeginShanheChanrang(frame);
    internal SkillProgramStepOutcome BeginShanheChanrang(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheChanrangSkill || frame.ShanheChanrang is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } context || ShanheChanrangEquipment(frame.OwnerSeat, frame.SkillInstanceId)is not { } equipment)
            throw new InvalidOperationException("The edict lost its real equipment grant or first-gain batch.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = new() { ParentWindowId = context.ParentFrameId, BatchId = batch.Id, ActualTurn = batch.TurnNumber, EquipmentId = equipment.Id, Recipients = ShanheChanrangRecipients(batch, frame.OwnerSeat) } });
        ContinueShanheChanrang(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card[] ShanheChanrangCards(int giver) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[giver]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[giver])).ToArray();
    internal IReadOnlyList<PromptChoice> ShanheChanrangChoices(ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!;
        PromptChoice Choice(string branch, string label, int? card = null) => new(new ChoiceId($"chanrang.{(frame.Id)}.{(draft.Cursor)}.{(branch)}.{(card)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "shanhe-chanrang", ["branch"] = branch });
        if (draft.Stage == "card")
            return ShanheChanrangCards(draft.GiverSeat!.Value).Select(card => Choice("card", "交给对方：" + card.DisplayName, card.Id)).ToArray();
        if (draft.Stage != "branch")
            return[];
        var recipient = draft.Recipients[draft.Cursor];
        var choices = new List<PromptChoice>
        {
            Choice("skip", "放弃")
        };
        if (ShanheChanrangCards(frame.OwnerSeat).Length != 0)
            choices.Add(Choice("give", "交给" + _coreMatchParticipantsPlayersAccess._players[recipient].Name + "一张牌"));
        if (ShanheChanrangCards(recipient).Length != 0)
            choices.Add(Choice("take", "令" + _coreMatchParticipantsPlayersAccess._players[recipient].Name + "交给你一张牌"));
        return choices;
    }

    internal void PublishShanheChanrang(ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!;
        var choices = ShanheChanrangChoices(frame);
        if (choices.Count == 0)
        {
            AdvanceShanheChanrangRecipient(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Choices = choices }, Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = draft.Stage == "card" ? draft.GiverSeat!.Value : frame.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, "禅让诏书：选择交牌方式", choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = draft.Recipients[draft.Cursor],
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shanhe-chanrang")]
    internal void AnswerShanheChanrang(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The edict lost its original frame.");
        var draft = frame.ShanheChanrang!;
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ShanheChanrangChoices(frame).Where(candidate => candidate.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The edict choice is no longer its exact issued card or branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "skip")
        {
            AdvanceShanheChanrangRecipient(frame);
            return;
        }

        if (draft.Stage == "branch")
        {
            var other = draft.Recipients[draft.Cursor];
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Stage = "card", GiverSeat = branch == "give" ? frame.OwnerSeat : other, RecipientSeat = branch == "give" ? other : frame.OwnerSeat, Choices = [] } });
            PublishShanheChanrang(frame);
            return;
        }

        var giver = draft.GiverSeat!.Value;
        var recipient = draft.RecipientSeat!.Value;
        var card = choice.Cards.Single();
        if (!_coreMatchParticipantsPlayersAccess._players[giver].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !ShanheChanrangCards(giver).Any(candidate => candidate.Id == card))
            throw new InvalidOperationException("The edict payment lost its actual giver, living recipient or owned entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft with { Stage = "paid", PaidCardId = card, Choices = [] } });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(recipient), CoreGameEngineShanheChanrangEdictDefinitions.ShanheChanrangGiveReason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, giver, recipient, [card], _coreCardMovementJournalMovementSequenceAccess._movementSequence, (_, movements) =>
        {
            var move = movements.Single(item => item.CardId == card && item.To == CardLocation.Hand(recipient));
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(active.Id, active with { ShanheChanrang = active.ShanheChanrang!with { PaidMovementSequence = move.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheChanrangPaidEvent(frame.Id, frame.OwnerSeat, giver, recipient, card, move.Sequence));
        }));
        ContinueShanheChanrang(frame.Id);
    }

    internal void AdvanceShanheChanrangRecipient(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = frame.ShanheChanrang!with { Cursor = frame.ShanheChanrang.Cursor + 1, Stage = "branch", GiverSeat = null, RecipientSeat = null, PaidCardId = null, PaidMovementSequence = null, Choices = [] }, PendingMovementContinuation = null });
        ContinueShanheChanrang(frame.Id);
    }

    internal void ContinueShanheChanrang(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame top || top.Id != frameId)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var draft = frame.ShanheChanrang!;
        if (draft.Stage == "paid")
        {
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program))
                return;
            AdvanceShanheChanrangRecipient(frame);
            return;
        }

        while (draft.Cursor < draft.Recipients.Count && !_coreMatchParticipantsPlayersAccess._players[draft.Recipients[draft.Cursor]].IsAlive)
            draft = draft with
            {
                Cursor = draft.Cursor + 1
            };
        if (draft.Cursor >= draft.Recipients.Count || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || ShanheChanrangEquipment(frame.OwnerSeat, frame.SkillInstanceId)?.Id != draft.EquipmentId)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheChanrang = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheChanrang = draft });
        PublishShanheChanrang(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 5434)]
    internal bool ResumeShanheChanrang(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheChanrang: not null } frame || frame.Id != frameId)
            return false;
        ContinueShanheChanrang(frameId);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5434)]
    internal bool ContinuePaidShanheChanrang(ProgramSkillFrame frame) => frame.ShanheChanrang is { Stage: "paid", PaidCardId: not null };

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheChanrangEdict)]
    internal PromptChoice AiShanheChanrang(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.ShanheChanrang!;
        if (draft.Stage == "card")
            return decision.Choices.OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[draft.GiverSeat!.Value])).First();
        var ally = CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[draft.Recipients[draft.Cursor]]);
        return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == (ally ? "give" : "take")) ?? decision.Choices[0];
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 100)]
    internal ProgramSkillWindowContext? TryCreateShanheChanrangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsShanheChanrangCandidate(candidate))
            return CreateShanheChanrangContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 900)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheChanrangFirstGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureShanheChanrangFirstGains(completed);
        return completed;
    }
}
