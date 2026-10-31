#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramArmorOperationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramArmorOperationsRuntime> _resolve;
    private CoreProgramArmorOperationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramArmorOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramArmorOperationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramArmorOperationsRuntime> resolve) => new(resolve);
}
