#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed partial class CoreNativeSlashUseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _resolve;
    private CoreNativeSlashUseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashUseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> resolve) => new(resolve);
}
