#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreShanheS5XuchuAiCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5XuchuAiCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreShanheS5XuchuAiCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5XuchuAiCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreShanheS5XuchuAiCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5XuchuAiCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public int GetTurnDrawCount(global::CardGame.Core.CharacterState current) => this._targetFactory().GetTurnDrawCount(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime))]
internal sealed class CoreShanheS5XuchuAiCoreShanheS5XuchuAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5XuchuAiCoreShanheS5XuchuOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleTriggerWindowFrame ShanheS5LuoyiDrawPlan(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ShanheS5LuoyiDrawPlan(f);
    public global::CardGame.Core.ShanheS5LuoyiFrozenEvent ShanheS5LuoyiFrozen(global::CardGame.Core.ProgramSkillFrame f, string bind) => this._targetFactory().ShanheS5LuoyiFrozen(f, bind);
}
