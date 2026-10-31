#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouEntries
{
    private readonly Func<EngineOwners.CharacterHuangGaiMouRuntime> _resolve;
    private CharacterHuangGaiMouEntries(Func<EngineOwners.CharacterHuangGaiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangGaiMouEntries Create(Func<EngineOwners.CharacterHuangGaiMouRuntime> resolve) => new(resolve);
}
