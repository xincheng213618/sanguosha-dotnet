#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouEnAnniversaryRuntime
{
    internal bool ValidXiahouEnShiwuUse(CardUseFrame use, ProgramSelectedActorDuelOrigin origin)
    {
        if (origin.XiahouEnShiwu is not { } issued || use.SelectedActorDuelOrigin != origin || use.CardId != 0 || use.CardKind != CardKind.Duel || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: false } action || use.Id != issued.UseId || action.ActionId != issued.ActionId || action.ProviderSeat != issued.Source.OwnerSeat || origin.OrderedPairPayment is not null || origin.ParticipantMode is not null || origin.MouDiaoChanRing is not null || origin.ParentProgramFrameId != issued.FrameId || origin.InstructionIndex != issued.InstructionIndex || origin.OwnerSeat != issued.Source.OwnerSeat || origin.InitialActorSeat != issued.Source.OwnerSeat || origin.InitialTargetSeat != issued.TurnOwner || origin.SkillId != issued.Source.SkillId || origin.ActivationId != issued.Source.BindingId || origin.SkillInstanceId != issued.Source.SkillInstanceId || origin.GameplayHash != issued.Hash || origin.TurnNumber != issued.ActualTurn || issued.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || issued.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouEnShiwuIssuedEvent>().Count(e => e.Origin == issued) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == use.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame { AnniversaryXiahouEn: { } receipt } parent && parent.Id == issued.FrameId && parent.InstructionIndex == issued.InstructionIndex && parent.OwnerSeat == issued.Source.OwnerSeat && parent.SkillId == issued.Source.SkillId && parent.SkillInstanceId == issued.Source.SkillInstanceId && parent.GameplayHash == issued.Hash && receipt.Operation == SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwu && receipt.Stage == AnniversaryXiahouEnStage.NativeUse && receipt.Origin == issued && receipt.Source == issued.Source && receipt.Hash == issued.Hash && receipt.ParentId == issued.ParentWindowId && _coreForeignTurnContestsOperations.Value.IsForeignActualTurnPindian(parent);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 45000)]
    internal bool ReturnXiahouEnShiwuAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXiahouEn: { Operation: SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwu, Stage: AnniversaryXiahouEnStage.NativeUse, Origin: { } origin } } f || completion.ResolutionId != origin.UseId)
            return false;
        if (completion.ProgramFrameId != f.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouEnShiwuIssuedEvent>().Count(e => e.Origin == origin) != 1)
            throw new InvalidOperationException("Shiwu must return its completely cleaned-up original Duel exactly once.");
        ContinueAnniversaryXiahouEn(f.Id);
        return true;
    }
}
