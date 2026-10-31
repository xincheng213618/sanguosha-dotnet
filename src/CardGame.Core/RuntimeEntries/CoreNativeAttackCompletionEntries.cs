#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed partial class CoreNativeAttackCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeAttackCompletionRuntime> _resolve;
    private CoreNativeAttackCompletionEntries(Func<EngineOwners.CoreNativeAttackCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeAttackCompletionEntries Create(Func<EngineOwners.CoreNativeAttackCompletionRuntime> resolve) => new(resolve);
}
