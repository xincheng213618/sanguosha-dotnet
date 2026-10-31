#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheXingtianRuntime))]
internal sealed partial class CoreGameEngineShanheXingtianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXingtianRuntime> _resolve;
    private CoreGameEngineShanheXingtianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXingtianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXingtianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheXingtianRuntime> resolve) => new(resolve);
}
