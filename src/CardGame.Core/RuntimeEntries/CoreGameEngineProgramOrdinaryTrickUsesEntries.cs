#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed partial class CoreGameEngineProgramOrdinaryTrickUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _resolve;
    private CoreGameEngineProgramOrdinaryTrickUsesEntries(Func<EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOrdinaryTrickUsesEntries Create(Func<EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> resolve) => new(resolve);
}
