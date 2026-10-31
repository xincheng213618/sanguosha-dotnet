#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramNativeDrawRuntime))]
internal sealed partial class CoreGameEngineProgramNativeDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramNativeDrawRuntime> _resolve;
    private CoreGameEngineProgramNativeDrawEntries(Func<EngineOwners.CoreGameEngineProgramNativeDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramNativeDrawEntries Create(Func<EngineOwners.CoreGameEngineProgramNativeDrawRuntime> resolve) => new(resolve);
}
