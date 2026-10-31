#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime))]
internal sealed partial class CoreGameEngineShanheSourceDifficultyEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> _resolve;
    private CoreGameEngineShanheSourceDifficultyEntries(Func<EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceDifficultyEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve) => new(resolve);
}
