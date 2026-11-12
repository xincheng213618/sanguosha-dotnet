# 界诸葛亮、界孙尚香规则与工程裁定

采用的普通 OL 文本、属性与工程默认见[来源档案](https://github.com/xincheng213618/sanguosha-dotnet/blob/77ab25557053b2e6c1033af6c6f392484c5b71dd/docs/content/sources/fenglin-twelfth-2026-10-02.json)。人物资料注册 boundary:zhuge-liang 和 boundary:sun-shangxiang；规则分别使用 boundary:guanxing-current、boundary:jieyin-current，并复用 classic:kongcheng、classic:xiaoji。

## 观星

ReorderTopCards 的 PopulationThresholdCount 在每次真正开始观看时按存活人数决定数量：至少四人看至多五张，否则至多三张，短堆使用实际可用数量。此模式与 numberExpression、exactTopCount 互斥。

TopReorder 固定一次 ViewedCardIds 及真实牌面，只给拥有者观看。逐张选择顶/底后，对真实顶片段和完整分区校验并一次重排；显示相同牌面不重新取看。

准备阶段的 AllBottomStateId 完成政策保存指令、实际回合和声明布尔状态。只有非空观看、全部实体实际置底并成功提交，才授予该实例的本回合资格。结束阶段另一个绑定只消费资格，不再次武装自身。新实例不继承；原状态按回合状态仓在下一实际回合开始重置。

排序提交前取消不产生奖励，提交后的真实牌序保留。结束阶段是否发动仍检查存活、有效技能与同一实例。AI 以 PublicControlValue(4) 提供公开控制先验，实际排序使用允许观看的信息，不以摸牌收据代替控制收益。

## 结姻

两个主动绑定共享一个实际出牌阶段额度：弃一张手牌，或把一张本人手牌/装备区的装备置入合法男性的装备区。男性按当前有效性别查询，符合条件的本人可选；原装备同区原地不构成费用。

PlaceSelectedEquipment 消费原始无条件 CaptureSelectedCards 的精确单实体绑定，保存原来源并按真实槽位容量、废除状态和替换规则移动；Transfer/Replacement 意图与外方弃置保护分别处理。装备/体力/移动子窗通过 SelectedCardPayment 返回，费用不重复。

FreezeSelectedHpPair 在所有付款子窗结束后，冻结两人的体力、存活与较大/较小方。先令较大者摸一，再令较小者回复一；相等或同一角色不产生这两项收益。后续摸牌引起体力变化不重算冻结分支。冻结后的尾序列只允许 Draw/Recover，不能追加费用或更换参与者。

程序状态属于原 ProgramSkillFrame，公开与私有选择由 CreateSnapshot(viewerSeat) 过滤冻结。配置见[当前技能配置](RUNTIME.md)，返回见[结算协议](../../RESOLUTION_PROTOCOLS.md)。

## 工程裁定

依据当前普通 OL 官网[界诸葛亮440](https://www.sanguosha.com/hero/440)、[界孙尚香443](https://www.sanguosha.com/hero/443)完整正文；HTTP UTC、原文、结构化体力和原始立绘 SHA见[来源档案](https://github.com/xincheng213618/sanguosha-dotnet/blob/77ab25557053b2e6c1033af6c6f392484c5b71dd/docs/content/sources/fenglin-twelfth-2026-10-02.json)。官网 API性别为空，两位 Male/Female沿仓库既有人物元数据，不能说是API明示。

观星每次启动实际观看时，存活角色至少四人看五，否则三。短堆只看实际可用牌、零张不授皆置底资格为工程解释；一次观看固定同组实体，继续展示已观看牌面不等于再次观看。准备实际全部置底后，真实结束阶段可以再次发动；结束的第二次完成不会重新武装自身。成熟运行时只有正常/额外实际回合的 TurnEnding，没有单独可插 Ending，不能声称覆盖不存在的调度。

准备全底资格是 private、same-instance、Turn BooleanState。原始状态下一实际 BeginTurn重置；结束后没有可用动作与raw store删除不是同一事实。来源压制后同instance可保留已提交事实，新instance不继承，属于既有key的工程解释。新估值只给 opt-in公开控制收益的小正prior，不读未观看牌/RNG，不代表最优AI策略。空城沿classic完整Locked定义。

结姻官网“然后你与其中体力值较大的角色”按你与目标两人中较大者摸1、较小者回复1解释；体力相等没有额外摸/回，都是工程解释，未声明为额外官方FAQ或用户裁决。比较在真实付款和所有付款子流程结束后冻结，先较大者摸1，再较小者回复1；第一步的后续HP变化不改已冻结的第二步。

两种费用共享同实际出牌阶段一次。任意一手牌真实弃置，或本人手牌/装备区一实体装备真实置入男性装备区。按实际有效Gender允许Male owner自选；self Hand弃/Hand装备真正置入可行，原Equipment同区无位移不作费用也不触发枭姬。装备置入按真实区域/slot容量、替换与离区钩子处理；Transfer/Replacement不被奇才外方弃置保护拦截。枭姬沿classic实际装备离区per-card定义。

回复替代、涅槃派生选择和荐言的当前能力见[界孙权、界庞统、界徐庶](FENG_LIN_SIXTEENTH_CAPABILITIES.md)。

### 最终新增能力的边界

观星使用 nullable PopulationThresholdCount 与拥有的 TopReorder.Population，成功实体排序后才授予 private all-bottom state。完整牌面仅 owner 可见；新增 SkillPrompt 的人工/AI路由仅 Population 非空分支。旧 free/exact 路由与估值保留。来源失去/重授采用单独 host 生命周期审计；真实低人口、结束/额外回合、scheduled阶段和死亡按接受命令验证。

装备置入冻结原 captured 单卡 HE SourceLocation，通过成熟 SelectedCardPayment 等待真实 HP/移牌子窗。新 resource 必须是原 unconditional CaptureSelectedCards binding，派生或其他 producer 拒绝。HpPairSnapshot 冻结付款子窗后的两人 HP/存活与较大/较小方；Freeze 之后仅允许 Draw/Recover 尾序列，不再重选参与者、插入独立阶段或追加付款。新字段旧 null 序列省略，旧 source/target 组合不追加限制。

结姻原生 AI 的公开机会先验为 14，观星控制先验为 4，限对应节点估值，不产生摸牌收据或读取敌方私有手牌。
