#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime))]
internal sealed partial class CoreGameEngineRecipientCategorySlashTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> _resolve;
    private CoreGameEngineRecipientCategorySlashTargetsEntries(Func<EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategorySlashTargetsEntries Create(Func<EngineOwners.CoreGameEngineRecipientCategorySlashTargetsRuntime> resolve) => new(resolve);
}
