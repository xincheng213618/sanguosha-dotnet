#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime))]
internal sealed partial class CoreGameEngineProgramVirtualSlashOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> _resolve;
    private CoreGameEngineProgramVirtualSlashOffersEntries(Func<EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramVirtualSlashOffersEntries Create(Func<EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> resolve) => new(resolve);
}
