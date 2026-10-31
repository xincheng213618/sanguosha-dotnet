#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed class CoreShanheSourceWeizhuCoreTeamActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheSourceWeizhuCoreTeamActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _targetFactory = targetFactory;
    public bool OtherRevealedPublicTeamAlly(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target) => this._targetFactory().OtherRevealedPublicTeamAlly(owner, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreShanheSourceWeizhuCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheSourceWeizhuCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableOwnedHand(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetSelfDiscardableOwnedHand(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreShanheSourceWeizhuCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheSourceWeizhuCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}
