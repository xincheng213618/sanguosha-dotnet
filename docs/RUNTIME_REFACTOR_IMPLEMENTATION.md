# 运行时状态与调度

本文件说明当前运行时的状态归属、提交及扩展边界。内容作者入口见 [人物模块与注册](CHARACTER_MODULES.md)，窗口与原生子结算返回见 [结算协议](RESOLUTION_PROTOCOLS.md)。

引擎职责、共用入口与按局装配约定见 [GameEngine 职责与装配](GAME_ENGINE_DECOMPOSITION.md)。

## 状态归属

| 数据 | 当前拥有者 |
| --- | --- |
| revision、接受命令、待提交命令、故障与可信恢复前缀 | 独立 `CommandSession` |
| 实体牌及精确位置 | `CardZoneStore` |
| 结算帧集合及身份序列 | `ResolutionScheduler` / `FrameStore` / `ResolutionIdentitySequence` |
| 待处理移牌批次、活动批次栈和事件抑制栈 | `CardMovementBatchStore`，旧原生调用保留同集合的临时别名 |
| 已提交与待提交事件的派生类型索引 | `ProgramEventHistoryIndex`，事实仍来自本局原事件日志 |
| 技能变更窗口已观察的授予版本 | `SkillGrantRevisionStore`，重复捕获保留首次基线，消费窗口时更新 |
| 角色属性、标记和技能授予 | `CharacterState` / `CharacterSkillSet` |
| 程序选择、绑定、已支付状态及指令游标 | `ProgramSkillFrame` |
| 用牌材料、目标游标、效果状态和使用完成标记 | `CardUseFrame` |
| 判定、无懈、伤害、回复、濒死及死亡的暂停信息 | 各自的类型化帧和返回凭据 |
| 已签发的有期限效果、技能使用及转换状态 | 对应规则存储及 `SkillRuntimeStateStore` |

定义、编译计划与共享注册元数据不持有对局。临时操作状态放在所属帧，长期政策由明确来源和期限标识。引擎中的轻量访问句柄只定位拥有帧，不复制一份结算状态。

## 命令提交与故障

独立 `CommandSession` 在提交之前运行规则、同步派生技能，并准备新 revision 的玩家视图和事件。`CommandEventJournal` 通过 `CommittedEventProjection` 冻结事件的嵌套集合，命令日志中的输入也经复制。预留容量后再追加命令和事件；外部观察者与调用者收到同一次准备的结果。`GameEngine.CommandSession.cs` 提供必要的原生端口及调用转接。

规则或准备异常将会话标记为故障。若此前拥有完整接受前缀，`LastTrustedCheckpoint` 提供异常前的可信恢复数据；异常后的可变状态不用于继续游戏。交付异常保留已经可见的提交，普通观察者异常逐个隔离。派发期间拒绝重入推进。

Checkpoint 保存初始化和接受命令，在当前实现中重放；恢复校验见 [核心控制模块](CORE_CONTROL_DESIGN.md)。当前格式来源为 `Replay.cs`，规则 JSON 来源为 `SkillPrograms.cs`，内容包版本来源为各包的 `CurrentVersion`。

## 帧调度

`GameEngine.Runtime.cs` 的兼容入口转到 `NativeFrameRoutingRuntime`，由它检查帧是否存在且位于栈顶；暂停中的父帧不分派。`NativeFrameRouteCatalog` 按处理器旁的 `[NativeFrameRoute]` 注册及显式顺序选择首个匹配项，普通程序仍先运行续接，再进入执行器。用牌的各个已支付分支保留原条件与优先序。新增原生续接在所属模块注册，无需扩充中央帧 `switch`。

主循环的普通原生帧准入也由处理器旁的 `[NativeFrameAutomaticStep]` 和 `NativeFrameAutomaticRuntime` 负责，四个固定阶段槽保留开局、声明质疑和判定决策前后的原顺序。待决检查的先后、不检查待决的原生步骤以及判定窗口无需额外发布的策略分别保存；自动准入仍调用完整显式路由，不改变用牌多重回执的优先序。

