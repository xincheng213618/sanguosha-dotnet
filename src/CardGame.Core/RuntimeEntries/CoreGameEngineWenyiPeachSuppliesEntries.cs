#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime))]
internal sealed partial class CoreGameEngineWenyiPeachSuppliesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> _resolve;
    private CoreGameEngineWenyiPeachSuppliesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenyiPeachSuppliesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> resolve) => new(resolve);
}
