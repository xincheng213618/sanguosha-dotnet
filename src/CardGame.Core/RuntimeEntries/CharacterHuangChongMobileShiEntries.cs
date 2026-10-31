#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime))]
internal sealed partial class CharacterHuangChongMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime> _resolve;
    private CharacterHuangChongMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangChongMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime> resolve) => new(resolve);
}
