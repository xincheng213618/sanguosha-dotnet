#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCurrentTurnUseKindsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCurrentTurnUseKindsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreCurrentTurnUseKindsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCurrentTurnUseKindsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCurrentTurnUseKindsCoreCurrentRoundActualUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksCurrentRoundActualUses { get; }

    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CurrentUseKindDeduplicationHistory();
}

internal interface ICoreCurrentTurnUseKindsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFeibaiDeclaredName(long origin, global::CardGame.Core.CardActionContext? action, global::CardGame.Core.CardKind kind, int? instruction);
}

internal interface ICoreCurrentTurnUseKindsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCurrentTurnUseKindsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeCaoZhiWenlanCapability { get; }
}

internal interface ICoreCurrentTurnUseKindsCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreCurrentTurnUseKindsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary);
}

internal interface ICoreCurrentTurnUseKindsCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksWholeActualTurnUseOrdinal { get; }

    bool HasOngoingWholeActualUseTurn { get; }

    global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent[] WholeActualTurnUseHistory();
}

internal interface ICoreCurrentTurnUseKindsCoreActualTurnLegacyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CapturedNormalizedLegacyActualTurnOrdinal(global::CardGame.Core.CardActionContext action);
}
