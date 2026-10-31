#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime))]
internal sealed partial class CoreGameEngineTriggeredCardProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> _resolve;
    private CoreGameEngineTriggeredCardProgramsEntries(Func<EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTriggeredCardProgramsEntries Create(Func<EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime> resolve) => new(resolve);
}
