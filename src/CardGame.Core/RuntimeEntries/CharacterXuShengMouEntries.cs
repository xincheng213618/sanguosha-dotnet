#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShengMouEntries
{
    private readonly Func<EngineOwners.CharacterXuShengMouRuntime> _resolve;
    private CharacterXuShengMouEntries(Func<EngineOwners.CharacterXuShengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuShengMouEntries Create(Func<EngineOwners.CharacterXuShengMouRuntime> resolve) => new(resolve);
}
