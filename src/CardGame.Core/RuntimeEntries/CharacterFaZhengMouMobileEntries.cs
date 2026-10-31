#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime))]
internal sealed partial class CharacterFaZhengMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime> _resolve;
    private CharacterFaZhengMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFaZhengMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime> resolve) => new(resolve);
}
