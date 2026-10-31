#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime))]
internal sealed partial class CoreGameEngineProgramDiscardOriginsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> _resolve;
    private CoreGameEngineProgramDiscardOriginsEntries(Func<EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardOriginsEntries Create(Func<EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> resolve) => new(resolve);
}
