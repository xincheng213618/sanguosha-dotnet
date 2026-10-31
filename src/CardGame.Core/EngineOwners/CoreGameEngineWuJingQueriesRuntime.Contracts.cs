#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWuJingQueriesCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
    bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit);
}

internal interface ICoreWuJingQueriesCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WuJingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool HasWuJingLiubing { get; }

    global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary);
}

internal interface ICoreWuJingQueriesCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHandCategoryMaterialRestricted(global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreWuJingQueriesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card);
}

internal interface ICoreWuJingQueriesCoreCuiYanNativeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanSlashChoiceForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreWuJingQueriesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreWuJingQueriesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWuJingQueriesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWuJingQueriesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}
