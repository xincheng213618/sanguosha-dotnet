#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunCeBoundaryMobileEntries
{
    internal void AnswerBoundarySunCeChallenge(PromptChoice choice) => _resolve().AnswerBoundarySunCeChallenge(choice);
    internal SkillProgramStepOutcome BeginBoundarySunCeMobileChallenge(ProgramSkillFrame f) => _resolve().BeginBoundarySunCeMobileChallenge(f);
    internal void BeginBoundarySunCeMobileContest(ProgramSkillFrame f) => _resolve().BeginBoundarySunCeMobileContest(f);
    internal bool BoundarySunCeLordQualified(CharacterState lord, string skillId) => _resolve().BoundarySunCeLordQualified(lord, skillId);
    internal IEnumerable<LegalAction> BuildBoundarySunCeMobileChallenges(CharacterState provider) => _resolve().BuildBoundarySunCeMobileChallenges(provider);
    internal bool ExecuteBoundarySunCeMobileChallenge(CharacterState provider, CharacterState lord, LegalAction a, int card) => _resolve().ExecuteBoundarySunCeMobileChallenge(provider, lord, a, card);
    internal bool KeepBoundarySunCeMobileContest(ProgramSkillFrame f) => _resolve().KeepBoundarySunCeMobileContest(f);
    internal bool TryBeginBoundarySunCeZhibaClaim(PindianFrame pair) => _resolve().TryBeginBoundarySunCeZhibaClaim(pair);
}
