#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime))]
internal sealed partial class CharacterCaoHongMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime> _resolve;
    private CharacterCaoHongMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoHongMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime> resolve) => new(resolve);
}
