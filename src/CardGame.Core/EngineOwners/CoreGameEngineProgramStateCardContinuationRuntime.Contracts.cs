#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramStateCardContinuationCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleIncomingRangeDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramStateCardContinuationCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativeCardEffectDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramStateCardContinuationCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWumiaoFloodStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramStateCardContinuationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramStateCardContinuationCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenheCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame state);
}

internal interface ICoreProgramStateCardContinuationCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramStateCardContinuationCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}
