#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangYiMobileRuntime))]
internal sealed partial class CharacterWangYiMobileEntries
{
    private readonly Func<EngineOwners.CharacterWangYiMobileRuntime> _resolve;
    private CharacterWangYiMobileEntries(Func<EngineOwners.CharacterWangYiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYiMobileEntries Create(Func<EngineOwners.CharacterWangYiMobileRuntime> resolve) => new(resolve);
}
