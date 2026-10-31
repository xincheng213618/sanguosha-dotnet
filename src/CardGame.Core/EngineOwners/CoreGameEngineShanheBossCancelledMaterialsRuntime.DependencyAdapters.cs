#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreShanheBossCancelledMaterialsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheBossCancelledMaterialsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public bool IsShanheStartingResource(int id) => this._targetFactory().IsShanheStartingResource(id);
    public bool ShanheStartingResourceWasAcquired(int id, int before = 2147483647) => this._targetFactory().ShanheStartingResourceWasAcquired(id, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreShanheBossCancelledMaterialsCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheBossCancelledMaterialsCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationParentActionId(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreShanheBossCancelledMaterialsCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheBossCancelledMaterialsCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool IsResponseCompletionProgramDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().IsResponseCompletionProgramDuelParent(frame, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreShanheBossCancelledMaterialsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheBossCancelledMaterialsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
