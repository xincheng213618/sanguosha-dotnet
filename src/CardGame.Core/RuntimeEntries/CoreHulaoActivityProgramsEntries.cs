#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreHulaoActivityProgramsRuntime))]
internal sealed partial class CoreHulaoActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreHulaoActivityProgramsRuntime> _resolve;
    private CoreHulaoActivityProgramsEntries(Func<EngineOwners.CoreHulaoActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreHulaoActivityProgramsEntries Create(Func<EngineOwners.CoreHulaoActivityProgramsRuntime> resolve) => new(resolve);
}
