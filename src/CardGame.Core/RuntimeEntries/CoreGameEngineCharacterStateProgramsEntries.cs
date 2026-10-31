#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed partial class CoreGameEngineCharacterStateProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _resolve;
    private CoreGameEngineCharacterStateProgramsEntries(Func<EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterStateProgramsEntries Create(Func<EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> resolve) => new(resolve);
}
