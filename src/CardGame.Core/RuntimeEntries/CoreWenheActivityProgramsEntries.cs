#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreWenheActivityProgramsRuntime))]
internal sealed partial class CoreWenheActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreWenheActivityProgramsRuntime> _resolve;
    private CoreWenheActivityProgramsEntries(Func<EngineOwners.CoreWenheActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreWenheActivityProgramsEntries Create(Func<EngineOwners.CoreWenheActivityProgramsRuntime> resolve) => new(resolve);
}
