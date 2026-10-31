#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanShaoMouMobileRuntime))]
internal sealed partial class CharacterYuanShaoMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterYuanShaoMouMobileRuntime> _resolve;
    private CharacterYuanShaoMouMobileEntries(Func<EngineOwners.CharacterYuanShaoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanShaoMouMobileEntries Create(Func<EngineOwners.CharacterYuanShaoMouMobileRuntime> resolve) => new(resolve);
}