`ResolutionScheduler` 检查父关系，replace 保留 ID 和运行类型，完成操作仅弹出预期栈顶。`ProgramRuntimeFrameServices` 直接访问这一调度器及其身份序列，继续执行时延迟取得同局 `NativeFrameRoutingRuntime`。引擎保留 `PushRuntimeFrame` 等兼容调用点。`MatchExecutionLoop` 负责推进到人类输入边界和安全上限，`MatchStepDispatcher` 负责单步分派，`ProgramCompletionRuntime` 负责程序完成及移牌支付返回。子帧完成后使用类型化返回，父帧保留原费用、目标、事实和游标。`DamageAdjacencyRuntime` 拥有伤害候选推进和观察者链遍历；`NativeHealthRecoveryRuntime` 拥有原生回复回执与回复后伤害邻接证明；`ResponseCompletionRuntime` 拥有响应支付账单、移动和健康子帧及返回。它们通过窄事实及操作口读取原帧，不缓存另一份待决状态。

程序先运行适用的 `[ProgramContinuation]`，再进入 `SkillProgramExecutor`。注册分为三个位置：

| 位置 | 当前协议 |
| --- | --- |
| `Resume` | 活跃程序继续执行前；接受帧 ID 或程序帧，首个处理成功者结束分派。每次回调解析当前帧 |
| `Movement` | 程序移动返回开始，位于选牌支付及保留支付分支之前；接收调用者捕获的 `ProgramSkillFrame`，原移动续接凭据保留在该帧上 |
| `MovementTail` | 前述支付分支未处理时，在通用移动返回之前运行；接收同一程序帧及其原移动续接凭据 |

顺序在每类注册中显式声明且唯一。注册本身不证明费用、窗口或来源有效，这些检查由处理器完成。异常直接传播。

`ProgramTriggerWindowDriver` 复用候选推进；触发程序完成后由 `[ProgramWindowReturn]` 返回唯一所属窗口。原生用牌及攻击分别通过 `[NativeUseValidation]`、`[NativeUsePreparation]`、`[NativeUseReturn]` 和 `[NativeAttackReturn]` 注册返回边界。用牌准备在弹帧前捕获本次返回事实，之后才调用返回处理器。

## 规则与输出边界

规则事件经 `AdvanceEventRulesAndQueueFact` 运行内部观察，然后排队提交；状态推进经 `AdvanceRulesAndPublishState` 处理清理和窗口后发布。`QueueGameEvent`、`PublishState` 是输出操作。

两个规则入口转到独立 `RuleProgressionRuntime`。提交后通知由 `ObserverDispatcher` 逐个隔离观察者异常；`ProgramEventHistoryIndex` 按本局维护已提交与待提交事实的类型索引，替换为冻结后的提交事实时不复用旧待提交身份。

同一日志所有者通过 `IProgramEventBoundaryQueries` 提供边界之后的事实：先倒序查 pending，找不到才查已提交日志，返回原集合的延迟尾部视图。触发事实由 `ProgramTriggerFactProjector` 按原字段顺序组合，未声明的内容需求不触发对应查询；双体力未选定、固定最小值及 null/空集合含义保持原有契约。

`IProgramEventJournalQueries` 由同一日志索引提供完整原序枚举和最新匹配事实。它不保留另一份日志，也不为调用者提前冻结；摸牌序数和阈值证明仍在自己的原始读取点捕获完整前缀，当前阶段查询则先反向搜索 pending，再搜索已提交事实。

`[ProgramRuleObserver]` 的 `BeforeEvent`、`AfterEvent`、`BeforeStatePublish` 保留明确位置和顺序；嵌套事实同步完成后返回。同步订阅者收到的是提交结果，不能承担规则步骤。

复杂观察者子树通过 `GameEngine.ObserverSubtrees.cs` 复用推进与遍历。调用者保留精确根、首子帧、边、支付及完成凭据检查；不能只因子帧类型相同就获得推进权限。共享执行器不提供默认放行策略。

## 扩展注册

专用效果接口由独立 `[ProgramRuntimeModule, ProgramCapability]` 实现或迁移中的私有嵌套适配器提供。共享目录只保存类型和工厂；实例由 `MatchRuntime` 按对局延迟建立，处理器和能力入口共享同一实例。牌区、历史、批次状态和内容等实际所有者通过专用原生接口显式绑定；重复来源在装配时拒绝，独立测试目录不会隐式导入整局能力。`ProgramSkillHost` 只实现共同执行及效果 host，通过 `GetCapability<T>()` 获得专用能力。

跨职责调用复用以 `[ProgramRuntimeEntry(typeof(TargetRuntime))]` 注册的共享入口，只保存该类型的解析函数。目录在注册时检查目标与构造形状，本局核心装配和规则模块使用同一个缓存入口，不按消费方复制接口和适配类。调用参数先求值，再解析实际模块；引用参数、引用返回及相互延迟调用保持原语义，入口不保存待决状态。

