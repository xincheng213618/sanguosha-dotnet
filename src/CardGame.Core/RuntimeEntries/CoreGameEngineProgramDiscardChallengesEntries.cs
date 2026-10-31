#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime))]
internal sealed partial class CoreGameEngineProgramDiscardChallengesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> _resolve;
    private CoreGameEngineProgramDiscardChallengesEntries(Func<EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardChallengesEntries Create(Func<EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime> resolve) => new(resolve);
}
