#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime))]
internal sealed partial class CoreGameEngineProgramHandLimitKindExemptionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> _resolve;
    private CoreGameEngineProgramHandLimitKindExemptionsEntries(Func<EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandLimitKindExemptionsEntries Create(Func<EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> resolve) => new(resolve);
}
