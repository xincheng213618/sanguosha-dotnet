#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramRuntimeHooksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _resolve;
    private CoreProgramRuntimeHooksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramRuntimeHooksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> resolve) => new(resolve);
}
