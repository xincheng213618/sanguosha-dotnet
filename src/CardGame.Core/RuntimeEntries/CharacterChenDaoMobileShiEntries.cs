#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime))]
internal sealed partial class CharacterChenDaoMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> _resolve;
    private CharacterChenDaoMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenDaoMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> resolve) => new(resolve);
}
