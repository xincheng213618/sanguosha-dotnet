#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeFrameAutomaticRuntimeKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameAutomaticRuntimeKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CoreNativeFrameAutomaticRuntimeNativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameAutomaticRuntimeNativeSystemFuncCardGameCoreGameEngineCardGameCoreResolutionFrameBoolOperations
    {
        public bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Matches(engine, frame);
    }
}
