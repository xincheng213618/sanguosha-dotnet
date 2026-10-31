#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreModePresentationCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreModePresentationCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime))]
internal sealed class CoreModePresentationCoreMatchPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreModePresentationCoreMatchPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _targetFactory = targetFactory;
    public string GetRoleName(global::CardGame.Core.Role role) => this._targetFactory().GetRoleName(role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime))]
internal sealed class CoreModePresentationCoreMatchParticipantSetupAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreModePresentationCoreMatchParticipantSetupOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Role GetTeamRole(string teamId) => this._targetFactory().GetTeamRole(teamId);
}
