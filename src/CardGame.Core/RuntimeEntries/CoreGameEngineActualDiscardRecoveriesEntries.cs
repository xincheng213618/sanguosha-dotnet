#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveriesEntries(Func<EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveriesEntries Create(Func<EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> resolve) => new(resolve);
}
