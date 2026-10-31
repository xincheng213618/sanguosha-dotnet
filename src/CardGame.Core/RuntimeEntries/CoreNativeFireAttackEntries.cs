#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime))]
internal sealed partial class CoreNativeFireAttackEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> _resolve;
    private CoreNativeFireAttackEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFireAttackEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> resolve) => new(resolve);
}
