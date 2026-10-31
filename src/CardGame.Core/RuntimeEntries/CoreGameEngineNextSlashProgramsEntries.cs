#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNextSlashProgramsRuntime))]
internal sealed partial class CoreGameEngineNextSlashProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNextSlashProgramsRuntime> _resolve;
    private CoreGameEngineNextSlashProgramsEntries(Func<EngineOwners.CoreGameEngineNextSlashProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextSlashProgramsEntries Create(Func<EngineOwners.CoreGameEngineNextSlashProgramsRuntime> resolve) => new(resolve);
}
