#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime))]
internal sealed partial class CharacterDaQiaoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> _resolve;
    private CharacterDaQiaoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoMobileRuntime> resolve) => new(resolve);
}
