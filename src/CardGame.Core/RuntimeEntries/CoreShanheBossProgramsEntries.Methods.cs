#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShanheBossProgramsEntries
{
    internal int GetBattleSlashCountResponseRequirement(global::CardGame.Core.CharacterState owner, int sourceSeat, global::CardGame.Core.CardKind incomingKind, global::CardGame.Core.CardKind requiredKind) => _resolve().GetBattleSlashCountResponseRequirement(owner, sourceSeat, incomingKind, requiredKind);
}
