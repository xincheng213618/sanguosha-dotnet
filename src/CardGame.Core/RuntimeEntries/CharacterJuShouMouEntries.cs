#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJuShouMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _resolve;
    private CharacterJuShouMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJuShouMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> resolve) => new(resolve);
}
