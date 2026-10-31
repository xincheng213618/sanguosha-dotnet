#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMouEntries
{
    private readonly Func<EngineOwners.CharacterZhugeJinMouRuntime> _resolve;
    private CharacterZhugeJinMouEntries(Func<EngineOwners.CharacterZhugeJinMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeJinMouEntries Create(Func<EngineOwners.CharacterZhugeJinMouRuntime> resolve) => new(resolve);
}
