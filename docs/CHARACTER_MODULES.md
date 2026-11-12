# 人物模块与注册

新增人物优先复用已有技能 JSON 能力。人物资料、技能定义与专属运行代码放在 `src/Characters` 的同一人物目录，由主程序的内容和运行注册表接入。

开发先让完整技能的主流程尽早编译、运行；纯复用配置沿用共享加载与机制检查，新机制只补未覆盖的行为和真实回归。不要逐人物重建隐私、伪造输入、集合冻结和所有暂停点的重放矩阵；新待决状态保留代表性的恢复检查，跨技能组合留给独立机制批次。并行扩充按独立人物分工，共享文件整合与批次验证统一进行。

玩家的 `Seat` 是稳定参与者标识，移动座次不改写角色、手牌所有者、命令和帧上的身份。`GameEngine.TableOrder.cs` 从公开的标量移动事实派生顺时针排列，`GameSnapshot.TableOrder` 提供冻结视图；距离、上下家、正常回合、群体牌、救援和同优先级触发使用这个排列。新规则通过 `TableStepSeat`、`RelativeTableOrder` 和 `GetAdjacentLivingSeat` 查询，不能再用参与者编号加减推算桌面位置。已捕获的原生目标列表仍在原帧上按原顺序完成。

## 内容声明

目录先按人物身份分组，再按版本分组。例如：

```text
src/Characters/wang-can/
  ol/
    Content/ol-wang-can.character.json
    Skills/ol-wang-can.rules.json
    Skills/ol-wang-can.presentation.json
    Runtime/GameEngine.Sanwen.cs
    Runtime/Sanwen.Operations.cs
```

`Content/*.character.json` 是人物的基础资料和技能元信息声明，包含 `packageId`、人物身份、武将版本、技能名称与说明、标签、执行形式、AI 权重以及注册顺序。技能通过 `programBundle` 引用同目录 `Skills` 中的规则和展示文件；多个角色复用的规则放在 `_shared/Skills`。授予或升级后才获得的技能也要声明，不能只注册武将的初始技能。

同一人物跨版本共用的专属运行代码可放在人物直属的 `Runtime` 中；多人复用的通用机制继续由 Core 管理。人物肖像和音频仍通过现有资源目录、目录表和 `portraitKey` 引用，保留素材工具与 WPF 的资源约定。

Runtime 文件按技能组织：独立规则保存在所属技能的 Runtime 类型中，`GameEngine.<Skill>[.<职责>].cs` 保留引擎兼容桥及尚未迁出的原生状态操作；以技能名命名的 `Programs` 或 `Operations` 文件保存明确归属该技能的独立数据类型、操作描述和处理器。同一技能较大时可以保留多个职责文件；多个技能复用同一机制时共用其实现。人物级文件仅保留跨技能的宿主分派、组合契约和确有共享归属的部分，不要求给每个技能新建 Runtime 类型。

文件名不参与技能分派。纯计算规则可批量迁到 `RuntimeSkills` 命名空间的 `*.RuntimeRules.cs`，有查询依赖的规则使用窄查询口；注册仍由明确属性和原阶段顺序决定。初始化字段和帧的 partial 声明保留原位置，避免改变初始化及序列化成员顺序。纯搬迁不新增逐人物、逐技能测试或逐技能验收清单；复用既有机制检查和代表行为，整批统一验证。

`CharacterContentCatalog` 发现编入内容程序集的资料，校验后按各包的 `registrationOrder` 顺序注册技能和武将。内容包只调用目录注册器，并声明共用卡牌、牌堆、模式和依赖。人物自己的 `pools` 指定其选将池及位置，经典包的当前人物池和标准演示池从这些声明生成，不再另维护人物列表。

新增人物在自己的文件夹添加资料和技能文件；复用现有能力时无需修改 `GameEngine`、项目文件或中央人物清单。选择该包未占用的注册顺序值和选将池顺序值。顺序会影响确定性的选将结果，不能为了整理目录而重排。加载器拒绝重复身份、重复顺序、错误格式和缺失的规则引用；最终内容注册器继续校验技能依赖和模式约束。

