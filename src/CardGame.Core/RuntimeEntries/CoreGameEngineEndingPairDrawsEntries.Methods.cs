#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairDrawsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginEndingPairDraw(global::CardGame.Core.ProgramSkillFrame supplied, string state) => _resolve().BeginEndingPairDraw(supplied, state);
    internal bool CanRunEndingPair(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunEndingPair(c, t, context);
    internal void ContinueEndingPairDraw(long id) => _resolve().ContinueEndingPairDraw(id);
    internal global::CardGame.Core.CardConversionSource EndingPairSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().EndingPairSource(f);
    internal bool ExactEndingPairParent(global::CardGame.Core.ProgramSkillFrame f, long? expected = default) => _resolve().ExactEndingPairParent(f, expected);
    internal global::System.Collections.Generic.IReadOnlyList<int>? PublishedEndingPairActorForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().PublishedEndingPairActorForAi(owner, effects, context);
    internal bool ValidEndingPairDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidEndingPairDraw(f);
    internal bool ValidEndingPairInvoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramOneCardDrawInvoice? invoice, int cursor, int seat, bool required) => _resolve().ValidEndingPairInvoice(f, invoice, cursor, seat, required);
    internal long EndingPairSequence { get => _resolve().EndingPairSequence; }
}
