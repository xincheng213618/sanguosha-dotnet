#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoAngMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> _resolve;
    private CharacterCaoAngMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoAngMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoAngMouRuntime> resolve) => new(resolve);
}
