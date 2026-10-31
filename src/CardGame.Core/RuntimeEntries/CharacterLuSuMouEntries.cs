#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuSuMouRuntime))]
internal sealed partial class CharacterLuSuMouEntries
{
    private readonly Func<EngineOwners.CharacterLuSuMouRuntime> _resolve;
    private CharacterLuSuMouEntries(Func<EngineOwners.CharacterLuSuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMouEntries Create(Func<EngineOwners.CharacterLuSuMouRuntime> resolve) => new(resolve);
}
