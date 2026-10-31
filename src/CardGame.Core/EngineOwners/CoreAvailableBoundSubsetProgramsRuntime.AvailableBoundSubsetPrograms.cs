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

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 290, group: 10)]
    internal bool IsAvailableBoundDamageProgramDying()
    {
        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } trigger || _coreDyingCursorRuntimeOperations.ActiveDying is not { ResumesProgramSkill: true } dying || GetAvailableBoundDamageObserverRoot(trigger.Id)is not { } root || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(frame => frame.Id == dying.FrameId)is not { } dyingFrame || dyingFrame.ParentFrameId != dying.ParentFrameId)
            return false;
        var parentIndex = -1;
        for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame parent && parent.Id == dying.ParentFrameId)
                parentIndex = index;
        if (parentIndex < 0 || parentIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[parentIndex + 1].Id != dyingFrame.Id)
            return false;
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (!(top is DyingFrame topDying && topDying.Id == dyingFrame.Id || top is ProgramSkillFrame { WindowContext: { } response } && response.ParentFrameId == dyingFrame.Id && response.Window is SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse || IsAvailableBoundPeachRescueRide(parentIndex + 1, dyingFrame)))
            return false;
        while (parentIndex >= 1 && (_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1]) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[parentIndex], _resolutionStackOperations._resolutionStack[parentIndex - 1])))
            parentIndex--;
        return _resolutionStackOperations._resolutionStack[parentIndex].Id == root.Id;
    }

    internal bool IsAvailableBoundPeachRescueRide(int dyingIndex, DyingFrame dying)
    {
        var useIndex = dyingIndex + 1;
        if (useIndex >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[useIndex] is not CardUseFrame rescue || rescue.DyingResponse is not { UsedPeach: true, UsedAlcohol: false } response || response.ResolutionId != dying.Id || response.ResponderSeat != dying.ResponderSeat || response.PeachCardId != rescue.CardId || response.AlcoholCardId is not null || rescue.SourceSeat != response.ResponderSeat || rescue.CardKind != CardKind.Peach || !(rescue.PhysicalCardIds ?? [rescue.CardId]).SequenceEqual([rescue.CardId]) || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Peach } action || action.ActorSeat != rescue.SourceSeat || action.ProviderSeat != rescue.SourceSeat || !action.EffectiveDesignatedTargetSeats.SequenceEqual(rescue.TargetSeats) || action.PhysicalCards.Count != 1 || action.PhysicalCards[0].CardId != rescue.CardId || action.PhysicalCards[0].From != CardLocation.Hand(rescue.SourceSeat) || !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(rescue.CardId)).Any(card => card.Id == rescue.CardId && card.Kind == action.PhysicalCards[0].CardKind))
            return false;
        var physicalKind = action.PhysicalCards[0].CardKind;
        if (physicalKind == CardKind.Peach)
        {
            if (response.UsedPeachPhysicalCardKind is not null)
                return false;
        }
        else if (response.UsedPeachPhysicalCardKind != physicalKind || action.ConversionChain.Count != 1 || action.ConversionChain[0].OwnerSeat != rescue.SourceSeat || string.IsNullOrWhiteSpace(action.ConversionChain[0].SkillId) || string.IsNullOrWhiteSpace(action.ConversionChain[0].BindingId) || string.IsNullOrWhiteSpace(action.ConversionChain[0].SkillInstanceId))
            return false;
        var index = _resolutionStackOperations._resolutionStack.Count - 1;
        while (index > useIndex && (_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1])))
            index--;
        return index == useIndex;
    }
}
