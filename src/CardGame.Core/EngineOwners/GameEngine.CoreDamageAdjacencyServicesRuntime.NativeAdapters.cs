#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDamageAdjacencyServicesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDamageAdjacencyServicesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDamageAdjacencyServicesKernelCoreDamageAdjacencyServicesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageAdjacencyServicesKernelCoreDamageAdjacencyServicesOperations
    {
        public global::CardGame.Core.NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack) => engine.ComposeNativeDamageTriggerCapturedAttack(attack);
    }
}
