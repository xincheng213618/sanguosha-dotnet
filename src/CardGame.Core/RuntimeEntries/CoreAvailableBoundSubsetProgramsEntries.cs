#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreAvailableBoundSubsetProgramsRuntime))]
internal sealed partial class CoreAvailableBoundSubsetProgramsEntries
{
    private readonly Func<EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> _resolve;
    private CoreAvailableBoundSubsetProgramsEntries(Func<EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAvailableBoundSubsetProgramsEntries Create(Func<EngineOwners.CoreAvailableBoundSubsetProgramsRuntime> resolve) => new(resolve);
}
