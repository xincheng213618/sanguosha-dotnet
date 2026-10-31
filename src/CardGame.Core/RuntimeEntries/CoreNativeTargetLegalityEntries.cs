#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed partial class CoreNativeTargetLegalityEntries
{
    private readonly Func<EngineOwners.CoreNativeTargetLegalityRuntime> _resolve;
    private CoreNativeTargetLegalityEntries(Func<EngineOwners.CoreNativeTargetLegalityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTargetLegalityEntries Create(Func<EngineOwners.CoreNativeTargetLegalityRuntime> resolve) => new(resolve);
}
