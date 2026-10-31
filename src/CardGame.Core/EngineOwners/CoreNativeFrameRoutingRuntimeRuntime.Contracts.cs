#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeFrameRoutingRuntimeCoreModeNativeFrameRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int StepCount(global::CardGame.Core.NativeFrameAutomaticPhase phase);
    global::CardGame.Core.NativeFrameAutomaticStepMetadata StepAt(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index);
    bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame);
    bool TryAdvance(global::CardGame.Core.ResolutionFrame frame);
}
