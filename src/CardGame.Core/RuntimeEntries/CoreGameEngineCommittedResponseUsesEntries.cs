#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCommittedResponseUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _resolve;
    private CoreGameEngineCommittedResponseUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCommittedResponseUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve) => new(resolve);
}
