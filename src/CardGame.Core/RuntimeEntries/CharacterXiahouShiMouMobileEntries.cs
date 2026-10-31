#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime))]
internal sealed partial class CharacterXiahouShiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime> _resolve;
    private CharacterXiahouShiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouShiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShiMouMobileRuntime> resolve) => new(resolve);
}
