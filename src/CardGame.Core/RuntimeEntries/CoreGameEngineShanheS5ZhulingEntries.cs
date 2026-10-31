#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime))]
internal sealed partial class CoreGameEngineShanheS5ZhulingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> _resolve;
    private CoreGameEngineShanheS5ZhulingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ZhulingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ZhulingRuntime> resolve) => new(resolve);
}
