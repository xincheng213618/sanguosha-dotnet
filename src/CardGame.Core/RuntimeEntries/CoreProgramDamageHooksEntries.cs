#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageHooksEntries
{
    private readonly Func<EngineOwners.CoreProgramDamageHooksRuntime> _resolve;
    private CoreProgramDamageHooksEntries(Func<EngineOwners.CoreProgramDamageHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramDamageHooksEntries Create(Func<EngineOwners.CoreProgramDamageHooksRuntime> resolve) => new(resolve);
}
