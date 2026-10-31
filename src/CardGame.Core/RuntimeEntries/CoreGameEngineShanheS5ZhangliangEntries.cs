#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime))]
internal sealed partial class CoreGameEngineShanheS5ZhangliangEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> _resolve;
    private CoreGameEngineShanheS5ZhangliangEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ZhangliangEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhangliangRuntime> resolve) => new(resolve);
}
