#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed partial class CoreNativeTrickUseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _resolve;
    private CoreNativeTrickUseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTrickUseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> resolve) => new(resolve);
}
