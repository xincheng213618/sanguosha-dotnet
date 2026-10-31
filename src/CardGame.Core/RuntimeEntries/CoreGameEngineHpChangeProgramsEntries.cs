#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed partial class CoreGameEngineHpChangeProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _resolve;
    private CoreGameEngineHpChangeProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpChangeProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> resolve) => new(resolve);
}
