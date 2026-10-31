#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime))]
internal sealed partial class CoreGameEngineDuanhunEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> _resolve;
    private CoreGameEngineDuanhunEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDuanhunEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDuanhunRuntime> resolve) => new(resolve);
}
