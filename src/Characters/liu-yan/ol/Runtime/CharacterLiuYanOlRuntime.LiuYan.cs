#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 30)]
    internal bool LiuYanNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasLiuYanCapability;
    internal const string LiuYanDrawReason = "skill-program.liu-yan.tushe.draw";
    internal bool HasLiuYanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.UseLimuSelfIndulgenceAndRecover) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawTusheForActualNonEquipmentTargets);

    internal static CardConversionSource LiuYanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_yan_ol_LiuYan.LiuYanSource(f);
    internal static bool LiuYanBasic(CardKind k) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_yan_ol_LiuYan.LiuYanBasic(k);
    internal bool LiuYanNoBasicHand(CharacterState p) => _corePhysicalOwnedCardQueriesOperations.GetHand(p).All(c => !LiuYanBasic(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(p, c)));
    SkillProgramStepOutcome global::CardGame.Core.ILiuYanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginLiuYan(f, op);
    bool global::CardGame.Core.ILiuYanSelectionQuery.CanUse(int owner, Card card, string skill) => LiuYanLimuMaterialLegal(_coreMatchParticipantsPlayersAccess._players[owner], card, skill);
    internal SkillProgramStepOutcome BeginLiuYan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LiuYan is not null || f.InstructionIndex != 1 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Liu Yan requires his exact living, unissued native operation.");
        if (op == SkillProgramEffectOp.DrawTusheForActualNonEquipmentTargets)
        {
            if (f.SkillId != "ol:tushe" || f.TriggerId != "actual-targets" || f.WindowContext is not { } context || LiuYanTusheUse(f.OwnerSeat, context)is not { } use || !LiuYanNoBasicHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
                throw new InvalidOperationException("Tushe requires an actual non-equipment designation and no effective basic hand cards after payment.");
            var r = new LiuYanReceipt(LiuYanSource(f), f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, op, LiuYanStage.TusheDrawChildren)
            {
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                ParentWindowId = context.ParentFrameId,
                OriginalUseId = use.Id,
                ActionId = use.Action!.ActionId,
                Targets = use.Action.EffectiveDesignatedTargetSeats.Distinct().ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = r });
            RecordLiuYanStart(f);
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], r.Targets.Count, true, new(LiuYanDrawReason)).Count;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = f.LiuYan!with { DrawCount = count, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanTusheDrawnEvent(f.Id, f.OwnerSeat, r.Targets.Count, count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueLiuYan(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != "ol:limu" || f.TriggerId is not null || f.ActivationId != "self-indulgence" || f.WindowContext is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Limu requires one real diamond entity in the owner's actual Play.");
        var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(id);
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!LiuYanLimuMaterialLegal(actor, card, f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Limu's original diamond entity or self delayed target is illegal.");
        var origin = new LiuYanLimuUseOrigin(f.Id, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence + 1, LiuYanSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.OwnerSeat, id, card.Kind, _cardZonesOperations._cardZones.GetLocation(id), _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), card.Rank);
        var receipt = new LiuYanReceipt(origin.Source, f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, op, LiuYanStage.LimuNative)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Use = origin,
            OriginalUseId = origin.CardUseFrameId,
            ActionId = origin.ActionId,
            Targets = [f.OwnerSeat],
            Auxiliaries = card.Kind == CardKind.WoodenOx && origin.From == CardLocation.Equipment(f.OwnerSeat) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(actor).Select(c => new LiuYanMaterial(c.Id, c.Kind, CardLocation.WoodenOxGrain(f.OwnerSeat))).ToArray() : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = receipt });
        RecordLiuYanStart(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLimuIssuedEvent(origin));
        BeginLiuYanLimu(f, origin, card);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RecordLiuYanStart(ProgramSkillFrame f)
    {
        var r = f.LiuYan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.Operation, r.ParentWindowId, r.OriginalUseId, r.ActionId, string.Join(',', r.Targets)));
    }

    internal bool DrainLiuYan(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueLiuYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuYan: { } r } f || f.Id != id)
            return;
        if (r.Stage == LiuYanStage.LimuNative)
            return;
        if (DrainLiuYan(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.LiuYan!;
        if (r.Stage == LiuYanStage.LimuReturnedChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = r with { Stage = LiuYanStage.LimuRecoveryChildren, RecoveryIssued = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive } });
            if (f.LiuYan!.RecoveryIssued)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLimuRecoveryIssuedEvent(f.Id, f.OwnerSeat, 1));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                if (DrainLiuYan(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            }
        }

        FinishLiuYan(f);
    }

    internal void FinishLiuYan(ProgramSkillFrame f)
    {
        var r = f.LiuYan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanCompletedEvent(f.Id, r.Operation, r.DrawCount, r.RecoveryIssued));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27230)]
    internal bool ResumeLiuYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuYan: not null } f || f.Id != id)
            return false;
        ContinueLiuYan(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27230)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27230)]
    internal bool ContinuePaidLiuYan(ProgramSkillFrame f) => f.LiuYan is not null && ValidLiuYan(f);
}
