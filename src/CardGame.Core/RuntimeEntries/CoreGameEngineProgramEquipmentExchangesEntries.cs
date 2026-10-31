#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime))]
internal sealed partial class CoreGameEngineProgramEquipmentExchangesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> _resolve;
    private CoreGameEngineProgramEquipmentExchangesEntries(Func<EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEquipmentExchangesEntries Create(Func<EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> resolve) => new(resolve);
}
