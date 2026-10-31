#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime))]
internal sealed partial class CoreGameEngineActualDiscardRecoveriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> _resolve;
    private CoreGameEngineActualDiscardRecoveriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualDiscardRecoveriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime> resolve) => new(resolve);
}