装备栏费用沿用原生容量和实体移动。预检废栏后的距离、攻击范围及【杀】次数时，`excludedEquipmentSlot` 排除该栏内全部实际装备；【杀】次数查询通过 `GetEquipmentRuleCards` 重建废栏后的武器能力，保留独立继承的能力。这类查询绕过普通动作查询缓存，避免不同废栏方案共用结果。复原单栏遇到历史整区废除标志时，先把各栏转换为零容量，再启用所选栏；全部复原的效果按本次实际由废除变为可用的栏数计算收益。

文件物理位置与程序集职责分开：`Runtime/**/*.cs` 链接编译进 Core；内容 JSON 嵌入 Content.Standard；可选 `Wpf/**/*.cs` 链接进 WPF。专属 partial 和私有适配器仍在原程序集内，不开放引擎可变状态。技能 JSON 保留原文件名和逻辑资源名，目录变动不会改变历史规则资源身份。

## 运行能力

通用 `ProgramSkillHost` 只实现 `ISkillProgramExecutionHost` 与 `ISkillProgramEffectHost`。人物或共享机制的专用接口由独立的 `[ProgramCapability]` 适配器实现，效果处理器通过 `host.GetCapability<IExampleProgramHost>()` 获取能力；可选能力使用 `TryGetCapability`。

能力目录兼容接收 `GameEngine` 的原私有嵌套适配器，也发现同程序集内同时显式声明 `[ProgramRuntimeModule]` 和 `[ProgramCapability]` 的独立类型。独立类型有且仅有一个构造函数，参数只能是继承 `IProgramSelectionQuery` 或 `IProgramRuntimeOperations` 的专用窄接口，不接受完整引擎或通用执行、效果宿主。注册元数据只保存类型与工厂；实际适配器按对局惰性创建、缓存。同一独立类型通过能力解析和规则钩子取得同一个本局实例，不跨对局共享，也不因注册多个入口而重复构造。

这是运行能力接口的组合边界。结算状态仍属于引擎与对应帧，适配器不保存另一套 pending 状态，不自行提交命令或通知 UI。已有实现专用接口的测试宿主仍可通过通用宿主的默认能力解析使用自身实现。

模块的目录级组合契约通过 `[ProgramCatalogValidation(order)]` 标注在原静态校验方法旁。方法接收整个 `IReadOnlyDictionary<string, SkillProgram>`，或接收 `(string path, SkillProgram program)`；加载器按注册顺序校验，逐程序方法保留目录内原枚举顺序和首个失败。新增契约不再向 `SkillProgramCatalog.ValidateTriggerSources` 追加人物调用。这些校验只在定义加载时执行。

目标已死亡时的效果资格由 descriptor 的 `AllowsDeadTarget(in ProgramEffectContinuationContext)` 声明，默认拒绝。通用执行器仍先验证所属帧、技能实例、内容指纹以及 owner-death 续接许可，再查询该操作。上下文携带原效果、推进后的原帧和当前宿主；`PaidReturnAccepted` 保留游标推进前已经确认的支付返回判断。该策略不会代替付款、原生回执或父帧校验，也不授予失技续接权限。

descriptor 的 `FactRequirements` 按 `Triggered`、`Activated` 分别声明逻辑弃牌、真实阶段与原生用牌处理区来源的采集需求。`ProgramInstructionFeatures` 和 `SkillProgramDependencies` 从整个冻结内容目录聚合，包括本局无人持有或暂不可用的技能；不改为只看当前存活武将。原生协议仍决定事实产生时机、实体与行动身份，并保留非操作来源的精确需求条件。

新增 descriptor 的 `Resources(SkillProgramEffect)` 沿用 [ProgramOperationDescriptorBase](../src/CardGame.Core/ProgramOperationDefinitions.cs) 的抽象合同，可由共享基类实现或由具体操作实现；[连策描述符](../src/Characters/lu-zhi/mou-ol/Runtime/MouLuZhiOlPrograms.cs)已有触发窗口、事实需求及处理器的完整组合。私有测试规则也经过同一加载器：例如 `modifiers` 的 `priority` 是必填整数，单个修正项也不能省略。无需为这些既有合同新增一套静态检查。

