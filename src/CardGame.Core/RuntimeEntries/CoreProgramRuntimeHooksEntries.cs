#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramRuntimeHooksEntries
{
    private readonly Func<EngineOwners.CoreProgramRuntimeHooksRuntime> _resolve;
    private CoreProgramRuntimeHooksEntries(Func<EngineOwners.CoreProgramRuntimeHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramRuntimeHooksEntries Create(Func<EngineOwners.CoreProgramRuntimeHooksRuntime> resolve) => new(resolve);
}
