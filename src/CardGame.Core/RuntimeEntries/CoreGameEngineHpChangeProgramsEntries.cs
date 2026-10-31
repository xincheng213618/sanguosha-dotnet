#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed partial class CoreGameEngineHpChangeProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _resolve;
    private CoreGameEngineHpChangeProgramsEntries(Func<EngineOwners.CoreGameEngineHpChangeProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpChangeProgramsEntries Create(Func<EngineOwners.CoreGameEngineHpChangeProgramsRuntime> resolve) => new(resolve);
}
