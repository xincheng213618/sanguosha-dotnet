#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeQilinBowKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeQilinBowKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQilinBowHandle(attack);
    }
}
