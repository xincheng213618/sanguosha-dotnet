#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriRuntime))]
internal sealed partial class CoreGameEngineShanheSourceBiriEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriRuntime> _resolve;
    private CoreGameEngineShanheSourceBiriEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceBiriEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriRuntime> resolve) => new(resolve);
}
