#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime))]
internal sealed partial class CharacterCaoPiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> _resolve;
    private CharacterCaoPiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiMouRuntime> resolve) => new(resolve);
}
