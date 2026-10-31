#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTriggerWindowsKernelCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost();
}

internal interface ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseBoundaryRuntime Get();
}

internal interface ICoreProgramTriggerWindowsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramTriggerWindowsKernelCoreGameEngineProgramTriggerWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
    global::CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
    global::CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface ICoreProgramTriggerWindowsProgramLifecycleWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleWindowRuntime Get();
}

internal interface ICoreProgramTriggerWindowsKernelCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost();
}
