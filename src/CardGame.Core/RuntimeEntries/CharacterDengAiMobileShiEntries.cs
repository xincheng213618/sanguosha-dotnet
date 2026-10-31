#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDengAiMobileShiRuntime))]
internal sealed partial class CharacterDengAiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterDengAiMobileShiRuntime> _resolve;
    private CharacterDengAiMobileShiEntries(Func<EngineOwners.CharacterDengAiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMobileShiEntries Create(Func<EngineOwners.CharacterDengAiMobileShiRuntime> resolve) => new(resolve);
}
