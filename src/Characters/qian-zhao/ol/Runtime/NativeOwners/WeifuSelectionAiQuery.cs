#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WeifuSelectionAiQuery(Func<RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime) : IWeifuSelectionAiQuery
{
    public int UnusedGrants(int owner, SkillProgramCardCategory category) => getCharacterQianZhaoOlRuntime().ActiveWeifuGrants(owner, category == SkillProgramCardCategory.Basic ? CardKind.Slash : CardKind.DrawTwo).Count;
}
