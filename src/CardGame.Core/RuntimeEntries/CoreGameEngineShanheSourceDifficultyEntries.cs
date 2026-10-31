#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceDifficultyEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> _resolve;
    private CoreGameEngineShanheSourceDifficultyEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceDifficultyEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceDifficultyRuntime> resolve) => new(resolve);
}
