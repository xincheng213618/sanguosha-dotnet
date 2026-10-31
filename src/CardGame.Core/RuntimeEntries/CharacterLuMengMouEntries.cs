#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuMengMouRuntime))]
internal sealed partial class CharacterLuMengMouEntries
{
    private readonly Func<EngineOwners.CharacterLuMengMouRuntime> _resolve;
    private CharacterLuMengMouEntries(Func<EngineOwners.CharacterLuMengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMouEntries Create(Func<EngineOwners.CharacterLuMengMouRuntime> resolve) => new(resolve);
}
