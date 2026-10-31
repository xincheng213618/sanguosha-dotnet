#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanPingBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingBoundaryRuntime> _resolve;
    private CharacterGuanPingBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanPingBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanPingBoundaryRuntime> resolve) => new(resolve);
}
