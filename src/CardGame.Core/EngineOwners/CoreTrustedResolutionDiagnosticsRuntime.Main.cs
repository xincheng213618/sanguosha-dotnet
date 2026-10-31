#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTrustedResolutionDiagnosticsRuntime
{
    internal IReadOnlyList<ResolutionFrame> ResolutionStack => _resolutionStackOperations._resolutionStack.AsReadOnly();
}
