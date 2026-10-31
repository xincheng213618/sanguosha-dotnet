#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed partial class CoreNativeAttackCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _resolve;
    private CoreNativeAttackCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeAttackCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> resolve) => new(resolve);
}
