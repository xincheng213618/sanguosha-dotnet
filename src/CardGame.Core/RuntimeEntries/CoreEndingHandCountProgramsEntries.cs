#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreEndingHandCountProgramsEntries
{
    private readonly Func<EngineOwners.CoreEndingHandCountProgramsRuntime> _resolve;
    private CoreEndingHandCountProgramsEntries(Func<EngineOwners.CoreEndingHandCountProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreEndingHandCountProgramsEntries Create(Func<EngineOwners.CoreEndingHandCountProgramsRuntime> resolve) => new(resolve);
}
