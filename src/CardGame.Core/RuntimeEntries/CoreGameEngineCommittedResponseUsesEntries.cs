#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime))]
internal sealed partial class CoreGameEngineCommittedResponseUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _resolve;
    private CoreGameEngineCommittedResponseUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCommittedResponseUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve) => new(resolve);
}
