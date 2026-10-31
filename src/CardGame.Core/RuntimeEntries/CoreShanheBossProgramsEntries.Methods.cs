#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShanheBossProgramsEntries
{
    internal int GetBattleSlashCountResponseRequirement(CharacterState owner, int sourceSeat, CardKind incomingKind, CardKind requiredKind) => _resolve().GetBattleSlashCountResponseRequirement(owner, sourceSeat, incomingKind, requiredKind);
}
