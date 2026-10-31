#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime))]
internal sealed partial class CharacterZhugeGuoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> _resolve;
    private CharacterZhugeGuoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> resolve) => new(resolve);
}
