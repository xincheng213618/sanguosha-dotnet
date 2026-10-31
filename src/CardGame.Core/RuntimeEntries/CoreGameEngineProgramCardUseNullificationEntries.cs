#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed partial class CoreGameEngineProgramCardUseNullificationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _resolve;
    private CoreGameEngineProgramCardUseNullificationEntries(Func<EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseNullificationEntries Create(Func<EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> resolve) => new(resolve);
}
