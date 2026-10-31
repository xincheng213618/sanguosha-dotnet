#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime))]
internal sealed partial class CoreGameEngineProgramAlcoholConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> _resolve;
    private CoreGameEngineProgramAlcoholConversionsEntries(Func<EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAlcoholConversionsEntries Create(Func<EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> resolve) => new(resolve);
}
