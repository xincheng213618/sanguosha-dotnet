#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenDaoMobileShiRuntime))]
internal sealed partial class CharacterChenDaoMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterChenDaoMobileShiRuntime> _resolve;
    private CharacterChenDaoMobileShiEntries(Func<EngineOwners.CharacterChenDaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenDaoMobileShiEntries Create(Func<EngineOwners.CharacterChenDaoMobileShiRuntime> resolve) => new(resolve);
}
