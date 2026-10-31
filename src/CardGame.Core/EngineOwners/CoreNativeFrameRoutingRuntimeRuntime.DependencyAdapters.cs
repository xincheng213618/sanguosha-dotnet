#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreNativeFrameRoutingRuntimeCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeFrameRoutingRuntimeCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramSkillInvocation(long id) => this._targetFactory().TryBeginProgramSkillInvocation(id);
}

[global::CardGame.Core.ProgramCapability]
internal sealed class CoreNativeFrameRoutingRuntimeCoreModeNativeFrameRuntimeAdapter(global::CardGame.Core.GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeFrameRoutingRuntimeCoreModeNativeFrameRuntimeOperations
{
    public int StepCount(global::CardGame.Core.NativeFrameAutomaticPhase phase) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase].Count;
    public global::CardGame.Core.NativeFrameAutomaticStepMetadata StepAt(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Step;
    public bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Matches(engine, frame);
    public bool TryAdvance(global::CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.TryAdvance(engine, frame);
}
