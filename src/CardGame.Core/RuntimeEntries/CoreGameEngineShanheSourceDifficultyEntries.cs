#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime))]
internal sealed partial class CoreGameEngineShanheSourceDifficultyEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> _resolve;
    private CoreGameEngineShanheSourceDifficultyEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceDifficultyEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve) => new(resolve);
}
