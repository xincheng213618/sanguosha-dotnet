#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePublicDamagePredictionSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CorePublicDamagePredictionTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CorePublicDamagePredictionContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePublicDamagePredictionContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
