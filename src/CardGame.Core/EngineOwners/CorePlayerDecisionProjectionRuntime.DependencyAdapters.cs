#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed class CorePlayerDecisionProjectionCoreProgramParticipantPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerDecisionProjectionCoreProgramParticipantPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => this._targetFactory().GetPendingDecisionForActor(actorSeat);
}
