#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed partial class CoreGameEngineSlashSuitDiscardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _resolve;
    private CoreGameEngineSlashSuitDiscardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashSuitDiscardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve) => new(resolve);
}
