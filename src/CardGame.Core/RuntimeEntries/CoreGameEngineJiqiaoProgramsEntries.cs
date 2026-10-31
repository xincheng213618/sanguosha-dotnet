#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJiqiaoProgramsRuntime))]
internal sealed partial class CoreGameEngineJiqiaoProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> _resolve;
    private CoreGameEngineJiqiaoProgramsEntries(Func<EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJiqiaoProgramsEntries Create(Func<EngineOwners.CoreGameEngineJiqiaoProgramsRuntime> resolve) => new(resolve);
}
