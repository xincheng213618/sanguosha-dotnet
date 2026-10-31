#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JueshengSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries> getCharacterFanJiangZhangDaOlRuntime) : IJueshengTargetQuery
{
    public bool Allows(int actor, int target) => getCharacterFanJiangZhangDaOlRuntime().CanIssueJueshengDuel(actor, target);
}
