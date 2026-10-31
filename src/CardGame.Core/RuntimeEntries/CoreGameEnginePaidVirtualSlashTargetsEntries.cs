#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime))]
internal sealed partial class CoreGameEnginePaidVirtualSlashTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> _resolve;
    private CoreGameEnginePaidVirtualSlashTargetsEntries(Func<EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidVirtualSlashTargetsEntries Create(Func<EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> resolve) => new(resolve);
}
