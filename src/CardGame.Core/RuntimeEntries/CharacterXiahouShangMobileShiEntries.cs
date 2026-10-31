#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime))]
internal sealed partial class CharacterXiahouShangMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime> _resolve;
    private CharacterXiahouShangMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouShangMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouShangMobileShiRuntime> resolve) => new(resolve);
}
