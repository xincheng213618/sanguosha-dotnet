#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceGrainEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceGrainRuntime> _resolve;
    private CoreGameEngineShanheSourceGrainEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceGrainRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceGrainEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceGrainRuntime> resolve) => new(resolve);
}
