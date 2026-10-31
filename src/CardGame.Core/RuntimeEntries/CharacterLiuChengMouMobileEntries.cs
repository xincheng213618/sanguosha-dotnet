#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed partial class CharacterLiuChengMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _resolve;
    private CharacterLiuChengMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuChengMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> resolve) => new(resolve);
}
