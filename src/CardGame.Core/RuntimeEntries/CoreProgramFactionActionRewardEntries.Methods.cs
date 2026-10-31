#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramFactionActionRewardEntries
{
    internal bool CanRunFactionActionReward(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunFactionActionReward(candidate, trigger, context);
    internal global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => _resolve().CaptureFactionAction(action);
    internal global::CardGame.Core.CardActionContext CaptureReplacedFactionActor(global::CardGame.Core.CardActionContext action, int actorSeat) => _resolve().CaptureReplacedFactionActor(action, actorSeat);
    internal int? FactionActionRewardChooser(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action) => _resolve().FactionActionRewardChooser(owner, trigger, action);
    internal void RecordFactionRewardOffers(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => _resolve().RecordFactionRewardOffers(action, candidates);
    internal bool WasFactionRewardOffered(int owner, string skillId, global::CardGame.Core.CardActionContext action) => _resolve().WasFactionRewardOffered(owner, skillId, action);
    internal bool TracksFactionActionOrigin { get => _resolve().TracksFactionActionOrigin; }
}
