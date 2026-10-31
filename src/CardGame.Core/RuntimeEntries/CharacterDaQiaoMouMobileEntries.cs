#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDaQiaoMouMobileRuntime))]
internal sealed partial class CharacterDaQiaoMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterDaQiaoMouMobileRuntime> _resolve;
    private CharacterDaQiaoMouMobileEntries(Func<EngineOwners.CharacterDaQiaoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoMouMobileEntries Create(Func<EngineOwners.CharacterDaQiaoMouMobileRuntime> resolve) => new(resolve);
}
