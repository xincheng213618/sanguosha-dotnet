#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageHooksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> _resolve;
    private CoreProgramDamageHooksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramDamageHooksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> resolve) => new(resolve);
}
