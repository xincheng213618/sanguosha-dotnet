#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingMouEntries
{
    private readonly Func<EngineOwners.CharacterGuanPingMouRuntime> _resolve;
    private CharacterGuanPingMouEntries(Func<EngineOwners.CharacterGuanPingMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanPingMouEntries Create(Func<EngineOwners.CharacterGuanPingMouRuntime> resolve) => new(resolve);
}
