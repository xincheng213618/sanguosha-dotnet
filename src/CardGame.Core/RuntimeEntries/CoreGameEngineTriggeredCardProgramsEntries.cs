#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime))]
internal sealed partial class CoreGameEngineTriggeredCardProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> _resolve;
    private CoreGameEngineTriggeredCardProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTriggeredCardProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> resolve) => new(resolve);
}
