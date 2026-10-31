#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5LiJueRuntime))]
internal sealed partial class CoreGameEngineShanheS5LiJueEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5LiJueRuntime> _resolve;
    private CoreGameEngineShanheS5LiJueEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5LiJueRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5LiJueEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5LiJueRuntime> resolve) => new(resolve);
}