`CardMovementBatchCoordinator` 独立负责批次开始和完成，扩展分为开始材料、已提交单条观察、上下文前事实及有序完成四个阶段。每项扩展在所属机制注册，前一项返回的上下文直接传给下一项；失败不进入后续阶段。开始材料在批次成为活动批次前封闭，待处理批次仍只追加到原 `CardMovementBatchStore`。

`[ProgramRuntimeHook]` 在声明前、响应体力前后、判定阶段前、回复应用前和伤害应用前等规定位置推进所属机制；死亡后和失技后的继续权限独立声明。阶段不是可以互换的优先级，不能跨过已支付体力或原生声明边界移动处理器。

选择来源、卡牌/目标过滤、提交验证、目标解析、AI 修饰、动作来源及动作处理分别使用自身注册目录。只读选择模块通过 `IProgramSelectionQueries` 查询，不能取得引擎可变状态或提交入口。

`ProgramTriggerRuntime` 负责候选准备、通用准入阶段、初始支付与目标前置及使用次数。已签发原生收据的提前决定由 `ProgramIssuedTriggerRuntime` 按所属规则旁的显式顺序读取：`null` 继续，`true` 提前接受，`false` 提前拒绝，任何决定均停止后续查询。它们先于普通存活、技能实例与限次检查；原生证明不能仅凭相同窗口类型获得权限。

原生证明通过 `ProgramTriggerProofRuntime` 的明确阶段注册，存活例外、实例例外、体力/移动归属、特定原生窗口及遗留窗口不能相互替代。阶段使用不同的只读请求类型，并在实例解析前排除不匹配窗口。绑定启动及人类回答由 `ProgramBindingRuntime` 处理，`ProgramActivationWindowRuntime` 选择原 typed parent，`ProgramLifecycleWindowRuntime` 推进生命周期窗口与候选；具体窗口的事实签发和领域返回仍保留原证明。

`ProgramTurnWindowEntryRuntime` 与 `ProgramDrawDiscardWindowEntryRuntime` 分别拥有回合/轮次/出牌结束及摸弃牌窗口的准入、候选和事实冻结。两者与 `ProgramPhaseBoundaryRuntime` 共用本局 `IProgramWindowCandidateQueries`，直接调用已迁出的事实和资格服务，不另设候选缓存。各窗口保留原角色引用、短路次序及自己的排序规则；实际弃牌确认仍先于弃牌结束窗口的资格判断。

`ProgramPhaseBoundaryRuntime` 拥有出牌开始与回合结束边界、顺序组提示/回答、选择组和两条类型化返回。选择组的恢复索引、原候选身份及已支付子程序都保留在原帧。领域收据和原生阶段结束通过专用协议执行；激活与选择入口延迟分派，避免装配形成循环依赖。

`OwnedDrawPhaseRuntime` 负责实际摸牌阶段、原生请求生产、阶段子窗口、跳过与额外摸牌的类型化续接；观察者与物理摸牌直接解析同局独立服务。`ManualDiscardRuntime` 持有手动弃牌的校验与推进算法，通过原命令会话接受支付；冻结的弃牌配额、部分支付和子窗口暂停仍只在原 `ManualDiscardFrame` 上推进。

`PhysicalCardDrawRuntime` 保留底抽、领域材料选择、实际移牌、发票、摸牌事实和日志的先后顺序。抽牌堆顺序接口由原 `CardZoneStore` 提供，使用本局原随机源的既有洗牌算法。每次摸牌只检查本次游标之后的真实移动，重定向实体不算实际摸牌，已正常入手后立即离手的实体仍计入本次结果。

`NativeDrawInvoiceRuntime` 负责发票的完整准入、程序来源捕获、实体序列匹配、最后未认领批次归属和事实一致性验证，直接给原 `CardMovementBatchStore` 的对应批次赋予证明。发票草稿只跨同步抽牌调用传递，不产生第二份待处理状态；序数和阈值直接组合独立来源服务，剩余人物专属来源收据仍由原领域协议证明并按原序完成。

摸牌序数来源由 `NativeDrawOrdinalRuntime` 签发和验证，触发窗口由 `NativeDrawOrdinalTriggerRuntime` 准入；累计阈值来源由 `NativeDrawThresholdInvoiceRuntime` 处理，选择、伤害及原始濒死 token 由 `NativeDrawThresholdRuntime` 证明。主发票只依赖来源服务，触发执行才依赖主发票查询，依赖方向避免构造环；来源失去技能后的已签发资格仍遵守原帧和原事实证明。

