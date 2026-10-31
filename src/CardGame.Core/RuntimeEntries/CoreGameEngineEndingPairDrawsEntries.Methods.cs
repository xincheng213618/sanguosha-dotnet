#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairDrawsEntries
{
    internal SkillProgramStepOutcome BeginEndingPairDraw(ProgramSkillFrame supplied, string state) => _resolve().BeginEndingPairDraw(supplied, state);
    internal bool CanRunEndingPair(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanRunEndingPair(c, t, context);
    internal void ContinueEndingPairDraw(long id) => _resolve().ContinueEndingPairDraw(id);
    internal CardConversionSource EndingPairSource(ProgramSkillFrame f) => _resolve().EndingPairSource(f);
    internal bool ExactEndingPairParent(ProgramSkillFrame f, long? expected = default) => _resolve().ExactEndingPairParent(f, expected);
    internal IReadOnlyList<int>? PublishedEndingPairActorForAi(CharacterState owner, IReadOnlyList<SkillProgramEffect> effects, ProgramSkillWindowContext? context) => _resolve().PublishedEndingPairActorForAi(owner, effects, context);
    internal bool ValidEndingPairDraw(ProgramSkillFrame f) => _resolve().ValidEndingPairDraw(f);
    internal bool ValidEndingPairInvoice(ProgramSkillFrame f, ProgramOneCardDrawInvoice? invoice, int cursor, int seat, bool required) => _resolve().ValidEndingPairInvoice(f, invoice, cursor, seat, required);
    internal long EndingPairSequence { get => _resolve().EndingPairSequence; }
}
