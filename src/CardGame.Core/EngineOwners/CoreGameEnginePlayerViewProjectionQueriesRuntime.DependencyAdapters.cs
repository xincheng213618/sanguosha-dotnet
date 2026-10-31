#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CorePlayerViewProjectionQueriesCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.ActionQueryScope BeginActionQuery() => this._targetFactory().BeginActionQuery();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().GetProgramCardIdentityMatches(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.PlayerViewProjector))]
internal sealed class CorePlayerViewProjectionQueriesPlayerViewProjectorAdapter(global::System.Func<global::CardGame.Core.PlayerViewProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.PlayerViewProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerViewProjector Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CorePlayerViewProjectionQueriesCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewProjectionQueriesCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().JuewuForeignHandRank(owner, card);
}
