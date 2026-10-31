#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDaQiaoMobileRuntime))]
internal sealed partial class CharacterDaQiaoMobileEntries
{
    private readonly Func<EngineOwners.CharacterDaQiaoMobileRuntime> _resolve;
    private CharacterDaQiaoMobileEntries(Func<EngineOwners.CharacterDaQiaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoMobileEntries Create(Func<EngineOwners.CharacterDaQiaoMobileRuntime> resolve) => new(resolve);
}
