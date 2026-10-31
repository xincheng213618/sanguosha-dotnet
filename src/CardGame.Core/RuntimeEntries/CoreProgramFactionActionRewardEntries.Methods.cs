#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramFactionActionRewardEntries
{
    internal bool CanRunFactionActionReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunFactionActionReward(candidate, trigger, context);
    internal CardActionContext CaptureFactionAction(CardActionContext action) => _resolve().CaptureFactionAction(action);
    internal CardActionContext CaptureReplacedFactionActor(CardActionContext action, int actorSeat) => _resolve().CaptureReplacedFactionActor(action, actorSeat);
    internal int? FactionActionRewardChooser(int owner, SkillProgramTrigger trigger, CardActionContext action) => _resolve().FactionActionRewardChooser(owner, trigger, action);
    internal void RecordFactionRewardOffers(CardActionContext action, IReadOnlyList<ProgramCardTriggerCandidate> candidates) => _resolve().RecordFactionRewardOffers(action, candidates);
    internal bool WasFactionRewardOffered(int owner, string skillId, CardActionContext action) => _resolve().WasFactionRewardOffered(owner, skillId, action);
    internal bool TracksFactionActionOrigin { get => _resolve().TracksFactionActionOrigin; }
}
