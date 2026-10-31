#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    int global::CardGame.Core.IWeifuSelectionAiQuery.UnusedGrants(int owner, SkillProgramCardCategory category) => ActiveWeifuGrants(owner, category == SkillProgramCardCategory.Basic ? CardKind.Slash : CardKind.DrawTwo).Count;
}
