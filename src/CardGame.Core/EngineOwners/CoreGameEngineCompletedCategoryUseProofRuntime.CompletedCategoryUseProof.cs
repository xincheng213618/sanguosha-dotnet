#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofRuntime
{
    // Acceptance and subsequent mutations are owned by the native execution
    // boundaries. A completion consumes that current action, not a replay of
    // character-specific witnesses for how it reached this state.
    internal bool HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext completed)
    {
        if (use.Action is not { Type: CardActionType.Use } current || completed.Type != CardActionType.Use ||
            current.ActionId != completed.ActionId || current.ParentActionId != completed.ParentActionId ||
            current.ActorSeat != completed.ActorSeat || current.ProviderSeat != completed.ProviderSeat ||
            current.RequesterSeat != completed.RequesterSeat || current.ResponderSeat != completed.ResponderSeat ||
            current.OpponentSeat != completed.OpponentSeat || current.EffectiveKind != completed.EffectiveKind ||
            current.EffectiveSuit != completed.EffectiveSuit || current.EffectiveRank != completed.EffectiveRank ||
            !current.TargetSeats.SequenceEqual(completed.TargetSeats) ||
            !current.PhysicalCards.SequenceEqual(completed.PhysicalCards) ||
            !current.ConversionChain.SequenceEqual(completed.ConversionChain))
            return false;
        return true;
    }
    internal bool ActualHandGainMutationBinding(long frameId, string skill, int owner, SkillProgramTriggerWindow window, SkillProgramEffectOp operation)
    {
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Where(e => e.FrameId == frameId && e.SkillId == skill && e.OwnerSeat == owner && e.Window == window).ToArray();
        return started is [var fact] && _contentRegistryOperations._contentRegistry.GetSkill(skill).Program is { } program && ProgramInstructionResolver.Default.FindTrigger(program, fact.BindingId)is { } trigger && ProgramInstructionResolver.Default.Features(trigger).HasOperation(operation);
    }

}