专用子结算接入前，直接使用[结算返回中的签名及源码入口](RESOLUTION_PROTOCOLS.md)核对续接、摸牌结果和回复子帧类型；不根据其他帧的相似字段名猜测 API。普通武将仍优先复用已有 DSL，无须因这些范例改写在制技能。

## 规则观察与资格检查

事件规则在实现方法旁使用 `[ProgramRuleObserver(phase, order)]` 注册。`BeforeEvent` 在事实进入输出队列前运行，`AfterEvent` 在入队后运行，`BeforeStatePublish` 在打开原有状态窗口前运行。无参数方法用于同步状态；单个事件参数会按事件类型过滤。递归产生的事实仍先完整处理后返回父调用。

激活与触发资格分别通过 `[ProgramActivationLegality]` 和 `[ProgramTriggerLegality]` 声明阶段、顺序和适用操作。资格方法与原能力放在一起；多个阶段对应通用条件、次数限制和支付检查的原有位置，不能随意合并或重排。未限定操作的检查对该阶段全部程序适用。

`AwakeningConditionOverride` 是觉醒条件的许可查询：按顺序读取注册者，任一返回真时可免除该次觉醒的条件。它不替代技能实例、触发时机、存活、费用和次数检查，也不自动放行同技能内的觉醒后奖励。人物自定义的觉醒门槛与后续回执必须同样识别实际授予的许可，不能伪造原门槛的历史事实。

专用操作产生通用选择结果时，描述符声明 `CreateChoiceResult` 的绑定与有限选项，并实现 `IProgramChoiceResultProof`。通用帧检查只接受原游标内唯一的已执行生产者、声明内选项和能力提供的实际付款/分支证明；选择者、原父帧、技能实例与内容指纹由所属能力核对。普通 `chooseOption` 保持原有检查，新的专用操作不向中央人物名单追加特判。

私有选择与 AI 策略继续使用已有 `[ProgramAnswer]`、`[ProgramAiChoice]`，子帧返回继续使用 `[ProgramContinuation]`。新增能力应在自己的模块声明相应入口，不修改集中人物分派表。

需要观察响应窗口中的实际「使用」时，人物模块可提供无状态、无参的 `IProgramCommittedUseContract`。注册表按技能身份发现唯一契约，技能 JSON 加载与内容注册均校验其完整触发绑定；未注册的格式继续受原校验约束。契约只校验声明，实际使用与单纯打出的区分仍由原生动作上下文和人物资格检查负责。

## 共享管线挂点与事实发射声明

`[ProgramPipelineHook(phase, order)]` 覆盖此前只能向中央管线追加人物调用的固定插入点：触发候选纳入、已发卡牌触发候选收集、虚拟决斗遗留捕获、完成绑定返回、出牌决策准备与强制手牌选择及过滤、脱离绑定续接、回合开始未发行义务、响应完成观察、牌实体技能标签和弃牌批次索引。声明放在所属实现旁；签名由阶段契约在加载期严格校验，order 在阶段内唯一并复现原中央调用顺序。新增人物在这些位置只添加自己的注册，不再编辑中央调用链；模块自有的子结算帧实现 `IProgramChildResolutionFrame` 暴露父帧，`PushRuntimeFrame` 不再按帧类型追加中央分支。

`ActualTurnCursorReturnedEvent` 等事实的发射资格由描述符声明：需要该事实的操作在自己的描述符类上按 `ProgramDependencyKinds` 精确重写 `TracksActualTurnCursorReturn`，`ProgramOperationCatalog` 按本局冻结目录的实际引用聚合。中央只保留引擎级聚合与非 op 依赖项，不再维护逐人物能力位链。

## 卡牌、目标与动作筛选