`ActualDiscardRecoveryPhaseRuntime` 从原事实派生当前阶段，负责开始、清除、恢复及弃牌批资格。摸牌发票、实际摸牌阶段和插入阶段调度直接组合该模块。合法动作查询通过窄接口访问原 `ActionQuery` 的两项缓存字段，模块在局部保存同一个作用域引用；缓存值和空值都只存活到该次查询结束，缓存命中时不创建读取委托，也不建立规则侧缓存。

`NativeLogicalDiscardRuntime` 负责原始材料捕获、预付费用证明、跨提示聚合、实际完成和帧校验。它直接使用原子移牌运行时，保留原 movement counter 的语义；该计数器不能替换为移动日志的最后一个 Sequence。人类弃牌、绑定牌、整区弃牌和程序付款直接共享该服务，尚未完成的草稿仍只属于原指令帧。

`ActualPhaseEndRuntime` 签发并推进实际阶段结束，保留候选合并、晚到义务去重、精确母子返回和原生后续选择。摸牌阶段、弃牌确认及生命周期窗口返回直接组合该模块；死亡、胜负、插入阶段恢复及 OwnedDraw 的特殊边界仍按原顺序处理。

`GameStartingDrawOriginRuntime` 统一开局原生摸牌、赠牌/获牌派生链以及已付颜色收益的来源与付款证明；批次元数据直接赋给原 `CardMovementBatchStore`。由于开局来源与主发票相互验证，只有 tracking/proof 两项窄读延迟解析主发票，其余算法不绕回引擎。`ZhanjiDrawEntitlementRuntime` 独立冻结和验证战绩来源，保留 null/空集合差别以及技能来源改变后的原事实授权。

`ProgramPhaseSchedulingRuntime` 校验并挂起原程序及父帧，结束插入阶段时恢复同一已支付续接；唯一 `ProgramPhaseSchedule` 仍在原状态位置，运行实例没有第二份计划。实际阶段入口、抓牌、弃牌令牌与人物返回通过各自的原生协议执行。`ProgramPhaseFrameQueries` 统一证明 own PlayEnding 与 foreign actual-turn 帧，其他机制无需依赖拼点签发器；`ProgramPindianRuntime` 在此基础上检查原生来源、事件目标和当前指令，再签发同一原生拼点。

`ForeignActualTurnWindowRuntime` 拥有他人实际回合开始窗口的候选排序、冻结、推进和类型化完成；窗口帧继续唯一持有候选、上下文和游标。人物专属资格和实际回合后续通过既有窄协议接入。

`ForeignTurnContestRuntime` 保存规则在原帧上的结果归属，检查原始拼点事实、输赢效果、认领及 Slash 返回，原生签发只承担下层用牌入口。`ProgramHealthStateRuntime` 直接修改 `MatchParticipantStateAccess` 返回的原角色对象；跨事件后不重新构造人物状态。恢复、双体力和体力上限等原生协议仍保持原子结算及其支付凭据。

`ProgramDrawRuntime` 决定摸牌数、接收者顺序及结果绑定，正常摸牌的冻结基数仍回写原生命周期父帧；物理摸牌和专属计数证明分别由原接口提供。`ProgramOwnedZoneDiscardRuntime` 保留逐区快照与批量替换的不同执行方式，实际材料回调先于原生收据完成，完成逻辑弃牌后才发布整区弃牌事实。原生聚合凭据不在模块内复制或解释。

`ProgramJudgmentRuntime` 负责判定指令、父帧、目标和来源校验，保持各原生证明的早返回顺序。`ProgramTargetSelectionRuntime` 拥有目标域、组合、排序、提示和选择提交；原对象与已有领域资格继续按需读取，提示仍先写入 pending 再查询人类控制状态。武庙陆抗的目标资格组合直接复用同一模块与共享拼点政策，不再绕回引擎重新解析目标。

`ProgramRandomCardAcquisitionRuntime` 在访问每个角色时读取当时区域，按原规则选择候选及披露方式。`IMatchRandomSource` 直接绑定本局原生成器，生成器先于模式和模块装配建立；模块构造与能力绑定不消费随机数。每个非空候选池仍只抽取一次，销毁后的实体不计入实际获牌完成事实。

`MatchParticipantSetup` 按模式计划构造角色、授予技能、冻结私有候选并应用选将结果，直接使用本局内容查询和原角色、候选及选择顺序集合。同步上下文只提供座位所有权、模式草稿事实和原存储写入口，不持有独立随机源或另一份开局状态。

