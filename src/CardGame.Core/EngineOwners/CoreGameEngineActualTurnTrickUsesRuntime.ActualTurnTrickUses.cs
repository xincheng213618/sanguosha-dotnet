#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnTrickUsesRuntime
{
    internal bool TracksActualTurnTrickUses => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerValue(SkillProgramTriggerValueKind.OwnerTrickUsesThisActualTurn);

    // Use the accepted action, not a play-phase counter or material movement.
    // Counterspells are uses even though their accepted action type is Response.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 280)]
    internal void ObserveActualTurnTrickUse(IGameEvent payload)
    {
        if (!TracksActualTurnTrickUses)
            return;
        CardActionContext? action = payload switch
        {
            CardUseDeclaredEvent declared => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == declared.ResolutionId)?.Action,
            CardActionAcceptedEvent accepted when accepted.Action.Type == CardActionType.Response && accepted.Action.EffectiveKind == CardKind.Nullification && (accepted.Action.ActorSeat == accepted.Action.ProviderSeat || _characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(accepted.Action)) && accepted.Action.ResponderSeat == accepted.Action.ActorSeat && accepted.Action.RequesterSeat is null && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().Any(window => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == window.ParentFrameId)?.Action?.ActionId == accepted.Action.ParentActionId) => accepted.Action,
            _ => null
        };
        if (action is null || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Trick || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnTrickUseRecordedEvent>().Any(fact => fact.ActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnTrickUseRecordedEvent(_coreActualTurnProgressTurnNumberAccess._turnNumber, action.ActorSeat, action.ActionId, action.EffectiveKind));
    }

    internal int ActualTurnTrickUseCount(int ownerSeat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnTrickUseRecordedEvent>().Count(fact => fact.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && fact.ActorSeat == ownerSeat);
}