人物选择规则由独立的无状态类实现，使用 `[ProgramSelectionModule(stage, order, operations)]` 自行注册。注册表只扫描 Core 自身程序集中的显式声明，拒绝重复阶段顺序、错误接口、泛型类型和携带实例字段的模块。共享缓存只保存不可变的模块清单；候选结果不缓存，每次查询重新读取当前事实。

模块按职责实现 `IProgramSelectionCardFilter`、`IProgramSelectionTargetFilter`、`IProgramSelectionSourceProvider`、`IProgramSelectionCandidateFilter`、`IProgramSelectionSuitProvider`、`IProgramSelectionPhaseLimitProvider` 或 `IProgramSelectionAiDecorator`。仅为现有能力增加人物时，仍只修改内容和技能 JSON；新增机制才需要新选择模块。

选择模块只接收 `ProgramSelectionContext`、本次动作的 `ProgramSelectionCandidates` 草稿和 `IProgramSelectionQueries`。查询接口必须继承 `IProgramSelectionQuery`，由所属能力的私有适配器提供只读事实或单个通用合法性查询；选择模块不能取得引擎、可变角色、效果宿主或提交命令的入口。卡牌事实连同授予技能列表一起冻结，避免只读外层仍携带可变内容。

阶段保留原有求值位置：卡牌来源先生成卡牌，`AfterCards` 后才生成来源对应的目标；普通来源才运行 `OrdinaryTarget`。来源返回已处理且结果为空时，不回退到普通来源。`AfterTargets` 支持目标重建、卡牌和目标双向过滤；通用数量检查之后才运行最终排序和目标调整，最后生成关联关系、次数及 AI 提示。

不能把有序目标组合或卡牌目标关系简化成两个独立集合。`TargetSequences` 表示有顺序的合法目标组合，`CardTargets` 表示实际允许的卡牌目标配对；发布时仍使用现有不可变合法动作边界。人物模块不能支付代价、移动牌、压入结算帧或发布事件。

提交重验通过 `[ProgramSelectionValidator(stage, order, operations)]` 独立注册，保留通用数量、重复项、候选成员、花色及公开条件校验之间的原有位置，以及最先返回的错误代码和文案。带人物状态的目标域通过 `[ProgramTargetResolver]` 自行提供，通用目标选择继续负责生命周期、隐藏信息和选择提交边界。

额外动作来源使用 `[ProgramActionSource]`，特殊动作的提交、执行及贡献入口使用 `[ProgramActionHandler]`，声明仍放在所属能力实现旁。动作发布和命令处理有各自的显式顺序；命令处理只由可信命令会话调用，选择查询模块不能调用这些入口。

修改已有物理用牌动作的能力使用 `[ProgramCardAction]`，分别在程序动作生成之前和之后接入。它复用已有可信动作构建器参数，不把这些可变参数开放给只读选择模块；新增能力不再向通用动作方法追加人物函数调用。

原生用牌资格通过 `[ProgramCardLegality(stage, order)]` 注册在所属机制旁。用牌禁令、动作投影、物理材料、ViewAs 定义资格、次数资格、来源费用资格和真实提交验证分处原有位置；不同阶段不能合并成一次总判断。查询仍每次读取当前事实，保留短路和材料序列的惰性，不移动牌或支付费用。`SubmittedUseValidation` 按顺序运行，保留首个异常；动作生成和提交重验不能相互替代。

目标资格通过 `[ProgramTargetRule(point, order)]` 注册。有向目标禁止与声明全局目标入集各保留独立范围；目标资格与原生材料资格逐项通过，禁止及许可链则在原位置短路。目标次数许可分为共享【杀】次数上限求值之前和之后两个阶段，不能提前计算材料、有效点数或花色。额外目标数量贡献全部按顺序运行，保留原贡献来源与固定加一语义。

