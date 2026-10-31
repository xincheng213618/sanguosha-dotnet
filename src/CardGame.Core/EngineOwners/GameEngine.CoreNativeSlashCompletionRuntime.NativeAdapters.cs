#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeSlashCompletionResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSlashCompletionResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeSlashCompletionCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSlashCompletionCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNativeSlashCompletionKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSlashCompletionKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeSlashCompletionKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeSlashCompletionKernelCoreMainOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }
}
