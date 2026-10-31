#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheDoubleStealEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDoubleStealRuntime> _resolve;
    private CoreGameEngineShanheDoubleStealEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDoubleStealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheDoubleStealEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheDoubleStealRuntime> resolve) => new(resolve);
}