物理距离、攻击范围和用牌距离豁免通过 `[ProgramDistanceRule(stage, order)]` 分开注册。数值贡献保留来源标识、原先先后叠加及 `Set` 投影；废装备、废装备栏的查询参数原样传递，继续遵守现有查询缓存边界。物理杀、虚拟杀、提供杀、借刀杀及支付武器后的杀各有独立阶段，范围查询在部分链中间、部分链末尾，不能统一移到最后。签发帧、原始材料、牌的派生身份和候选花色只在原条件触及时读取。

目标窗口订阅通过 `[ProgramTargetWindow(stage, order)]` 注册，分别保留活动检查之前、牌种分类之后、普通锦囊要求、摸两张的隐式自身目标、单目标重定向来源及替换事实证明。订阅仅决定是否经过已有窗口；候选排序、去重、冻结、消费游标、已签发父帧及支付回执继续由原协议负责。现有通用 BeforeTarget 目录查询无需再添加人物名单。

实际用牌支付通过 `[ProgramCardUsePayment(stage, order)]` 注册。材料支付和回执许可按顺序短路，移动捕获、付款完成和已接受转换的费用处理则逐项执行。`LeadingTail` 与 `TrailingTail` 保留原生已提交杀的插入位置；`HpObserverTail`、`SharedProcessingTail` 和 `ProcessingAllowance` 继续使用各自的许可范围。新声明只接入原证明方法，不放宽实体、移动批次、父帧、序号或已付费用的校验，也不能用付款资格代替已支付回执。

实际伤害的修正与推进通过 `[ProgramDamageHook(phase, order)]` 注册。事实修正来源保留先调用、后枚举的顺序，事实总和仍由原 `Sum()` 检查溢出；后续加成按原来的非检查左结合顺序执行，包括只签发一次的状态副作用。拦截区分继续、防止与挂起，攻击完成区分已处理与是否暂停；连环推进前后保持独立阶段。这些入口不用于公开伤害预测，也不移动原生伤害前窗口、装备选择、体力变化、濒死及连环的相对位置。

## 子结算推进与帧检查

公开的阶段已用次数通过 `[ProgramPhaseUsedProjection(order)]` 注册只读 `int(int ownerSeat, string skillId, int previous)` 投影。各投影按顺序接收前值，全部执行；`0` 是有效覆盖，不能作为未处理而短路。元数据共享，委托只绑定本局引擎，不新增运行或存档状态。

玩家视图使用 `[ProgramViewContributor(contribution, order)]` 注册原有位置的公开牌、私看牌、人物牌堆和处理区变换。不同渠道保留原顺序、去重位置及最终冻结边界；来源方法和私有性证明继续由所属人物维护。`InvariantProcessing`、`PublicProcessing` 和各类排除条件不可互换：盖牌、秘密交换牌及暂存手牌即使 `revealAll` 也不计公共处理区，同时拼点及武陆抗的诊断可见性仍由原渠道控制。

子结算推进和续接权限在所属机制的方法旁使用 `[ProgramRuntimeHook(phase, order)]` 注册。推进方法无参数并返回 `bool`，按阶段内的显式顺序运行，首个返回 `true` 的方法结束本轮分派。`BeforeDeclaration`、`BeforeResponseHealth`、`AfterResponseHealth` 保留原调度位置，不能跨过声明、响应体力子帧或其他原生结算分支移动。

`PaidInitialSelection` 是已支付初始选牌的资格证明挂点。卡牌费用实际提交后，新的动态类型筛选可能立即排除这张原费用牌；人物通过原帧、原内容指纹、冻结选牌及真实逻辑弃牌回执证明该历史选择，通用帧检查据此保留原费用。它不放宽新动作的选牌或提交验证，也不代替费用支付。新消费者在所属 Runtime 用 `ProgramRuntimeHook` 注册，不向中央人物条件链追加名单。

`BeforeJudgmentPhase` 位于全部准备阶段子结算之后、原生跳判判断之前，用于消费已经签发的下次判定阶段义务。它在翻面跳过整个回合时不会经过，也不会为已跳过的判定阶段虚构阶段开始窗口；同步完成义务的钩子返回 `false`，由原生回合流程继续推进。

