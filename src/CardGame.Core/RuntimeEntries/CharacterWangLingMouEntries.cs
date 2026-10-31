#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangLingMouEntries
{
    private readonly Func<EngineOwners.CharacterWangLingMouRuntime> _resolve;
    private CharacterWangLingMouEntries(Func<EngineOwners.CharacterWangLingMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangLingMouEntries Create(Func<EngineOwners.CharacterWangLingMouRuntime> resolve) => new(resolve);
}
