#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime))]
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> _resolve;
    private CoreGameEngineProgramOtherOwnedCardDiscardsEntries(Func<EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOtherOwnedCardDiscardsEntries Create(Func<EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> resolve) => new(resolve);
}
