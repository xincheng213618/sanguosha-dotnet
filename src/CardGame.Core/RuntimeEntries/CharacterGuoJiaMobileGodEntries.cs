#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed partial class CharacterGuoJiaMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> _resolve;
    private CharacterGuoJiaMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> resolve) => new(resolve);
}
