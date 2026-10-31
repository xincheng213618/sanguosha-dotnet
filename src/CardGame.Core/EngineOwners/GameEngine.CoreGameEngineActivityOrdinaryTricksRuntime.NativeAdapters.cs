#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActivityOrdinaryTricksContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActivityOrdinaryTricksContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreActivityOrdinaryTricksModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActivityOrdinaryTricksModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }
}