## 缓存与玩家投影

`ProgramInstructionResolver` 缓存不可变计划和指令特征，`MatchSkillBindingIndex` 缓存每席位当前绑定的能力索引。规则注册表和依赖目录缓存定义事实，操作候选和当前牌区、限制、费用仍在适当时机读取。计划/索引刷新不改变已经冻结的窗口事实。

`CreateSnapshot(viewerSeat)` 通过独立 `PlayerViewProjector` 组装玩家投影并过滤私有数据。查询期值端口保留原角色引用与逐项延迟读取，模式完成游戏投影后，`PlayerViewFreezer` 在提交前复制和冻结其公开集合及嵌套提示。领域专属事实仍有临时引擎桥。`ResolutionStack` 可含隐藏候选、私有草稿和原生材料，是可信诊断接口。

普通牌与判定牌外观由独立 `CardSnapshotProjector` 组合，`CardSnapshotCache` 只保存同局派生结果。每次投影先重读可见外观事实，再复用完全相同的结果；动态隐去身份、琴判定槽和标记不能复用失效缓存。可见性决策仍属于调用方的玩家投影或已验证提示，缓存本身不授予访问隐藏牌的权限。

主玩家视图直接注入同局的牌区、内容查询、普通牌和手牌投影服务；这些基础读取不再经过引擎事实适配器。装备骨牌先取得缓存中的隐藏外观，再只为来源玩家或显式诊断生成显示原牌身份的副本，不能将揭示后的身份写回共享外观缓存。

`ActivePlayerEffectProjector` 从原 `TurnCardUseEffectStore` 的专用只读接口、事件索引和牌区组合效果描述，保留各列表现有读取方式、顺序和期限。人物资格及尚未迁出的规则存储通过查询期值端口提供，不按历史激活推断效果仍然有效，也不维护投影侧政策副本。

主玩家视图通过同局 `IActivePlayerEffectProjection` 直接调用这一组合器，将查询期值端口作为本次调用参数传入，不再从视图规则桥回到引擎解析投影实例。该能力入口和模块注册取得同一实例，构造函数仍只保存依赖。

可见卡牌标记通过 `CardSkillMarkProjector` 的有序来源组合；本次累积状态使用局部值类型并按引用传递，标记为空与没有标记保持不同。需要末尾显示的标记先在原规则阶段捕获值，再于组合结束追加，不保留会重新读取事实的延迟回调。

`ProgramPublicCardProjector` 保留公开程序牌的来源门控、收据面优先、重复收据拒绝及实体顺序，直接读取原调度器、事件索引和牌区。`HandCardProjector` 先组合实体、身份与点数，再执行所属机制注册的手牌外观装饰；特殊牌区的独有投影仍按原入口执行，不能因为持有实体就授予手牌视图访问权。

WPF 使用提交后的人类动作视图、revision、当前引擎和规范目标列表维护选择预览。界面草稿不替代实际合法动作，最终命令仍携带转换来源、目标、PromptId 和 revision。

## 验证范围

当前约束由命令、帧身份、牌区、隐私、事件冻结、支付续接和命令重放的机制检查覆盖。纯内容复用已有检查；新增状态、交互或缺陷修复补聚焦行为。开发使用名称过滤，批次运行 `tools/Test-Changed.ps1` 常规范围。完整验证仅按用户明确要求执行。

## 源码入口

- [命令会话](../src/CardGame.Core/GameEngine.CommandSession.cs)、[帧存储](../src/CardGame.Core/FrameStore.cs)、[运行调度](../src/CardGame.Core/GameEngine.Runtime.cs)
- [程序续接](../src/CardGame.Core/GameEngine.ProgramContinuations.cs)、[窗口返回](../src/CardGame.Core/ProgramWindowReturns.cs)、[原生返回](../src/CardGame.Core/ProgramNativeReturns.cs)
- [运行钩子](../src/CardGame.Core/ProgramRuntimeHooks.cs)、[规则观察](../src/CardGame.Core/ProgramRuleObservers.cs)、[能力目录](../src/CardGame.Core/ProgramCapabilities.cs)
- [观察者子树](../src/CardGame.Core/GameEngine.ObserverSubtrees.cs)、[指令计划](../src/CardGame.Core/ProgramExecutionPlan.cs)、[绑定索引](../src/CardGame.Core/MatchSkillBindingIndex.cs)
