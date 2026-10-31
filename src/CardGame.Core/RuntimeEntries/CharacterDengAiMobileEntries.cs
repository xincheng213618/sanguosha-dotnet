#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMobileEntries
{
    private readonly Func<EngineOwners.CharacterDengAiMobileRuntime> _resolve;
    private CharacterDengAiMobileEntries(Func<EngineOwners.CharacterDengAiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMobileEntries Create(Func<EngineOwners.CharacterDengAiMobileRuntime> resolve) => new(resolve);
}
