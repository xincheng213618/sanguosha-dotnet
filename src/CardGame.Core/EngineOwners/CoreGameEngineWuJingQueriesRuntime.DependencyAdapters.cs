#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreWuJingQueriesCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsTurnPhysicalUseForbidden(seat, ids);
    public bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit) => this._targetFactory().IsTurnSuitUseForbidden(seat, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreWuJingQueriesCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public bool WuJingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().WuJingFirstChild(f, child);
    public bool HasWuJingLiubing { get => this._targetFactory().HasWuJingLiubing; }

    public global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary) => this._targetFactory().WuJingLiubingUsedSuit(actorSeat, card, effectiveKind, ids, conversions, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreWuJingQueriesCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsHandCategoryMaterialRestricted(global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsHandCategoryMaterialRestricted(ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreWuJingQueriesCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card) => this._targetFactory().CaptureUsedCardSuit(actorSeat, physicalIds, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime))]
internal sealed class CoreWuJingQueriesCoreCuiYanNativeQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreCuiYanNativeQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> _targetFactory = targetFactory;
    public bool IsCuiYanSlashChoiceForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice) => this._targetFactory().IsCuiYanSlashChoiceForbidden(actorSeat, choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CoreWuJingQueriesCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreWuJingQueriesCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreWuJingQueriesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWuJingQueriesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}
