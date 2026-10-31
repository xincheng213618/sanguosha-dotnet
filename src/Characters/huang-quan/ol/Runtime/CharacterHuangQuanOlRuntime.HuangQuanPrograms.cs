#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanOlRuntime
{
    internal CardConversionSource HuangQuanSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome IssueHuangQuanDraw(ProgramSkillFrame f, int target, string kind, DianhuOccurrence? occurrence)
    {
        if (f.HuangQuan is not null)
            throw new InvalidOperationException("Huang Quan cannot repay an issued native draw.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var pending = new HuangQuanPending(HuangQuanSource(f), f.GameplayHash, f.InstructionIndex, kind, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, before, before, Array.AsReadOnly(Array.Empty<int>()), occurrence, "drawing");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = pending, PendingMovementContinuation = new(target, 0, null) });
        if (f.SkillId == "dragonboat:jianji")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 1, new("program.jianji.draw"));
        }

        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, target, 1, null, null, SkillProgramCardSetVisibility.Private, new("program." + kind + ".draw"));
        var ids = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Reason.Value == "program." + kind + ".draw" && m.To == CardLocation.Hand(target)).Select(m => m.CardId).ToArray();
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = f.HuangQuan!with { After = after, CardIds = Array.AsReadOnly(ids), Stage = "draw-paid" } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuangQuanDrawPaidEvent(f.Id, pending.Source, f.GameplayHash, kind, target, pending.ActualTurn, pending.TurnOwner, pending.ActualPhase, before, after, ids.Length, occurrence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24000)]
    internal bool ResumeHuangQuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangQuan: { } p } f || f.Id != id)
            return false;
        RequireIssuedHuangQuan(f);
        if (p.Stage == "draw-paid")
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            p = f.HuangQuan!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (p.Kind == "dianhu" || !_coreMatchParticipantsPlayersAccess._players[p.RecipientSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || p.CardIds.Count != 1)
            {
                FinishHuangQuan(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = p with { Stage = "offer" } });
            PublishJianjiChoice(f);
            return true;
        }

        if (p.Stage == "offer")
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishJianjiChoice(f);
            return true;
        }

        if (p.Stage == "returned")
        {
            FinishHuangQuan(f);
            return true;
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24000)]
    internal bool ReturnHuangQuanMovement(ProgramSkillFrame f) => f.HuangQuan is not null && ResumeHuangQuan(f.Id);
    internal void FinishHuangQuan(ProgramSkillFrame f)
    {
        if (_coreGuanduGeneralNativeCardsOperations.FinishGuanduYuanlveUse(f))
            return;
        var p = f.HuangQuan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuangQuanCompletedEvent(f.Id, p.Source, p.Kind, p.NativeUse?.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IHuangQuanProgramHost.ArmDianhuRelation(ProgramSkillFrame f, int target) => ArmDianhuRelation(f, target);
    SkillProgramStepOutcome global::CardGame.Core.IHuangQuanProgramHost.DrawForDianhuActualOccurrence(ProgramSkillFrame f) => DrawForDianhuActualOccurrence(f);
    SkillProgramStepOutcome global::CardGame.Core.IHuangQuanProgramHost.DrawAndOfferJianjiExactCard(ProgramSkillFrame f, int target) => DrawAndOfferJianjiExactCard(f, target);
}
