#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreShanheStartingResourcesCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationParentActionId(window);
    public bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().HasNullificationEffectOwner(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreShanheStartingResourcesCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime))]
internal sealed class CoreShanheStartingResourcesCoreShanheBossCancelledMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreShanheBossCancelledMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> _targetFactory = targetFactory;
    public bool TryDescribeShanheBossCancelledMaterial(int cardId, global::CardGame.Core.CardMovementRecord entry, global::CardGame.Core.CardMoveReason reason, out global::CardGame.Core.ShanheBossMaterialCancelledEvent cancelled) => this._targetFactory().TryDescribeShanheBossCancelledMaterial(cardId, entry, reason, out cancelled);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreShanheStartingResourcesCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool IsResponseCompletionProgramDuelParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().IsResponseCompletionProgramDuelParent(frame, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreShanheStartingResourcesCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreShanheStartingResourcesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime))]
internal sealed class CoreShanheStartingResourcesCoreShanheMarketApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheStartingResourcesCoreShanheMarketApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> _targetFactory = targetFactory;
    public bool IsShanheMarketResource(int id) => this._targetFactory().IsShanheMarketResource(id);
}
