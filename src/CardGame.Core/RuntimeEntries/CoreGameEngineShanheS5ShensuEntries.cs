#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime))]
internal sealed partial class CoreGameEngineShanheS5ShensuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> _resolve;
    private CoreGameEngineShanheS5ShensuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShensuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> resolve) => new(resolve);
}
