#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePhaseHandExtraTurnAiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseHandExtraTurnAiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CorePhaseHandExtraTurnAiSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePhaseHandExtraTurnAiSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }
}
