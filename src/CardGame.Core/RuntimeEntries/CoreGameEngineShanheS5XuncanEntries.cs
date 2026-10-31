#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuncanEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> _resolve;
    private CoreGameEngineShanheS5XuncanEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuncanEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuncanRuntime> resolve) => new(resolve);
}
