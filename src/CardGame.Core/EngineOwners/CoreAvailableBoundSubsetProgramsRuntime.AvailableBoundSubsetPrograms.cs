#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAvailableBoundSubsetProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IAvailableBoundSubsetProgramEffectHost.SelectAvailableBoundSubset(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreProgramLifecycleOperations.SelectProgramCardSubset(frame.Id, frame.OwnerSeat, effect.SourceBind!, effect.ResultBind!, effect.MinimumCards, effect.MaximumCards, effect.MaximumRankSum, effect.AiOrder!.Value, effect.AllowFewerWhenInsufficient, effect.OnePerSuit, availableAtSourceOnly: true);
    // The executor commits its cursor before Draw suspends. Only that actual
    // paused producer, its frozen own-hand receipt, the immediate opt-in consumer
    // and its exact movement continuation enable existing typed observer rides.
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 20)]
    internal bool HasAvailableBoundDamageObserver(long damageWindowId) => GetAvailableBoundDamageObserverRoot(damageWindowId)is not null;
    internal ProgramSkillFrame? GetAvailableBoundDamageObserverRoot(long damageWindowId)
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame frame || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.ParentFrameId != damageWindowId || frame.InstructionIndex < 1 || _resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame movement || movement.Batch.ParentFrameId != frame.Id || movement.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != frame.Id || movement.Batch.OriginOwnerSeat != frame.OwnerSeat || movement.Batch.OriginSkillId != frame.SkillId || movement.Batch.OriginSkillInstanceId != frame.SkillInstanceId)
                continue;
            var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
            if (frame.InstructionIndex >= plan.Instructions.Count)
                continue;
            var producer = plan.GetPausedInstruction(frame.InstructionIndex).Effect;
            var consumer = plan.GetInstruction(frame.InstructionIndex).Effect;
            if (producer.Op != SkillProgramEffectOp.Draw || producer.Target != SkillProgramEffectTarget.Owner || producer.ResultBind is null || consumer.Op != SkillProgramEffectOp.SelectCardSubset || consumer.AvailableAtSourceOnly != true || consumer.SourceBind != producer.ResultBind)
                continue;
            var receipt = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == producer.ResultBind);
            if (receipt is null || receipt.CardIds.Count == 0 || receipt.CardIds.Count != receipt.SourceLocations.Count || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count || receipt.SourceLocations.Any(location => location != CardLocation.Hand(frame.OwnerSeat)))
                continue;
            // Draw returns the complete immutable receipt once, while each real
            // entity may have its own atomic movement batch. Validate every
            // intersecting movement in this exact producer batch, not current
            // locations (which a lawful child or death cleanup may have changed).
            var receiptMovements = movement.Batch.Movements.Where(move => receipt.CardIds.Contains(move.CardId)).ToArray();
            if (receiptMovements.Length == 0 || receiptMovements.Select(move => move.CardId).Distinct().Count() != receiptMovements.Length || receiptMovements.Any(move => move.From != CardLocation.DrawPile || move.To != CardLocation.Hand(frame.OwnerSeat) || move.Reason.Value != $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.Draw)}"))
                continue;
            return frame;
        }

        return null;
    }
}
