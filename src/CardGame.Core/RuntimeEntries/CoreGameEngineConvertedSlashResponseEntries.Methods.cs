#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConvertedSlashResponseEntries
{
    internal bool? ConvertedSlashSameColorResponseColor(NativeOwners.CardAttackHandle attack) => _resolve().ConvertedSlashSameColorResponseColor(attack);
    internal bool HasConvertedSlashSameColorResponseRestriction(NativeOwners.CardAttackHandle attack) => _resolve().HasConvertedSlashSameColorResponseRestriction(attack);
}
