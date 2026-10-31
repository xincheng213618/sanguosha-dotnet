#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextSlashProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime> _resolve;
    private CoreGameEngineNextSlashProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNextSlashProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextSlashProgramsRuntime> resolve) => new(resolve);
}
