#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5SimayiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5SimayiRuntime> _resolve;
    private CoreGameEngineShanheS5SimayiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5SimayiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5SimayiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5SimayiRuntime> resolve) => new(resolve);
}
