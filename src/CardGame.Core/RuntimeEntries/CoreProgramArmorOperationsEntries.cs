#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramArmorOperationsEntries
{
    private readonly Func<EngineOwners.CoreProgramArmorOperationsRuntime> _resolve;
    private CoreProgramArmorOperationsEntries(Func<EngineOwners.CoreProgramArmorOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramArmorOperationsEntries Create(Func<EngineOwners.CoreProgramArmorOperationsRuntime> resolve) => new(resolve);
}
