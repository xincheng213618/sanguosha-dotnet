#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnexpectedAssaultEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _resolve;
    private CoreGameEngineUnexpectedAssaultEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnexpectedAssaultEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> resolve) => new(resolve);
}
