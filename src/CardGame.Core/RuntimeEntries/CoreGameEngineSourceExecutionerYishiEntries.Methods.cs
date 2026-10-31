#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerYishiEntries
{
    internal bool CanOfferSourceExecutionerYishi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceExecutionerYishi(gate);
    internal SkillProgramStepOutcome ResolveSourceExecutionerYishi(ProgramSkillFrame supplied) => _resolve().ResolveSourceExecutionerYishi(supplied);
    internal bool SourceYishiQualifies(int owner, ProgramSkillWindowContext context) => _resolve().SourceYishiQualifies(owner, context);
    internal bool SourceExecutionerMode { get => _resolve().SourceExecutionerMode; }
}
