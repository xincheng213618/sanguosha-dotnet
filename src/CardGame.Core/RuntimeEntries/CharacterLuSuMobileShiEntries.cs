#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed partial class CharacterLuSuMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterLuSuMobileShiRuntime> _resolve;
    private CharacterLuSuMobileShiEntries(Func<EngineOwners.CharacterLuSuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileShiEntries Create(Func<EngineOwners.CharacterLuSuMobileShiRuntime> resolve) => new(resolve);
}
