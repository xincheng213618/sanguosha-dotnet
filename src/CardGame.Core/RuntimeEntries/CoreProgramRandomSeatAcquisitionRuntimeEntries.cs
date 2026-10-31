#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramRandomSeatAcquisitionRuntimeEntries
{
    private readonly Func<EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> _resolve;
    private CoreProgramRandomSeatAcquisitionRuntimeEntries(Func<EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramRandomSeatAcquisitionRuntimeEntries Create(Func<EngineOwners.CoreProgramRandomSeatAcquisitionRuntimeRuntime> resolve) => new(resolve);
}
