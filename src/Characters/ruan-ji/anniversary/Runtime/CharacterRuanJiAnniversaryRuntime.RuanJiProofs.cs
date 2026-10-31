#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanJiAnniversaryRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42800)]
    internal bool ContinuePaidRuanJiAfterSkillLoss(ProgramSkillFrame f)
    {
        if (f.RuanJi is null)
            return false;
        return true;
    }

    [NativeUseValidation(42800)]
    internal void ValidateRuanJiWine(CardUseFrame use)
    {
        if (use.VirtualBasicReturn is not { } returned || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == returned.ParentFrameId)is not { RuanJi: { Stage: RuanJiStage.WineIssued } r } parent)
            return;
        if (use.Id != r.WineId || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || action.ActionId != r.WineActionId || action.ActorSeat != parent.OwnerSeat || action.ProviderSeat != parent.OwnerSeat || action.ParentActionId != r.ActionId || !action.ConversionChain.SequenceEqual([r.Source]) || use.CardId != 0 || use.CardKind != CardKind.Alcohol || use.PhysicalCardIds is not { Count: 0 } || !use.TargetSeats.SequenceEqual([parent.OwnerSeat]) || returned.InstructionIndex != r.InstructionIndex || use.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("Jiudun cannot replace the exact native Alcohol Use or its original typed parent return.");
    }
}
