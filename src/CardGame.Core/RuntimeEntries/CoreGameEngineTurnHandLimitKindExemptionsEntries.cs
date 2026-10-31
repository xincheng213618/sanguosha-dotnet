#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime))]
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> _resolve;
    private CoreGameEngineTurnHandLimitKindExemptionsEntries(Func<EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnHandLimitKindExemptionsEntries Create(Func<EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> resolve) => new(resolve);
}
