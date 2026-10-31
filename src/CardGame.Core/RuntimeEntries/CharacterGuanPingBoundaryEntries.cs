#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingBoundaryEntries
{
    private readonly Func<EngineOwners.CharacterGuanPingBoundaryRuntime> _resolve;
    private CharacterGuanPingBoundaryEntries(Func<EngineOwners.CharacterGuanPingBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanPingBoundaryEntries Create(Func<EngineOwners.CharacterGuanPingBoundaryRuntime> resolve) => new(resolve);
}
