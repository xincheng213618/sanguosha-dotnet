#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceExecutionerYishiEntries
{
    internal bool CanOfferSourceExecutionerYishi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferSourceExecutionerYishi(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveSourceExecutionerYishi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveSourceExecutionerYishi(supplied);
    internal bool SourceYishiQualifies(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().SourceYishiQualifies(owner, context);
    internal bool SourceExecutionerMode { get => _resolve().SourceExecutionerMode; }
}
