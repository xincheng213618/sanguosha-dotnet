#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDiaoChanMouMobileRuntime))]
internal sealed partial class CharacterDiaoChanMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterDiaoChanMouMobileRuntime> _resolve;
    private CharacterDiaoChanMouMobileEntries(Func<EngineOwners.CharacterDiaoChanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanMouMobileEntries Create(Func<EngineOwners.CharacterDiaoChanMouMobileRuntime> resolve) => new(resolve);
}
