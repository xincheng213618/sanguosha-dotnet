#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime))]
internal sealed partial class CoreGameEngineProgramEquipmentExchangesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> _resolve;
    private CoreGameEngineProgramEquipmentExchangesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEquipmentExchangesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentExchangesRuntime> resolve) => new(resolve);
}
