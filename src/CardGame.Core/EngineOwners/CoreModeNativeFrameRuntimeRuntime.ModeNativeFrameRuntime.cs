#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreModeNativeFrameRuntimeRuntime
{
    internal ModeNativeFrameRuntime ModeNativeFrames => _modeRuntimeOperations._modeRuntime.NativeFrames;
}
