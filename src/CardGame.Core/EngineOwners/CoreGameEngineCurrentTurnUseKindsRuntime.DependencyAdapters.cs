#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCurrentTurnUseKindsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreCurrentTurnUseKindsCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().HasNullificationEffectOwner(window);
    public long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationParentActionId(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime))]
internal sealed class CoreCurrentTurnUseKindsCoreCurrentRoundActualUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCoreCurrentRoundActualUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> _targetFactory = targetFactory;
    public bool TracksCurrentRoundActualUses { get => this._targetFactory().TracksCurrentRoundActualUses; }

    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CurrentUseKindDeduplicationHistory() => this._targetFactory().CurrentUseKindDeduplicationHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime))]
internal sealed class CoreCurrentTurnUseKindsCharacterCaiYongLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCharacterCaiYongLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYongLeRuntime> _targetFactory = targetFactory;
    public void CaptureFeibaiDeclaredName(long origin, global::CardGame.Core.CardActionContext? action, global::CardGame.Core.CardKind kind, int? instruction) => this._targetFactory().CaptureFeibaiDeclaredName(origin, action, kind, instruction);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreCurrentTurnUseKindsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeCaoZhiWenlanCapability { get => this._targetFactory().HasLeCaoZhiWenlanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreCurrentTurnUseKindsCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a) => this._targetFactory().IsShiLsAutonomousResponse(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreCurrentTurnUseKindsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary) => this._targetFactory().EventsSinceLastBoundary(boundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed class CoreCurrentTurnUseKindsCoreWholeActualTurnUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCoreWholeActualTurnUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _targetFactory = targetFactory;
    public bool TracksWholeActualTurnUseOrdinal { get => this._targetFactory().TracksWholeActualTurnUseOrdinal; }
    public bool HasOngoingWholeActualUseTurn { get => this._targetFactory().HasOngoingWholeActualUseTurn; }

    public global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent[] WholeActualTurnUseHistory() => this._targetFactory().WholeActualTurnUseHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime))]
internal sealed class CoreCurrentTurnUseKindsCoreActualTurnLegacyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentTurnUseKindsCoreActualTurnLegacyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> _targetFactory = targetFactory;
    public int? CapturedNormalizedLegacyActualTurnOrdinal(global::CardGame.Core.CardActionContext action) => this._targetFactory().CapturedNormalizedLegacyActualTurnOrdinal(action);
}
