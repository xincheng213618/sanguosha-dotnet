#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActivityBattleRulesKernelCoreGameEngineActivityBattleRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActivityBattleRulesKernelCoreGameEngineActivityBattleRulesOperations
    {
        public global::CardGame.Core.NativeOwners.ActivityBattleRulesHost CreateActivityBattleRulesHost() => engine.ComposeNativeActivityBattleRulesHost();
        public global::CardGame.Core.NativeOwners.WenheRuleFlowHost CreateWenheRuleFlowHost() => engine.ComposeNativeWenheRuleFlowHost();
        public global::CardGame.Core.NativeOwners.GuanduEventFlowHost CreateGuanduEventFlowHost() => engine.ComposeNativeGuanduEventFlowHost();
    }

    [ProgramCapability]
    private sealed class CoreActivityBattleRulesModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActivityBattleRulesModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }
}
