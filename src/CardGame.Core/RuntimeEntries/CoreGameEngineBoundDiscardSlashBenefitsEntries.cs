#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime))]
internal sealed partial class CoreGameEngineBoundDiscardSlashBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> _resolve;
    private CoreGameEngineBoundDiscardSlashBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundDiscardSlashBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> resolve) => new(resolve);
}
