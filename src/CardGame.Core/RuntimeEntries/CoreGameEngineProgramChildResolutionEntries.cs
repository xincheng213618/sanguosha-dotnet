#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed partial class CoreGameEngineProgramChildResolutionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _resolve;
    private CoreGameEngineProgramChildResolutionEntries(Func<EngineOwners.CoreGameEngineProgramChildResolutionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramChildResolutionEntries Create(Func<EngineOwners.CoreGameEngineProgramChildResolutionRuntime> resolve) => new(resolve);
}
