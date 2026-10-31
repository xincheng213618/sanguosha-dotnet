#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime))]
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> _resolve;
    private CoreGameEngineAlcoholSlashSuppressionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholSlashSuppressionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSuppressionsRuntime> resolve) => new(resolve);
}
