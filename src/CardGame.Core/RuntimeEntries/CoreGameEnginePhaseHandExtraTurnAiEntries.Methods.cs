#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiEntries
{
    internal ProgramAiEstimate IncludePhaseHandExtraTurnDeclarationForAi(ProgramAiEstimate baseline, CharacterState owner, string skillId, string skillInstanceId, SkillProgram program, SkillProgramTrigger declaration, ProgramAiPublicContext context, ProgramSkillWindowContext windowContext) => _resolve().IncludePhaseHandExtraTurnDeclarationForAi(baseline, owner, skillId, skillInstanceId, program, declaration, context, windowContext);
}
