#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed partial class CoreGameEngineQimeiProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> _resolve;
    private CoreGameEngineQimeiProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQimeiProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> resolve) => new(resolve);
}
