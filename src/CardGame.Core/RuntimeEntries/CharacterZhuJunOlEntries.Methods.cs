#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuJunOlEntries
{
    internal bool AdvanceZhuJunSubtree() => _resolve().AdvanceZhuJunSubtree();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhuJunCuipo(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginZhuJunCuipo(input);
    internal bool CanOfferZhuJun(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhuJun(candidate, trigger, context);
    internal bool ContinuePaidZhuJun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidZhuJun(f);
    internal void ContinueZhuJunCuipo(long id) => _resolve().ContinueZhuJunCuipo(id);
    internal bool IsZhuJunDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsZhuJunDrawMovement(f, m);
    internal void ObserveZhuJunActualUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveZhuJunActualUse(payload);
    internal bool ResumeZhuJunCuipo(long id) => _resolve().ResumeZhuJunCuipo(id);
    internal bool ReturnZhuJunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhuJunMovement(f);
    internal bool TryBeginZhuJunLegacyAlcoholUse(long useId, long programParentId, long dyingId) => _resolve().TryBeginZhuJunLegacyAlcoholUse(useId, programParentId, dyingId);
    internal bool TryReturnZhuJunLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().TryReturnZhuJunLegacyAlcohol(frame);
    internal int ZhuJunCuipoDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ZhuJunCuipoDamageBonus(attack);
    internal bool ZhuJunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhuJunFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? ZhuJunObserverRoot() => _resolve().ZhuJunObserverRoot();
    internal global::CardGame.Core.ZhuJunCuipoQualifiedEvent? ZhuJunQualification(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ZhuJunQualification(candidate, context);
    internal bool HasZhuJunCapability { get => _resolve().HasZhuJunCapability; }
}
