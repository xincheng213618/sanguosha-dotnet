#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime))]
internal sealed partial class CoreGameEngineProgramCategoryChallengesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> _resolve;
    private CoreGameEngineProgramCategoryChallengesEntries(Func<EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCategoryChallengesEntries Create(Func<EngineOwners.CoreGameEngineProgramCategoryChallengesRuntime> resolve) => new(resolve);
}
