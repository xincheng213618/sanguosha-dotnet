#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime))]
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> _resolve;
    private CoreGameEngineProgramBooleanStateExpirationEntries(Func<EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramBooleanStateExpirationEntries Create(Func<EngineOwners.CoreGameEngineProgramBooleanStateExpirationRuntime> resolve) => new(resolve);
}
