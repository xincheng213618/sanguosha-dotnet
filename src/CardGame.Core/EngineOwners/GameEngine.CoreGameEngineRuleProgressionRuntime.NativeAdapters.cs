#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreRuleProgressionProgramRuleObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuleProgressionProgramRuleObserversOperations
    {
        public ref readonly global::CardGame.Core.ProgramRuleObservers _programRuleObservers => ref engine._programRuleObservers;
    }

    [ProgramCapability]
    private sealed class CoreRuleProgressionKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreRuleProgressionKernelCoreGameEngineRuleProgressionOperations
    {
        public global::CardGame.Core.NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations() => engine.ComposeNativeRuleProgressionOperations();
    }
}
