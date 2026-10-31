#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramRandomSeatAcquisitionRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> _resolve;
    private CoreProgramRandomSeatAcquisitionRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramRandomSeatAcquisitionRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> resolve) => new(resolve);
}
