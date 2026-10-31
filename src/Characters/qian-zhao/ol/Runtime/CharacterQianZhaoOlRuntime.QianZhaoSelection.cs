#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    int IWeifuSelectionAiQuery.UnusedGrants(int owner, SkillProgramCardCategory category) => ActiveWeifuGrants(owner, category == SkillProgramCardCategory.Basic ? CardKind.Slash : CardKind.DrawTwo).Count;
}
