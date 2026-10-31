#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime))]
internal sealed partial class CoreGameEngineRecipientCategorySlashTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> _resolve;
    private CoreGameEngineRecipientCategorySlashTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategorySlashTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> resolve) => new(resolve);
}
