#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime))]
internal sealed partial class CoreGameEngineProgramVirtualSlashOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> _resolve;
    private CoreGameEngineProgramVirtualSlashOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramVirtualSlashOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualSlashOffersRuntime> resolve) => new(resolve);
}