`BeforeRecoveryApply` 位于恢复替代选择之后、实际体力变化之前。最终来源、对象、请求量和替代候选保存在恢复帧的 `PendingApplication`，支付子结算返回时继续使用该凭据，不重新询问替代选择。`BeforeDamageApply` 在原生伤害前窗口推进时处理已经签发的数值义务。两者的支付状态属于各自的原生帧，体力子窗通过 `AmountSupport` 和严格的父帧凭据返回，已完成的代价不能再次支付。

`AfterOwnerDeath` 和 `AfterSkillLoss` 分别判断所属帧能否继续，方法接收 `ProgramSkillFrame` 并返回 `bool`。两种权限独立注册；已支付代价、来源身份、原生完成凭据等原条件仍由所属机制核对，不能统一放行，也不能让恢复后的子帧重复支付。

这些注册只保存不可变方法元数据，实际委托按对局绑定到引擎兼容入口或独立 Runtime 实例，禁止跨对局保存绑定实例。注册表校验阶段、顺序与签名，不新增可序列化状态或另一套 pending 对象。新增机制只在自身模块声明入口，无需向通用调度、死亡与失技权限追加人物名称。

## Runtime 独立类型迁移评估

用 `[ProgramRuntimeModule]` 注册独立、密封、非泛型类型。纯函数可放在无对局状态的静态模块，依赖查询的模块由 `MatchRuntime` 按局创建。构造函数只保存依赖，实际事实读取在规则调用时进行。通过窄能力接口接入，保存待决状态和已支付回执的仍是原帧。装配和依赖规则见 [GameEngine 职责与装配](GAME_ENGINE_DECOMPOSITION.md)。

原生续接在所属模块通过 `[NativeFrameRoute]` 注册。条件入口只读当前帧，按注册顺序选择首个匹配项。需要主循环自动推进时另声明 `[NativeFrameAutomaticStep]`，自动准入沿完整路由分派。程序窗口完成和原生摸牌返回的签名见 [结算返回协议](RESOLUTION_PROTOCOLS.md)。帧、事件、提示和窗口游标分别使用 `IProgramRuntimeFrames`、`IProgramRuntimeEvents`、`IProgramRuntimePrompts` 和 `IProgramWindowCursors`。

## 验证与兼容

纯人物配置复用内容加载和注册验证；新增机制或缺陷修复增加实际行为检查。开发时使用 `tools/Test-Changed.ps1 -CoreFilter <名称>`，批次结束运行无过滤器的常规范围。完整验证仍需用户明确要求。

注册调整必须保留声明顺序、资格短路顺序、事件时机、技能实例与帧身份、支付次数、隐私投影、牌移动和回放结果。普通人物配置由内容 hash 区分；技能 JSON schema 由格式兼容性决定，包版本用于有意发布的内容批次。Checkpoint 使用当前实现，根据格式、包签名、内容 hash 和命令校验恢复。

## 其他角色发动与阶段获得技能

主动配置可声明 `otherCharactersOnly: true`，将入口提供给其他当前出牌角色。当前共用契约接受零选牌、零选目标的阶段限次入口，`activator` 指发动者，所属帧的 owner 仍是技能拥有者。合法动作、命令和技能栏均保留准确的来源座位；原有献牌、陷嗣等入口继续使用各自的费用选择流程。

`offerRandomPlayPhaseSkill` 接受明确的 `skillIds` 列表和候选数 `amount`。候选只在执行时抽取一次，保存在所属帧中，由技能拥有者选择；`target` 决定获得者。候选提示仅投影给选择者，最终获得的技能公开进入获得者的技能区。阶段来源复用现有 `SkillGrantPhaseExpiry`，正常结束、插入新出牌阶段、阶段角色死亡和终局均按原有边界移除，其他永久来源不受影响。代价子窗返回后不重新付费；获得者已死亡或原阶段已结束时不授予技能。

核对人物专属裁定时，从 `src/Characters/<人物>/<版本>/` 和[内容来源入口](content/CONTENT_MANIFEST.md#来源与素材)定位准确版本及原始依据。
