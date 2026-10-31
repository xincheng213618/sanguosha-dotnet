#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime))]
internal sealed partial class CoreGameEngineProgramOwnedCardDistributionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> _resolve;
    private CoreGameEngineProgramOwnedCardDistributionsEntries(Func<EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnedCardDistributionsEntries Create(Func<EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> resolve) => new(resolve);
}
