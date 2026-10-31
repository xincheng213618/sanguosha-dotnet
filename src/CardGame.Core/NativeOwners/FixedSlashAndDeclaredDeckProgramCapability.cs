#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FixedSlashAndDeclaredDeckProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries> getCoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime) : IFixedSlashAndDeclaredDeckProgramHost
{
    public SkillProgramStepOutcome UseOwnerSlashAgainstTurnOwner(ProgramSkillFrame frame, bool ignoreDistance) => getCoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime().BeginOwnerSlashAgainstTurnOwner(frame, ignoreDistance);
    public SkillProgramStepOutcome DeclareDeckCriterionAndGiveMatchingCard(ProgramSkillFrame frame) => getCoreGameEngineFixedSlashAndDeclaredDeckProgramsRuntime().BeginDeclaredDeckCriterion(frame);
}
