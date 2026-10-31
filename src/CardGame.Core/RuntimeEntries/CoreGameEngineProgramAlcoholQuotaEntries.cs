#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime))]
internal sealed partial class CoreGameEngineProgramAlcoholQuotaEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> _resolve;
    private CoreGameEngineProgramAlcoholQuotaEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAlcoholQuotaEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> resolve) => new(resolve);
}
