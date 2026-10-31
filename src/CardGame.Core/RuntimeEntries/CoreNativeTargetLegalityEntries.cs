#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed partial class CoreNativeTargetLegalityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _resolve;
    private CoreNativeTargetLegalityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTargetLegalityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> resolve) => new(resolve);
}
