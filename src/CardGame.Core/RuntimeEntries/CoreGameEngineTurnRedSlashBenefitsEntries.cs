#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _resolve;
    private CoreGameEngineTurnRedSlashBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnRedSlashBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> resolve) => new(resolve);
}
