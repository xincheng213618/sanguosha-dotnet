#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActiveNativeAttackQueriesKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CoreActiveNativeAttackQueriesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActiveNativeAttackQueriesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
