#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramEquipmentRecastRecoveriesRuntime
{
    internal bool TryPauseProgramEquipmentRecast(ProgramSkillFrame paidFrame, Card card)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(paidFrame.Id);
        if (frame.PendingRecoveryAttempts is not { Count: > 0 })
            return false;
        if (frame.EquipmentRecast is not null)
            throw new InvalidOperationException("A program equipment recast already has a paid receipt.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentRecast = new(frame.InstructionIndex, card.Id, card.Kind) });
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 800)]
    internal bool ResumeProgramEquipmentRecast(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.EquipmentRecast is not { } receipt)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (frame.InstructionIndex != receipt.InstructionIndex || receipt.InstructionIndex < 1 || receipt.InstructionIndex > plan.Instructions.Count || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(receipt.CardKind) || plan.Instructions[receipt.InstructionIndex - 1].Op != SkillProgramEffectOp.RecastSelectedEquipment)
            throw new InvalidOperationException("A paid equipment recast lost its exact owning instruction.");
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        if (!receipt.DrawApplied)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentRecast = receipt with { DrawApplied = true } });
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var drawn = _coreMatchOutcomeWinnerAccess._winner == Winner.None && owner.IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, CardMoveReasons.RecastDraw) : Array.Empty<int>();
            frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentRecast = frame.EquipmentRecast!with { DrawCount = drawn.Count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, receipt.CardId, receipt.CardKind, drawn.Count));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), drawn.Count);
            return ResumeProgramEquipmentRecast(frame.Id);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { EquipmentRecast = null });
        return false;
    }
}
