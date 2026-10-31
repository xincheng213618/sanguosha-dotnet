#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCommittedResponseUsesRuntime))]
internal sealed partial class CoreGameEngineCommittedResponseUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _resolve;
    private CoreGameEngineCommittedResponseUsesEntries(Func<EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCommittedResponseUsesEntries Create(Func<EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> resolve) => new(resolve);
}
