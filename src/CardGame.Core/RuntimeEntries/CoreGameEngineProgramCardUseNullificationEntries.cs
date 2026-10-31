#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed partial class CoreGameEngineProgramCardUseNullificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _resolve;
    private CoreGameEngineProgramCardUseNullificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseNullificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> resolve) => new(resolve);
}
