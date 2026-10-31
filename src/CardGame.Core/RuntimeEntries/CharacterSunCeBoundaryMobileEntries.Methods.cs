#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunCeBoundaryMobileEntries
{
    internal void AnswerBoundarySunCeChallenge(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundarySunCeChallenge(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundarySunCeMobileChallenge(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundarySunCeMobileChallenge(f);
    internal void BeginBoundarySunCeMobileContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundarySunCeMobileContest(f);
    internal bool BoundarySunCeLordQualified(global::CardGame.Core.CharacterState lord, string skillId) => _resolve().BoundarySunCeLordQualified(lord, skillId);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildBoundarySunCeMobileChallenges(global::CardGame.Core.CharacterState provider) => _resolve().BuildBoundarySunCeMobileChallenges(provider);
    internal bool ExecuteBoundarySunCeMobileChallenge(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CharacterState lord, global::CardGame.Core.LegalAction a, int card) => _resolve().ExecuteBoundarySunCeMobileChallenge(provider, lord, a, card);
    internal bool KeepBoundarySunCeMobileContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundarySunCeMobileContest(f);
    internal bool TryBeginBoundarySunCeZhibaClaim(global::CardGame.Core.PindianFrame pair) => _resolve().TryBeginBoundarySunCeZhibaClaim(pair);
}
