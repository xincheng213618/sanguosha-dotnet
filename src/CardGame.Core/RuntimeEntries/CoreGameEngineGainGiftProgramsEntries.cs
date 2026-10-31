#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed partial class CoreGameEngineGainGiftProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _resolve;
    private CoreGameEngineGainGiftProgramsEntries(Func<EngineOwners.CoreGameEngineGainGiftProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGainGiftProgramsEntries Create(Func<EngineOwners.CoreGameEngineGainGiftProgramsRuntime> resolve) => new(resolve);
}
