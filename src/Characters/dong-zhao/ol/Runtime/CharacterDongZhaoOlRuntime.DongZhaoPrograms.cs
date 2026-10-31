#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoOlRuntime
{
    internal const string XianlueDrawBind = "xianlue-exact-drawn";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30)]
    internal bool CanOfferDongZhao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.XianlueRecord))
            return _coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Identity && w.SourceSeat is { } lord && _coreTableSeatQueriesOperations.IsValidPlayerSeat(lord) && _coreMatchParticipantsPlayersAccess._players[lord].Role == Role.Lord && _coreMatchParticipantsPlayersAccess._players[lord].IsAlive;
        if (!t.Effects.Any(e => e.Op == SkillProgramEffectOp.XianlueRedistribute))
            return true;
        var recorded = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianlueRecordedEvent>().LastOrDefault(e => e.OwnerSeat == c.OwnerSeat && e.SkillInstanceId == c.SkillInstanceId);
        return recorded is not null && w.CardUse is { } use && use.ActorSeat != c.OwnerSeat && use.EffectiveKind == recorded.Kind && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianlueRewardIssuedEvent>().Any(e => e.OwnerSeat == c.OwnerSeat && e.SkillInstanceId == c.SkillInstanceId && e.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
    }

    internal SkillProgramStepOutcome BeginXianlueRecord(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XianluePending = new(XianlueStage.Record, 0) });
        PublishXianluePrompt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome BeginXianlueRedistribute(ProgramSkillFrame f)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (f.WindowContext?.CardUse is not { } use)
            throw new InvalidOperationException("Xianlue lost its actual completed card use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XianluePending = new(XianlueStage.DrawChildren, 0, f.SkillId, f.SkillInstanceId, f.GameplayHash, f.WindowContext.ParentFrameId, use.ParentCardUseFrameId, use.CardActionId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianlueRewardIssuedEvent(f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, f.WindowContext.ParentFrameId, use.ParentCardUseFrameId, use.CardActionId));
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new("program.xianlue.draw"));
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, XianlueDrawBind, drawn, SkillProgramCardSetVisibility.Private);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XianluePending = f.XianluePending!with { DrawAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianlueMovementIssuedEvent(f.Id, XianlueStage.DrawChildren, f.XianluePending!.DrawBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null, null));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PublishXianluePrompt(ProgramSkillFrame f)
    {
        var pending = f.XianluePending!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string suffix, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets, string value) => new(new ChoiceId($"xianlue.{(f.Id)}.{(suffix)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "xianlue", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["value"] = value });
        if (pending.Stage == XianlueStage.Record)
        {
            foreach (var kind in Enum.GetValues<CardKind>().Where(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick).Distinct())
                choices.Add(Choice(kind.ToString(), $"记录【{(CardCatalog.Get(kind).DisplayName)}】", [], [], kind.ToString()));
        }
        else
        {
            var binding = _coreProgramLifecycleOperations.GetProgramCardSet(f, XianlueDrawBind);
            var cardId = binding.CardIds[pending.CardIndex];
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                choices.Add(Choice($"give.{(cardId)}.{(player.Seat)}", $"将此牌分配给 {(player.Name)}", [cardId], [player.Seat], player.Seat.ToString(CultureInfo.InvariantCulture)));
        }

        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, pending.Stage == XianlueStage.Record ? "先略：记录普通锦囊牌" : "先略：分配此次摸到的牌", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = pending.Stage != XianlueStage.Record,
            Choices = choices.AsReadOnly(),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("xianlue")]
    internal void ResolveDongZhaoChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Xianlue lost its frame.");
        if (f.XianluePending is not { } r || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Xianlue choice belongs to a different frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == XianlueStage.Record)
        {
            var kind = Enum.Parse<CardKind>(choice.Parameters["value"]);
            if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
                throw new InvalidOperationException("Xianlue can only record an ordinary trick.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianlueRecordedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, kind));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { XianluePending = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage != XianlueStage.Distribute)
            throw new InvalidOperationException("Xianlue cannot repay a completed gift.");
        var id = _coreProgramLifecycleOperations.GetProgramCardSet(f, XianlueDrawBind).CardIds[r.CardIndex];
        var target = int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture);
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || choice.Cards is not [var chosen] || chosen != id || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Xianlue gift lost its actual drawn entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XianluePending = r with { Stage = XianlueStage.GiftChildren, CardIndex = r.CardIndex + 1, GiftBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, GiftAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, GiftCardId = id, GiftTargetSeat = target }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (target != f.OwnerSeat)
            _corePhysicalMovementFacadesOperations.MoveCards([_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == id)], CardLocation.Hand(f.OwnerSeat), CardLocation.Hand(target), new("program.xianlue.gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XianluePending = f.XianluePending!with { GiftAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianlueMovementIssuedEvent(f.Id, XianlueStage.GiftChildren, f.XianluePending!.GiftBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, id, target));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21900)]
    internal bool ResumeDongZhao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XianluePending: { } r } f || f.Id != id)
            return false;
        if (r.Stage is XianlueStage.Record or XianlueStage.Distribute)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishXianluePrompt(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.XianluePending!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { XianluePending = null, PendingMovementContinuation = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, XianlueDrawBind).CardIds;
        var index = r.CardIndex;
        while (index < cards.Count && _cardZonesOperations._cardZones.GetLocation(cards[index]) != CardLocation.Hand(f.OwnerSeat))
            index++;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, XianluePending = r with { Stage = index < cards.Count ? XianlueStage.Distribute : XianlueStage.Record, CardIndex = index } });
        PublishXianluePrompt(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 10)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 10)]
    internal bool CanContinueIssuedDongZhao(ProgramSkillFrame f) => ValidIssuedXianlue(f) || ValidIssuedZaowang(f);
    internal bool ValidIssuedXianlue(ProgramSkillFrame f)
    {
        if (f.XianluePending is not { } r || f.InstructionIndex != 1 || r.SkillId != f.SkillId || r.SkillInstanceId != f.SkillInstanceId || r.GameplayHash != f.GameplayHash || f.TriggerId is null || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.XianlueRedistribute }] || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } u } w || w.ParentFrameId != r.WindowId || u.ParentCardUseFrameId != r.UseId || u.CardActionId != r.ActionId || u.ActorSeat == f.OwnerSeat || r.DrawBefore < 0 || r.DrawAfter < r.DrawBefore || r.DrawAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.CardIndex < 0)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianlueRewardIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (facts is not [var fact] || fact.SkillId != r.SkillId || fact.SkillInstanceId != r.SkillInstanceId || fact.GameplayHash != r.GameplayHash || fact.WindowId != r.WindowId || fact.CardUseId != r.UseId || fact.CardActionId != r.ActionId)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame window || window.Id != r.WindowId || window.ParentFrameId != r.UseId || window.Action.ActionId != r.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Step: ResolutionFrameStep.Completed, Action: { } action } || action.ActionId != r.ActionId || action.ActorSeat != u.ActorSeat)
            return false;
        var b = f.CardSetBindings.SingleOrDefault(b => b.Name == XianlueDrawBind);
        if (b is null || r.CardIndex > b.CardIds.Count || b.CardIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && m.CardId == id && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.xianlue.draw") != 1))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianlueMovementIssuedEvent>().Any(e => e.FrameId == f.Id && e.Stage == XianlueStage.DrawChildren && e.Before == r.DrawBefore && e.After == r.DrawAfter) && (r.Stage != XianlueStage.GiftChildren || r.GiftCardId is { } gift && r.GiftTargetSeat is { } target && b.CardIds.Contains(gift) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XianlueMovementIssuedEvent>().Any(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Before == r.GiftBefore && e.After == r.GiftAfter && e.GiftCardId == gift && e.GiftTargetSeat == target) && (target == f.OwnerSeat ? r.GiftBefore == r.GiftAfter : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.GiftBefore && m.Sequence <= r.GiftAfter && m.CardId == gift && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(target) && m.Reason.Value == "program.xianlue.gift") == 1));
    }
    internal bool DongZhaoFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (ValidIssuedZaowang(root))
            return ZaowangFirstChild(root, child);
        if (!ValidIssuedXianlue(root) || root.XianluePending is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == XianlueStage.DrawChildren ? m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && m.Reason.Value == "program.xianlue.draw" : r.Stage == XianlueStage.GiftChildren && m.Sequence > r.GiftBefore && m.Sequence <= r.GiftAfter && m.CardId == r.GiftCardId && m.Reason.Value == "program.xianlue.gift"));
    }

    SkillProgramStepOutcome global::CardGame.Core.IDongZhaoProgramHost.XianlueRecord(ProgramSkillFrame f) => BeginXianlueRecord(f);
    SkillProgramStepOutcome global::CardGame.Core.IDongZhaoProgramHost.XianlueRedistribute(ProgramSkillFrame f) => BeginXianlueRedistribute(f);
    SkillProgramStepOutcome global::CardGame.Core.IDongZhaoProgramHost.ZaowangResolve(ProgramSkillFrame f) => ArmZaowang(f);
}
