#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeFireAttackRuntime))]
internal sealed partial class CoreNativeFireAttackEntries
{
    private readonly Func<EngineOwners.CoreNativeFireAttackRuntime> _resolve;
    private CoreNativeFireAttackEntries(Func<EngineOwners.CoreNativeFireAttackRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFireAttackEntries Create(Func<EngineOwners.CoreNativeFireAttackRuntime> resolve) => new(resolve);
}
