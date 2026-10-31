#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed partial class CoreGameEngineMantianGuohaiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _resolve;
    private CoreGameEngineMantianGuohaiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMantianGuohaiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve) => new(resolve);
}
