#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTrustedResolutionDiagnosticsRuntime
{
    internal IReadOnlyList<ResolutionFrame> ResolutionStack => _resolutionStackOperations._resolutionStack.AsReadOnly();
}
