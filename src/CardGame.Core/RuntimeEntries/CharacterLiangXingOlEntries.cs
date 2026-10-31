#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiangXingOlEntries
{
    private readonly Func<EngineOwners.CharacterLiangXingOlRuntime> _resolve;
    private CharacterLiangXingOlEntries(Func<EngineOwners.CharacterLiangXingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiangXingOlEntries Create(Func<EngineOwners.CharacterLiangXingOlRuntime> resolve) => new(resolve);
}
