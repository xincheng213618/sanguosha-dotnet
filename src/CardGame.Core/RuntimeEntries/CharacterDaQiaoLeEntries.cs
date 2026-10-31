#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed partial class CharacterDaQiaoLeEntries
{
    private readonly Func<EngineOwners.CharacterDaQiaoLeRuntime> _resolve;
    private CharacterDaQiaoLeEntries(Func<EngineOwners.CharacterDaQiaoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoLeEntries Create(Func<EngineOwners.CharacterDaQiaoLeRuntime> resolve) => new(resolve);
}
