#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed partial class CoreGameEngineQimeiProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineQimeiProgramsRuntime> _resolve;
    private CoreGameEngineQimeiProgramsEntries(Func<EngineOwners.CoreGameEngineQimeiProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQimeiProgramsEntries Create(Func<EngineOwners.CoreGameEngineQimeiProgramsRuntime> resolve) => new(resolve);
}
