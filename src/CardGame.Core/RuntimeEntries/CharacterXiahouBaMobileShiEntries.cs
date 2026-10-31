#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouBaMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> _resolve;
    private CharacterXiahouBaMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouBaMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve) => new(resolve);
}
