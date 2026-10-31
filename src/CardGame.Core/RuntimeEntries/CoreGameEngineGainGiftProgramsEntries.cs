#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed partial class CoreGameEngineGainGiftProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _resolve;
    private CoreGameEngineGainGiftProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGainGiftProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> resolve) => new(resolve);
}
