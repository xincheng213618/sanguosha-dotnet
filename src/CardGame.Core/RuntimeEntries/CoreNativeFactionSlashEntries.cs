#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed partial class CoreNativeFactionSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> _resolve;
    private CoreNativeFactionSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> resolve) => new(resolve);
}
