#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime))]
internal sealed partial class CoreGameEngineProgramAlcoholQuotaEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> _resolve;
    private CoreGameEngineProgramAlcoholQuotaEntries(Func<EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAlcoholQuotaEntries Create(Func<EngineOwners.CoreGameEngineProgramAlcoholQuotaRuntime> resolve) => new(resolve);
}
