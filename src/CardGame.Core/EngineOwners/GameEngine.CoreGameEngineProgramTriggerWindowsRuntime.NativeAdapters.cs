#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramTriggerWindowsKernelCharacterLuKangWumiaoAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsKernelCharacterLuKangWumiaoOperations
    {
        public global::CardGame.Core.NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost() => engine.ComposeNativeWuLuKangWindowHost();
    }

    [ProgramCapability]
    private sealed class CoreProgramTriggerWindowsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramTriggerWindowsKernelCoreGameEngineProgramTriggerWindowsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsKernelCoreGameEngineProgramTriggerWindowsOperations
    {
        public global::CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost() => engine.ComposeNativeCardsMovedTriggerWindowHost();
        public global::CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
        public global::CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class CoreProgramTriggerWindowsKernelCharacterPangJiOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTriggerWindowsKernelCharacterPangJiOlOperations
    {
        public global::CardGame.Core.NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost() => engine.ComposeNativeDouyuTriggerWindowHost();
    }
}
