#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    internal bool HasWeifuTargetTail(CardUseFrame use) =>
        use.WeifuUse is { Offered: true, AddedTargets.Count: > 0 };

    internal bool TryGetWeifuVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> primary)
    {
        primary = [];
        if (((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterLiuPanOlOperations.HasPijingTargetTail(use)) || HasWeifuTargetTail(use)) && _characterYangFuOlOperations.TryGetSuyiVirtualPrimaryReturn(use, parent, out primary))
            return true;
        if (!(HasWeifuTargetTail(use) || (_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != CardKind.Slash || use.CardAttack is not { CardId: null, EffectiveCardKind: CardKind.Slash, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id || attack.SourceSeat != use.SourceSeat || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action || action.ActorSeat != use.SourceSeat || action.ProviderSeat != action.ActorSeat || action.PhysicalCards.Count != 0 || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || parent.InstructionIndex < 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != parent.Id || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } p || p.GameplayHash != parent.GameplayHash)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)is not { TargetSeats.Count: 1 } confirmed || history.OfType<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)is not { CardId: 0, CardKind: CardKind.Slash } declared || declared.SourceSeat != action.ActorSeat || history.OfType<CardActionAcceptedEvent>().Select(e => e.Action).FirstOrDefault(a => a.ActionId == action.ActionId)is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } accepted || accepted.ActorSeat != action.ActorSeat || accepted.ProviderSeat != action.ProviderSeat || !accepted.TargetSeats.SequenceEqual(confirmed.TargetSeats) || !accepted.ConversionChain.SequenceEqual(action.ConversionChain))
            return false;
        var original = confirmed.TargetSeats[0];
        var op = ProgramInstructionResolver.Default.Resolve(parent, p).GetPausedInstruction(parent.InstructionIndex).Effect;
        var exact = op.Op switch
        {
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats.SequenceEqual([action.ActorSeat]) && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == action.ActorSeat && action.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) && (op.TargetReference is null ? parent.SelectedTargetSeats.SequenceEqual([original]) : op.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == original),
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == action.ActorSeat && parent.SelectedTargetSeats.SequenceEqual([original]) && action.ConversionChain.Count == 0 && op.UseCardActionWindows && op.OutputKind == CardKind.Slash && op.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget && (parent.TriggerId is not null || op.Op == SkillProgramEffectOp.OfferUnlimitedVirtualSlash),
            _ => false
        };
        if (!exact)
            return false;
        primary = Array.AsReadOnly(new[] { _characterLiuPanOlOperations.HasPijingTargetTail(use) ? original : use.TargetSeats[0] });
        return true;
    }
}
