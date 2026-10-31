#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangYueyingMouMobileRuntime))]
internal sealed partial class CharacterHuangYueyingMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterHuangYueyingMouMobileRuntime> _resolve;
    private CharacterHuangYueyingMouMobileEntries(Func<EngineOwners.CharacterHuangYueyingMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangYueyingMouMobileEntries Create(Func<EngineOwners.CharacterHuangYueyingMouMobileRuntime> resolve) => new(resolve);
}
