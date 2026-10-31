#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal CardUseFrame? NingwuEligibleUse(int holder, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != identity.CardActionId || action.ActorSeat == holder || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || use.SourceSeat != action.ActorSeat || action.EffectiveDesignatedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || ZhouQunNeighbors(action.ActorSeat).Contains(target))
            return null;
        return use;
    }
}
