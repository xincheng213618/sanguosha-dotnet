#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMouEntries
{
    private readonly Func<EngineOwners.CharacterJiangWeiMouRuntime> _resolve;
    private CharacterJiangWeiMouEntries(Func<EngineOwners.CharacterJiangWeiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMouEntries Create(Func<EngineOwners.CharacterJiangWeiMouRuntime> resolve) => new(resolve);
}
