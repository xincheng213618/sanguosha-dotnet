#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    internal static CardConversionSource WangCanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wang_can_ol_WangCanPrograms.WangCanSource(f);
    internal static CardLocation WangCanLease(int owner) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wang_can_ol_WangCanPrograms.WangCanLease(owner);
    internal static string WangCanReason(WangCanStage stage) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wang_can_ol_WangCanPrograms.WangCanReason(stage);
    internal SkillProgramStepOutcome ExecuteWangCan(SkillProgramEffectOp op, ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var source = WangCanSource(f);
        if (f.WangCan is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Wang Can requires its actual unpaid native source and window.");
        var initial = op switch
        {
            SkillProgramEffectOp.ResolveSanwenActualGainDuplicates => WangCanStage.SanwenDiscard,
            SkillProgramEffectOp.CollectQiaiLivingContributions => WangCanStage.QiaiChoose,
            _ => WangCanStage.DenglouViewChildren
        };
        var r = new WangCanReceipt(source, f.GameplayHash, 1, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, initial, Sanwen: w.Sanwen);
        if (op == SkillProgramEffectOp.ResolveSanwenActualGainDuplicates && (w.Window != SkillProgramTriggerWindow.CardsGained || w.Sanwen is not { } q || q.OwnerSeat != f.OwnerSeat || w.MovementBatch is not { } batch || !ValidSanwenProof(q, batch) || SanwenPayable(q).Count == 0))
            throw new InvalidOperationException("Sanwen lost the frozen real gain batch or its original hand witnesses.");
        if (op == SkillProgramEffectOp.CollectQiaiLivingContributions && (w.Window != SkillProgramTriggerWindow.DyingEntering || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.VictimSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(entry => entry.Id == w.ParentFrameId)?.ResumeDyingFrameId != dying.Id))
            throw new InvalidOperationException("Qiai requires its actual own entering-dying frame.");
        if (op == SkillProgramEffectOp.ViewDenglouTopAndUseBasics && (w.Window != SkillProgramTriggerWindow.TurnEnding || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count != 0))
            throw new InvalidOperationException("Denglou requires an actual own Ending with no hand.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangCanStartedEvent(f.Id, source, f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, op));
        if (op == SkillProgramEffectOp.ResolveSanwenActualGainDuplicates)
        {
            var ids = SanwenPayable(r.Sanwen!);
            var names = r.Sanwen!.Incoming.Where(i => ids.Contains(i.CardId)).Select(i => i.Name).ToHashSet();
            var witnesses = r.Sanwen.Witnesses.Where(i => names.Contains(i.Name) && _cardZonesOperations._cardZones.GetLocation(i.CardId) == CardLocation.Hand(f.OwnerSeat)).Select(i => i.CardId).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Entities = ids } });
            _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "sanwen-witness", witnesses, SkillProgramCardSetVisibility.Public);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, "sanwen-witness", Array.AsReadOnly(witnesses.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "sanwen-witness-hand"))
                return SkillProgramStepOutcome.AwaitChild;
            ContinueSanwenAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.CollectQiaiLivingContributions)
        {
            var seats = Enumerable.Range(1, _coreMatchParticipantsPlayersAccess._players.Count - 1).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Seats = seats, DyingFrameId = _coreDyingCursorRuntimeOperations.ActiveDying!.Id } });
            ContinueQiai(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        BeginDenglouLease(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [PublicHandRevealReturn("sanwen-witness-hand")]
    internal void ContinueSanwenAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.WangCan ?? throw new InvalidOperationException("Sanwen lost its original duplicate gain receipt.");
        var ids = r.Entities.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)).ToArray();
        PayWangCanMove(f, ids, CardLocation.DiscardPile, WangCanStage.SanwenDiscard);
    }

    internal void PayWangCanMove(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, WangCanStage stage)
    {
        var r = f.WangCan!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        r = r with
        {
            Stage = stage,
            Before = before,
            After = before,
            ActualCount = 0,
            Entities = r.Lease is null ? ids : r.Entities
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        if (ids.Count > 0)
        {
            if (stage == WangCanStage.SanwenDiscard)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(WangCanReason(stage)));
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(WangCanReason(stage)), (stage == WangCanStage.QiaiGift ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.Seats[r.Cursor], destination.OwnerSeat ?? f.OwnerSeat, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) : null));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.WangCan!with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualCount = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.Reason.Value == WangCanReason(stage))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r });
        IssueWangCanSegment(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssueWangCanSegment(ProgramSkillFrame f)
    {
        var r = f.WangCan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangCanSegmentPaidEvent(f.Id, r.Source, r.GameplayHash, r.Stage, r.ActualTurn, r.Stage == WangCanStage.QiaiGift ? r.Seats[r.Cursor] : f.OwnerSeat, r.Before, r.After, r.ActualCount));
    }

    internal void PublishWangCan(ProgramSkillFrame f, int seat, IReadOnlyList<PromptChoice> choices, string text)
    {
        var def = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, def.Name, def.Name, def.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> WangCanChoices(ProgramSkillFrame f)
    {
        var r = f.WangCan!;
        if (r.Stage == WangCanStage.QiaiChoose)
            return QiaiChoices(f);
        if (r.Stage == WangCanStage.DenglouView)
            return[new(new($"denglou.{(f.Id)}.view"), "确认观看并获得非基本牌", r.Remaining, [], new Dictionary<string, string> { { "program-action", "wangcan" }, { "branch", "view" } })];
        if (r.Stage != WangCanStage.DenglouChoose)
            return[];
        return Array.AsReadOnly(BuildDenglouNativeBasicUseOptions(f).Select((a, i) => new PromptChoice(new($"denglou.{(f.Id)}.use-{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string> { { "program-action", "wangcan" }, { "branch", "use" }, { "option", i.ToString() } })).ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.CollectQiaiLivingContributions, SkillProgramEffectOp.ViewDenglouTopAndUseBasics)]
    internal PromptChoice WangCanAiChoice(ProgramSkillFrame f) => WangCanChoices(f).First();
    [ProgramAnswer("wangcan")]
    internal void ResolveWangCanChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireIssuedWangCan(f);
        var r = f.WangCan!;
        var seat = r.Stage == WangCanStage.QiaiChoose ? r.Seats[r.Cursor] : f.OwnerSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !WangCanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("Wang Can choice changed the private actual payer, lease entity or legal native use.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == WangCanStage.QiaiChoose)
        {
            var id = choice.Cards.Single();
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != seat || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Qiai payer no longer holds its actual chosen transferable entity.");
            PayWangCanMove(f, [id], CardLocation.Hand(f.OwnerSeat), WangCanStage.QiaiGift);
            return;
        }

        if (r.Stage == WangCanStage.DenglouView)
        {
            var nonbasic = r.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == r.Lease && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind) != SkillProgramCardCategory.Basic).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Remaining = r.Remaining.Except(nonbasic).ToArray() } });
            PayWangCanMove(f, nonbasic, CardLocation.Hand(f.OwnerSeat), WangCanStage.DenglouGain);
            return;
        }

        IssueDenglouNativeUse(f, BuildDenglouNativeBasicUseOptions(f)[int.Parse(choice.Parameters["option"])]);
    }

    internal bool DrainWangCan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 24600)]
    internal bool ResumeWangCan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangCan: { } r } f || f.Id != id || r.Stage is WangCanStage.QiaiChoose or WangCanStage.DenglouView or WangCanStage.DenglouChoose or WangCanStage.DenglouNative)
            return false;
        RequireIssuedWangCan(f);
        if (DrainWangCan(f))
            return true;
        if (r.Stage == WangCanStage.SanwenDiscard)
        {
            PaySanwenDraw(f);
            return true;
        }

        if (r.Stage == WangCanStage.QiaiGift)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Cursor = r.Cursor + 1, Stage = WangCanStage.QiaiChoose }, PendingMovementContinuation = null });
            ContinueQiai(f);
            return true;
        }

        if (r.Stage == WangCanStage.DenglouViewChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = r with { Stage = WangCanStage.DenglouView }, PendingMovementContinuation = null });
            PublishWangCan(f, f.OwnerSeat, WangCanChoices(f), "登楼：私下观看实际牌堆顶的牌。");
            return true;
        }

        if (r.Stage is WangCanStage.DenglouGain or WangCanStage.Completed)
        {
            ContinueDenglou(f);
            return true;
        }

        if (r.Stage == WangCanStage.DenglouViewChildren)
        {
            ContinueDenglou(f);
            return true;
        }

        FinishWangCan(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24600)]
    internal bool ReturnWangCanMovement(ProgramSkillFrame f) => f.WangCan is not null && ResumeWangCan(f.Id);
    internal void FinishWangCan(ProgramSkillFrame f)
    {
        var r = f.WangCan!;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Wang Can completed twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangCanCompletedEvent(f.Id, r.Source, r.GameplayHash));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangCan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IWangCanProgramHost.ExecuteWangCan(SkillProgramEffectOp op, ProgramSkillFrame f) => ExecuteWangCan(op, f);
}
