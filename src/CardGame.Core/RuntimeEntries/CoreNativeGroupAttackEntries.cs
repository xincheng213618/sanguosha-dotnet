#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeGroupAttackRuntime))]
internal sealed partial class CoreNativeGroupAttackEntries
{
    private readonly Func<EngineOwners.CoreNativeGroupAttackRuntime> _resolve;
    private CoreNativeGroupAttackEntries(Func<EngineOwners.CoreNativeGroupAttackRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupAttackEntries Create(Func<EngineOwners.CoreNativeGroupAttackRuntime> resolve) => new(resolve);
}
