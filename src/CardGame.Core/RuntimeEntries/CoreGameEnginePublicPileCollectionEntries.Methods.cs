#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCollectionEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ExecutePublicPileCollection(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecutePublicPileCollection(effect, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PublicPileCollectionChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PublicPileCollectionChoices(frame, effect);
    internal void ResolvePublicPileCollectionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePublicPileCollectionChoice(selected);
}
