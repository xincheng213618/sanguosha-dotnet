#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime))]
internal sealed partial class CoreGameEngineHpLossSlashMaterialsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> _resolve;
    private CoreGameEngineHpLossSlashMaterialsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpLossSlashMaterialsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> resolve) => new(resolve);
}
