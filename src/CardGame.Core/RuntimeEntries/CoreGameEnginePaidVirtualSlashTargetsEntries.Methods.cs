#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidVirtualSlashTargetsEntries
{
    internal bool TryContinuePaidVirtualSlashTargets(NativeOwners.CardAttackHandle attack) => _resolve().TryContinuePaidVirtualSlashTargets(attack);
}
