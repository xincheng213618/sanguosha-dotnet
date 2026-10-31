#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime))]
internal sealed partial class CoreGameEngineShanheS5ShuangxiongEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> _resolve;
    private CoreGameEngineShanheS5ShuangxiongEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShuangxiongEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShuangxiongRuntime> resolve) => new(resolve);
}
